using System.Net;
using System.Runtime.Serialization;
using Domain.Base.Exceptions;

namespace Domain.Company.BusinessOperation.Exceptions;

public class CompanyWithSameTaxNumberExistException : DomainException
{
    public CompanyWithSameTaxNumberExistException() : base((int)HttpStatusCode.Conflict)
    {
    }

    public CompanyWithSameTaxNumberExistException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }

    public CompanyWithSameTaxNumberExistException(string message, Exception inner) : base(message, inner, (int)HttpStatusCode.Conflict)
    {
    }

    protected CompanyWithSameTaxNumberExistException(
        SerializationInfo info,
        StreamingContext context) : base(info, context, (int)HttpStatusCode.Conflict)
    {
    }
}