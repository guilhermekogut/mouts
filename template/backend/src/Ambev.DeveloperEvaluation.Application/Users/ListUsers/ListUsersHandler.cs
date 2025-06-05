using System.Linq.Dynamic.Core;

using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers
{
    /// <summary>
    /// Handler for processing ListUsersQueryCommand requests
    /// </summary>
    public class ListUsersHandler : IRequestHandler<ListUsersQueryCommand, ListUsersResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ListUsersHandler
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListUsersHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ListUsersResult> Handle(ListUsersQueryCommand request, CancellationToken cancellationToken)
        {
            var query = _userRepository.Query();

            // Filtering (partial or exact)

            if (!string.IsNullOrWhiteSpace(request.Email))
                query = ApplyStringFilter(query, "Email", request.Email);

            if (!string.IsNullOrWhiteSpace(request.Phone))
                query = ApplyStringFilter(query, "Phone", request.Phone);

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(u => u.Status.ToString() == request.Status);

            if (!string.IsNullOrWhiteSpace(request.Role))
                query = query.Where(u => u.Role.ToString() == request.Role);

            if (!string.IsNullOrWhiteSpace(request.NameFirstname))
                query = ApplyStringFilter(query, "Name.Firstname", request.NameFirstname);

            if (!string.IsNullOrWhiteSpace(request.NameLastname))
                query = ApplyStringFilter(query, "Name.Lastname", request.NameLastname);

            if (!string.IsNullOrWhiteSpace(request.AddressCity))
                query = ApplyStringFilter(query, "Address.City", request.AddressCity);

            if (!string.IsNullOrWhiteSpace(request.AddressStreet))
                query = ApplyStringFilter(query, "Address.Street", request.AddressStreet);

            if (!string.IsNullOrWhiteSpace(request.AddressZipcode))
                query = ApplyStringFilter(query, "Address.Zipcode", request.AddressZipcode);

            if (!string.IsNullOrWhiteSpace(request.AddressGeolocationLat))
                query = ApplyStringFilter(query, "Address.Geolocation.Lat", request.AddressGeolocationLat);

            if (!string.IsNullOrWhiteSpace(request.AddressGeolocationLong))
                query = ApplyStringFilter(query, "Address.Geolocation.Long", request.AddressGeolocationLong);

            if (request.AddressNumber.HasValue)
                query = query.Where(u => u.Address.Number == request.AddressNumber.Value);

            if (request.MinAddressNumber.HasValue)
                query = query.Where(u => u.Address.Number >= request.MinAddressNumber.Value);

            if (request.MaxAddressNumber.HasValue)
                query = query.Where(u => u.Address.Number <= request.MaxAddressNumber.Value);

            // Ordering
            if (!string.IsNullOrWhiteSpace(request.Order))
            {
                query = OrderByDynamic(query, request.Order);
            }
            else
            {
                query = query.OrderBy(u => u.Name.Firstname);
            }

            // Pagination
            var totalItems = query.Count();
            var users = query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .AsEnumerable();

            return await Task.FromResult(new ListUsersResult
            {
                Items = _mapper.Map<IEnumerable<ListUserItemResult>>(users),
                TotalCount = totalItems,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.Size)
            });
        }

        // Helper method for dynamic ordering
        private static IQueryable<User> OrderByDynamic(IQueryable<User> query, string order)
        {
            var cleanedOrder = string.Join(",",
            order.Split(',')
                    .Select(part => part.Trim())
                    .Where(part => !string.IsNullOrWhiteSpace(part))
            );
            return query.OrderBy(cleanedOrder);
        }

        private static IQueryable<User> ApplyStringFilter(IQueryable<User> query, string property, string value)
        {
            if (value.StartsWith("*") && value.EndsWith("*"))
                return query.Where($"{property}.Contains(@0)", value.Trim('*'));
            if (value.StartsWith("*"))
                return query.Where($"{property}.EndsWith(@0)", value.TrimStart('*'));
            if (value.EndsWith("*"))
                return query.Where($"{property}.StartsWith(@0)", value.TrimEnd('*'));
            return query.Where($"{property} == @0", value);
        }
    }
}
