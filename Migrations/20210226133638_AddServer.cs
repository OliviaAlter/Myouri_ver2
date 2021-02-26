using Microsoft.EntityFrameworkCore.Migrations;

namespace AnotherMyouri.Migrations
{
    public partial class AddServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageLogChannel = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    EventLogChannel = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    WelcomeChannel = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LeaveChannel = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UserUpdateChannel = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    WelcomeUrl = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    WelcomeMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteToggle = table.Column<bool>(type: "bit", nullable: false),
                    RoleMentionToggle = table.Column<bool>(type: "bit", nullable: false),
                    UserMentionToggle = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
