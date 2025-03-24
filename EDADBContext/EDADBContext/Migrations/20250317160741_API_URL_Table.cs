using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDADBContext.Migrations
{
    /// <inheritdoc />
    public partial class API_URL_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APIURLs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FetchURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PutURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APIURLs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_APIURLs_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APIURLs_ScreenId",
                table: "APIURLs",
                column: "ScreenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APIURLs");
        }
    }
}
