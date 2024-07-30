using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Features.UserFeatures.Commands.Login;
using SyriaTrustPlanning.Application.Features.UserFeatures.Commands.SignUp;

namespace SyriaTrustPlanning.Api.Controllers
{
    // 20 Minutes
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public UsersController(IMediator Mediator)
        {
            _Mediator = Mediator;
        }

        // 10 Minutes
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateProduct([FromBody] SignUpCommand SignUpCommand)
        {
            BaseResponse<AuthenticationResponse>? Response = await _Mediator.Send(SignUpCommand);

            return Response.statusCode switch
            {
                200 => Ok(Response),
                404 => NotFound(Response),
                _ => BadRequest(Response)
            };
        }

        // 10 Minutes
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login([FromBody] LoginCommand LoginCommand)
        {
            BaseResponse<AuthenticationResponse>? Response = await _Mediator.Send(LoginCommand);

            return Response.statusCode switch
            {
                200 => Ok(Response),
                404 => NotFound(Response),
                _ => BadRequest(Response)
            };
        }
    }
}
