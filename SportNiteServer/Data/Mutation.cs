using System.Security.Claims;
using SportNiteServer.Dto;
using SportNiteServer.Entities;
using SportNiteServer.Services;

namespace SportNiteServer.Data;

// Available mutations that can be performed on the database
// ReSharper disable once ClassNeverInstantiated.Global
public class Mutation
{
    [GraphQLDescription("Update personal profile")]
    public async Task<User> UpdateUser(UpdateUserInput payload, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await authService.UpdateUser(
            await authService.GetUser(claimsPrincipal), payload);
    }

    public async Task<Offer> CreateOffer(CreateOfferInput input, OfferService offerService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await offerService.CreateOffer(await authService.GetUser(claimsPrincipal),
            input);
    }

    public async Task<Offer> DeleteOffer(Guid id, OfferService offerService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await offerService.DeleteOffer(await authService.GetUser(claimsPrincipal), id);
    }

    public async Task<Response?> CreateResponse(CreateResponseInput input, ResponseService responseService,
        AuthService authService, ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.CreateResponse(await authService.GetUser(claimsPrincipal),
            input);
    }

    public async Task<Response> DeleteResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.DeleteResponse(await authService.GetUser(claimsPrincipal),
            id);
    }

    [GraphQLDescription("Accept response to offer")]
    public async Task<Response?> AcceptResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.AcceptResponse(await authService.GetUser(claimsPrincipal),
            id);
    }

    [GraphQLDescription("Decline response to offer")]
    public async Task<Response?> RejectResponse(Guid id, ResponseService responseService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.RejectResponse(await authService.GetUser(claimsPrincipal),
            id);
    }


    public async Task<Skill> SetSkill(SetSkillInput input, AuthService authService, ClaimsPrincipal claimsPrincipal)
    {
        return await authService.SetSkill(await authService.GetUser(claimsPrincipal), input);
    }

    public async Task<Skill> DeleteSkill(Offer.SportType sportType, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await authService.DeleteSkill(await authService.GetUser(claimsPrincipal), sportType);
    }

    [GraphQLDescription("Add another place to exist alongside OSM places")]
    public async Task<Place> CreatePlace(CreatePlaceInput input, PlaceService placeService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await placeService.CreatePlace(await authService.GetUser(claimsPrincipal),
            input);
    }

    public async Task<Place> DeletePlace(long id, PlaceService placeService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await placeService.DeletePlace(await authService.GetUser(claimsPrincipal), id);
    }

    [GraphQLDescription("Import places from OSM Overpass API dump")]
    public async Task<int> ImportPlaces(PlaceService placeService)
    {
        // Seed the database with places file (Assets module)
        return await placeService.ImportPlaces();
    }

    public async Task<Device> CreateDevice(CreateDeviceInput input, [Service] DeviceService deviceService,
        AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await deviceService.CreateDevice(await authService.GetUser(claimsPrincipal),
            input);
    }

    public async Task<Device> UpdateDevice(UpdateDeviceInput input, [Service] DeviceService deviceService,
        AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await deviceService.UpdateDevice(await authService.GetUser(claimsPrincipal),
            input);
    }

    public async Task<Device> DeleteDevice(Guid id, [Service] DeviceService deviceService, AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await deviceService.DeleteDevice(await authService.GetUser(claimsPrincipal), id);
    }
}