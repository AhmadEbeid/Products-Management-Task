CREATE TABLE Suppliers
(
  SupplierID int PRIMARY KEY IDENTITY(1000,100) NOT NULL,
  SupplierName nvarchar(255) UNIQUE NOT NULL,
);

CREATE TABLE Products
(
  ProductID int PRIMARY KEY IDENTITY(1000,100) NOT NULL,
  ProductName nvarchar(255) UNIQUE NOT NULL, 
  QuantityPerUnit varchar(10) NOT NULL,
  ReorderLevel int NOT NULL,
  SupplierID int,
  UnitPrice money NOT NULL,
  UnitsInStock int NOT NULL,
  UnitsOnOrder int NOT NULL
  FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE CASCADE
);

CREATE TABLE Orders
(
  OrderID int PRIMARY KEY IDENTITY(1000,100) NOT NULL,
  ProductID int,
  CustomerName nvarchar(255) NOT NULL,
  UnitsOrdered int NOT NULL,
  TotalAmount money NOT NULL
  FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

CREATE PROCEDURE SelectAllOrders AS
  BEGIN
    SELECT OrderID, P.ProductName, CustomerName, UnitsOrdered, TotalAmount FROM Orders as O, Products as P where O.ProductID = P.ProductID;
  END
GO

CREATE PROCEDURE MakeOrder @ProductID int, @CustomerName nvarchar(255), @UnitsOrdered int AS
  BEGIN
    INSERT INTO Orders VALUES (@ProductID, @CustomerName, @UnitsOrdered, (Select UnitPrice from Products where ProductID = @ProductID) * @UnitsOrdered);
    UPDATE Products SET UnitsInStock = UnitsInStock - @UnitsOrdered WHERE ProductID = @ProductID;
  END
GO

CREATE PROCEDURE SelectSomeOrders @SearchParam nvarchar(255) AS
  BEGIN
    SELECT OrderID, P.ProductName, CustomerName, UnitsOrdered, TotalAmount FROM Orders as O, Products as P 
      WHERE O.ProductID = P.ProductID AND (O.CustomerName LIKE '%'+@SearchParam+'%' OR P.ProductName LIKE '%'+@SearchParam+'%');
  END
GO

CREATE PROCEDURE SelectAllProducts AS
  BEGIN
    SELECT ProductID, ProductName, QuantityPerUnit, ReorderLevel, S.SupplierName, UnitPrice, UnitsInStock, UnitsOnOrder FROM Products as P, Suppliers as S WHERE S.SupplierID = P.SupplierID;
  END
GO

CREATE PROCEDURE SelectProductsCanBeBought AS
  BEGIN
    SELECT ProductName, ProductID FROM Products 
      WHERE UnitsInStock > 0;
  END
GO

CREATE PROCEDURE GetProductStock @ProductID int AS
  BEGIN
    SELECT UnitsInStock FROM Products 
      WHERE ProductID = @ProductID;
  END
GO

CREATE PROCEDURE SelectProductWithMinimumOrder AS
  BEGIN
    SELECT P.ProductID, P.ProductName from Products as P, 
      (SELECT TOP 1 ProductID, COUNT(ProductID) as times from Orders group by ProductID order by times ASC) as D
        WHERE P.ProductID = D.ProductID
  END
GO

CREATE PROCEDURE SelectAllProductsNeedsReOrder AS
  BEGIN
    SELECT ProductID, ProductName, QuantityPerUnit, ReorderLevel, S.SupplierName, UnitPrice, UnitsInStock, UnitsOnOrder FROM Products as P, Suppliers as S 
      WHERE S.SupplierID = P.SupplierID AND P.UnitsInStock < P.ReorderLevel;
  END
GO

CREATE PROCEDURE SelectSomeProducts @SearchParam nvarchar(255) AS
  BEGIN
    SELECT ProductID, ProductName, QuantityPerUnit, ReorderLevel, S.SupplierName, UnitPrice, UnitsInStock, UnitsOnOrder FROM Products as P, Suppliers as S 
      WHERE S.SupplierID = P.SupplierID AND (P.ProductName LIKE '%'+@SearchParam+'%' OR S.SupplierName LIKE '%'+@SearchParam+'%');
  END
GO

CREATE PROCEDURE InsertProduct @ProductName nvarchar(255), @QuantityPerUnit varchar(10), @ReorderLevel int, @SupplierID int, @UnitPrice money, @UnitsInStock int, @UnitsOnOrder int AS
  BEGIN
    INSERT INTO Products VALUES (@ProductName, @QuantityPerUnit, @ReorderLevel, @SupplierID, @UnitPrice, @UnitsInStock, @UnitsOnOrder);
  END
GO

CREATE PROCEDURE DeleteProduct @ProductID int AS
  BEGIN
    DELETE FROM Products WHERE ProductID = @ProductID;
  END
GO

CREATE PROCEDURE UpdateProduct @ProductID int, @ProductName nvarchar(255), @QuantityPerUnit varchar(10), @ReorderLevel int, @SupplierID int, @UnitPrice money, @UnitsInStock int, @UnitsOnOrder int AS
  BEGIN
    UPDATE Products SET ProductName = @ProductName, QuantityPerUnit = @QuantityPerUnit, ReorderLevel = @ReorderLevel, SupplierID = @SupplierID, UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock, UnitsOnOrder = @UnitsOnOrder WHERE ProductID = @ProductID;
  END
GO

CREATE PROCEDURE SelectAllSuppliers AS
  BEGIN
    SELECT * FROM Suppliers;
  END
GO

CREATE PROCEDURE SelectLargestSupplier AS
  BEGIN
    SELECT S.SupplierID, S.SupplierName from Suppliers as S, 
      (SELECT TOP 1 SupplierID, COUNT(SupplierID) as times from Products group by SupplierID order by times DESC) as D
        WHERE S.SupplierID = D.SupplierID
  END
GO

CREATE PROCEDURE SelectSomeSuppliers @SearchParam nvarchar(255) AS
  BEGIN
    SELECT * FROM Suppliers 
      WHERE SupplierName LIKE '%'+@SearchParam+'%';
  END
GO

CREATE PROCEDURE InsertSupplier @SupplierName nvarchar(255) AS
  BEGIN
    INSERT INTO Suppliers VALUES (@SupplierName);
  END
GO

CREATE PROCEDURE DeleteSupplier @SupplierID int AS
  BEGIN
    DELETE FROM Suppliers WHERE SupplierID = @SupplierID;
  END
GO

CREATE PROCEDURE UpdateSupplier @SupplierID int, @SupplierName nvarchar(255) AS
  BEGIN
    UPDATE Suppliers SET SupplierName = @SupplierName WHERE SupplierID = @SupplierID;
  END
GO


EXEC InsertSupplier @SupplierName = 'Ahmad Ebeid'
EXEC InsertSupplier @SupplierName = 'Islam Ebeid'
EXEC InsertSupplier @SupplierName = 'Amr Ebeid'
EXEC InsertSupplier @SupplierName = 'Seif Ebeid'
EXEC InsertProduct @ProductName = 'Product 1', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1000, @UnitPrice = 120, @UnitsInStock = 110, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 2', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1100, @UnitPrice = 120, @UnitsInStock = 105, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 3', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1200, @UnitPrice = 120, @UnitsInStock = 101, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 4', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1300, @UnitPrice = 120, @UnitsInStock = 120, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 5', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1100, @UnitPrice = 120, @UnitsInStock = 110, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 6', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1200, @UnitPrice = 120, @UnitsInStock = 130, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 7', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1200, @UnitPrice = 120, @UnitsInStock = 103, @UnitsOnOrder = 150
EXEC InsertProduct @ProductName = 'Product 8', @QuantityPerUnit = 'Kilo', @ReorderLevel = 100, @SupplierID = 1300, @UnitPrice = 120, @UnitsInStock = 102, @UnitsOnOrder = 150
EXEC MakeOrder @ProductID = 1300, @CustomerName = 'Mark', @UnitsOrdered = 5
EXEC MakeOrder @ProductID = 1300, @CustomerName = 'Mohamed', @UnitsOrdered = 2
EXEC MakeOrder @ProductID = 1300, @CustomerName = 'Moussa', @UnitsOrdered = 10
EXEC MakeOrder @ProductID = 1100, @CustomerName = 'Ahmed', @UnitsOrdered = 4
EXEC MakeOrder @ProductID = 1200, @CustomerName = 'Amr', @UnitsOrdered = 5
EXEC MakeOrder @ProductID = 1100, @CustomerName = 'Islam', @UnitsOrdered = 10
EXEC MakeOrder @ProductID = 1800, @CustomerName = 'Ahmed', @UnitsOrdered = 2
EXEC MakeOrder @ProductID = 1800, @CustomerName = 'Moustafa', @UnitsOrdered = 10



