using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_MozArt.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComment_AspNetUsers_AppUserId",
                table: "BlogComment");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogComment_Blogs_BlogId",
                table: "BlogComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogComment",
                table: "BlogComment");

            migrationBuilder.RenameTable(
                name: "BlogComment",
                newName: "BlogComments");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComment_BlogId",
                table: "BlogComments",
                newName: "IX_BlogComments_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComment_AppUserId",
                table: "BlogComments",
                newName: "IX_BlogComments_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogComments",
                table: "BlogComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogComments",
                table: "BlogComments");

            migrationBuilder.RenameTable(
                name: "BlogComments",
                newName: "BlogComment");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_BlogId",
                table: "BlogComment",
                newName: "IX_BlogComment_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComment",
                newName: "IX_BlogComment_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogComment",
                table: "BlogComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComment_AspNetUsers_AppUserId",
                table: "BlogComment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComment_Blogs_BlogId",
                table: "BlogComment",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
