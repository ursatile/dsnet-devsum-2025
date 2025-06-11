using EasyNetQ;
using Messages;

const string AMQP = "amqps://esgjrvkt:iamkB8Z_yGNEWZQmtirq7B81J6YDfz1G@hefty-black-wolverine.rmq6.cloudamqp.com/esgjrvkt";
var bus = RabbitHutch.CreateBus(AMQP);
const string SUBSCRIPTION_ID = "dylan";

await bus.PubSub.SubscribeAsync<Greeting>(
	SUBSCRIPTION_ID, handleGreeting
);
Console.WriteLine("Listening for messages. Press Ctrl-C to quit");
Console.ReadLine();

return;

static void handleGreeting(Greeting greeting) {
	if (greeting.Number % 5 == 0) {
		throw new Exception("Bad thing happened!");
	}
	Console.WriteLine($"Received greeting #{greeting.Number}");
}