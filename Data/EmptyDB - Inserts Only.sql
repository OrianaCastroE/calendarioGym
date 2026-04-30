-- INSERT Roles

-- Admin Role

INSERT INTO DarkKitchenDB.dbo.[RolePermissions] ([Role], [Permissions]) VALUES (0, '[3,4,5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22]');

-- Client Role

INSERT INTO DarkKitchenDB.dbo.[RolePermissions] ([Role], [Permissions]) VALUES (1, '[0,1,14,19]');

-- Dispatcher Role

INSERT INTO DarkKitchenDB.dbo.[RolePermissions] ([Role], [Permissions]) VALUES (2, '[2,3,4,6,7,8]');

-- INSERT Admin User

INSERT INTO DarkKitchenDB.dbo.[Users] ([Name], [Surname], [Email], [Phone], [Password], [Role])
	VALUES ('Admin', 'DarkKitchen', 'admin@darkkitchen.com', '59899999999', '$2a$11$exI1fZF13YyNbJ818DSt4eNJ3bKjaz0/i7bRrZiLlKb3feu1jQPxy', 0);