using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;
using System.Dynamic;

namespace Autobarn.Website.Api;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController(
	AutobarnDbContext db,
	ILogger<VehiclesController> logger,
	IBus bus
) : ControllerBase {


	private dynamic Paginate(string url, int index, int count, int total) {
		dynamic links = new ExpandoObject();
		links.self = new { href = url };
		links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
		links.first = new { href = $"{url}?index=0&count={count}" };
		if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
		if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
		return links;
	}

	[HttpGet]
	public async Task<ActionResult<object>> GetVehicles(
		int index, int count = 10) {
		var total = await db.Vehicles.CountAsync();
		var items = await db.Vehicles
			.Include(v => v.Model)
			.ThenInclude(m => m.Make)
			.Skip(index)
			.Take(count)
			.ToListAsync();
		return new {
			_links = Paginate("/api/vehicles", index, count, total),
			items
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(404)]
	public async Task<ActionResult<ExpandoObject>> GetVehicle(string id) {
		var vehicle = await db.Vehicles
			.Include(v => v.Model)
			.ThenInclude(m => m.Make)
			.FirstOrDefaultAsync(v => v.Registration == id);
		if (vehicle == null) return NotFound();
		var result = vehicle.ToDynamic();
		result._links = new {
			self = new {
				href = $"/api/vehicles/{id}"
			},
			make = new {
				href = $"/api/makes/{vehicle.Model.Make.Code}"
			},
			model = new {
				href = $"/api/makes/{vehicle.Model.Code}"
			}
		};
		return result;
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> PutVehicle(string id, Vehicle vehicle) {
		if (id != vehicle.Registration) return BadRequest();
		db.Entry(vehicle).State = EntityState.Modified;
		try {
			await db.SaveChangesAsync();
		} catch (DbUpdateConcurrencyException) {
			if (!VehicleExists(id)) return NotFound();
			throw;
		}
		return NoContent();
	}

	[HttpPost]
	public async Task<ActionResult<Vehicle>> PostVehicle(VehicleDto dto) {
		var model = await db.Models
			.Include(m => m.Make)
			.FirstOrDefaultAsync(m => m.Code == dto.ModelCode);
		if (model == null) return BadRequest($"Sorry, we don't have a car called {dto.ModelCode}");

		var vehicle = new Vehicle {
			Model = model,
			Registration = dto.Registration,
			Year = dto.Year,
			Color = dto.Color
		};

		db.Vehicles.Add(vehicle);

		try {
			await db.SaveChangesAsync();
			await PublishNewVehicleMessage(vehicle);
		}
		catch (DbUpdateException) {
			if (VehicleExists(vehicle.Registration!)) return Conflict();
			throw;
		}
		logger.LogInformation($"Created vehicle: {vehicle}");
		return CreatedAtAction("GetVehicle", new { id = vehicle.Registration }, vehicle);
	}

	private async Task PublishNewVehicleMessage(Vehicle vehicle) {
		var message = new NewVehicleMessage(
			vehicle.Registration!,
			vehicle.Model?.Make?.Name ?? "(null make)",
			vehicle.Model?.Name ?? "(null model)",
			vehicle.Year,
			vehicle.Color ?? "(null color)"
		);
		await bus.PubSub.PublishAsync(message);
	}

	// DELETE: api/Vehicles/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteVehicle(string id) {
		var vehicle = await db.Vehicles.FindAsync(id);
		if (vehicle == null) return NotFound();
		db.Vehicles.Remove(vehicle);
		await db.SaveChangesAsync();
		return NoContent();
	}

	private bool VehicleExists(string id)
		=> db.Vehicles.Any(e => e.Registration == id);
}