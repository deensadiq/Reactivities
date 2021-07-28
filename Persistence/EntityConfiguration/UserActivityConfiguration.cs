using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration
{
    public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            builder.HasKey(ua => new { ua.AppUserId, ua.ActivityId });

            builder.HasOne(u => u.AppUser)
            .WithMany(a => a.UserActivities)
            .HasForeignKey(u => u.AppUserId);

            builder.HasOne(a => a.Activity)
            .WithMany(u => u.UserActivities)
            .HasForeignKey(a => a.ActivityId);
        }
    }
}