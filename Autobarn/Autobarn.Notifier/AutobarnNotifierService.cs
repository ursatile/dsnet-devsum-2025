using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier;

public class AutobarnNotifierService(IBus bus,
	ILogger<AutobarnNotifierService> logger) : IHostedService {
	public async Task StartAsync(CancellationToken token) {
		await bus.PubSub.SubscribeAsync<NewVehicleMessage>("autobarn.Notifier", OnMessage);
		logger.LogInformation("Started Autobarn Audit Log service");
	}

	private void OnMessage(NewVehicleMessage nvm) {
		logger.LogInformation($"New vehicle {nvm.Registration}");
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping Autobarn Audit Log service");
		return Task.CompletedTask;
	}
}