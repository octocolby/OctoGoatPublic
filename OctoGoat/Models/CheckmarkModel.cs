namespace OctoGoat.Models;

public class CheckmarkModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Approved { get; set; }
    public SecretModel? Secret { get; set; }
}
