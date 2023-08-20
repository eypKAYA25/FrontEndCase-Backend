using System.Net;
using System.Runtime.Serialization;

namespace Domain.Base.Exceptions;

[Serializable]
public abstract class DomainException : ApplicationException
{
    protected DomainException()
    {
        this.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    
    protected DomainException(int statusCode)
    {
        this.StatusCode = statusCode;
    }

    protected DomainException(string message, int statusCode) : base(message)
    {
        this.StatusCode = statusCode;
    }

    protected DomainException(string message, Exception inner, int statusCode) : base(message, inner)
    {
        this.StatusCode = statusCode;
    }

    protected DomainException(
        SerializationInfo info,
        StreamingContext context, int statusCode) : base(info, context)
    {
        this.StatusCode = statusCode;
    }
    
    public int StatusCode { get; set; }
}