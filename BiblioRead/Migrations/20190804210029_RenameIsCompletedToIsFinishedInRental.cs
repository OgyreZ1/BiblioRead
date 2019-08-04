using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioRead.Migrations
{
    public partial class RenameIsCompletedToIsFinishedInRental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "Rentals",
                newName: "IsFinished");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "Rentals",
                newName: "IsCompleted");
        }
    }
}
