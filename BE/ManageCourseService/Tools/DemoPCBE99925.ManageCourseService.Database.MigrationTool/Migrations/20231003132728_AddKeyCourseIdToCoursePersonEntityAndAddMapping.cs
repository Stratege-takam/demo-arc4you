using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoPCBE99925.ManageCourseService.Database.MigrationTool.Migrations
{
    public partial class AddKeyCourseIdToCoursePersonEntityAndAddMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                schema: "app",
                table: "CoursePeople",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CoursePeople_CourseId",
                schema: "app",
                table: "CoursePeople",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePeople_Courses_CourseId",
                schema: "app",
                table: "CoursePeople",
                column: "CourseId",
                principalSchema: "app",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePeople_Courses_CourseId",
                schema: "app",
                table: "CoursePeople");

            migrationBuilder.DropIndex(
                name: "IX_CoursePeople_CourseId",
                schema: "app",
                table: "CoursePeople");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "app",
                table: "CoursePeople");
        }
    }
}
