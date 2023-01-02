using System.Diagnostics.CodeAnalysis;

namespace SportNiteServer.Exceptions;

[ExcludeFromCodeCoverage]
// ReSharper disable once ClassNeverInstantiated.Global
public class GraphQlErrorFilter : IErrorFilter
{
    // Transform exceptions into pretty error messages
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