using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankApi.Migrations
{
    public partial class AddLoanApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // garante o enum sem erro se já existir
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'tx_type') THEN
        CREATE TYPE public.tx_type AS ENUM ('credit','debit');
    END IF;
END $$;
");

            migrationBuilder.CreateTable(
                name: "LoanApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: true),
                    Income = table.Column<double>(type: "double precision", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    LatePayments = table.Column<int>(type: "integer", nullable: false),
                    Requested = table.Column<double>(type: "double precision", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: false),
                    MaxAmount = table.Column<double>(type: "double precision", nullable: false),
                    SuggestedApr = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplications", x => x.Id);
                });

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_loanapplications_createdat
                                   ON ""LoanApplications""(""CreatedAt"");");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "LoanApplications");
            migrationBuilder.Sql("DROP TYPE IF EXISTS public.tx_type;");
        }
    }
}
