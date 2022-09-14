using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Customers
{
    public partial class CustomerActivationCodeMap : NopEntityTypeConfiguration<CustomerActivationCode>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerActivationCode> builder)
        {
            builder.ToTable(nameof(CustomerActivationCode));
            builder.HasKey(customer => customer.Id);

            builder.Property(customer => customer.ActivationCode).HasMaxLength(255);

            builder.HasOne(customer => customer.Customer)
                .WithMany()
                .HasForeignKey(customer => customer.CustomerId);

            base.Configure(builder);
        }
    }
}
