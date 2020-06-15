using System;
using System.IO;
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

                double[] x = new double[] { 1.5, 0.0, 3.12, 4.0, 5.0, 6.1 };
                double[] y = new double[] { 6.6, 0.0, 8.87, 0.09, 10.0, 0.0 };

                var calculator = channel.CreateGrpcService<ICalculator>();

                for (var i = 0; i < x.Length; i++)
                {
                    try
                    {
                        var multiplyResult = await calculator.MultiplyAsync(new MultiplyRequest { X = x[i], Y = y[i] });

                        Console.Out.WriteLine("{0:F} * {1:F} = {2:F}", x[i], y[i], multiplyResult.Result);

                        var divisionResult = await calculator.DivisionAsync(new DivisionRequest { X = x[i], Y = y[i] });

                        Console.Out.WriteLine("{0:F} / {1:F} = {2:F}", x[i], y[i], divisionResult.Result);
                    }
                    catch (RpcException rEx)
                    {
                        Console.Out.WriteLine("Got error code: \"{0}\", message: \"{1}\"", rEx.Status.StatusCode, rEx.Status.Detail);
                    }
                }

                Console.Out.WriteLine("Calling service: TimeService");

                var clock = channel.CreateGrpcService<ITimeService>();
                var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                var options = new CallOptions(cancellationToken: cancel.Token);

                try
                {
                    await foreach (var time in clock.SubscribeAsync(new CallContext(options)))
                    {
                        Console.Out.WriteLine("The time is now: {0:yyyy-MM-dd HH:mm:ss}", time.Time);
                    }
                }
                catch (IOException ioE)
                {
                    Console.Out.WriteLine(ioE.Message);
                }
            }
        }
    }
}