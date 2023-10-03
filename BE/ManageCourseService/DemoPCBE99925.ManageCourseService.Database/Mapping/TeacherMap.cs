using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
	public void Configure(EntityTypeBuilder<Teacher> modelBuilder)
	{
        #region Properties
        modelBuilder.Property(p => p.HireDate).IsRequired();
        modelBuilder.Property(p => p.Salary).IsRequired();
        #endregion

        #region Foreign keys

        modelBuilder.HasMany(p => p.LeadCourses)
                .WithOne(p => p.Lead)
                .HasForeignKey(p => p.LeadId);
        #endregion
    }
}
