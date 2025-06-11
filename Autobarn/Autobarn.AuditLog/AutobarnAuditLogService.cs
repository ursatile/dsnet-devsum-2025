using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog;

public class AutobarnAuditLogService(IBus bus,
	ILogger<AutobarnAuditLogService> logger) : IHostedService {
	public async Task StartAsync(CancellationToken token) {
		await bus.PubSub.SubscribeAsync<NewVehicleMessage>("autobarn.auditlog", OnMessage);
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