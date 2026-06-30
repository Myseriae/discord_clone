using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class DirectMessageChannelConfiguration : IEntityTypeConfiguration<DirectMessageChannel>
{
    public void Configure(EntityTypeBuilder<DirectMessageChannel> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Participants)
            .WithOne(p => p.DMChannel)
            .HasForeignKey(p => p.DMChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.DMChannel)
            .HasForeignKey(m => m.DMChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
