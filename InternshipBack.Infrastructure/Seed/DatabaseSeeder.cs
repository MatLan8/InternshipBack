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

        // ---------------------------
        // USERS
        // ---------------------------
        var users = new List<User>
        {
            new() { FirstName = "Matas",   LastName = "Jonaitis",   Identifier = "USR-0001" },
            new() { FirstName = "Ieva",    LastName = "Petrauskaitė", Identifier = "USR-0002" },
            new() { FirstName = "Tomas",   LastName = "Kazlauskas", Identifier = "USR-0003" },
            new() { FirstName = "Greta",   LastName = "Vaitkutė",   Identifier = "USR-0004" },
            new() { FirstName = "Lukas",   LastName = "Stankevičius", Identifier = "USR-0005" },
            new() { FirstName = "Eglė",    LastName = "Zemaitė",    Identifier = "USR-0006" },
            new() { FirstName = "Domantas", LastName = "Balčiūnas",  Identifier = "USR-0007" },
            new() { FirstName = "Karolina", LastName = "Paulauskaitė", Identifier = "USR-0008" },
            new() { FirstName = "Paulius",  LastName = "Rimkus",     Identifier = "USR-0009" },
            new() { FirstName = "Monika",   LastName = "Urbonaitė",  Identifier = "USR-0010" }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
        
        var u1 = users[0].Id;
        var u2 = users[1].Id;
        var u3 = users[2].Id;
        var u4 = users[3].Id;
        var u5 = users[4].Id;
        var u6 = users[5].Id;
        var u7 = users[6].Id;
        var u8 = users[7].Id;
        var u9 = users[8].Id;
        var u10 = users[9].Id;

        // ---------------------------
        // ITEMS
        // ---------------------------
        var items = new List<Item>
        {
            // USER 1 (Matas)
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0001", PurchaseDate = DateTime.UtcNow.AddDays(-10), AssignedUserId = u1, Comment = "Assigned for development - laptop setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0002", PurchaseDate = DateTime.UtcNow.AddDays(-5),  AssignedUserId = u1, Comment = "Assigned for testing - mobile environment setup" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0003", PurchaseDate = DateTime.UtcNow.AddDays(-3),  AssignedUserId = u1, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0004", PurchaseDate = DateTime.UtcNow.AddDays(-1),  AssignedUserId = u1, Comment = "Client onboarding SIM card activation" },

            // USER 2 (Ieva)
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0005", PurchaseDate = DateTime.UtcNow.AddDays(-20), AssignedUserId = u2, Comment = "Assigned for development - phone setup" },
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0006", PurchaseDate = DateTime.UtcNow.AddDays(-15), AssignedUserId = u2, Comment = "Assigned for development - backend workstation" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0007", PurchaseDate = DateTime.UtcNow.AddDays(-8),  AssignedUserId = u2, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0008", PurchaseDate = DateTime.UtcNow.AddDays(-2),  AssignedUserId = u2, Comment = "Client onboarding SIM card activation" },

            // USER 3 (Tomas)
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0009", PurchaseDate = DateTime.UtcNow.AddDays(-30), AssignedUserId = u3, Comment = "Assigned for testing - QA environment setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0010", PurchaseDate = DateTime.UtcNow.AddDays(-25), AssignedUserId = u3, Comment = "Assigned for testing - mobile QA device" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0011", PurchaseDate = DateTime.UtcNow.AddDays(-12), AssignedUserId = u3, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0012", PurchaseDate = DateTime.UtcNow.AddDays(-6),  AssignedUserId = u3, Comment = "Client onboarding SIM card activation" },

            // USER 4 (Greta)
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0013", PurchaseDate = DateTime.UtcNow.AddDays(-18), AssignedUserId = u4, Comment = "Assigned for development - tablet UI work" },
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0014", PurchaseDate = DateTime.UtcNow.AddDays(-14), AssignedUserId = u4, Comment = "Assigned for development - frontend setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0015", PurchaseDate = DateTime.UtcNow.AddDays(-9),  AssignedUserId = u4, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0016", PurchaseDate = DateTime.UtcNow.AddDays(-4),  AssignedUserId = u4, Comment = "Client onboarding SIM card activation" },

            // USER 5 (Lukas)
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0017", PurchaseDate = DateTime.UtcNow.AddDays(-40), AssignedUserId = u5, Comment = "Assigned for development - backend services setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0018", PurchaseDate = DateTime.UtcNow.AddDays(-35), AssignedUserId = u5, Comment = "Assigned for testing - phone validation device" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0019", PurchaseDate = DateTime.UtcNow.AddDays(-22), AssignedUserId = u5, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0020", PurchaseDate = DateTime.UtcNow.AddDays(-7),  AssignedUserId = u5, Comment = "Client onboarding SIM card activation" },

            // USER 6 (Eglė)
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0021", PurchaseDate = DateTime.UtcNow.AddDays(-50), AssignedUserId = u6, Comment = "Assigned for testing - mobile QA device" },
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0022", PurchaseDate = DateTime.UtcNow.AddDays(-45), AssignedUserId = u6, Comment = "Assigned for development - dev environment setup" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0023", PurchaseDate = DateTime.UtcNow.AddDays(-28), AssignedUserId = u6, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0024", PurchaseDate = DateTime.UtcNow.AddDays(-11), AssignedUserId = u6, Comment = "Client onboarding SIM card activation" },

            // USER 7 (Domantas)
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0025", PurchaseDate = DateTime.UtcNow.AddDays(-60), AssignedUserId = u7, Comment = "Assigned for development - full stack setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0026", PurchaseDate = DateTime.UtcNow.AddDays(-55), AssignedUserId = u7, Comment = "Assigned for testing - Android QA device" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0027", PurchaseDate = DateTime.UtcNow.AddDays(-33), AssignedUserId = u7, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0028", PurchaseDate = DateTime.UtcNow.AddDays(-13), AssignedUserId = u7, Comment = "Client onboarding SIM card activation" },

            // USER 8 (Karolina)
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0029", PurchaseDate = DateTime.UtcNow.AddDays(-70), AssignedUserId = u8, Comment = "Assigned for testing - mobile regression testing" },
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0030", PurchaseDate = DateTime.UtcNow.AddDays(-65), AssignedUserId = u8, Comment = "Assigned for development - frontend system" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0031", PurchaseDate = DateTime.UtcNow.AddDays(-41), AssignedUserId = u8, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0032", PurchaseDate = DateTime.UtcNow.AddDays(-16), AssignedUserId = u8, Comment = "Client onboarding SIM card activation" },

            // USER 9 (Paulius)
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0033", PurchaseDate = DateTime.UtcNow.AddDays(-80), AssignedUserId = u9, Comment = "Assigned for development - microservices setup" },
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0034", PurchaseDate = DateTime.UtcNow.AddDays(-75), AssignedUserId = u9, Comment = "Assigned for testing - iOS QA device" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0035", PurchaseDate = DateTime.UtcNow.AddDays(-52), AssignedUserId = u9, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0036", PurchaseDate = DateTime.UtcNow.AddDays(-19), AssignedUserId = u9, Comment = "Client onboarding SIM card activation" },

            // USER 10 (Monika)
            new() { ItemType = ItemsEnum.Phone,  Identifier = "ITM-0037", PurchaseDate = DateTime.UtcNow.AddDays(-90), AssignedUserId = u10, Comment = "Assigned for testing - mobile API validation" },
            new() { ItemType = ItemsEnum.Laptop, Identifier = "ITM-0038", PurchaseDate = DateTime.UtcNow.AddDays(-85), AssignedUserId = u10, Comment = "Assigned for development - backend services" },
            new() { ItemType = ItemsEnum.Tablet, Identifier = "ITM-0039", PurchaseDate = DateTime.UtcNow.AddDays(-61), AssignedUserId = u10, Comment = "Temporary device issued for UI testing" },
            new() { ItemType = ItemsEnum.SimCard,Identifier = "ITM-0040", PurchaseDate = DateTime.UtcNow.AddDays(-21), AssignedUserId = u10, Comment = "Client onboarding SIM card activation" },
        };

        await context.Items.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}