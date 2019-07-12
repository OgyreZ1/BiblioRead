using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioRead.Migrations
{
    public partial class DeleteUnneccesaryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
            name: "MyProperty",
            table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
