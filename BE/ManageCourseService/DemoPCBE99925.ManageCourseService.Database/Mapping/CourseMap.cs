using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
	public void Configure(EntityTypeBuilder<Course> modelBuilder)
	{
        #region Audi properties
        modelBuilder.HasKey(p => p.Id).IsClustered(false);
		modelBuilder.Property(p => p.Id);
       // modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Ignore(p => p.PersistChange);

		modelBuilder.Property(p => p.AuditedBy).IsRequired().HasMaxLength(50);
		modelBuilder.Property(p => p.AuditedOn).IsRequired();
        #endregion

        #region Properties
        modelBuilder.Property(p => p.Name).IsRequired().HasMaxLength(100);
		modelBuilder.Property(p => p.Unity).IsRequired();
		modelBuilder.Property(p => p.OwnerId).IsRequired();
		modelBuilder.Property(p => p.Description).IsRequired(false);
        #endregion Properties

        #region Foreign keys
        modelBuilder.HasMany(p => p.CoursePeople)
                   .WithOne(p => p.Course)
                   .HasForeignKey(p => p.CourseId);
        #endregion Foreign keys
    }
}
