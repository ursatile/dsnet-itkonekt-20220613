using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot configuration = ReadConfiguration();

        static async Task Main(string[] args) {
            var amqp = configuration.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>("autobarn.notifier", HandleNewVehiclePriceMessage, options => options.WithAutoDelete());
            Console.WriteLine("Autobarn.Notifier started - listening for NewVehiclePriceMessages...");
            Console.ReadLine();
        }

        private static void HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvpm) {
            Console.WriteLine($"Handling NewVehiclePriceMessage: {nvpm}");
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
