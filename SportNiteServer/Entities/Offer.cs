using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using SportNiteServer.Services;

namespace SportNiteServer.Entities;

public class Offer
{
    [Key] public Guid OfferId { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; } = "";
    public DateTime DateTime { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [GraphQLDescription("Sport discipline")]
    public SportType Sport { get; set; }

    [GraphQLDescription("Is an offer available (not accepted yet)")]
    public bool IsAvailable { get; set; } = true;

    public List<Response> Responses { get; set; } = new();

    [GraphQLDescription("Weather forest in that day")]
    [NotMapped]
    public Weather? Weather { get; set; }

    public long PlaceId { get; set; }
    [NotMapped] public Place? Place { get; set; }

    public enum SportType
    {
        Run,
        TrailRun,
        Walk,
        Hike,
        Ride,
        MountainBikeRide,
        GravelBikeRide,
        EBikeRide,
        EMountainBikeRide,
        Canoe,
        Kayak,
        KiteSurfing,
        Row,
        StandUpPaddle,
        Swim,
        Windsurfing,
        IceSkate,
        AlpineSki,
        NordicSki,
        Snowboard,
        Snowshoe,
        Golf,
        InlineSkate,
        RockClimb,
        RollerSki,
        Wheelchair,
        Crossfit,
        Elliptical,
        Sailing,
        Skateboarding,
        Soccer,
        StairStepper,
        WeightTraining,
        Yoga,
        Workout,
        Tennis
    }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string City { get; set; } = "";

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string Street { get; set; } = "";

    [GraphQLDescription("Author of the offer")]
    public async Task<User> User([Service] UserService userService)
    {
        return await userService.GetUserById(UserId) ?? throw new Exception("user_not_found");
    }

    public async Task<Response> MyResponse([Service] ResponseService responseService, [Service] AuthService authService,
        ClaimsPrincipal claimsPrincipal)
    {
        return await responseService.GetMyResponse(OfferId, await authService.GetUser(claimsPrincipal));
    }
}