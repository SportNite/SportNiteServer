using System.ComponentModel.DataAnnotations;

namespace SportNiteServer.Entities;

public class Skill
{
    [Key] public Guid SkillId { get; set; }
    public Guid UserId { get; set; }
    public Offer.SportType Sport { get; set; }
    public double Level { get; set; }
}