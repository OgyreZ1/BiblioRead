using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioRead.Migrations
{
    public partial class SeedDatabaseAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Authors");

            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES (N'Булгаков Михаил Афанасьевич')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES (N'Толстой Лев Николаевич')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES (N'Пушкин Александр Сергеевич')");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Мастер и Маргарита', 1967, (SELECT ID FROM Authors WHERE Name = N'Булгаков Михаил Афанасьевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Собачье сердце', 1925, (SELECT ID FROM Authors WHERE Name = N'Булгаков Михаил Афанасьевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Белая гвардия', 1925, (SELECT ID FROM Authors WHERE Name = N'Булгаков Михаил Афанасьевич'))");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Война и мир', 1869, (SELECT ID FROM Authors WHERE Name = N'Толстой Лев Николаевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Анна Каренина', 1873, (SELECT ID FROM Authors WHERE Name = N'Толстой Лев Николаевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Детство', 1852, (SELECT ID FROM Authors WHERE Name = N'Толстой Лев Николаевич'))");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Капитанская дочка', 1836, (SELECT ID FROM Authors WHERE Name = N'Пушкин Александр Сергеевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Дубровский', 1841, (SELECT ID FROM Authors WHERE Name = N'Пушкин Александр Сергеевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES (N'Медный всадник', 1837, (SELECT ID FROM Authors WHERE Name = N'Пушкин Александр Сергеевич'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Authors");
        }
    }
}
