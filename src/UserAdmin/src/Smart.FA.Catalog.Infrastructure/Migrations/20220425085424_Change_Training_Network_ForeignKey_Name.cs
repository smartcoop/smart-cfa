using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Change_Training_Network_ForeignKey_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerSocialNetworkId",
                table: "TrainerSocialNetwork",
                newName: "SocialNetworkId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainerSocialNetwork_TrainerSocialNetworkId",
                table: "TrainerSocialNetwork",
                newName: "IX_TrainerSocialNetwork_SocialNetworkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialNetworkId",
                table: "TrainerSocialNetwork",
                newName: "TrainerSocialNetworkId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainerSocialNetwork_SocialNetworkId",
                table: "TrainerSocialNetwork",
                newName: "IX_TrainerSocialNetwork_TrainerSocialNetworkId");
        }
    }
}
