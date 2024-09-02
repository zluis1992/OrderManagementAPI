using System.Runtime.Serialization;

namespace Domain.Exceptions;

[Serializable]
public sealed class OrderException : CoreBusinessException
{
    public OrderException()
    {
    }

    public OrderException(string msg) : base(msg)
    {
    }

    public OrderException(string message, Exception inner) : base(message, inner)
    {
    }

    [Obsolete("Obsolete")]
    private OrderException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
