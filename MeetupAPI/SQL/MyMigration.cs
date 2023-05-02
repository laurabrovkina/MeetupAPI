using System.Data.Entity.Migrations;

namespace MeetupAPI.SQL
{
    public class MyMigration : DbMigration
    {
        public override void Up()
        {
            this.AlterStoredProcedure("dbo.EditEmail", c => new
            {
                Id = c.Int(),
                Email = c.String(maxLength: int.MaxValue),
            }, @"UPDATE Users SET Email = @Email WHERE Id = @Id");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.EditEmail");
        }
    }
}
