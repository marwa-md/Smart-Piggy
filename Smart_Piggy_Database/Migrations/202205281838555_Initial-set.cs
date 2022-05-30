namespace Smart_Piggy_Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                 .ForeignKey("dbo.Owners", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);

            CreateTable(
                "dbo.Charges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.Int(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .Index(t => t.BudgetId);
            
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .Index(t => t.BudgetId);

            CreateTable(
                "dbo.Owners",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(),
                    LastName = c.String(),
                    UserName = c.String(),
                    Password = c.String(),
                    SecQuestion1 = c.String(),
                    SecQuestion2 = c.String(),
                    Answ1 = c.String(),
                    Answ2 = c.String(),
                })
                .PrimaryKey(t => t.Id);
           

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incomes", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Charges", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Budgets", "OwnerId", "dbo.Owners");
            DropIndex("dbo.Incomes", new[] { "BudgetId" });
            DropIndex("dbo.Charges", new[] { "BudgetId" });
            DropIndex("dbo.Budgets", new[] { "OwnerId" });
            DropTable("dbo.Owners");
            DropTable("dbo.Incomes");
            DropTable("dbo.Charges");
            DropTable("dbo.Budgets");
        }
    }
}
