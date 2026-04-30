-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- DarkKitchenDB.dbo.Products definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.Products;

CREATE TABLE DarkKitchenDB.dbo.Products (
	Id int IDENTITY(1,1) NOT NULL,
	Code nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ProductLine nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Category nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Price decimal(18,2) NOT NULL,
	IsActive bit NOT NULL,
	UnitsSold int NOT NULL,
	CONSTRAINT PK_Products PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Products_Code ON DarkKitchenDB.dbo.Products (  Code ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DarkKitchenDB.dbo.Promotion definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.Promotion;

CREATE TABLE DarkKitchenDB.dbo.Promotion (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DiscountPercentage int NOT NULL,
	DateFrom datetime2 NOT NULL,
	DateTo datetime2 NOT NULL,
	CONSTRAINT PK_Promotion PRIMARY KEY (Id)
);


-- DarkKitchenDB.dbo.RolePermissions definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.RolePermissions;

CREATE TABLE DarkKitchenDB.dbo.RolePermissions (
	[Role] int NOT NULL,
	Permissions nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_RolePermissions PRIMARY KEY ([Role])
);


-- DarkKitchenDB.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.[__EFMigrationsHistory];

CREATE TABLE DarkKitchenDB.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- DarkKitchenDB.dbo.ProductImages definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.ProductImages;

CREATE TABLE DarkKitchenDB.dbo.ProductImages (
	Id int IDENTITY(1,1) NOT NULL,
	Url nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductId int NOT NULL,
	CONSTRAINT PK_ProductImages PRIMARY KEY (Id),
	CONSTRAINT FK_ProductImages_Products_ProductId FOREIGN KEY (ProductId) REFERENCES DarkKitchenDB.dbo.Products(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_ProductImages_ProductId ON DarkKitchenDB.dbo.ProductImages (  ProductId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DarkKitchenDB.dbo.ProductPromotion definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.ProductPromotion;

CREATE TABLE DarkKitchenDB.dbo.ProductPromotion (
	ProductsId int NOT NULL,
	PromotionId int NOT NULL,
	CONSTRAINT PK_ProductPromotion PRIMARY KEY (ProductsId,PromotionId),
	CONSTRAINT FK_ProductPromotion_Products_ProductsId FOREIGN KEY (ProductsId) REFERENCES DarkKitchenDB.dbo.Products(Id) ON DELETE CASCADE,
	CONSTRAINT FK_ProductPromotion_Promotion_PromotionId FOREIGN KEY (PromotionId) REFERENCES DarkKitchenDB.dbo.Promotion(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_ProductPromotion_PromotionId ON DarkKitchenDB.dbo.ProductPromotion (  PromotionId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DarkKitchenDB.dbo.Users definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.Users;

CREATE TABLE DarkKitchenDB.dbo.Users (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Surname nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Phone nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Role] int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT FK_Users_RolePermissions_Role FOREIGN KEY ([Role]) REFERENCES DarkKitchenDB.dbo.RolePermissions([Role])
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Email ON DarkKitchenDB.dbo.Users (  Email ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Users_Role ON DarkKitchenDB.dbo.Users (  Role ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DarkKitchenDB.dbo.Orders definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.Orders;

CREATE TABLE DarkKitchenDB.dbo.Orders (
	Id int IDENTITY(1,1) NOT NULL,
	ClientId int NOT NULL,
	DeliveryType nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Street nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DoorNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Apartment nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Status nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 NOT NULL,
	UpdatedAt datetime2 NULL,
	Subtotal decimal(18,2) NOT NULL,
	Discount decimal(18,2) NOT NULL,
	Iva decimal(18,2) NOT NULL,
	ShippingCost decimal(18,2) NOT NULL,
	Total decimal(18,2) NOT NULL,
	CONSTRAINT PK_Orders PRIMARY KEY (Id),
	CONSTRAINT FK_Orders_Users_ClientId FOREIGN KEY (ClientId) REFERENCES DarkKitchenDB.dbo.Users(Id)
);
 CREATE NONCLUSTERED INDEX IX_Orders_ClientId ON DarkKitchenDB.dbo.Orders (  ClientId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DarkKitchenDB.dbo.OrderProducts definition

-- Drop table

-- DROP TABLE DarkKitchenDB.dbo.OrderProducts;

CREATE TABLE DarkKitchenDB.dbo.OrderProducts (
	Id int IDENTITY(1,1) NOT NULL,
	OrderId int NOT NULL,
	ProductId int NOT NULL,
	Quantity int NOT NULL,
	UnitPrice decimal(18,2) NOT NULL,
	DiscountPercentage decimal(18,2) NOT NULL,
	CONSTRAINT PK_OrderProducts PRIMARY KEY (Id),
	CONSTRAINT FK_OrderProducts_Orders_OrderId FOREIGN KEY (OrderId) REFERENCES DarkKitchenDB.dbo.Orders(Id) ON DELETE CASCADE,
	CONSTRAINT FK_OrderProducts_Products_ProductId FOREIGN KEY (ProductId) REFERENCES DarkKitchenDB.dbo.Products(Id)
);
CREATE NONCLUSTERED INDEX IX_OrderProducts_OrderId ON DarkKitchenDB.dbo.OrderProducts (  OrderId ASC  )  
	WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	ON [PRIMARY ] ;
CREATE NONCLUSTERED INDEX IX_OrderProducts_ProductId ON DarkKitchenDB.dbo.OrderProducts (  ProductId ASC  )  
	WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	ON [PRIMARY ] ;

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
