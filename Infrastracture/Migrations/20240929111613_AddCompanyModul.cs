using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyModul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "Catalog");

            migrationBuilder.EnsureSchema(name: "Company");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Catalog",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Zip = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "Company",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Agents",
                schema: "Company",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                        ContractNumber = table.Column<string>(
                            type: "nvarchar(max)",
                            nullable: false
                        ),
                        AddressId = table.Column<int>(type: "int", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agents_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Catalog",
                        principalTable: "Address",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_Agents_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Company",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        ExternalId = table.Column<int>(type: "int", nullable: false),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Ssn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Dob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Age = table.Column<int>(type: "int", nullable: false),
                        HomeId = table.Column<int>(type: "int", nullable: false),
                        OfficeId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Address_HomeId",
                        column: x => x.HomeId,
                        principalSchema: "Catalog",
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction
                    );
                    table.ForeignKey(
                        name: "FK_Customers_Address_OfficeId",
                        column: x => x.OfficeId,
                        principalSchema: "Catalog",
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Campaigns",
                schema: "Company",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        CompanyId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Company",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "AgentCampaigns",
                schema: "Company",
                columns: table =>
                    new
                    {
                        AgentId = table.Column<int>(type: "int", nullable: false),
                        CampaignId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentCampaigns", x => new { x.AgentId, x.CampaignId });
                    table.ForeignKey(
                        name: "FK_AgentCampaigns_Agents_AgentId",
                        column: x => x.AgentId,
                        principalSchema: "Company",
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_AgentCampaigns_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "Company",
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "CustomersRewards",
                schema: "Company",
                columns: table =>
                    new
                    {
                        CustomerId = table.Column<int>(type: "int", nullable: false),
                        CampaignId = table.Column<int>(type: "int", nullable: false),
                        AgentId = table.Column<int>(type: "int", nullable: false),
                        RewardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        RewardAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_CustomersRewards",
                        x => new { x.CustomerId, x.CampaignId }
                    );
                    table.ForeignKey(
                        name: "FK_CustomersRewards_Agents_AgentId",
                        column: x => x.AgentId,
                        principalSchema: "Company",
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_CustomersRewards_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "Company",
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_CustomersRewards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Company",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "UsedRewards",
                schema: "Company",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        CustomerId = table.Column<int>(type: "int", nullable: false),
                        CampaignId = table.Column<int>(type: "int", nullable: false),
                        UsedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedRewards_CustomersRewards_CustomerId_CampaignId",
                        columns: x => new { x.CustomerId, x.CampaignId },
                        principalSchema: "Company",
                        principalTable: "CustomersRewards",
                        principalColumns: new[] { "CustomerId", "CampaignId" },
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AgentCampaigns_CampaignId",
                schema: "Company",
                table: "AgentCampaigns",
                column: "CampaignId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Agents_AddressId",
                schema: "Company",
                table: "Agents",
                column: "AddressId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserId",
                schema: "Company",
                table: "Agents",
                column: "UserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CompanyId",
                schema: "Company",
                table: "Campaigns",
                column: "CompanyId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ExternalId",
                schema: "Company",
                table: "Customers",
                column: "ExternalId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Customers_HomeId",
                schema: "Company",
                table: "Customers",
                column: "HomeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OfficeId",
                schema: "Company",
                table: "Customers",
                column: "OfficeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_CustomersRewards_AgentId",
                schema: "Company",
                table: "CustomersRewards",
                column: "AgentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_CustomersRewards_CampaignId",
                schema: "Company",
                table: "CustomersRewards",
                column: "CampaignId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_UsedRewards_CustomerId_CampaignId",
                schema: "Company",
                table: "UsedRewards",
                columns: new[] { "CustomerId", "CampaignId" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AgentCampaigns", schema: "Company");

            migrationBuilder.DropTable(name: "UsedRewards", schema: "Company");

            migrationBuilder.DropTable(name: "CustomersRewards", schema: "Company");

            migrationBuilder.DropTable(name: "Agents", schema: "Company");

            migrationBuilder.DropTable(name: "Campaigns", schema: "Company");

            migrationBuilder.DropTable(name: "Customers", schema: "Company");

            migrationBuilder.DropTable(name: "Companies", schema: "Company");

            migrationBuilder.DropTable(name: "Address", schema: "Catalog");
        }
    }
}
