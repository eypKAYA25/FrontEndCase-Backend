using System.Net;
using System.Runtime.Serialization;
using Domain.Base.Exceptions;

namespace Domain.Company.BusinessOperation.Exceptions;

public class CompanyNotFoundException : DomainException
{
    public CompanyNotFoundException() : base((int)HttpStatusCode.NotFound)
    {
    }

    public CompanyNotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound)
    {
    }

    public CompanyNotFoundException(string message, Exception inner) : base(message, inner, (int)HttpStatusCode.NotFound)
    {
    }

    protected CompanyNotFoundException(
        SerializationInfo info,
        StreamingContext context) : base(info, context, (int)HttpStatusCode.NotFound)
    {
    }
}