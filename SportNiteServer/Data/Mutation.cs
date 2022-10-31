using System.Security.Claims;
using SportNiteServer.Dto;
using SportNiteServer.Entities;
using SportNiteServer.Services;

namespace SportNiteServer.Data;

public class Mutation
{
    public async Task<User> UpdateUser(UpdateUserInput payload, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await authService.UpdateUser(
            await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)), payload);
    }

    public async Task<Offer> CreateOffer(CreateOfferInput input, OfferService offerService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await offerService.CreateOffer(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            input);
    }
}