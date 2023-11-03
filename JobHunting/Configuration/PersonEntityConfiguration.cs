using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunting.Configuration;

public class PersonEntityConfiguration : BaseEntityConfiguration<Person>
{
    public override void ConfigEntity(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("person");

        builder.Property(x => x.Phone)
            .HasColumnName("mobile")
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(x => x.Name)
            .HasColumnType("nchar(100)");

        builder.Ignore(x => x.StartSession);

        builder.HasIndex(x => x.City);

        builder.HasMany(c => c.Resumes)
            .WithOne()
            .HasForeignKey(c => c.Id);
    }
}
