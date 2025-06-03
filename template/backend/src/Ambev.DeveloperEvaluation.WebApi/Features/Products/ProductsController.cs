using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListCategories;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
        {
            Success = true,
            Message = "Product created successfully",
            Data = _mapper.Map<CreateProductResponse>(response)
        });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetProductRequest { Id = id };
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetProductCommand>(request.Id);
        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(new ApiResponseWithData<GetProductResponse>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = _mapper.Map<GetProductResponse>(response)
            });
        }
        catch (KeyNotFoundException e)
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<DeleteProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteProductCommand>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);

        if (!response.Success)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = response.Message ?? "Product not found",
                Errors = []
            });
        }

        return Ok(new ApiResponseWithData<DeleteProductResponse>
        {
            Success = true,
            Message = "Product deleted successfully",
            Data = new DeleteProductResponse { Id = id }
        });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<UpdateProductResponse>
        {
            Success = true,
            Message = "Product updated successfully",
            Data = _mapper.Map<UpdateProductResponse>(response)
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ListProductsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<ListProductsCommand>(request);
        var result = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<ListProductsResponse>(result);

        return Ok(new PaginatedResponse<ListProductsItemResponse>
        {
            Data = response.Data,
            TotalCount = response.TotalItems,
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            Success = true,
            Message = "Products listed successfully"
        });
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListCategoriesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCategories([FromQuery] ListCategoriesRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<ListCategoriesCommand>(request);
        var result = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<ListCategoriesResponse>(result);

        return Ok(new ApiResponseWithData<ListCategoriesResponse>
        {
            Success = true,
            Message = "Categories listed successfully",
            Data = response
        });
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(PaginatedResponse<ListProductsItemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListProductsByCategory([FromRoute] string category, [FromQuery] ListProductsByCategoryRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<ListProductsByCategoryCommand>(request);
        query.Category = category;
        var result = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<ListProductsResponse>(result);

        return Ok(new PaginatedResponse<ListProductsItemResponse>
        {
            Data = response.Data,
            TotalCount = response.TotalItems,
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            Success = true,
            Message = "Products listed successfully"
        });
    }
}