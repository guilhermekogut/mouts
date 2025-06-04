using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing cart operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        try
        {
            var command = _mapper.Map<CreateCartCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateCartResponse>
            {
                Success = true,
                Message = "Cart created successfully",
                Data = _mapper.Map<CreateCartResponse>(response)
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
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<UpdateCartCommand>(request);
            command.Id = id;
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateCartResponse>
            {
                Success = true,
                Message = "Cart updated successfully",
                Data = _mapper.Map<UpdateCartResponse>(response)
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
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<DeleteCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = result.Message ?? "Cart not found"
            });
        }

        return Ok(new ApiResponseWithData<DeleteCartResponse>
        {
            Success = true,
            Message = "Cart deleted successfully",
            Data = _mapper.Map<DeleteCartResponse>(result)
        });
    }
}