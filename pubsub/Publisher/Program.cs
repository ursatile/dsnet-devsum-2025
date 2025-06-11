using EasyNetQ;
using Messages;

const string AMQP = "amqps://esgjrvkt:iamkB8Z_yGNEWZQmtirq7B81J6YDfz1G@hefty-black-wolverine.rmq6.cloudamqp.com/esgjrvkt";
var bus = RabbitHutch.CreateBus(AMQP);

Console.WriteLine("Press a key to publish a message");

var number = 1;
while (true) {
	Console.ReadKey();
	var message = new Greeting {
		Number = number++
	};
	await bus.PubSub.PublishAsync(message);
	Console.WriteLine($"Published message #{message.Number}");
}
