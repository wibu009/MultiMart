using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Infrastructure.Identity.Token;

namespace MultiMart.Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserRefreshTokenConfig : IEntityTypeConfiguration<ApplicationUserRefreshToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRefreshToken> builder)
    {
        builder
            .ToTable("UserRefreshTokens", SchemaNames.Identity)
            .IsMultiTenant();

        builder
            .Property(t => t.Token)
            .HasMaxLength(256);

        builder.HasOne(t => t.ApplicationUser)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}