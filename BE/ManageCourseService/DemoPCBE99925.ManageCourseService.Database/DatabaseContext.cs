using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

public partial class DatabaseContext : DbContext
{
	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//Custom code conventions in OnModelCreating
		//define the default schema
		modelBuilder.HasDefaultSchema("app");
		modelBuilder.ApplyConfiguration(new CourseConfiguration());
	}
	public DbSet<Course> Courses { get; set; }
}