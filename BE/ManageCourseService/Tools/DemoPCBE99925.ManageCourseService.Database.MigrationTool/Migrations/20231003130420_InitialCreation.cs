using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoPCBE99925.ManageCourseService.Database.MigrationTool.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Peoples",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peoples", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Courses_Peoples_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "app",
                        principalTable: "Peoples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Matricule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Students_Peoples_Id",
                        column: x => x.Id,
                        principalSchema: "app",
                        principalTable: "Peoples",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Teachers_Peoples_Id",
                        column: x => x.Id,
                        principalSchema: "app",
                        principalTable: "Peoples",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoursePeople",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePeople", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CoursePeople_Teachers_LeadId",
                        column: x => x.LeadId,
                        principalSchema: "app",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoursePersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Participants_CoursePeople_CoursePersonId",
                        column: x => x.CoursePersonId,
                        principalSchema: "app",
                        principalTable: "CoursePeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "app",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursePeople_LeadId",
                schema: "app",
                table: "CoursePeople",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OwnerId",
                schema: "app",
                table: "Courses",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CoursePersonId",
                schema: "app",
                table: "Participants",
                column: "CoursePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_StudentId",
                schema: "app",
                table: "Participants",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Participants",
                schema: "app");

            migrationBuilder.DropTable(
                name: "CoursePeople",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Peoples",
                schema: "app");
        }
    }
}
