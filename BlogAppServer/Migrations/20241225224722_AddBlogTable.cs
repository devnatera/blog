using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                schema: "BlogApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_AspNetUsers_AutorId",
                        column: x => x.AutorId,
                        principalSchema: "BlogApp",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_AutorId",
                schema: "BlogApp",
                table: "Blog",
                column: "AutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog",
                schema: "BlogApp");
        }
    }
}
