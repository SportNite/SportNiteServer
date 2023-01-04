using SportNiteServer.Entities;

namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class SetSkillInput
{
    public Guid? SkillId { get; set; }
    public Offer.SportType Sport { get; set; }
    public double? Level { get; set; }
    public double? Years { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    // ReSharper disable once IdentifierTypo
    public double? Nrtp { get; set; }
}