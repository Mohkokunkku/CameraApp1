using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CameraDataWebApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoringVisits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    visitname = table.Column<string>(nullable: true),
                    visitguid = table.Column<string>(nullable: true),
                    casename = table.Column<string>(nullable: true),
                    casenumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringVisits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    observation = table.Column<string>(nullable: true),
                    imageuri = table.Column<string>(nullable: true),
                    observationguid = table.Column<string>(nullable: true),
                    visitguid = table.Column<string>(nullable: true),
                    visitname = table.Column<string>(nullable: true),
                    MonitoringVisitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_MonitoringVisits_MonitoringVisitId",
                        column: x => x.MonitoringVisitId,
                        principalTable: "MonitoringVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_MonitoringVisitId",
                table: "Observations",
                column: "MonitoringVisitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "MonitoringVisits");
        }
    }
}
