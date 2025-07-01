using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_MozArt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Supports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Supports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Supports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
