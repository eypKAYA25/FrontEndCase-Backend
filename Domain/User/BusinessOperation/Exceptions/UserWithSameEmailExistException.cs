using System.Net;
using System.Runtime.Serialization;
using Domain.Base.Exceptions;

namespace Domain.User.BusinessOperation.Exceptions;

public class UserWithSameEmailExistException : DomainException
{
    public UserWithSameEmailExistException() : base((int)HttpStatusCode.Conflict)
    {
    }

    public UserWithSameEmailExistException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }

    public UserWithSameEmailExistException(string message, Exception inner) : base(message, inner, (int)HttpStatusCode.Conflict)
    {
    }

    protected UserWithSameEmailExistException(
        SerializationInfo info,
        StreamingContext context) : base(info, context, (int)HttpStatusCode.Conflict)
    {
    }
}