using GrpcGreeter;
using Grpc.Net.Client;
using System.Threading.Tasks;

using var channel = GrpcChannel.ForAddress("https://localhost:7254");

var client = new Greeter.GreeterClient(channel);
Console.WriteLine("Press a key for a greeting (1 = Serbian, 2 = French, 3 = Portuguese, 4 = English");
while (true) {
    var key = Console.ReadKey(true);
    var languageCode = key.KeyChar switch {
        '1' => "sr-SP",
        '2' => "fr-FR",
        '3' => "pt-PT",
        _ => "en-GB"
    };
    var request = new HelloRequest {
         FirstName = "ITkonekt",
         LastName = "People",
         LanguageCode = languageCode
     };
    var reply = await client.SayHelloAsync(request);
    Console.WriteLine(reply.Message);
}

