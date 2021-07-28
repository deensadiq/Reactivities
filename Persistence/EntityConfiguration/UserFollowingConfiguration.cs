using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration
{
    public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
    {
        public void Configure(EntityTypeBuilder<UserFollowing> builder)
        {
            builder.HasKey(a => new { a.TargetId, a.ObserverId });

            builder.HasOne(a => a.Target)
            .WithMany(u => u.Followers)
            .HasForeignKey(a => a.TargetId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Observer)
            .WithMany(u => u.Followings)
            .HasForeignKey(a => a.ObserverId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}