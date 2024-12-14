namespace Application.UseCases.UsersUseCase.ViewModels
{
    public record UserInfoViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
    }
}
