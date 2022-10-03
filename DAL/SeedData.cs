using System.Linq;
using DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ForumProjectContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<ForumProjectContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            
            //
        }
    }
}