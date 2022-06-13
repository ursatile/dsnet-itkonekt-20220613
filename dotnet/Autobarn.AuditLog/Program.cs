using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    class Program {
        private static readonly IConfigurationRoot configuration = ReadConfiguration();

        static async Task Main(string[] args) {
            var amqp = configuration.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>("autobarn.auditlog", HandleNewVehicleMessage, options => options.WithAutoDelete());
            Console.WriteLine("Autobarn.AuditLog started - listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage nvm) {
            Console.WriteLine($"Handling NewVehicleMessage: {nvm}");
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
