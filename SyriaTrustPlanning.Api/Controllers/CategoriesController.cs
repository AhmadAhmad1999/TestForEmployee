using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.CreateCategory;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.DeleteCategory;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.UpdateCategory;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetAllCategories;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetCategoryById;

namespace SyriaTrustPlanning.Api.Controllers
{
    // 25 Minutes
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public CategoriesController(IMediator Mediator)
        {
            _Mediator = Mediator;
        }

        // 5 Minutes
        [HttpPost("CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand CreateCategoryCommand)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(CreateCategoryCommand);

            return Response.statusCode switch
            {
                200 => Ok(Response),
                404 => NotFound(Response),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpDelete("DeleteCategory/{Id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(new DeleteCategoryCommand()
            {
                Id = Id,
                Token = Token
            });

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpPut("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand UpdateCategoryCommand)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(UpdateCategoryCommand);

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpGet("GetAllCategorys")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllCategorys(int Page = 1, int PerPage = 10)
        {
            BaseResponse<List<GetAllCategoriesListVM>> Response = await _Mediator.Send(new GetAllCategoriesQuery()
            {
                Page = Page,
                PerPage = PerPage
            });

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpGet("GetCategoryById/{Id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            BaseResponse<GetCategoryByIdDto> Response = await _Mediator.Send(new GetCategoryByIdQuery()
            {
                Id = Id
            });

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                _ => BadRequest(Response)
            };
        }
    }
}
