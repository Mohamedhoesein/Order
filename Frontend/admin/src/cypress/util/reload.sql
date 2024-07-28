TRUNCATE "AspNetUserClaims" RESTART IDENTITY CASCADE;
TRUNCATE "AspNetUserLogins" RESTART IDENTITY CASCADE;
TRUNCATE "AspNetUserRoles" RESTART IDENTITY CASCADE;
TRUNCATE "AspNetUserTokens" RESTART IDENTITY CASCADE;
TRUNCATE "AspNetUsers" RESTART IDENTITY CASCADE;
TRUNCATE "Filters" RESTART IDENTITY CASCADE;
TRUNCATE "ClosedSpecificationValues" RESTART IDENTITY CASCADE;
TRUNCATE "ClosedSpecificationValueProductVersion" RESTART IDENTITY CASCADE;
TRUNCATE "ClosedSpecifications" RESTART IDENTITY CASCADE;
TRUNCATE "OpenSpecificationValues" RESTART IDENTITY CASCADE;
TRUNCATE "OpenSpecifications" RESTART IDENTITY CASCADE;
TRUNCATE "Categories" RESTART IDENTITY CASCADE;
TRUNCATE "ProductImages" RESTART IDENTITY CASCADE;
TRUNCATE "ProductVersions" RESTART IDENTITY CASCADE;
TRUNCATE "Products" RESTART IDENTITY CASCADE;

INSERT INTO "AspNetUsers" ("Name", "Address", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
VALUES ('TempAdmin', 'Address', 'test@test.com', 'TEST@TEST.COM', 'test@test.com', 'TEST@TEST.COM', true, 'AQAAAAEAACcQAAAAEIj9FzLn96pa8OlStBMrYAgEUenp56bUarbToERhE5NPCTn1EDiBdw7ff0VDJiUjnA==', 'BVVSLPHREZXATUD2QOIVYZS6FZYNJRJY', '4e1124c2-4ef4-40a3-b211-2e9fa2b0099f', false, false, false, 0);

INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES (1, 1);

INSERT INTO "Categories" ("Name", "Deleted")
VALUES ('InitialCategory', false);

INSERT INTO "ClosedSpecifications" ("CategoryId", "Name", "Deleted")
VALUES (1, 'ClosedSpecification', false);

INSERT INTO "ClosedSpecificationValues" ("SpecificationId", "Value", "Deleted")
VALUES (1, 'ClosedSpecificationValue', false);

INSERT INTO "Filters" ("CategoryId", "SpecificationId", "Title")
VALUES (1, 1, 'Filter');

INSERT INTO "OpenSpecifications" ("CategoryId", "Name", "Deleted")
VALUES (1, 'OpenSpecification', false);