using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarea1_Workshop.Migrations
{
    /// <inheritdoc />
    public partial class Fixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subjects",
                table: "Teachers",
                newName: "Subject");

            migrationBuilder.AlterColumn<string>(
                name: "Career",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Teachers",
                newName: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "Career",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
