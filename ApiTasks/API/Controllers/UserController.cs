using Application.UsersUseCase.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
