using DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Injections
{
    public static class MigrateDb
    {
        public static void Migrate(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            
            if (serviceScope == null) return;
            var context = serviceScope.ServiceProvider.GetRequiredService<ForumProjectContext>();
            context.Database.Migrate();
        }
    }
}