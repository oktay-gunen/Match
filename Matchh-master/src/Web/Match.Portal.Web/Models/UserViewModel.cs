namespace Match.Web.Models;

public class UserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int OperationClaimId { get; set; }
    public string OperationClaimName { get; set; }
     public int Status { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string StatusName { get; set; }
}
