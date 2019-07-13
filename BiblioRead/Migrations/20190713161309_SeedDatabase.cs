using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioRead.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES ('Булгаков Михаил Афанасьевич')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES ('Толстой Лев Николаевич')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) VALUES ('Пушкин Александр Сергеевич')");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Мастер и Маргарита', 1967, (SELECT ID FROM Authors WHERE Name = 'Булгаков Михаил Афанасьевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Собачье сердце', 1925, (SELECT ID FROM Authors WHERE Name = 'Булгаков Михаил Афанасьевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Белая гвардия', 1925, (SELECT ID FROM Authors WHERE Name = 'Булгаков Михаил Афанасьевич'))");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Война и мир', 1869, (SELECT ID FROM Authors WHERE Name = 'Толстой Лев Николаевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Анна Каренина', 1873, (SELECT ID FROM Authors WHERE Name = 'Толстой Лев Николаевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Детство', 1852, (SELECT ID FROM Authors WHERE Name = 'Толстой Лев Николаевич'))");

            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Капитанская дочка', 1836, (SELECT ID FROM Authors WHERE Name = 'Пушкин Александр Сергеевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Дубровский', 1841, (SELECT ID FROM Authors WHERE Name = 'Пушкин Александр Сергеевич'))");
            migrationBuilder.Sql("INSERT INTO Books (Title, Year, AuthorId) VALUES ('Медный всадник', 1837, (SELECT ID FROM Authors WHERE Name = 'Пушкин Александр Сергеевич'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DELETE FROM Authors");
        }
    }
}
