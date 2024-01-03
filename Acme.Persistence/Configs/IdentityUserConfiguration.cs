using Acme.Domain.Acme.Users.Identities;
using Acme.Domain.Acme.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Persistence.Configs;

//TODO: learn how to config with owns many
internal class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );

        builder.HasOne(r => r.User)
            .WithOne(x => x.IdentityUser)
            .HasForeignKey<IdentityUser>(x => x.Id)
            .IsRequired();
        
        builder.HasOne(r => r.IdentityRole)
            .WithMany()
            .HasForeignKey(x => x.IdentityRoleId)
            .IsRequired();

        builder.Property(r => r.PasswordHash).IsRequired();
        builder.Property(r => r.PasswordSalt).IsRequired();

        builder.Property(r => r.UserName).HasMaxLength(128);
        builder.Property(r => r.NormalizedUserName).HasMaxLength(128);

        builder.Property(r => r.PasswordHash).HasMaxLength(128);

        //Email is unique
        builder.HasIndex(r => r.Email).IsUnique();
        builder.Property(r => r.Email).HasMaxLength(128);
        
        builder.Property(r => r.NormalizedEmail).HasMaxLength(128);
        builder.Property(r => r.EmailConfirmed);

        builder.Property(r => r.PhoneNumber).HasMaxLength(11);
        builder.Property(r => r.PhoneNumberConfirmed);

        builder.Property(r => r.ConcurrencyStamp).IsRequired();

        builder.OwnsOne(r => r.OtpCode,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Value)
                    .HasColumnName("OtpCode")
                    .HasMaxLength(6);
                navigationBuilder.Property(address => address.ExpiredTime)
                    .HasColumnName("ExpiredTime");
            });
    }
}