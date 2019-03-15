namespace WcfBackEndv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCasePosts", "Name", c => c.String());
            DropColumn("dbo.ServiceCasePosts", "UserDisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceCasePosts", "UserDisplayName", c => c.String());
            DropColumn("dbo.ServiceCasePosts", "Name");
        }
    }
}
