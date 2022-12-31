using System.ComponentModel.DataAnnotations;

namespace SportNiteServer.Entities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class User
{
    [Key] public Guid UserId { get; set; }
#pragma warning disable CS8618
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string FirebaseUserId { get; set; }
#pragma warning restore CS8618
    public string Avatar { get; set; } = "";
    public string Name { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public int Sex { get; set; }
    public string City { get; set; } = "";
    public string Availability { get; set; } = "";

    [GraphQLDescription("Short description of user")]
    public string Bio { get; set; } = "";

    [GraphQLIgnore] public List<Offer> Offers { get; set; } = new();
    [GraphQLIgnore] public List<Response> Responses { get; set; } = new();
    [GraphQLDescription("User skills")] public List<Skill> Skills { get; set; } = new();
    public string? Phone { get; set; }
}