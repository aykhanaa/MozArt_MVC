using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_MozArt.Migrations
{
    /// <inheritdoc />
    public partial class CreatedProductCommitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories");

            migrationBuilder.DropIndex(
                name: "IX_BlogCategories_BlogId",
                table: "BlogCategories");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "BlogCategories");

            // ProductComments tablosu zaten mevcut, bu yüzden yeniden oluşturulmuyor.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Bu satır da silindi çünkü yukarıda tablo oluşturulmadı.
            // migrationBuilder.DropTable(name: "ProductComments");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "BlogCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_BlogId",
                table: "BlogCategories",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }
    }
}
