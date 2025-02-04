using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Id)
                .IsUnique();

            builder.Property(user => user.Id).HasConversion(
                id => id.Value,
                value => new UserId(value));

            builder.Property(user => user.Username)
                .HasMaxLength(Username.MaxLength)
                .HasConversion(
                    username => username.Value,
                    value => Username.Create(value).Value);

            builder.HasIndex(user => user.Username)
                .IsUnique();

            builder.Property(user => user.Name)
                .HasMaxLength(Name.MaxLength)
                .HasConversion(
                    name => name.Value,
                    value => Name.Create(value).Value);

            builder.Property(user => user.Email)
                .HasMaxLength(Email.MaxLength)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value);

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.IconUri)
                .HasMaxLength(ImageUri.MaxLength)
                .HasConversion(
                    iconUri => iconUri.Value,
                    value => ImageUri.Create(value).Value);

            builder
                .HasMany(user => user.Chats)
                .WithMany(chat => chat.Users)
                .UsingEntity(join => join.ToTable(ManyToManyTables.UsersChats));
        }
    }
}
