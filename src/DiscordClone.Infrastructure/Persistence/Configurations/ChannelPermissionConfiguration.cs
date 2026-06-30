using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ChannelPermissionConfiguration : IEntityTypeConfiguration<ChannelPermission>
{
    public void Configure(EntityTypeBuilder<ChannelPermission> builder)
    {
        builder.HasKey(cp => cp.Id);
        builder.HasIndex(cp => new { cp.ChannelId, cp.RoleId }).IsUnique();
        // FK relationships are defined from the Channel and ServerRole sides.
    }
}
