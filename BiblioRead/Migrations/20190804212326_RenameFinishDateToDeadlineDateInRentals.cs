using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioRead.Migrations
{
    public partial class RenameFinishDateToDeadlineDateInRentals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndingDate",
                table: "Rentals",
                newName: "DeadlineDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeadlineDate",
                table: "Rentals",
                newName: "EndingDate");
        }
    }
}
