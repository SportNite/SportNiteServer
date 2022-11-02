using System.ComponentModel.DataAnnotations;
using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class SetSkillInput
{
    public Guid? SkillId { get; set; }
    public Offer.SportType Sport { get; set; }
    public double Level { get; set; }
    public double? Years { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public double? Nrtp { get; set; }
}