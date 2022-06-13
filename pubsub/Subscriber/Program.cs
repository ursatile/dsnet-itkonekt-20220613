using EasyNetQ;
using Messages;

const string AMQP = "amqps://vplvrfmm:k-NylqD4wHurQvUndFb3nHpX33-M8r-a@armadillo.rmq.cloudamqp.com/vplvrfmm";
using var bus = RabbitHutch.CreateBus(AMQP);

await bus.PubSub.SubscribeAsync<Greeting>("SUBSCRIPTION_ID", greeting => {
    Console.WriteLine($"Received greeting #{greeting.Number} from {greeting.MachineName} (sent at {greeting.SentAt:O}");
    Console.WriteLine($"  Name: {greeting.Name}");
}, x => x.WithAutoDelete());

Console.WriteLine("Listening for Greeting messages; press Enter to exit...");
Console.ReadLine();



