namespace Tutor_Finder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentstutors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        StudentFirstName = c.String(),
                        StudentLastName = c.String(),
                        StudentSemester = c.String(),
                        StudentNumber = c.String(),
                    })
                .PrimaryKey(t => t.StudentID);
            
            CreateTable(
                "dbo.StudentTutors",
                c => new
                    {
                        Student_StudentID = c.Int(nullable: false),
                        Tutor_TutorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentID, t.Tutor_TutorID })
                .ForeignKey("dbo.Students", t => t.Student_StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Tutors", t => t.Tutor_TutorID, cascadeDelete: true)
                .Index(t => t.Student_StudentID)
                .Index(t => t.Tutor_TutorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTutors", "Tutor_TutorID", "dbo.Tutors");
            DropForeignKey("dbo.StudentTutors", "Student_StudentID", "dbo.Students");
            DropIndex("dbo.StudentTutors", new[] { "Tutor_TutorID" });
            DropIndex("dbo.StudentTutors", new[] { "Student_StudentID" });
            DropTable("dbo.StudentTutors");
            DropTable("dbo.Students");
        }
    }
}
