using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_BlackList_User_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlackListedUser",
                schema: "Cfa",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListedUser", x => new { x.UserId, x.ApplicationType })
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackListedUser",
                schema: "Cfa");
        }
    }
}
