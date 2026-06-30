using DiscordClone.Domain.Entities;
using DiscordClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ServerConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.IconUrl).HasMaxLength(2048);
        builder.Property(s => s.InviteCode).IsRequired().HasMaxLength(32);
        builder.HasIndex(s => s.InviteCode).IsUnique();

        // Owner is an ApplicationUser (Infrastructure); no navigation on the Domain side.
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Channels)
            .WithOne(c => c.Server)
            .HasForeignKey(c => c.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Members)
            .WithOne(m => m.Server)
            .HasForeignKey(m => m.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Roles)
            .WithOne(r => r.Server)
            .HasForeignKey(r => r.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
