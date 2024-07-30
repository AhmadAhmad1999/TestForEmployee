using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.DeleteProduct;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.UpdateProduct;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetAllProducts;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetProductById;

namespace SyriaTrustPlanning.Api.Controllers
{
    // 25 Minutes
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public ProductsController(IMediator Mediator)
        {
            _Mediator = Mediator;
        }

        // 5 Minutes
        [HttpPost("CreateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand CreateProductCommand)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(CreateProductCommand);

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                401 => Unauthorized(),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpDelete("DeleteProduct/{Id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(new DeleteProductCommand()
            {
                Id = Id,
                Token = Token
            });

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                401 => Unauthorized(),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpPut("UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand UpdateProductCommand)
        {
            string? Token = HttpContext.Request.Headers.Authorization!;

            if (Token.IsNullOrEmpty())
                return Unauthorized();

            BaseResponse<object>? Response = await _Mediator.Send(UpdateProductCommand);

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                401 => Unauthorized(),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllProducts(int? CategoryId, int Page = 1, int PerPage = 10)
        {
            BaseResponse<List<GetAllProductsListVM>> Response = await _Mediator.Send(new GetAllProductsQuery()
            {
                Page = Page,
                PerPage = PerPage,
                CategoryId = CategoryId
            });

            return Response.statusCode switch
            {
                404 => NotFound(Response),
                200 => Ok(Response),
                _ => BadRequest(Response)
            };
        }

        // 5 Minutes
        [HttpGet("GetProductById/{Id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetProductById(int Id)
        {
            BaseResponse<GetProductByIdDto> Response = await _Mediator.Send(new GetProductByIdQuery()
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
