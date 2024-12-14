using Application.Response;
using Application.UseCases.UsersUseCase.Commands;
using Application.UseCases.UsersUseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator,
                              IConfiguration configuration,
                              IMapper mapper)
        {
            _mediator = mediator;
            _configuration = configuration;
            _mapper = mapper;
        }


        /// <summary>
        /// Rota responsável pela criação de um usuário.
        /// </summary>
        /// <param name="command">
        /// Um objeto CreateUserCommand
        /// </param>
        /// <returns>Os dados do usuário criado.</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /auth/create-user
        /// {
        ///    "name": "Johh",
        ///     "surname": "Doe",
        ///     "username": "JDoe",
        ///     "email": "jdoe@mail.com",
        ///     "password": "123456"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados de um novo usuário</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ResponseInfo is null && request.Value is not null)
            {
                HandleCookies(request);
                return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
            }

            return BadRequest(request);
        }


        /// <summary>
        /// Rota responsável pelo login na API.
        /// </summary>
        /// <param name="command">
        /// Um objeto LoginUserCommand
        /// </param>
        /// <returns>Os dados do usuário.</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /auth/login
        /// {
        ///    "email": "jdoe@mail.com",
        ///    "password": "123456789",
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados do usuário logado</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ResponseInfo is null && request.Value is not null)
            {
                HandleCookies(request);
                return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
            }

            return BadRequest(request);
        }


        [HttpPost("refreshToken")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> RefreshTokenAsync(RefreshTokenCommand command)
        {
            var request = await _mediator.Send(new RefreshTokenCommand
            {
                Username = command.Username,
                RefreshToken = Request.Cookies["refreshToken"]
            });

            if (request.ResponseInfo is null && request.Value is not null)
            {
                HandleCookies(request);
                return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
            }

            return BadRequest(request);
        }

        private void HandleCookies(ResponseBase<RefreshTokenViewModel> request)
        {
            var cookieOptionsToken = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);

            var cookieOptionsRefreshToken = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationTimeInDays)
            };

            Response.Cookies.Append("jwt", request.Value!.TokenJwt!, cookieOptionsToken);
            Response.Cookies.Append("refreshToken", request.Value!.RefreshToken!, cookieOptionsRefreshToken);
        }
    }
}
