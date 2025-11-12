namespace ShortUrl.Api.Models;


public class Link
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Url { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Hits { get; set; }
}