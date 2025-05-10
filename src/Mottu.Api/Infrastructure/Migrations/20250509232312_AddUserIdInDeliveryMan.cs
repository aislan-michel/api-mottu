using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdInDeliveryMan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rents_deliveryMen_delivery_man_id",
                table: "rents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_deliveryMen",
                table: "deliveryMen");

            migrationBuilder.RenameTable(
                name: "deliveryMen",
                newName: "delivery_men");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "delivery_men",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_delivery_men",
                table: "delivery_men",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_men_user_id",
                table: "delivery_men",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_delivery_men_AspNetUsers_user_id",
                table: "delivery_men",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rents_delivery_men_delivery_man_id",
                table: "rents",
                column: "delivery_man_id",
                principalTable: "delivery_men",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_delivery_men_AspNetUsers_user_id",
                table: "delivery_men");

            migrationBuilder.DropForeignKey(
                name: "FK_rents_delivery_men_delivery_man_id",
                table: "rents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_delivery_men",
                table: "delivery_men");

            migrationBuilder.DropIndex(
                name: "IX_delivery_men_user_id",
                table: "delivery_men");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "delivery_men");

            migrationBuilder.RenameTable(
                name: "delivery_men",
                newName: "deliveryMen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deliveryMen",
                table: "deliveryMen",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rents_deliveryMen_delivery_man_id",
                table: "rents",
                column: "delivery_man_id",
                principalTable: "deliveryMen",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
