namespace Troonch.User.Domain.DTOs.Requests;
public class LoginRequestDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
