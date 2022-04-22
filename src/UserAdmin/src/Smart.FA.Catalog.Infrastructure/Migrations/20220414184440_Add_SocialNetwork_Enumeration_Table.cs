using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_SocialNetwork_Enumeration_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialNetwork",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetwork", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SocialNetwork",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Twitter" },
                    { 2, "Instagram" },
                    { 3, "Facebook" },
                    { 4, "Github" },
                    { 5, "LinkedIn" },
                    { 6, "Personal website" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetwork_Name",
                table: "SocialNetwork",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialNetwork");
        }
    }
}
