using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> modelBuilder)
	{
        #region Audi properties
        modelBuilder.HasKey(p => p.Id).IsClustered(false);
		modelBuilder.Property(p => p.Id);
    //    modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Ignore(p => p.PersistChange);

		modelBuilder.Property(p => p.AuditedBy).IsRequired().HasMaxLength(50);
		modelBuilder.Property(p => p.AuditedOn).IsRequired();
        #endregion

        #region Properties
        modelBuilder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        modelBuilder.Property(p => p.FirstName).IsRequired(false).HasMaxLength(100);
        #endregion

        #region Foreign keys
        modelBuilder.HasMany(p => p.Courses)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId);
        #endregion
    }
}
