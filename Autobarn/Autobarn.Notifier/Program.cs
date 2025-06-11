using Autobarn.Notifier;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var amqp = builder.Configuration.GetConnectionString("rabbitmq");
var bus = RabbitHutch.CreateBus(amqp);

var hub = new HubConnectionBuilder()
	.WithUrl("https://autobarn.dev/hub")
	.Build();
builder.Services.AddSingleton(hub);
builder.Services.AddSingleton(bus);
builder.Services.AddHostedService<AutobarnNotifierService>();

var host = builder.Build();
host.Run();