using System.ComponentModel.DataAnnotations;
using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class SetSkillInput
{
    public Guid? SkillId { get; set; }
    public Offer.SportType Sport { get; set; }
    public double Level { get; set; }
}