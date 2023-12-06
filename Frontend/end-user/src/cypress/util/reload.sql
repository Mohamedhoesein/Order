DELETE FROM "AspNetUserClaims";
DELETE FROM "AspNetUserLogins";
DELETE FROM "AspNetUserRoles";
DELETE FROM "AspNetUserTokens";
DELETE FROM "AspNetUsers";

INSERT INTO "AspNetUsers" ("Id", "Name", "Address", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
VALUES (1, 'TempAdmin', 'Address', 'test@test.com', 'TEST@TEST.COM', 'test@test.com', 'TEST@TEST.COM', true, 'AQAAAAEAACcQAAAAEIj9FzLn96pa8OlStBMrYAgEUenp56bUarbToERhE5NPCTn1EDiBdw7ff0VDJiUjnA==', 'BVVSLPHREZXATUD2QOIVYZS6FZYNJRJY', '4e1124c2-4ef4-40a3-b211-2e9fa2b0099f', false, false, false, 0);

INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES (1, 1);