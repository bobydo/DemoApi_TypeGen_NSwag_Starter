namespace Demo.Api.Data.Entities;

public class Student
{
    public int StudentId { get; set; }
    public string StudentNo { get; set; } = "";
    public string Name { get; set; } = "";
    public bool Active { get; set; }
    
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}
