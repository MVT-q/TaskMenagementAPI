using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMenagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAssigneeToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssigneedId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneedId",
                table: "Tasks",
                column: "AssigneedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssigneedId",
                table: "Tasks",
                column: "AssigneedId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssigneedId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssigneedId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssigneedId",
                table: "Tasks");
        }
    }
}
