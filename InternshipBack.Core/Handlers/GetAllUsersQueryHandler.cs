using InternshipBack.Core.Queries;
using InternshipBack.Domain.Dtos;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;

public class GetAllUsersQueryHandler(InternshipBackDbContext dbContext) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        
        return await dbContext.Users
            .Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Identifier = u.Identifier,
                Id = u.Id,
            })
            .ToListAsync(cancellationToken);
    }
}