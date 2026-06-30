using DiscordClone.Domain.Entities;
using DiscordClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ServerMemberConfiguration : IEntityTypeConfiguration<ServerMember>
{
    public void Configure(EntityTypeBuilder<ServerMember> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.ServerId, m.UserId }).IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MemberRoles)
            .WithOne(mr => mr.ServerMember)
            .HasForeignKey(mr => mr.ServerMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
