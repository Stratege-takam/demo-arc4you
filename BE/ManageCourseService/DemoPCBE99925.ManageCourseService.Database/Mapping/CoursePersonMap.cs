using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class CoursePersonConfiguration : IEntityTypeConfiguration<CoursePerson>
{
	public void Configure(EntityTypeBuilder<CoursePerson> modelBuilder)
	{
		modelBuilder.HasKey(p => p.Id).IsClustered(false);
		modelBuilder.Property(p => p.Id);

		modelBuilder.Ignore(p => p.PersistChange);

		modelBuilder.Property(p => p.AuditedBy).IsRequired().HasMaxLength(50);
		modelBuilder.Property(p => p.AuditedOn).IsRequired();
	}
}