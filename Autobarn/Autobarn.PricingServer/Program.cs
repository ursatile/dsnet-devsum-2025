using Autobarn.PricingServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PricerService>();
app.MapGet("/", () => "Welcome to our awesome website!");

app.Run();
