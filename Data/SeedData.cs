using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            // Create roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            
            // Create admin user
            if (await userManager.FindByEmailAsync("admin@hotel.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@hotel.com",
                    Email = "admin@hotel.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            
            var seedRooms = new List<Room>
            {
                new Room
                {
                    RoomNumber = "101",
                    RoomType = "Single",
                    Description = "Удобна единична стая с изглед към града",
                    PricePerNight = 80.00m,
                    Capacity = 1,
                    ImageUrl = "/images/room-single.png",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "102",
                    RoomType = "Double",
                    Description = "Просторна двойна стая с двойно легло",
                    PricePerNight = 120.00m,
                    Capacity = 2,
                    ImageUrl = "/images/room-double.png",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "201",
                    RoomType = "Suite",
                    Description = "Луксозна суит стая с джакузи и балкон",
                    PricePerNight = 250.00m,
                    Capacity = 4,
                    ImageUrl = "/images/room-suite.png",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "202",
                    RoomType = "Double",
                    Description = "Модерна двойна стая с изглед към морето",
                    PricePerNight = 150.00m,
                    Capacity = 2,
                    ImageUrl = "/images/room-double-2.png",
                    IsAvailable = true
                },
                new Room
                {
                    RoomNumber = "301",
                    RoomType = "Family",
                    Description = "Семейна стая с две спални и всекидневна",
                    PricePerNight = 200.00m,
                    Capacity = 5,
                    ImageUrl = "/images/room-family.png",
                    IsAvailable = true
                }
            };

            var hasChanges = false;

            foreach (var seedRoom in seedRooms)
            {
                var existingRoom = await context.Rooms
                    .FirstOrDefaultAsync(r => r.RoomNumber == seedRoom.RoomNumber);

                if (existingRoom == null)
                {
                    context.Rooms.Add(seedRoom);
                    hasChanges = true;
                }
                else
                {
                    bool roomUpdated = false;

                    if (!string.Equals(existingRoom.ImageUrl, seedRoom.ImageUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        existingRoom.ImageUrl = seedRoom.ImageUrl;
                        roomUpdated = true;
                    }

                    if (existingRoom.Description != seedRoom.Description)
                    {
                        existingRoom.Description = seedRoom.Description;
                        roomUpdated = true;
                    }

                    if (existingRoom.PricePerNight != seedRoom.PricePerNight)
                    {
                        existingRoom.PricePerNight = seedRoom.PricePerNight;
                        roomUpdated = true;
                    }

                    if (existingRoom.Capacity != seedRoom.Capacity)
                    {
                        existingRoom.Capacity = seedRoom.Capacity;
                        roomUpdated = true;
                    }

                    if (roomUpdated)
                    {
                        context.Rooms.Update(existingRoom);
                        hasChanges = true;
                    }
                }
            }

            if (hasChanges)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}

