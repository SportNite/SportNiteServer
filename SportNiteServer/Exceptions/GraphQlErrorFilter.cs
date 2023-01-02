using System.Diagnostics.CodeAnalysis;

namespace SportNiteServer.Exceptions;

[ExcludeFromCodeCoverage]
public class GraphQlErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            ForbiddenException => error.WithMessage("access_forbidden").WithCode("ForbiddenException"),
            DuplicateKeyException => error.WithMessage("duplicate_key").WithCode("DuplicateKeyException"),
            _ => error
        };
    }
}