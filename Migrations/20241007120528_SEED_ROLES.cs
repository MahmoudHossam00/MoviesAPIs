using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication2.Helpers;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    
    public partial class SEED_ROLES : Migration
    {
		 
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.InsertData(
		  table: "AspNetRoles",
		  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
		  values: new object[] { Guid.NewGuid().ToString(), RolesStrings._user.ToUpper(), RolesStrings._user.ToUpper(), Guid.NewGuid().ToString() }
	  );

			migrationBuilder.InsertData(
		  table: "AspNetRoles",
		  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
		  values: new object[] { Guid.NewGuid().ToString(), RolesStrings._admin.ToUpper(), RolesStrings._admin.ToUpper(), Guid.NewGuid().ToString() }
	  );
			migrationBuilder.InsertData(
	  table: "AspNetRoles",
	  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
	  values: new object[] { Guid.NewGuid().ToString(), RolesStrings._Moderator.ToUpper(), RolesStrings._Moderator.ToUpper(), Guid.NewGuid().ToString() }
  );

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
