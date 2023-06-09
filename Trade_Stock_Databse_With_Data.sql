USE [master]
GO
/****** Object:  Database [Stock_Trade]    Script Date: 13/04/2023 14:32:15 ******/
CREATE DATABASE [Stock_Trade]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Stock_Trade', FILENAME = N'D:\SQL Server Management\MSSQL15.MSSQLSERVER\MSSQL\DATA\Stock_Trade.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Stock_Trade_log', FILENAME = N'D:\SQL Server Management\MSSQL15.MSSQLSERVER\MSSQL\DATA\Stock_Trade_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Stock_Trade] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Stock_Trade].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Stock_Trade] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Stock_Trade] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Stock_Trade] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Stock_Trade] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Stock_Trade] SET ARITHABORT OFF 
GO
ALTER DATABASE [Stock_Trade] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Stock_Trade] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Stock_Trade] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Stock_Trade] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Stock_Trade] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Stock_Trade] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Stock_Trade] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Stock_Trade] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Stock_Trade] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Stock_Trade] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Stock_Trade] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Stock_Trade] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Stock_Trade] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Stock_Trade] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Stock_Trade] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Stock_Trade] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Stock_Trade] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Stock_Trade] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Stock_Trade] SET  MULTI_USER 
GO
ALTER DATABASE [Stock_Trade] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Stock_Trade] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Stock_Trade] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Stock_Trade] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Stock_Trade] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Stock_Trade] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Stock_Trade', N'ON'
GO
ALTER DATABASE [Stock_Trade] SET QUERY_STORE = OFF
GO
USE [Stock_Trade]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 13/04/2023 14:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Customer_id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_user_name] [nvarchar](50) NOT NULL,
	[Customer_name] [nvarchar](50) NOT NULL,
	[Customer_password] [nvarchar](50) NOT NULL,
	[Customer_mail] [nvarchar](50) NOT NULL,
	[Customer_tel] [nvarchar](50) NOT NULL,
	[Customer_type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED 
(
	[Customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 13/04/2023 14:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[Stock_id] [int] IDENTITY(1,1) NOT NULL,
	[Stock_value_id] [int] NOT NULL,
	[Stock_name] [nvarchar](50) NOT NULL,
	[Company_name] [nvarchar](50) NOT NULL,
	[Company_industry] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[Stock_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock_Value]    Script Date: 13/04/2023 14:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock_Value](
	[Stock_value_id] [int] IDENTITY(1,1) NOT NULL,
	[Stock_price] [float] NOT NULL,
	[Stock_high_price] [float] NOT NULL,
	[Stock_low_price] [float] NOT NULL,
 CONSTRAINT [PK_Stock_Value] PRIMARY KEY CLUSTERED 
(
	[Stock_value_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trader]    Script Date: 13/04/2023 14:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trader](
	[Trader_id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_id] [int] NOT NULL,
	[Total_value] [float] NOT NULL,
	[Balance] [float] NOT NULL,
 CONSTRAINT [PK_Trader] PRIMARY KEY CLUSTERED 
(
	[Trader_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 13/04/2023 14:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[Trader_id] [int] NOT NULL,
	[Stock_id] [int] NOT NULL,
	[Transaction_value] [float] NOT NULL,
	[Stock_quantity] [int] NOT NULL,
	[Transaction_Type] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (1, N'NurhakS', N'Nurhak', N'161545', N'numnum', N'54516456', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (2, N'Nurhak', N'NurhakSozer', N'123456', N'nurhak@yahoo.com', N'5455652', N'Business')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (3, N'Sena', N'Senanur', N'123456', N'sena00@puppy.com', N'789544', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (4, N'admin', N'admin', N'admin', N'admin@admin.com', N'657556', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (5, N'Tuncay', N'TuncaySalı', N'123', N'tuncaysalı@gmail.com', N'5523123', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (7, N'Ciguli', N'Esnaf', N'123', N'ciguli@dabulı.com', N'5523123', N'Standard                      ')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (8, N'Pamuk', N'UcanPamuk', N'123', N'ıbelıeveıcanfly@meow.com', N'8557857', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (9, N'Mehmet16', N'MehmetSozer', N'123', N'mehmet@loki.com', N'545645', N'Standard')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (10, N'Mahmut', N'Mahmut', N'123', N'mahmut@gmail.com', N'548965', N'Premium')
INSERT [dbo].[Customer] ([Customer_id], [Customer_user_name], [Customer_name], [Customer_password], [Customer_mail], [Customer_tel], [Customer_type]) VALUES (11, N'Loki', N'Loki', N'123', N'loki@gmail.com', N'25615', N'Standard')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Stock] ON 

INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (1, 1, N'TSL', N'TESLA', N'Automotive')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (3, 2, N'INTL', N'INTEL', N'Technology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (7, 4, N'AMZN', N'Amazon', N'Commerce')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (8, 5, N'Alphabet', N'Google', N'Technology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (9, 6, N'META', N'Meta', N'Technology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (11, 7, N'MSFT', N'Microsoft', N'Techonology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (12, 8, N'NVDA', N'NVIDIA', N'Technology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (13, 9, N'AAPL', N'Apple', N'Technology')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (14, 10, N'MCD', N'McDonald', N'Food')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (15, 11, N'JPM', N'JPMorgan', N'Finance')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (16, 12, N'WMT', N'Walmart', N'Commerce')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (17, 13, N'NKE', N'Nike', N'Clothing')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (18, 14, N'PEP', N'PepsiCo', N'Beverage')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (19, 15, N'PFE', N'Pfizer', N'Medical')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (20, 16, N'SBUX', N'Starbucks', N'Beverage')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (21, 17, N'ADBE', N'Adobe', N'Software')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (22, 18, N'F', N'Ford', N'Automotive')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (23, 19, N'AAL', N'AmericanAirlines', N'Transport')
INSERT [dbo].[Stock] ([Stock_id], [Stock_value_id], [Stock_name], [Company_name], [Company_industry]) VALUES (24, 20, N'NFLX', N'Netflix', N'Entertainment')
SET IDENTITY_INSERT [dbo].[Stock] OFF
GO
SET IDENTITY_INSERT [dbo].[Stock_Value] ON 

INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (1, 15, 19, 11)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (2, 78, 86, 56)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (4, 25, 42, 18)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (5, 56, 63, 50)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (6, 214, 224, 88)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (7, 283, 294, 213)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (8, 264, 280, 108)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (9, 160, 176, 124)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (10, 285, 302, 228)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (11, 128, 144, 101)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (12, 149, 160, 117)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (13, 123, 139, 82)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (14, 182, 186, 154)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (15, 41, 54, 39)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (16, 105, 110, 68)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (17, 369, 451, 274)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (18, 12, 16, 10)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (19, 13, 21, 11)
INSERT [dbo].[Stock_Value] ([Stock_value_id], [Stock_price], [Stock_high_price], [Stock_low_price]) VALUES (20, 331, 379, 162)
SET IDENTITY_INSERT [dbo].[Stock_Value] OFF
GO
SET IDENTITY_INSERT [dbo].[Trader] ON 

INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1010, 2, 411, 1769)
INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1011, 8, 590, 590)
INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1012, 9, 780, 780)
INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1013, 10, 0, 0)
INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1014, 11, 0, 0)
INSERT [dbo].[Trader] ([Trader_id], [Customer_id], [Total_value], [Balance]) VALUES (1015, 5, 0, 0)
SET IDENTITY_INSERT [dbo].[Trader] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (2, 1010, 3, 390, 5, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (3, 1010, 7, 25, 4, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (12, 1010, 1, 75, 5, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (20, 1010, 7, 200, 8, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (21, 1010, 7, 50, 2, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (27, 1010, 1, 30, 2, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (32, 1010, 1, 150, 10, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (33, 1010, 1, 150, 10, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (35, 1010, 1, 150, 10, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (36, 1010, 1, 45, 3, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (37, 1010, 1, 150, 10, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (38, 1010, 1, 75, 5, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (39, 1010, 1, 75, 5, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (40, 1010, 1, 60, 4, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (41, 1010, 3, 390, 5, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (42, 1010, 1, 60, 4, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (43, 1010, 1, 75, 5, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (44, 1010, 7, 350, 14, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (45, 1010, 8, 336, 6, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (46, 1010, 3, 78, 1, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (47, 1010, 3, 78, 1, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (48, 1010, 1, 30, 2, N'Sell      ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (49, 1011, 3, 390, 5, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (50, 1011, 7, 200, 8, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (51, 1012, 3, 780, 10, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (52, 1015, 7, 75, 3, N'Buy       ')
INSERT [dbo].[Transaction] ([Transaction_id], [Trader_id], [Stock_id], [Transaction_value], [Stock_quantity], [Transaction_Type]) VALUES (53, 1012, 3, 312, 4, N'Buy       ')
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_Customer_type]  DEFAULT (N'Standard') FOR [Customer_type]
GO
ALTER TABLE [dbo].[Trader] ADD  DEFAULT ((0)) FOR [Total_value]
GO
ALTER TABLE [dbo].[Trader] ADD  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Stock_Value1] FOREIGN KEY([Stock_value_id])
REFERENCES [dbo].[Stock_Value] ([Stock_value_id])
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Stock_Value1]
GO
ALTER TABLE [dbo].[Trader]  WITH CHECK ADD  CONSTRAINT [FK_Trader_Customer] FOREIGN KEY([Customer_id])
REFERENCES [dbo].[Customer] ([Customer_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trader] CHECK CONSTRAINT [FK_Trader_Customer]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Stock] FOREIGN KEY([Stock_id])
REFERENCES [dbo].[Stock] ([Stock_id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Stock]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Trader] FOREIGN KEY([Trader_id])
REFERENCES [dbo].[Trader] ([Trader_id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Trader]
GO
USE [master]
GO
ALTER DATABASE [Stock_Trade] SET  READ_WRITE 
GO
