using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot configuration = ReadConfiguration();
        private static HubConnection hub;
        static async Task Main(string[] args) {
            JsonConvert.DefaultSettings = JsonSettings;

            var signalRHubUrl = configuration["AutobarnSignalRHubUrl"];
            hub = new HubConnectionBuilder().WithUrl(signalRHubUrl).Build();
            Console.WriteLine($"Using SignalR hub at {signalRHubUrl}"); 
            await hub.StartAsync();
            Console.WriteLine("Connected to SignalR");
            var amqp = configuration.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>("autobarn.notifier", HandleNewVehiclePriceMessage, options => options.WithAutoDelete());
            Console.WriteLine("Autobarn.Notifier started - listening for NewVehiclePriceMessages...");
            Console.ReadLine();
        }

        private static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvpm, CancellationToken token) {
            Console.WriteLine($"Handling NewVehiclePriceMessage: {nvpm}");
            var json = JsonConvert.SerializeObject(nvpm);
            await hub.SendAsync("MagicMethodNameNumberOne", "Autobarn.Notifier", json);
            Console.WriteLine("Sent JSON to SignalR hub:");
            Console.WriteLine(json);
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static JsonSerializerSettings JsonSettings() =>
            new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

    }
}
