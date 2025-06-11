using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Api;

[Route("api")]
[ApiController]
public class DefaultController : ControllerBase {
	[HttpGet]
	public IActionResult Index() {
		return Ok(new {
			_links = new {
				vehicles = new {
					href = "/api/vehicles"
				}
			},
			message = "Welcome to the Autobarn API"
		});
	}
}
