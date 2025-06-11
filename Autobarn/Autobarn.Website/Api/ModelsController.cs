using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autobarn.Data;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Api;

[Route("api/[controller]")]
[ApiController]
public class ModelsController(AutobarnDbContext db) : ControllerBase {

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CarModel>>> GetModels()
		=> await db.Models
			.Include(m => m.Make)
			.ToListAsync();

	[HttpGet("{id}")]
	public async Task<ActionResult<CarModel>> GetCarModel(string id) {
		var carModel = await db.Models
			.Include(m => m.Make)
			.FirstOrDefaultAsync(m => m.Code == id);
		if (carModel == null) return NotFound();
		return carModel;
	}

	[HttpPost]
	public async Task<ActionResult<CarModel>> PostCarModel(CarModel carModel) {
		db.Models.Add(carModel);
		try {
			await db.SaveChangesAsync();
		} catch (DbUpdateException) {
			if (db.Models.Any(e => e.Code == carModel.Code)) return Conflict();
		}
		return CreatedAtAction("GetCarModel", new { id = carModel.Code }, carModel);
	}
}