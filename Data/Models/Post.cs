namespace Data.Models;

public class Post
{
    public Guid PostId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}