using InternshipBack.Domain.Entities;
using MediatR;

namespace InternshipBack.Core.Queries;

public class GetAllUsersQuery : IRequest<List<User>>;