using EasyNetQ;
using Messages;

const string AMQP = "amqps://vplvrfmm:k-NylqD4wHurQvUndFb3nHpX33-M8r-a@armadillo.rmq.cloudamqp.com/vplvrfmm";
using var bus = RabbitHutch.CreateBus(AMQP);
var number = 0;
Console.WriteLine("Press any key to send a message...");
while (true) {
    Console.ReadKey(true);
    var greeting = new Greeting {
        Name = "ITkonekt",
        Number = number++
    };
    await bus.PubSub.PublishAsync(greeting);
    Console.WriteLine($"Published greeting #{greeting.Number}");
}