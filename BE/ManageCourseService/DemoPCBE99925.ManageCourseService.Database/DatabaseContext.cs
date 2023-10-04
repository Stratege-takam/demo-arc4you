using EG.DemoPCBE99925.ManageCourseService.Domain;
using Microsoft.EntityFrameworkCore;

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
		modelBuilder.ApplyConfiguration(new CoursePersonConfiguration());
		modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
		modelBuilder.ApplyConfiguration(new PersonConfiguration());
		modelBuilder.ApplyConfiguration(new StudentConfiguration());
		modelBuilder.ApplyConfiguration(new TeacherConfiguration());
	}
	public DbSet<Course> Courses { get; set; }
	public DbSet<CoursePerson> CoursePeople { get; set; }
	public DbSet<Participant> Participants { get; set; }
	public DbSet<Person> People { get; set; }
	public DbSet<Student> Students { get; set; }
	public DbSet<Teacher> Teachers { get; set; }
}
