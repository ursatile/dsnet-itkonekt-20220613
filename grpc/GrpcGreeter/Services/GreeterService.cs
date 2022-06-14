using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var name = $"{request.FirstName} {request.LastName}";

        var greeting = request.LanguageCode switch {
            "fr-FR" => $"Bonjour, {name}",
            "pt-PT" => $"Bom dia, {name}",
            "sr-SP" => $"Zdravo, {name}",
            _ => $"Hello, {name}"
        };
        return Task.FromResult(new HelloReply { Message = greeting });
    }
}
