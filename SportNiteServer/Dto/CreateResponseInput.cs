namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class CreateResponseInput
{
    public Guid? ResponseId { get; set; }
    public Guid OfferId { get; set; }
    public string Description { get; set; } = "";
}