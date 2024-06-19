namespace Troonch.User.Domain.DTOs.Requests;

public class SetPasswordRequestDTO
{
    public string Id { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
    public string Code { get; set; }
}
