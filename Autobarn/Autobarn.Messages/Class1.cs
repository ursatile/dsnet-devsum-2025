namespace Autobarn.Messages {
	public record NewVehicleMessage(
		string Registration,
		string Make,
		string Model,
		int Year,
		string Color
	);
	//public string Registration { get; set; } = registration;
	//public string Make { get; set; } = make;
	//public string Model { get; set; } = model;
	//public int Year { get; set; } = year;
	//public string Color { get; set; } = color;
	//	}
}
