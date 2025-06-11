using Autobarn.PricingClient;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pricing;

var builder = Host.CreateApplicationBuilder(args);
var amqp = builder.Configuration.GetConnectionString("rabbitmq");
var bus = RabbitHutch.CreateBus(amqp);
var grpcUrl = builder.Configuration.GetConnectionString("grpc")!;
var channel = GrpcChannel.ForAddress(grpcUrl);
var client = new Pricer.PricerClient(channel);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton(bus);
builder.Services.AddHostedService<AutobarnPricingClientService>();

var host = builder.Build();
host.Run();