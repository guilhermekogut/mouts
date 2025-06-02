using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

/// <summary>
/// DTO for paginated, filtered, and ordered user list requests.
/// </summary>
public class ListUsersRequest
{
    // Pagination
    [FromQuery(Name = "_page")]
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    [Range(1, 100)]
    public int Size { get; set; } = 10;

    // Ordering
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }

    // Filtering - String fields (partial or exact)
    [FromQuery(Name = "username")]
    public string? Username { get; set; }

    [FromQuery(Name = "email")]
    public string? Email { get; set; }

    [FromQuery(Name = "phone")]
    public string? Phone { get; set; }

    [FromQuery(Name = "status")]
    public string? Status { get; set; }

    [FromQuery(Name = "role")]
    public string? Role { get; set; }

    [FromQuery(Name = "name.firstname")]
    public string? NameFirstname { get; set; }

    [FromQuery(Name = "name.lastname")]
    public string? NameLastname { get; set; }

    [FromQuery(Name = "address.city")]
    public string? AddressCity { get; set; }

    [FromQuery(Name = "address.street")]
    public string? AddressStreet { get; set; }

    [FromQuery(Name = "address.zipcode")]
    public string? AddressZipcode { get; set; }

    [FromQuery(Name = "address.geolocation.lat")]
    public string? AddressGeolocationLat { get; set; }

    [FromQuery(Name = "address.geolocation.long")]
    public string? AddressGeolocationLong { get; set; }

    // Filtering - Numeric fields (exact or range)
    [FromQuery(Name = "address.number")]
    public int? AddressNumber { get; set; }

    [FromQuery(Name = "_minAddressNumber")]
    public int? MinAddressNumber { get; set; }

    [FromQuery(Name = "_maxAddressNumber")]
    public int? MaxAddressNumber { get; set; }
}