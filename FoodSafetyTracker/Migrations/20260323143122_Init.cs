using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace FoodSafetyTracker.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Premises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Town = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RiskRating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PremisesId = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Outcome = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Premises_PremisesId",
                        column: x => x.PremisesId,
                        principalTable: "Premises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUps_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Premises",
                columns: new[] { "Id", "Address", "Name", "RiskRating", "Town" },
                values: new object[,]
                {
                    { 1, "1 Main St", "Le Bon Goût", 2, "Cork" },
                    { 2, "5 Shop St", "Pizza Palace", 1, "Dublin" },
                    { 3, "12 Park Ave", "Green Bowl Café", 0, "Galway" },
                    { 4, "8 High St", "The Burger Joint", 2, "Cork" },
                    { 5, "22 Grafton St", "Sushi Express", 1, "Dublin" },
                    { 6, "3 Harbour Rd", "Ocean Catch", 2, "Galway" },
                    { 7, "17 Bridge St", "Corner Bakery", 0, "Cork" },
                    { 8, "9 Temple Bar", "Noodle House", 1, "Dublin" },
                    { 9, "45 Eyre Sq", "The Kebab King", 2, "Galway" },
                    { 10, "6 Patrick St", "Sunny Side Up", 0, "Cork" },
                    { 11, "33 O'Connell St", "Taco Town", 1, "Dublin" },
                    { 12, "2 Quay St", "The Vegan Spot", 0, "Galway" }
                });

            migrationBuilder.InsertData(
                table: "Inspections",
                columns: new[] { "Id", "InspectionDate", "Notes", "Outcome", "PremisesId", "Score" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Poor hygiene in kitchen", 1, 1, 42 },
                    { 2, new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expired food found", 1, 1, 55 },
                    { 3, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good overall", 0, 2, 85 },
                    { 4, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor issues noted", 0, 2, 70 },
                    { 5, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 3, 92 },
                    { 6, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pest evidence found", 1, 4, 38 },
                    { 7, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Temperature violations", 1, 4, 60 },
                    { 8, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent cleanliness", 0, 5, 88 },
                    { 9, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Storage issues", 1, 6, 45 },
                    { 10, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cross-contamination risk", 1, 6, 50 },
                    { 11, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 7, 95 },
                    { 12, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good practices", 0, 8, 72 },
                    { 13, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Multiple violations", 1, 9, 33 },
                    { 14, new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "No hand wash station", 1, 9, 48 },
                    { 15, new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 10, 90 },
                    { 16, new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Satisfactory", 0, 11, 78 },
                    { 17, new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 12, 88 },
                    { 18, new DateTime(2024, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good", 0, 3, 80 },
                    { 19, new DateTime(2024, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 5, 65 },
                    { 20, new DateTime(2024, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 7, 91 },
                    { 21, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Old violations", 1, 8, 55 },
                    { 22, new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 10, 82 },
                    { 23, new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 11, 74 },
                    { 24, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 12, 85 },
                    { 25, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recent check", 0, 2, 79 }
                });

            migrationBuilder.InsertData(
                table: "FollowUps",
                columns: new[] { "Id", "ClosedDate", "DueDate", "InspectionId", "Status" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0 },
                    { 2, null, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0 },
                    { 3, null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 0 },
                    { 4, null, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 0 },
                    { 5, null, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 0 },
                    { 6, null, new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 0 },
                    { 7, null, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 0 },
                    { 8, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1 },
                    { 9, new DateTime(2024, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 10, new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_InspectionId",
                table: "FollowUps",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PremisesId",
                table: "Inspections",
                column: "PremisesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FollowUps");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Premises");
        }
    }
}
