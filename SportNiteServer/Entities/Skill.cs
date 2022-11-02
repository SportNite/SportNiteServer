using System.ComponentModel.DataAnnotations;

namespace SportNiteServer.Entities;

public class Skill
{
    [Key] public Guid SkillId { get; set; }
    public Guid UserId { get; set; }
    public Offer.SportType Sport { get; set; }
    public double? Level { get; set; }
    public double? Years { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public double? Nrtp { get; set; }
}