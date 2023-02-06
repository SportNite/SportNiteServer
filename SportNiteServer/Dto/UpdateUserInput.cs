namespace SportNiteServer.Dto;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

public class UpdateUserInput
{
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Sex { get; set; }
    public string? City { get; set; }
    public string? Availability { get; set; }
    public string? Bio { get; set; }
    public string? MessagingToken { get; set; }
}