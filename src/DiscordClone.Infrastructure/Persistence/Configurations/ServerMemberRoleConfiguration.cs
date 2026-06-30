using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ServerMemberRoleConfiguration : IEntityTypeConfiguration<ServerMemberRole>
{
    public void Configure(EntityTypeBuilder<ServerMemberRole> builder)
    {
        builder.HasKey(mr => mr.Id);
        builder.HasIndex(mr => new { mr.ServerMemberId, mr.ServerRoleId }).IsUnique();
        // FK relationships are defined from the ServerMember and ServerRole sides.
    }
}
