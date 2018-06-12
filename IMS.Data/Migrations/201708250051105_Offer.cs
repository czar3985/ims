namespace IMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Offer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(),
                        FileName = c.String(),
                        FileUrl = c.String(),
                        Remarks = c.String(),
                        ClientId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.OfferStatus", t => t.StatusId)
                .Index(t => t.ClientId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.OfferStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "StatusId", "dbo.OfferStatus");
            DropForeignKey("dbo.Offers", "ClientId", "dbo.Clients");
            DropIndex("dbo.Offers", new[] { "StatusId" });
            DropIndex("dbo.Offers", new[] { "ClientId" });
            DropTable("dbo.OfferStatus");
            DropTable("dbo.Offers");
        }
    }
}
