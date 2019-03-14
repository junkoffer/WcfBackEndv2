namespace WcfBackEndv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCasePosts", "UserDisplayName", c => c.String());
            AddColumn("dbo.ServiceCasePosts", "UserEmail", c => c.String());
            AddColumn("dbo.ServiceCasePosts", "Private", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCasePosts", "Private");
            DropColumn("dbo.ServiceCasePosts", "UserEmail");
            DropColumn("dbo.ServiceCasePosts", "UserDisplayName");
        }
    }
}
