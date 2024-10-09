using System.ComponentModel.DataAnnotations.Schema;

namespace OctoGoat.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public bool? IsAdmin { get; set; }
    public string? ProfilePicture { get; set; }
}
