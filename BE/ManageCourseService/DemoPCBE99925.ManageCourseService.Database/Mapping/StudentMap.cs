using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> modelBuilder)
	{

        #region Properties
        modelBuilder.Property(p => p.Matricule).IsRequired();
        #endregion

        #region Foreign keys
        modelBuilder.HasMany(p => p.CourseParticipations)
                   .WithOne(p => p.Student)
                   .HasForeignKey(p=> p.StudentId);
        #endregion


    }
}
