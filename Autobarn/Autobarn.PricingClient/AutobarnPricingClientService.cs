using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pricing;
using static Pricing.Pricer;

namespace Autobarn.PricingClient;

public class AutobarnPricingClientService(
	IBus bus,
	ILogger<AutobarnPricingClientService> logger,
	PricerClient grpcPricerClient
	) : IHostedService {
	public async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Starting pricing client...");
		await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
			"autobarn.pricingclient",
			CalculatePrice);
	}

	private async Task CalculatePrice(NewVehicleMessage nvm) {
		logger.LogInformation("Getting price for {make} {model} {year}",
			nvm.Make, nvm.Model, nvm.Year);
		var request = new PriceRequest {
			Year = nvm.Year,
			Color = nvm.Color,
			Model = nvm.Model,
			Make = nvm.Make
		};
		var price = await grpcPricerClient.GetPriceAsync(request);
		logger.LogInformation("Got price: {price} {currencyCode}", price.Price, price.CurrencyCode);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping pricing client...");
		return Task.CompletedTask;
	}
}