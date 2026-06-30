using DiscordClone.Domain.Entities;
using DiscordClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class DMParticipantConfiguration : IEntityTypeConfiguration<DMParticipant>
{
    public void Configure(EntityTypeBuilder<DMParticipant> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.DMChannelId, p.UserId }).IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
