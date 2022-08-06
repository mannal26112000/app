namespace Estorenew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItemInStock = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemsCarts",
                c => new
                    {
                        Items_Id = c.Int(nullable: false),
                        Cart_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Items_Id, t.Cart_Id })
                .ForeignKey("dbo.Items", t => t.Items_Id, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.Cart_Id, cascadeDelete: true)
                .Index(t => t.Items_Id)
                .Index(t => t.Cart_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemsCarts", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.ItemsCarts", "Items_Id", "dbo.Items");
            DropIndex("dbo.ItemsCarts", new[] { "Cart_Id" });
            DropIndex("dbo.ItemsCarts", new[] { "Items_Id" });
            DropTable("dbo.ItemsCarts");
            DropTable("dbo.Items");
        }
    }
}
