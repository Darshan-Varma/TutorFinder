namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tutors1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tutors",
                c => new
                    {
                        TutorID = c.Int(nullable: false, identity: true),
                        TutorFirstName = c.String(),
                        TutorLastName = c.String(),
                        TutorRating = c.Int(nullable: false),
                        TutorDescription = c.String(),
                    })
                .PrimaryKey(t => t.TutorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tutors");
        }
    }
}
