using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class SeedData
    {
        public static void EnsurePopulated(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new UserRole() { Id = 2, Name = "Moderator", NormalizedName = "MODERATOR" },
                new UserRole() { Id = 3, Name = "User", NormalizedName = "USER" }
            );

            var passwordHasher = new PasswordHasher<User>();
            var users = new List<User>()

            {
                new()
                {
                    Id = 1,
                    UserName = "LevMyshkin",
                    NormalizedUserName = "LevMyshkin".ToUpper(),
                    FirstName = "Lev",
                    LastName = "Myshkin",
                    Email = "lev.myshkin@outlook.com",
                    NormalizedEmail = "lev.myshkin@outlook.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, "Myshkin0101"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },

                new()
                {
                    Id = 2,
                    UserName = "RodionRaskolnikov",
                    NormalizedUserName = "RodionRaskolnikov".ToUpper(),
                    FirstName = "Rodion",
                    LastName = "Raskolnikov",
                    Email = "rodion.raskolnikov@yandex.ru",
                    NormalizedEmail = "rodion.raskolnikov@yandex.ru".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, "RodRask0243"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },

                new()
                {
                    Id = 3,
                    UserName = "GeorgeWinterbourne",
                    NormalizedUserName = "GeorgeWinterbourne".ToUpper(),
                    FirstName = "George",
                    LastName = "Winterbourne",
                    Email = "george.winterbourne@gmail.com",
                    NormalizedEmail = "george.winterbourne@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, "Winter123Qwerty"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
            };
            modelBuilder.Entity<User>().HasData(users);

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 3, RoleId = 3 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "C# Programming Language" },
                new Category() { Id = 2, Name = "Python Programming Language" }
            );

            modelBuilder.Entity<Topic>().HasData(
                new Topic()
                {
                    Id = 1, Title = "Entity Framework",
                    CreateTime = DateTime.Now.AddDays(2),
                    CategoryId = 1, UserId = 2
                }
            );
            
            modelBuilder.Entity<Message>().HasData(
                new Message()
                {
                    Id = 1, Text = "The Include() method works quite well for Lists on objects. But what if I need to go two levels deep? Can I do an Include() on that property as well? Or how can I get that property to fully load?", CreateTime = DateTime.Now.AddDays(3),
                    TopicId = 1, UserId = 2
                },
                new Message()
                {
                    Id = 2, Text = "Make sure to add using System.Data.Entity; to get the version of Include that takes in a lambda.", CreateTime = DateTime.Now.AddDays(4),
                    TopicId = 1, UserId = 3
                }
            );
        }
    }
}