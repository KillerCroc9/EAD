using Furniture.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Furniture.Models;
using System.Diagnostics;
using Furniture.Helper;

namespace Furniture.Data.FurnitureContext;

public class AuthDB : IdentityDbContext<FurnitureUser>
{
    /*public AuthDB()
    {
        Database.Migrate();
    }*/
    private readonly ICurrentUserService currentUserService;
    public AuthDB(DbContextOptions<AuthDB> options, ICurrentUserService currentUserService)
        : base(options)
    {
        this.currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Furniture.Models.Product> Product { get; set; } = default!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server = localhost, 1440; Database = master; User = sa; Password =
Docker123!;");
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessSave();
        return base.SaveChangesAsync(cancellationToken);
    }
    private void ProcessSave()
    {
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is Entity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = DateTime.Now;
                            entity.ModifiedDate = DateTime.Now;
                            entity.CreatedByUser = currentUserService.GetCurrentUsername();
                            entity.ModifiedByUser = currentUserService.GetCurrentUsername();
                            break;
                        case EntityState.Modified:
                            entity.ModifiedDate = DateTime.Now;
                            entity.ModifiedByUser = currentUserService.GetCurrentUsername();
                            entry.Property("CreatedDate").IsModified = false;
                            entry.Property("CreatedByUser").IsModified = false;
                            break;
                    }
                }
            }
        }
    }
}
