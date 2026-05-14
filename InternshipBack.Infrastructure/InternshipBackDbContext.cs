using Microsoft.EntityFrameworkCore;
using InternshipBack.Domain.Entities;

namespace InternshipBack.Infrastructure;

public class InternshipBackDbContext(DbContextOptions<InternshipBackDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
}