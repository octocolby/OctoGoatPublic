namespace OctoGoat.Models;

public class TweeterModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Tweet { get; set; }
    public bool? Checkmark { get; set; }
    public string? ProfilePicture { get; set; }
    public SecretModel? Secret { get; set; }
}
