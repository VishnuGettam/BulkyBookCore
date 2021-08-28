using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkyBook.DataAccess.Migrations
{
    public partial class CoverTypeStoredProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create procedure GetAllCoverType
                                    As
                                    Begin
                                    Select * from tblCoverType
                                    End
                                 ");

            migrationBuilder.Sql(@"Create procedure GetCoverType ( @Id int )
                                                As
                                                Begin
                                                Select * from tblCoverType where Id = @Id
                                                End
                                            ");
            migrationBuilder.Sql(@"Create procedure AddCoverType(
                                        @name varchar(max)
                                        )
                                        As
                                        Begin
                                        Insert into tblCoverType values (@name)
                                        End
                                        ");
            migrationBuilder.Sql(@"Create procedure UpdateCoverType(
                                        @id int,
                                        @name varchar(100)
                                        )
                                        As
                                        Begin
                                        Update tblCoverType set Name= @name where Id=@id
                                        End");

            migrationBuilder.Sql(@"Create proc DeleteCoverType
                                        (
                                        @id int
                                        )
                                        As
                                        Begin
                                        Delete from tblCoverType where Id=@id
                                        End");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Drop procedure GetAllCoverType");
            migrationBuilder.Sql(@"Drop Procedure GetCoverType");
            migrationBuilder.Sql(@"Drop Procedure AddCoverType");
            migrationBuilder.Sql(@"Drop Procedure UpdateCoverType");
            migrationBuilder.Sql(@"Drop Procedure DeleteCoverType");
        }
    }
}