using Furniture.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Furniture.Models;
using System.Diagnostics;
using Furniture.Helper;
using Elfie.Serialization;
using Microsoft.Data.SqlClient;
using System;

namespace Furniture.Data.FurnitureContext;

public class AuthDB : IdentityDbContext<FurnitureUser>
{
    /*public AuthDB(Cu)
    {
        Database.Migrate();
    }*/
    private readonly ICurrentUserService currentUserService;
    public AuthDB(DbContextOptions<AuthDB> options, ICurrentUserService currentUserService)
        : base(options)
    {
        this.currentUserService = currentUserService;
        // Database.EnsureDeleted();
        Database.Migrate();
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
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
        //optionsBuilder.UseSqlServer(
        //    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Furniture;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //$"Data Source={dbHost};Initial Catalog={dbName};User ID=sa; Password=docker@12345#;Encrypt=false;Trusted_Connection=False; MultipleActiveResultSets=true"
        optionsBuilder.UseSqlServer($"Data Source={dbHost};Initial Catalog={dbName};User ID=sa; Password=docker@12345#;Encrypt=false;Trusted_Connection=False; MultipleActiveResultSets=true");
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
