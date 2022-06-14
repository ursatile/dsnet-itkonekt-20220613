using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private static Pricer.PricerClient grpc;
        private static readonly IConfigurationRoot configuration = ReadConfiguration();
        static async Task Main(string[] args) {
            var channel = GrpcChannel.ForAddress(configuration["AutobarnPricingServerUrl"]);
            grpc = new Pricer.PricerClient(channel);
            var amqp = configuration.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>("Autobarn.PricingClient",
                HandleNewVehicleMessage);
            Console.WriteLine("Subscribed to NewVehicleMessages");
            Console.ReadLine();
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage nvm) {
            var pr = new PriceRequest {
                Color = nvm.Color,
                Year = nvm.Year,
                Make = nvm.ManufacturerName,
                Model = nvm.ModelName
            };
            Console.WriteLine($"Sending PriceRequest: {pr.Color} {pr.Year} {pr.Make} {pr.Model}");
            var reply = await grpc.GetPriceAsync(pr);
            Console.WriteLine($"Price: {reply.Price} {reply.CurrencyCode}");
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
