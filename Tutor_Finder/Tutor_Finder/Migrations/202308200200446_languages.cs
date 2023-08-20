namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class languages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageID = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageDescription = c.String(),
                    })
                .PrimaryKey(t => t.LanguageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Languages");
        }
    }
}
