using System.Security.Claims;

using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var authenticatedUserId))
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Authenticated user not found."
                });
            }

            var command = _mapper.Map<CreateSaleCommand>(request);
            command.AuthenticatedUserId = authenticatedUserId;

            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<CreateSaleResponse>(result);

                return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
                {
                    Success = true,
                    Message = "Sale created successfully",
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{saleId}/items/{productId}/cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid saleId, [FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var request = new UpdateSaleRequest
            {
                SaleId = saleId,
                ProductId = productId
            };

            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var authenticatedUserId))
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Authenticated user not found."
                });
            }

            var command = _mapper.Map<UpdateSaleCommand>(request);
            command.AuthenticatedUserId = authenticatedUserId;

            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<UpdateSaleResponse>(result);

                return Ok(new ApiResponseWithData<UpdateSaleResponse>
                {
                    Success = true,
                    Message = "Sale item cancelled successfully",
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{saleId}/cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale([FromRoute] Guid saleId, CancellationToken cancellationToken)
        {
            var request = new CancelSaleRequest
            {
                SaleId = saleId
            };

            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var authenticatedUserId))
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Authenticated user not found."
                });
            }

            var command = _mapper.Map<Ambev.DeveloperEvaluation.Application.Sales.CancelSale.CancelSaleCommand>(request);
            command.AuthenticatedUserId = authenticatedUserId;

            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<CancelSaleResponse>(result);

                return Ok(new ApiResponseWithData<CancelSaleResponse>
                {
                    Success = true,
                    Message = "Sale cancelled successfully",
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetSaleCommand>(request.Id);
            try
            {
                var response = await _mediator.Send(command, cancellationToken);
                return Ok(new ApiResponseWithData<GetSaleResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = _mapper.Map<GetSaleResponse>(response)
                });
            }
            catch (InvalidOperationException e)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = e.Message,
                    Errors = []
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}