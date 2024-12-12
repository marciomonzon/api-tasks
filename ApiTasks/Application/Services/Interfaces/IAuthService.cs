using Domain.Enums;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        public string GenerateJWT(string email, string username);
        public string GenerateRefreshToken();
        public string HashingPassword(string password);
        public Task<ValidationFieldsUserEnum> UniqueEmailAndUsernameAsync(string email, string username);
    }
}
