using System.ComponentModel.DataAnnotations;
using SportNiteServer.Services;

namespace SportNiteServer.Entities;

public class User
{
    [Key] public Guid UserId { get; set; }
    public string FirebaseUserId { get; set; }
    public string Name { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public int Sex { get; set; }
    public string City { get; set; } = "";
    public string Availability { get; set; } = "";
    public string Bio { get; set; } = "";

    [GraphQLIgnore] public List<Offer> Offers { get; set; }
    [GraphQLIgnore] public List<Response> Responses { get; set; }
    public List<Skill> Skills { get; set; }
}