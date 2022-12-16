namespace Infrastructure.Ef.Repository.User;

public class FilteringUser
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public string? Search { get; set; }
    public int? RoleId { get; set; }
}