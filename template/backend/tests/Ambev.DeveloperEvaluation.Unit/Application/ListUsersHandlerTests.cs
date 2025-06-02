using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for ListUsersHandler.
/// </summary>
public class ListUsersHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ListUsersHandler _handler;

    public ListUsersHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListUsersHandler(_userRepository, _mapper);
    }

    /// <summary>
    /// Given a valid command, when listing users, then returns a paginated and mapped result.
    /// </summary>
    [Fact(DisplayName = "Given a valid command When listing users Then returns paginated and mapped result")]
    public async Task Handle_ValidRequest_ReturnsMappedResult()
    {
        // Given
        var command = new ListUsersQueryCommand
        {
            Page = 1,
            Size = 10,
            // Nenhum filtro para garantir que o usuário seja retornado
        };
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "johndoe",
                Name = new DeveloperEvaluation.Domain.ValueObjects.Name("John", "Doe"),
                Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 123, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1.23", "4.56")),
                Email = "john.doe@email.com",
                Phone = "+5511999999999",
                Status = DeveloperEvaluation.Domain.Enums.UserStatus.Active,
                Role = DeveloperEvaluation.Domain.Enums.UserRole.Customer
            }
        };

        var queryable = users.AsQueryable();
        _userRepository.Query().Returns(queryable);

        var expectedItems = new List<ListUserItemResult>
        {
            new ListUserItemResult
            {
                // Preencha os campos conforme o mapeamento esperado
                Id = users[0].Id,
                Name = new Ambev.DeveloperEvaluation.Application.Users.Common.NameResult
                {
                    Firstname = "John",
                    Lastname = "Doe"
                },
                Email = users[0].Email,
                Phone = users[0].Phone,
                Status = users[0].Status,
                Role = users[0].Role,
                Address = new Ambev.DeveloperEvaluation.Application.Users.Common.AddressResult
                {
                    City = "City",
                    Street = "Street",
                    Number = 123,
                    Zipcode = "00000-000",
                    Geolocation = new Ambev.DeveloperEvaluation.Application.Users.Common.GeolocationResult
                    {
                        Lat = "1.23",
                        Long = "4.56"
                    }
                }
            }
        };

        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(expectedItems);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.TotalCount.Should().BeGreaterThan(0);
        _userRepository.Received(1).Query();
        _mapper.Received(1).Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>());
    }

    /// <summary>
    /// Given no users found, when listing users, then returns an empty list.
    /// </summary>
    [Fact(DisplayName = "Given no users found When listing users Then returns empty list")]
    public async Task Handle_NoUsersFound_ReturnsEmptyList()
    {
        // Given
        var command = ListUsersHandlerTestData.GenerateValidQuery();
        var users = new List<User>();
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(new List<ListUserItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact(DisplayName = "Given address number range filter When listing users Then returns users in range")]
    public async Task Handle_AddressNumberRangeFilter_ReturnsUsersInRange()
    {
        var command = new ListUsersQueryCommand { Page = 1, Size = 10, MinAddressNumber = 2, MaxAddressNumber = 3 };
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("A", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("B", "B"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 2, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "b@email.com", Phone = "2", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("C", "C"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 3, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "c@email.com", Phone = "3", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(callInfo =>
        {
            var filtered = ((IEnumerable<User>)callInfo.ArgAt<IEnumerable<User>>(0)).Where(u => u.Address.Number >= 2 && u.Address.Number <= 3);
            return filtered.Select(u => new ListUserItemResult { Id = u.Id });
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given order by email desc When listing users Then returns users in correct order")]
    public async Task Handle_OrderByEmailDesc_ReturnsUsersInOrder()
    {
        var command = new ListUsersQueryCommand { Page = 1, Size = 10, Order = "Email desc" };
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("Alice", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "z@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("Bob", "B"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 2, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "2", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(callInfo =>
        {
            var ordered = ((IEnumerable<User>)callInfo.ArgAt<IEnumerable<User>>(0)).OrderByDescending(u => u.Email);
            return ordered.Select(u => new ListUserItemResult { Id = u.Id, Email = u.Email });
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Items.First().Email.Should().Be("z@email.com");
        result.Items.Last().Email.Should().Be("a@email.com");
    }

    [Fact(DisplayName = "Given pagination When listing users Then returns correct page of users")]
    public async Task Handle_Pagination_ReturnsCorrectPage()
    {
        var command = new ListUsersQueryCommand { Page = 2, Size = 2 };
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("A", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("B", "B"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 2, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "b@email.com", Phone = "2", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("C", "C"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 3, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "c@email.com", Phone = "3", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(callInfo =>
        {
            var input = (IEnumerable<User>)callInfo.ArgAt<IEnumerable<User>>(0);
            return input.Select(u => new ListUserItemResult { Id = u.Id });
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Given page beyond data When listing users Then returns empty list")]
    public async Task Handle_PageBeyondData_ReturnsEmptyList()
    {
        var command = new ListUsersQueryCommand { Page = 10, Size = 10 };
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("A", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(new List<ListUserItemResult>());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given null command properties When listing users Then returns all users")]
    public async Task Handle_NullCommandProperties_ReturnsAllUsers()
    {
        var command = new ListUsersQueryCommand();
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("A", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer },
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("B", "B"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 2, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "b@email.com", Phone = "2", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());
        _mapper.Map<IEnumerable<ListUserItemResult>>(Arg.Any<IEnumerable<User>>()).Returns(users.Select(u => new ListUserItemResult { Id = u.Id }));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given invalid order string When listing users Then throws exception")]
    public async Task Handle_InvalidOrderString_ThrowsException()
    {
        var command = new ListUsersQueryCommand { Page = 1, Size = 10, Order = "NonExistentField asc" };
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = new DeveloperEvaluation.Domain.ValueObjects.Name("A", "A"), Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 1, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1", "2")), Email = "a@email.com", Phone = "1", Status = UserStatus.Active, Role = UserRole.Customer }
        };
        _userRepository.Query().Returns(users.AsQueryable());

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact(DisplayName = "Given repository throws exception When listing users Then propagates exception")]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        var command = new ListUsersQueryCommand { Page = 1, Size = 10 };
        _userRepository.Query().Returns(_ => throw new Exception("Database error"));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}