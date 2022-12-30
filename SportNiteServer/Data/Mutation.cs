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

    public async Task<Offer> DeleteOffer(Guid id, OfferService offerService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await offerService.DeleteOffer(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)), id);
    }

    public async Task<Response?> CreateResponse(CreateResponseInput input, ResponseService responseService,
        AuthService authService, ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.CreateResponse(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            input);
    }

    public async Task<Response> DeleteResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.DeleteResponse(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            id);
    }

    public async Task<Response?> AcceptResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.AcceptResponse(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            id);
    }

    public async Task<Response?> RejectResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.RejectResponse(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            id);
    }

    public async Task<Skill> SetSkill(SetSkillInput input, AuthService authService, ClaimsPrincipal claimsPrincipal)
    {
        return await authService.SetSkill(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)), input);
    }

    public async Task<Skill> DeleteSkill(Offer.SportType sportType, AuthService authService, ClaimsPrincipal claimsPrincipal)
    {
        return await authService.DeleteSkill(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)), sportType);
    }
    
    public async Task<Place> CreatePlace(CreatePlaceInput input, PlaceService placeService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await placeService.CreatePlace(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)),
            input);
    }
    
    public async Task<Place> DeletePlace(long id, PlaceService placeService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await placeService.DeletePlace(await authService.GetUser(Utils.GetFirebaseUserId(claimsPrincipal)), id);
    }

    public async Task<int> ImportPlaces(PlaceService placeService)
    {
        return await placeService.ImportPlaces();
    }
}