namespace eShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'24962573-5a1d-491c-8ed1-40e2285afe9b', N'user@isec.pt', 0, N'AIKNKTwFpojvRfJq+2a8By6jeyh3b2O4Tf3Qkc4KfGSLKITumnuDQPadU+z9aiCWUA==', N'7e58efb1-fe71-4176-91a0-d42de0a0ad76', NULL, 0, 0, NULL, 1, 0, N'user')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'341db6e6-7697-4a16-b4fc-310376b8eb1a', N'admin@isec.pt', 0, N'ACBLn0C7GZkpcebKxfr9osmmrmU6VhfZEnAw0+ARKsOSlCN7Szy2S3OzuVwKSCo4pg==', N'a52d55cd-3293-4f76-b3f5-ffda4cb0f20b', NULL, 0, 0, NULL, 1, 0, N'admin')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'64f05596-78fb-47e8-96ce-fd7b48194ac0', N'employee@isec.pt', 0, N'AESq1fBf4PDxYZf4atB7FLmAq42fPPpKkeVdHctoXUtiTLpvSoN7LLFBwBFVjb816Q==', N'dfde3d5c-7396-48ec-8da0-e9f5aa4628ea', NULL, 0, 0, NULL, 1, 0, N'employee')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'84cbde25-406e-403a-ad84-8e0a3a987a20', N'company@isec.pt', 0, N'AA/IGHvLONXWr18ZVAWPcEa+yiN5LkRPmx1847XhUpou/kcGIDjGqvIK475TA8Vu4w==', N'7892bbf2-244b-48e6-87bc-2d6384ce546d', NULL, 0, 0, NULL, 1, 0, N'company')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'914463cf-d0d9-4223-928b-a597cc90b78e', N'admin')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'bcc7e0bb-f97e-45b2-904a-371321009061', N'company')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'330590af-4b4d-49f1-a2d6-24e873fa9cf6', N'employee')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'77bfa525-a12e-454d-bcd5-abc6a3336562', N'user')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'64f05596-78fb-47e8-96ce-fd7b48194ac0', N'330590af-4b4d-49f1-a2d6-24e873fa9cf6')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'24962573-5a1d-491c-8ed1-40e2285afe9b', N'77bfa525-a12e-454d-bcd5-abc6a3336562')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'341db6e6-7697-4a16-b4fc-310376b8eb1a', N'914463cf-d0d9-4223-928b-a597cc90b78e')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'84cbde25-406e-403a-ad84-8e0a3a987a20', N'bcc7e0bb-f97e-45b2-904a-371321009061')


");
        }
        
        public override void Down()
        {
        }
    }
}
