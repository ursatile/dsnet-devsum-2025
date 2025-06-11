using Grpc.Core;
using GreetingServer;

namespace GreetingServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
		var message = request.LanguageCode switch {
			1 => "Good morning, " + request.FirstName ,
			2 => "Hej, " + request.FirstName,
			_ => "Hello " + request.FirstName
		};

        return Task.FromResult(new HelloReply {
			Message = message
		});
    }
}

