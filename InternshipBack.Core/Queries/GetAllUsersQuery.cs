using InternshipBack.Domain.Dtos;
using MediatR;

namespace InternshipBack.Core.Queries;

public class GetAllUsersQuery : IRequest<List<UserDto>>;