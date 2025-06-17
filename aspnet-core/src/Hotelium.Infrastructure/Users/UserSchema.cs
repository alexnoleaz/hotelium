using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotelium.Users.Values;

namespace Hotelium.Users;

public class UserSchema : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(cfg => cfg.ToTable("UserRoles"));


        builder.Property(u => u.Name).HasMaxLength(UserConsts.MaxNameLength);
        builder.Property(u => u.Surname).HasMaxLength(UserConsts.MaxSurnameLength);
        builder.Property(u => u.Email).HasMaxLength(UserConsts.MaxEmailLength);
        builder.Property(u => u.Password).HasMaxLength(UserConsts.MaxPasswordLength);
        builder.Property(u => u.Phone).HasMaxLength(UserConsts.MaxPhoneLength);

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(AddressConsts.MaxStreetLength);
            address.Property(a => a.City).HasColumnName("City").HasMaxLength(AddressConsts.MaxCityLength);
        });
    }
}