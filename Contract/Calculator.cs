using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Contract
{
    [ProtoContract]
    public class MultiplyRequest
    {
        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }
    }

    [ProtoContract]
    public class MultiplyResult
    {
        [ProtoMember(1)]
        public int Result { get; set; }
    }

    [ServiceContract]
    public interface ICalculator
    {
        ValueTask<MultiplyResult> MultiplyAsync(MultiplyRequest request, CallContext context = default);
    }
}