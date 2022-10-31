namespace SportNiteServer.Entities;

public class Offer
{
    public int OfferId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public SportType Sport { get; set; }
    public bool IsAvailable { get; set; } = true;

    public List<Response> Responses { get; set; }

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
}