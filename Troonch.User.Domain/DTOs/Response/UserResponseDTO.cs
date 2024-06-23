namespace Troonch.User.Domain.DTOs.Response;

public class UserResponseDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool PhoneNumberConfirmed = false;
    public bool TwoFactorEnabled {  get; set; } = default(bool);
    public DateOnly? DateOfBirth { get; set; } = null;

    public string? DateOfBirthNormalized
    {
        get => DateOfBirth?.ToString("dd/MM/yyyy");
    }
    public DateTime CreatedOn { get; set; }
    public string CreatedOnNormalized
    {
        get => CreatedOn.ToString("dd/MM/yyyy");
    }
    public DateTime UpdatedOn { get; set; }

    public string UpdatedOnNormalized
    {
        get => UpdatedOn.ToString("dd/MM/yyyy");
    }
    public string FullName
    {
        get => $"{LastName} {Name}"; 
    }
    public int ProgressOfDataComplete { get; set; } = 0;

    public IList<string> RoleNames { get; set; }
}
