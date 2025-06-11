using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs {
	public class AutobarnHub : Hub {
		public async Task TellPeopleAboutANewCar(string user, string message) {
			await Clients.All.SendAsync("DisplayNewCarNotification", user, message);
		}
	}
}
