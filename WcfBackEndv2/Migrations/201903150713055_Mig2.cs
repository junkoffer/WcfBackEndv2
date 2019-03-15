namespace WcfBackEndv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCasePosts", "ContactEmail", c => c.String());
            DropColumn("dbo.ServiceCasePosts", "UserEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceCasePosts", "UserEmail", c => c.String());
            DropColumn("dbo.ServiceCasePosts", "ContactEmail");
        }
    }
}
