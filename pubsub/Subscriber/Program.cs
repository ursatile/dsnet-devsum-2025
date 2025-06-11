using EasyNetQ;
using Messages;

const string AMQP = "amqps://esgjrvkt:iamkB8Z_yGNEWZQmtirq7B81J6YDfz1G@hefty-black-wolverine.rmq6.cloudamqp.com/esgjrvkt";
var bus = RabbitHutch.CreateBus(AMQP);
const string SUBSCRIPTION_ID = "subscriber";

await bus.PubSub.SubscribeAsync<Greeting>(
	SUBSCRIPTION_ID, greeting => {
		Console.WriteLine(greeting.Number);
	}
);
Console.WriteLine("Listening for messages. Press Ctrl-C to quit");
Console.ReadLine();
