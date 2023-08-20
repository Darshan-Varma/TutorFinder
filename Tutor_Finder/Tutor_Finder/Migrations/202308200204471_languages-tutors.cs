namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class languagestutors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TutorLanguages",
                c => new
                    {
                        Tutor_TutorID = c.Int(nullable: false),
                        Language_LanguageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tutor_TutorID, t.Language_LanguageID })
                .ForeignKey("dbo.Tutors", t => t.Tutor_TutorID, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_LanguageID, cascadeDelete: true)
                .Index(t => t.Tutor_TutorID)
                .Index(t => t.Language_LanguageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorLanguages", "Language_LanguageID", "dbo.Languages");
            DropForeignKey("dbo.TutorLanguages", "Tutor_TutorID", "dbo.Tutors");
            DropIndex("dbo.TutorLanguages", new[] { "Language_LanguageID" });
            DropIndex("dbo.TutorLanguages", new[] { "Tutor_TutorID" });
            DropTable("dbo.TutorLanguages");
        }
    }
}
