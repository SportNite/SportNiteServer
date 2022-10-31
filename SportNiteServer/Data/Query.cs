using System.Security.Claims;
using SportNiteServer.Database;
using SportNiteServer.Entities;
using SportNiteServer.Services;

namespace SportNiteServer.Data;

public class Query
{
    public string hello() => "Hello world";
    

    public async Task<User> Me(ClaimsPrincipal claimsPrincipal, AuthService authService)
    {
        return await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal));
    }
}