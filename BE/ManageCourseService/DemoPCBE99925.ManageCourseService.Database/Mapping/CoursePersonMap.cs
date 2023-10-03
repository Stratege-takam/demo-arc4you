using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class CoursePersonConfiguration : IEntityTypeConfiguration<CoursePerson>
{
	public void Configure(EntityTypeBuilder<CoursePerson> modelBuilder)
	{
        #region Audi Properties
        modelBuilder.HasKey(p => p.Id).IsClustered(false);
		modelBuilder.Property(p => p.Id);
       // modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Ignore(p => p.PersistChange);

		modelBuilder.Property(p => p.AuditedBy).IsRequired().HasMaxLength(50);
		modelBuilder.Property(p => p.AuditedOn).IsRequired();
        #endregion Audi Properties

        #region Properties
        modelBuilder.Property(p => p.StartDate).IsRequired();
        modelBuilder.Property(p => p.EndDate).IsRequired();
        modelBuilder.Property(p => p.LeadId).IsRequired();
        modelBuilder.Property(p => p.CourseId).IsRequired();
        #endregion Properties

        #region Foreign keys
        modelBuilder.HasMany(p => p.Participants)
                   .WithOne(p => p.CoursePerson)
                   .HasForeignKey(p => p.CoursePersonId);
        #endregion Foreign keys

    }
}
