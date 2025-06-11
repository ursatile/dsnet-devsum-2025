using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autobarn.Notifier;

public class AutobarnNotifierService(IBus bus,
	ILogger<AutobarnNotifierService> logger,
	HubConnection hub
	) : IHostedService {
	public async Task StartAsync(CancellationToken token) {
		await hub.StartAsync(token);
		await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>("autobarn.Notifier", HandleNewVehiclePriceMessage);
		logger.LogInformation("Started Autobarn Notifier service");
	}

	private async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvpm) {
		logger.LogInformation("New vehicle {reg} ({price} {currencyCode}", nvpm.Registration, nvpm.Price, nvpm.CurrencyCode);
		var json = JsonConvert.SerializeObject(nvpm);
		await hub.SendAsync("TellPeopleAboutANewCar", "Autobarn.Notifier", json);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping Autobarn Notifier service");
		return Task.CompletedTask;
	}
}