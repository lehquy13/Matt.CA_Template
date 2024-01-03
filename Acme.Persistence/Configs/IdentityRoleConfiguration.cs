using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Persistence.Configs;

// internal class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
// {
//     public void Configure(EntityTypeBuilder<IdentityRole> builder)
//     {
//         builder.ToTable("IdentityRole");
//         builder.HasKey(r => r.Id);
//         builder.Property(r => r.Name).IsRequired().HasMaxLength(128);
//     }
// }