using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_MozArt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogComments");
        }
    }
}
