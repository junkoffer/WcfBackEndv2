namespace WcfBackEndv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Starter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseNr = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FlatNr = c.Int(nullable: false),
                        Name = c.String(),
                        ContactEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceCasePosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Message = c.String(),
                        ServiceCase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCases", t => t.ServiceCase_Id, cascadeDelete: true)
                .Index(t => t.ServiceCase_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCasePosts", "ServiceCase_Id", "dbo.ServiceCases");
            DropIndex("dbo.ServiceCasePosts", new[] { "ServiceCase_Id" });
            DropTable("dbo.ServiceCasePosts");
            DropTable("dbo.ServiceCases");
        }
    }
}
