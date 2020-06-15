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
        public double X { get; set; }

        [ProtoMember(2)]
        public double Y { get; set; }
    }

    [ProtoContract]
    public class MultiplyResult
    {
        [ProtoMember(1)]
        public double Result { get; set; }
    }

    [ProtoContract]
    public class DivisionRequest
    {
        [ProtoMember(1)]
        public double X { get; set; }

        [ProtoMember(2)]
        public double Y { get; set; }
    }

    [ProtoContract]
    public class DivisionResult
    {
        [ProtoMember(1)]
        public double Result { get; set; }
    }

    [ServiceContract]
    public interface ICalculator
    {
        ValueTask<MultiplyResult> MultiplyAsync(MultiplyRequest request, CallContext context = default);
        ValueTask<DivisionResult> DivisionAsync(DivisionRequest request, CallContext context = default);
    }
}