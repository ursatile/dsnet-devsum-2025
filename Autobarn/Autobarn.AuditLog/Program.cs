using Autobarn.AuditLog;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var amqp = builder.Configuration.GetConnectionString("rabbitmq");
var bus = RabbitHutch.CreateBus(amqp);
builder.Services.AddSingleton(bus);
builder.Services.AddHostedService<AutobarnAuditLogService>();

var host = builder.Build();
host.Run();