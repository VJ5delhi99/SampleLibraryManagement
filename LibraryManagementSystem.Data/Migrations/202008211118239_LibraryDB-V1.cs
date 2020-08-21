namespace LibraryManagementSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibraryDBV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Charges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Penalty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Charges");
        }
    }
}
