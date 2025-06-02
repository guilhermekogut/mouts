namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers
{
    public class ListUsersResult
    {
        /// <summary>
        /// List of user items (DTOs).
        /// </summary>
        public IEnumerable<ListUserItemResult> Items { get; set; } = [];

        /// <summary>
        /// Total number of items matching the query.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; set; }
    }
}
