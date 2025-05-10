using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsRequiredOfDriverLicensePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "driver_license_image_path",
                table: "delivery_men",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "delivery_men",
                keyColumn: "driver_license_image_path",
                keyValue: null,
                column: "driver_license_image_path",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "driver_license_image_path",
                table: "delivery_men",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
