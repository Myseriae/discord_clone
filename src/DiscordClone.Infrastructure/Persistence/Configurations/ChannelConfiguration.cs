using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Infrastructure.Persistence.Configurations;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(20);

        // Self-referencing category: a category is a Channel with Type=Category.
        builder.HasOne(c => c.Category)
            .WithMany(c => c.ChildChannels)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Channel)
            .HasForeignKey(m => m.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Permissions)
            .WithOne(p => p.Channel)
            .HasForeignKey(p => p.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
