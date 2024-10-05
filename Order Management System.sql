create database ordermanagementsystems;
use ordermanagementsystems;

create table Users (
    userId INT PRIMARY KEY IDENTITY,
    username NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL,
    role NVARCHAR(10) CHECK (role IN ('Admin', 'User')) NOT NULL
);

create table Products (
    productId INT PRIMARY KEY IDENTITY,
    productName NVARCHAR(100) NOT NULL,
    description NVARCHAR(255),
    price DECIMAL(10, 2) NOT NULL,
    quantityInStock INT NOT NULL,
    type NVARCHAR(20) CHECK (type IN ('Electronics', 'Clothing')) NOT NULL
);

create table Orders (
    orderId INT PRIMARY KEY IDENTITY,
    userId INT FOREIGN KEY REFERENCES Users(userId),
    orderDate DATETIME DEFAULT GETDATE(),
    TotalPrice DECIMAL(10, 2)
);

create table OrderProducts (
    orderProductId INT PRIMARY KEY IDENTITY,
    orderId INT FOREIGN KEY REFERENCES Orders(orderId) ON DELETE CASCADE,
    productId INT FOREIGN KEY REFERENCES Products(productId),
    quantity INT NOT NULL
);

create table Electronics (
    ProductId INT PRIMARY KEY,
    Brand VARCHAR(50),
    WarrantyPeriod INT,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);

create table Clothing (
    ProductId INT PRIMARY KEY,
    Size VARCHAR(10),
    Color VARCHAR(30),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);



select * from Products;
select * from Users;
select * from Orders;
select * from OrderProducts;
select * from Clothing;
select * from Electronics;
