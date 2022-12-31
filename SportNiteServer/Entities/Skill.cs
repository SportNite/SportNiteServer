using System.ComponentModel.DataAnnotations;

namespace SportNiteServer.Entities;

public class Skill
{
    [Key] public Guid SkillId { get; set; }
    public Guid UserId { get; set; }
    public Offer.SportType Sport { get; set; }
    [GraphQLDescription("Generic level of advance")]
    public double? Level { get; set; }
    public double? Years { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    [GraphQLDescription("Tennis skill level")]
    public double? Nrtp { get; set; }
}