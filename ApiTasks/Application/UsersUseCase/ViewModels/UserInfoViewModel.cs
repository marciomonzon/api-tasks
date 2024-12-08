﻿namespace Application.UsersUseCase.ViewModels
{
    public record UserInfoViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
        public string? TokenJwt { get; set; }
    }
}
