namespace Estorenew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'ef95671b-b5c3-4b88-852b-b5c0f4ccb1b8', N'Admin@gmail.com', 0, N'AF2wAUqoqFmAkVlLY101t+jV3Y48TifVT90hXXQloMg0DSYguvMCBirJZrpmjRjYLg==', N'80a1aceb-a76e-4429-a1c9-f5d41cf6af7d', NULL, 0, 0, NULL, 1, 0, N'Admin@gmail.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'81bbabd5-548a-4333-82dd-fe407dadd21d', N'Admin')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ef95671b-b5c3-4b88-852b-b5c0f4ccb1b8', N'81bbabd5-548a-4333-82dd-fe407dadd21d')
");
        }
        
        public override void Down()
        {
        }
    }
}
