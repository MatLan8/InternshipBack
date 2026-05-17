using InternshipBack.Domain.Entities;
using InternshipBack.Domain.Types;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(InternshipBackDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Users.AnyAsync())
            return;
        
        var users = new List<User>
        {
            new() { FirstName = "Matas", LastName = "Jonaitis", Identifier = "USR-0001" },
            new() { FirstName = "Ieva", LastName = "Petrauskaitė", Identifier = "USR-0002" },
            new() { FirstName = "Tomas", LastName = "Kazlauskas", Identifier = "USR-0003" },
            new() { FirstName = "Greta", LastName = "Vaitkutė", Identifier = "USR-0004" },
            new() { FirstName = "Lukas", LastName = "Stankevičius", Identifier = "USR-0005" },
            new() { FirstName = "Eglė", LastName = "Žemaitė", Identifier = "USR-0006" },
            new() { FirstName = "Domantas", LastName = "Balčiūnas", Identifier = "USR-0007" },
            new() { FirstName = "Karolina", LastName = "Paulauskaitė", Identifier = "USR-0008" },
            new() { FirstName = "Paulius", LastName = "Rimkus", Identifier = "USR-0009" },
            new() { FirstName = "Monika", LastName = "Urbonaitė", Identifier = "USR-0010" },
            new() { FirstName = "Andrius", LastName = "Petraitis", Identifier = "USR-0011" },
            new() { FirstName = "Laura", LastName = "Jankauskaitė", Identifier = "USR-0012" },
            new() { FirstName = "Marius", LastName = "Sadauskas", Identifier = "USR-0013" },
            new() { FirstName = "Agnė", LastName = "Kavaliauskaitė", Identifier = "USR-0014" },
            new() { FirstName = "Rokas", LastName = "Valančius", Identifier = "USR-0015" }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var userIds = users.Select(u => u.Id).ToList();


        var commentsByType = new Dictionary<ItemsEnum, List<string?>>
        {
            [ItemsEnum.Laptop] =
            [
                "Laptop configured for full-stack development",
                "Dev environment setup completed on laptop",
                "Used for backend API development",
                "Performance testing on staging via laptop",
                "Hotfix deployment validation machine",
                null,
                ""
            ],

            [ItemsEnum.Phone] =
            [
                "Android QA testing device",
                "iOS app regression testing phone",
                "Used for mobile API validation",
                "Carrier testing device for app calls",
                "Temporary replacement phone for testing",
                null,
                ""
            ],

            [ItemsEnum.Tablet] =
            [
                "UI/UX layout testing on tablet screen",
                "Tablet used for frontend responsiveness checks",
                "Prototype testing device for design review",
                "Used during sprint demo presentation",
                null,
                ""
            ],

            [ItemsEnum.SimCard] =
            [
                "SIM card for network integration testing",
                "Used for client onboarding verification",
                "Mobile data testing SIM provisioned",
                "Temporary SIM for SMS verification flow",
                null,
                ""
            ]
        };

        var rand = new Random();
        var items = new List<Item>();

        int itemCounter = 1;


        foreach (var userId in userIds)
        {
            int itemCount = rand.Next(2, 8);

            for (int i = 0; i < itemCount; i++)
            {
                var type = (ItemsEnum)rand.Next(0, 4);
                var commentPool = commentsByType[type];

                var item = new Item
                {
                    ItemType = type,
                    Identifier = $"ITM-{itemCounter:D4}",
                    PurchaseDate = DateTime.UtcNow.AddDays(-rand.Next(1, 120)),
                    AssignedUserId = userId,
                    Comment = commentPool[rand.Next(commentPool.Count)],
                    IsDeleted = false
                };

                items.Add(item);
                itemCounter++;
            }
        }
        
        while (items.Count < 60)
        {
            var userId = userIds[rand.Next(userIds.Count)];
            var type = (ItemsEnum)rand.Next(0, 4);
            var commentPool = commentsByType[type];

            items.Add(new Item
            {
                ItemType = type,
                Identifier = $"ITM-{itemCounter:D4}",
                PurchaseDate = DateTime.UtcNow.AddDays(-rand.Next(1, 120)),
                AssignedUserId = userId,
                Comment = commentPool[rand.Next(commentPool.Count)],
                IsDeleted = false
            });

            itemCounter++;
        }

        await context.Items.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}