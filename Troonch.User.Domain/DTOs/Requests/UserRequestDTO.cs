namespace Troonch.User.Domain.DTOs.Requests;

public class UserRequestDTO
{
    public Guid? Id { get; set; } = Guid.Empty; 
    public string Email { get; set; } 
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
}
