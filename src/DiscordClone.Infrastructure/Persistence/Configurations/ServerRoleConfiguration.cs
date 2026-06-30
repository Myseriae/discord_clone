using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ServerRoleConfiguration : IEntityTypeConfiguration<ServerRole>
{
    public void Configure(EntityTypeBuilder<ServerRole> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Color).HasMaxLength(9); // e.g. #RRGGBBAA
        builder.Property(r => r.IsDefault).HasDefaultValue(false);

        // At most one default ("@everyone") role per server — enforced by a partial unique index.
        builder.HasIndex(r => r.ServerId)
            .IsUnique()
            .HasFilter("\"IsDefault\"");

        builder.HasMany(r => r.MemberRoles)
            .WithOne(mr => mr.ServerRole)
            .HasForeignKey(mr => mr.ServerRoleId)
            // Restrict (not Cascade): ServerMember already cascades into ServerMemberRole,
            // and ServerRole also descends from Server — two cascade paths would conflict.
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.ChannelPermissions)
            .WithOne(cp => cp.Role)
            .HasForeignKey(cp => cp.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
