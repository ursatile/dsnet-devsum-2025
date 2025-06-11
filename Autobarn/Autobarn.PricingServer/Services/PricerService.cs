using Grpc.Core;
using Pricing;

namespace Autobarn.PricingServer.Services {
	public class PricerService(ILogger<PricerService> logger) : Pricer.PricerBase {
	private readonly ILogger<PricerService> logger = logger;

	public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
			return Task.FromResult(new PriceReply {
				Price = 123456,
				CurrencyCode = "HUF"
			});
		}
	}
}
