using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunting.Configuration;

public class PersonEntityConfiguration : BaseEntityConfiguration<Person>
{
    public override void ConfigEntity(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("person");

        builder.HasMany(c => c.Resumes)
            .WithOne()
            .HasForeignKey(c => c.Id);
    }
}
