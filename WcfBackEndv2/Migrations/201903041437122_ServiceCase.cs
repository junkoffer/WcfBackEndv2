namespace WcfBackEndv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCases",
                c => new
                    {
                        CaseNr = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FlatNr = c.Int(nullable: false),
                        Name = c.String(),
                        ContactEmail = c.String(),
                    })
                .PrimaryKey(t => t.CaseNr);
            
            CreateTable(
                "dbo.ServiceCasePosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        ServiceCase_CaseNr = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCases", t => t.ServiceCase_CaseNr, cascadeDelete: true)
                .Index(t => t.ServiceCase_CaseNr);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCasePosts", "ServiceCase_CaseNr", "dbo.ServiceCases");
            DropIndex("dbo.ServiceCasePosts", new[] { "ServiceCase_CaseNr" });
            DropTable("dbo.ServiceCasePosts");
            DropTable("dbo.ServiceCases");
        }
    }
}
