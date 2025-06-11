using System.Net.Http.Json;
using System.Text.Json;
using Autobarn.Data.Entities;
using Microsoft.Extensions.Logging;

namespace Autobarn.ApiClient;

public class AutobarnApiClient(HttpClient http, ILogger<AutobarnApiClient> logger) {

	private readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

	private string[]? modelCodes;

	private string[] ModelCodes
		=> modelCodes ??= ListModelCodes().Result.Select(c => c.Code).ToArray();

	public async Task<CarModel[]> ListModelCodes() {
		var json = await http.GetStringAsync("/api/models");
		logger.LogInformation(json);
		var models = JsonSerializer.Deserialize<CarModel[]>(json, options)!;
		return models;
	}

	public async Task<(VehicleData, Uri?)> CreateRandomVehicleAsync() {
		var color = WebColors[Random.Shared.Next(WebColors.Length)];
		var year = 1960 + Random.Shared.Next(65);
		var registration = Guid.NewGuid().ToString("N")[..8];
		var modelCode = ModelCodes[Random.Shared.Next(ModelCodes.Length)];
		var data = new VehicleData {
			ModelCode = modelCode,
			Year = year,
			Registration = registration,
			Color = color
		};
		var response = await http.PostAsJsonAsync("/api/vehicles", data);
		logger.LogInformation(response.StatusCode.ToString());
		return (data, response.Headers.Location);
	}

	private string[] WebColors = [
		"AliceBlue",
		"AntiqueWhite",
		"Aqua",
		"Aquamarine",
		"Azure",
		"Beige",
		"Bisque",
		"Black",
		"BlanchedAlmond",
		"Blue",
		"BlueViolet",
		"Brown",
		"BurlyWood",
		"CadetBlue",
		"Chartreuse",
		"Chocolate",
		"Coral",
		"CornflowerBlue",
		"Cornsilk",
		"Crimson",
		"Cyan",
		"DarkBlue",
		"DarkCyan",
		"DarkGoldenrod",
		"DarkGray",
		"DarkGreen",
		"DarkKhaki",
		"DarkMagenta",
		"DarkOliveGreen",
		"DarkOrange",
		"DarkOrchid",
		"DarkRed",
		"DarkSalmon",
		"DarkSeaGreen",
		"DarkSlateBlue",
		"DarkSlateGray",
		"DarkTurquoise",
		"DarkViolet",
		"DeepPink",
		"DeepSkyBlue",
		"DimGray",
		"DodgerBlue",
		"Firebrick",
		"FloralWhite",
		"ForestGreen",
		"Fuchsia",
		"Gainsboro",
		"GhostWhite",
		"Gold",
		"Goldenrod",
		"Gray",
		"Green",
		"GreenYellow",
		"Honeydew",
		"HotPink",
		"IndianRed",
		"Indigo",
		"Ivory",
		"Khaki",
		"Lavender",
		"LavenderBlush",
		"LawnGreen",
		"LemonChiffon",
		"LightBlue",
		"LightCoral",
		"LightCyan",
		"LightGoldenrodYellow",
		"LightGray",
		"LightGreen",
		"LightPink",
		"LightSalmon",
		"LightSeaGreen",
		"LightSkyBlue",
		"LightSlateGray",
		"LightSteelBlue",
		"LightYellow",
		"Lime",
		"LimeGreen",
		"Linen",
		"Magenta",
		"Maroon",
		"MediumAquamarine",
		"MediumBlue",
		"MediumOrchid",
		"MediumPurple",
		"MediumSeaGreen",
		"MediumSlateBlue",
		"MediumSpringGreen",
		"MediumTurquoise",
		"MediumVioletRed",
		"MidnightBlue",
		"MintCream",
		"MistyRose",
		"Moccasin",
		"NavajoWhite",
		"Navy",
		"OldLace",
		"Olive",
		"OliveDrab",
		"Orange",
		"OrangeRed",
		"Orchid",
		"PaleGoldenrod",
		"PaleGreen",
		"PaleTurquoise",
		"PaleVioletRed",
		"PapayaWhip",
		"PeachPuff",
		"Peru",
		"Pink",
		"Plum",
		"PowderBlue",
		"Purple",
		"Red",
		"RosyBrown",
		"RoyalBlue",
		"SaddleBrown",
		"Salmon",
		"SandyBrown",
		"SeaGreen",
		"Seashell",
		"Sienna",
		"Silver",
		"SkyBlue",
		"SlateBlue",
		"SlateGray",
		"Snow",
		"SpringGreen",
		"SteelBlue",
		"Tan",
		"Teal",
		"Thistle",
		"Tomato",
		"Turquoise",
		"Violet",
		"Wheat",
		"White",
		"WhiteSmoke",
		"Yellow",
		"YellowGreen"
	];
}

