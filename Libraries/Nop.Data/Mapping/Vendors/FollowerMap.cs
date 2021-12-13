using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Vendors;

namespace Nop.Data.Mapping.Vendors
{
    public partial class FollowerMap : NopEntityTypeConfiguration<Follower>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Follower> builder)
        {
            builder.ToTable(nameof(Follower));
            builder.HasKey(follower => follower.Id);

            base.Configure(builder);
        }

        #endregion
    }
}
