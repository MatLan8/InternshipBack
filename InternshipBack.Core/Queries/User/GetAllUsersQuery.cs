using InternshipBack.Domain.Dtos;
using MediatR;

namespace InternshipBack.Core.Queries.User;

public class GetAllUsersQuery : IRequest<List<UserDto>>;