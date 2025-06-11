using GreetingServer;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5246");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("gRPC client running!");
Console.WriteLine("""
1: English (UK)
2: Swedish
3: English (US)
""");
while (true) {
	var languageCode = Console.ReadLine() switch {
		"1" => "en-GB",
		"2" => "sv-SE",
		_ => "en-US"
	};
	var request = new HelloRequest {
		FirstName = "Dev",
		LastName = "Sum",
		LanguageCode = languageCode
	};
	var reply = await grpcClient.SayHelloAsync(request);
	Console.WriteLine(reply.Message);
}


