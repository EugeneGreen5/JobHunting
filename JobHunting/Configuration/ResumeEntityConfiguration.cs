using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunting.Configuration;

public class ResumeEntityConfiguration : BaseEntityConfiguration<Resume>
{
    public override void ConfigEntity(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable("resume");

    }
}
