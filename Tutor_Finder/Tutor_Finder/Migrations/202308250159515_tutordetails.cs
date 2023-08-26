namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tutordetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tutors", "ContactNumber", c => c.String());
            AddColumn("dbo.Tutors", "SocialMedia", c => c.String());
            AddColumn("dbo.Tutors", "EmailID", c => c.String());
            AddColumn("dbo.Tutors", "Password", c => c.String());
            DropColumn("dbo.Tutors", "TutorRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tutors", "TutorRating", c => c.Int(nullable: false));
            DropColumn("dbo.Tutors", "Password");
            DropColumn("dbo.Tutors", "EmailID");
            DropColumn("dbo.Tutors", "SocialMedia");
            DropColumn("dbo.Tutors", "ContactNumber");
        }
    }
}
