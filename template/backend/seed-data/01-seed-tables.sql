CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "Username" varchar(50) NOT NULL UNIQUE,
    "Password" varchar(100) NOT NULL,
    "Phone" varchar(20) NOT NULL,
    "Email" varchar(100) NOT NULL UNIQUE,
    "Status" varchar(20) NOT NULL,
    "Role" varchar(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT NOW(),
    "UpdatedAt" timestamp with time zone
);

CREATE TABLE IF NOT EXISTS "Products" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "Title" varchar(100) NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "Description" varchar(500),
    "Category" varchar(50) NOT NULL,
    "ImageUrl" varchar(255),
    "RatingRate" double precision NOT NULL,
    "RatingCount" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT NOW(),
    "UpdatedAt" timestamp with time zone
);

CREATE TABLE IF NOT EXISTS "Sales" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "SaleNumber" varchar(50) NOT NULL UNIQUE,
    "SaleDate" timestamp with time zone NOT NULL,
    "CustomerId" uuid NOT NULL,
    "CustomerUserName" varchar(100) NOT NULL,
    "CustomerEmail" varchar(150),
    "CustomerPhone" varchar(20),
    "CustomerCategory" varchar(50),
    "BranchId" uuid NOT NULL,
    "BranchName" varchar(100) NOT NULL,
    "TotalAmount" numeric(18,2) NOT NULL,
    "IsCancelled" boolean NOT NULL DEFAULT false,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT NOW(),
    "UpdatedAt" timestamp with time zone
);

CREATE TABLE IF NOT EXISTS "SaleProducts" (
    "SaleId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    PRIMARY KEY ("SaleId", "ProductId"),
    CONSTRAINT "FK_SaleProducts_Sales" FOREIGN KEY ("SaleId") 
        REFERENCES "Sales" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_SaleProducts_Products" FOREIGN KEY ("ProductId") 
        REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users" ("Email");
CREATE INDEX IF NOT EXISTS "IX_Products_Category" ON "Products" ("Category");
CREATE INDEX IF NOT EXISTS "IX_Sales_SaleDate_IsCancelled" ON "Sales" ("SaleDate", "IsCancelled");
CREATE INDEX IF NOT EXISTS "IX_SaleProducts_ProductId" ON "SaleProducts" ("ProductId");