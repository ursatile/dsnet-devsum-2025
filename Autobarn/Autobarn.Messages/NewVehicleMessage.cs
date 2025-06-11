namespace Autobarn.Messages {
	public record NewVehicleMessage(
		string Registration,
		string Make,
		string Model,
		int Year,
		string Color
	);

	public record NewVehiclePriceMessage(
		string Registration,
		string Make,
		string Model,
		int Year,
		string Color,
		int Price,
		string CurrencyCode);
}
