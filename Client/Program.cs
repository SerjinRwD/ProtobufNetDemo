using System;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using Grpc.Core;
using Grpc.Net.Client;
using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;

namespace Client
{
    class Program
    {
        static Program()
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
        }

        static async Task Main(string[] args)
        {
            using (var channel = GrpcChannel.ForAddress("http://localhost:5000"))
            {
                Console.Out.WriteLine("Calling service: Calculator");

                int x = 8, y = 6;

                var calculator = channel.CreateGrpcService<ICalculator>();
                var result = await calculator.MultiplyAsync(new MultiplyRequest { X = x, Y = y });

                Console.Out.WriteLine("{0} * {1} = {2}", x, y, result.Result);

                Console.Out.WriteLine("Calling service: TimeService");

                var clock = channel.CreateGrpcService<ITimeService>();
                var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                var options = new CallOptions(cancellationToken: cancel.Token);

                await foreach (var time in clock.SubscribeAsync(new CallContext(options)))
                {
                    Console.Out.WriteLine("The time is now: {0:yyyy-MM-dd HH:mm:ss}", time.Time);
                }
            }
        }
    }
}