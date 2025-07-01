using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_MozArt.Migrations
{
    /// <inheritdoc />
    public partial class IsHoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHover",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHover",
                table: "ProductImages");
        }
    }
}
