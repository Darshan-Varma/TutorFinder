namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentdetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "StudentEmailID", c => c.String());
            AddColumn("dbo.Students", "StudentContactNumber", c => c.String());
            AddColumn("dbo.Students", "StudentPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "StudentPassword");
            DropColumn("dbo.Students", "StudentContactNumber");
            DropColumn("dbo.Students", "StudentEmailID");
        }
    }
}
