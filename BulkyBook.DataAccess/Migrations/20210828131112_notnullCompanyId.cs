using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkyBook.DataAccess.Migrations
{
    public partial class notnullCompanyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_tblCompany_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_tblCompany_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "tblCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_tblCompany_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_tblCompany_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "tblCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
