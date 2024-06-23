namespace Troonch.User.Domain.DTOs.Requests;

public class ChangePasswordRequestDTO
{
    public string? Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
