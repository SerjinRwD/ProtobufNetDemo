using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using Grpc.Core;
using ProtoBuf.Grpc;

namespace Service
{
    public class Calculator : ICalculator
    {
        public ValueTask<MultiplyResult> MultiplyAsync(MultiplyRequest request, CallContext context = default) =>
            new ValueTask<MultiplyResult>(new MultiplyResult
            {
                Result = request.X * request.Y
            });

        public ValueTask<DivisionResult> DivisionAsync(DivisionRequest request, CallContext context = default)
        {
            if (request.Y == 0.0D)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Divider Y is zero"));
            }

            return new ValueTask<DivisionResult>(new DivisionResult
            {
                Result = request.X / request.Y
            });
        }
    }

    public class Clock : ITimeService
    {
        public IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default) =>
            SubscribeInternalAsync(context.CancellationToken);

        private async IAsyncEnumerable<TimeResult> SubscribeInternalAsync([EnumeratorCancellation] CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                yield return new TimeResult { Time = DateTime.UtcNow };
            }
        }
    }
}