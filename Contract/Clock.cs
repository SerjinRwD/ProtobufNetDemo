using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Contract
{
    [ProtoContract]
    public class TimeResult
    {
        [ProtoMember(1, DataFormat = DataFormat.WellKnown)]
        public DateTime Time { get; set; }
    }

    [ServiceContract]
    public interface ITimeService
    {
        IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default);
    }
}