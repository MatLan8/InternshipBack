using InternshipBack.Core.Queries;
using InternshipBack.Domain.Entities;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;

public class GetAllUsersQueryHandler(InternshipBackDbContext dbContext) : IRequestHandler<GetAllUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        
        return await dbContext.Users
            .ToListAsync(cancellationToken);
    }
}