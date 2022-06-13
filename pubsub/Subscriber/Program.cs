using EasyNetQ;
using Messages;

const string AMQP = "amqps://vplvrfmm:k-NylqD4wHurQvUndFb3nHpX33-M8r-a@armadillo.rmq.cloudamqp.com/vplvrfmm";
using var bus = RabbitHutch.CreateBus(AMQP);
// set your subscriptionId to something unique:
var subscriptionId = "dylan_in_london";
await bus.PubSub.SubscribeAsync<Greeting>(subscriptionId, HandleGreeting, x => x.WithAutoDelete());

try {
    Console.WriteLine("Listening for Greeting messages; press Enter to exit...");
    Console.ReadLine();
    Console.WriteLine("Exited cleanly!");
} catch (Exception) {
    Console.WriteLine("Didn't exit cleanly - crashed!");
}


void HandleGreeting(Greeting greeting) {   
    if (greeting.Number % 5 == 0) {
        throw new InvalidOperationException("Numbers divisible by 5 cannot be processed - sorry!");
    } 
    Console.WriteLine($"Received greeting #{greeting.Number} from {greeting.MachineName} (sent at {greeting.SentAt:O}");
    Console.WriteLine($"  Name: {greeting.Name}");
    // Thread.Sleep(TimeSpan.FromSeconds(3));
}


