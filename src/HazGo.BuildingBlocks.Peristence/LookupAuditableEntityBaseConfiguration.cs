namespace HazGo.BuildingBlocks.Persistence.EF
{
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LookupAuditableEntityBaseConfiguration<T, TPrimaryKey> : AuditableEntityBaseConfiguration<T, TPrimaryKey>
        where T : LookupAuditableEntityBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.Code)
               .HasMaxLength(25)
               .IsRequired();
            builder.Property(t => t.Description)
               .HasMaxLength(100)
               .IsRequired();
            builder.Property(t => t.Shortdescription)
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}
