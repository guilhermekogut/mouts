namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers
{

    /// <summary>
    /// DTO for paginated user list response.
    /// </summary>
    public class ListUsersResponse
    {
        public IEnumerable<ListUsersItemResponse> Data { get; set; } = [];
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
