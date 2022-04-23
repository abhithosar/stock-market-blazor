USE [master]
GO
/****** Object:  Database [StockMarket]    Script Date: 3/10/2022 12:03:23 AM ******/
CREATE DATABASE [StockMarket]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockMarket', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\StockMarket.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StockMarket_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\StockMarket_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [StockMarket] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockMarket].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StockMarket] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StockMarket] SET ARITHABORT OFF 
GO
ALTER DATABASE [StockMarket] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [StockMarket] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StockMarket] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StockMarket] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StockMarket] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StockMarket] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StockMarket] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StockMarket] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StockMarket] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StockMarket] SET  ENABLE_BROKER 
GO
ALTER DATABASE [StockMarket] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StockMarket] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StockMarket] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StockMarket] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StockMarket] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StockMarket] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StockMarket] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StockMarket] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StockMarket] SET  MULTI_USER 
GO
ALTER DATABASE [StockMarket] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StockMarket] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StockMarket] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StockMarket] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StockMarket] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StockMarket] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [StockMarket] SET QUERY_STORE = OFF
GO
USE [StockMarket]
GO
/****** Object:  Table [dbo].[CashLedger]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CashLedger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](200) NULL,
	[Amount] [decimal](19, 4) NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CashLedgerHistory]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CashLedgerHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](200) NULL,
	[Amount] [decimal](19, 4) NOT NULL,
	[TransactionCode] [varchar](10) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarketHours]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarketHours](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DayOfWeek] [varchar](20) NOT NULL,
	[OpeningHours] [time](7) NOT NULL,
	[ClosingHours] [time](7) NOT NULL,
	[IsMarketDay] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderHistory]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](200) NULL,
	[StockId] [int] NOT NULL,
	[OrderCode] [varchar](10) NOT NULL,
	[PurchaseCode] [varchar](10) NOT NULL,
	[StockTicker] [varchar](20) NOT NULL,
	[ExecutedPrice] [decimal](19, 4) NOT NULL,
	[Volume] [int] NOT NULL,
	[IsBuy] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[OrderId] [varchar](200) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](200) NULL,
	[StockId] [int] NOT NULL,
	[OrderCode] [varchar](10) NOT NULL,
	[PurchaseCode] [varchar](10) NOT NULL,
	[StockTicker] [varchar](20) NOT NULL,
	[OrderedPrice] [decimal](19, 4) NOT NULL,
	[Volume] [int] NOT NULL,
	[IsBuy] [int] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[OrderTotal] [decimal](19, 4) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderType]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[OrderTypeCode] [varchar](10) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portfolio]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portfolio](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](200) NULL,
	[StockId] [int] NOT NULL,
	[StockTicker] [varchar](10) NOT NULL,
	[StockAmount] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseType]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseCode] [varchar](10) NOT NULL,
	[PurchaseType] [varchar](20) NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RunningDayStockLedger]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RunningDayStockLedger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockId] [int] NOT NULL,
	[StockTicker] [varchar](20) NOT NULL,
	[Date] [datetime] NOT NULL,
	[CurrentPrice] [decimal](19, 4) NOT NULL,
	[OpenPrice] [decimal](19, 4) NOT NULL,
	[ClosePrice] [decimal](19, 4) NOT NULL,
	[DayLowPrice] [decimal](19, 4) NOT NULL,
	[DayHighPrice] [decimal](19, 4) NOT NULL,
	[DayVolume] [bigint] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockLedger]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockLedger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[OpenPrice] [decimal](19, 4) NOT NULL,
	[ClosePrice] [decimal](19, 4) NOT NULL,
	[DayLowPrice] [decimal](19, 4) NOT NULL,
	[DayHighPrice] [decimal](19, 4) NOT NULL,
	[DayVolume] [bigint] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stocks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](200) NOT NULL,
	[TickerName] [varchar](20) NOT NULL,
	[InitialPrice] [decimal](19, 4) NOT NULL,
	[CurrentPrice] [decimal](19, 4) NOT NULL,
	[RemainingStockVolume] [bigint] NOT NULL,
	[InitialStockVolume] [bigint] NOT NULL,
	[LastTradedShareVolume] [bigint] NULL,
	[IsEnabledForTrading] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionType] [varchar](20) NOT NULL,
	[TransactionCode] [varchar](10) NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/10/2022 12:03:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [varchar](200) NOT NULL,
	[UserName] [varchar](500) NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MarketHours] ON 
GO
INSERT [dbo].[MarketHours] ([Id], [DayOfWeek], [OpeningHours], [ClosingHours], [IsMarketDay], [CreatedOn], [UpdatedOn]) VALUES (1, N'Monday', CAST(N'00:27:00' AS Time), CAST(N'23:55:00' AS Time), 1, CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
INSERT [dbo].[MarketHours] ([Id], [DayOfWeek], [OpeningHours], [ClosingHours], [IsMarketDay], [CreatedOn], [UpdatedOn]) VALUES (2, N'Tuesday', CAST(N'00:00:00' AS Time), CAST(N'23:05:00' AS Time), 1, CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
INSERT [dbo].[MarketHours] ([Id], [DayOfWeek], [OpeningHours], [ClosingHours], [IsMarketDay], [CreatedOn], [UpdatedOn]) VALUES (3, N'Wednesday', CAST(N'15:00:00' AS Time), CAST(N'23:58:00' AS Time), 1, CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
INSERT [dbo].[MarketHours] ([Id], [DayOfWeek], [OpeningHours], [ClosingHours], [IsMarketDay], [CreatedOn], [UpdatedOn]) VALUES (4, N'Thursday', CAST(N'10:00:00' AS Time), CAST(N'17:00:00' AS Time), 1, CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
INSERT [dbo].[MarketHours] ([Id], [DayOfWeek], [OpeningHours], [ClosingHours], [IsMarketDay], [CreatedOn], [UpdatedOn]) VALUES (5, N'Sunday', CAST(N'00:00:00' AS Time), CAST(N'17:00:00' AS Time), 1, CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MarketHours] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderType] ON 
GO
INSERT [dbo].[OrderType] ([Id], [OrderType], [OrderTypeCode], [CreatedOn], [UpdatedOn]) VALUES (1, N'Normal', N'NOR', CAST(N'2022-02-27T21:16:30.680' AS DateTime), CAST(N'2022-02-27T21:16:30.680' AS DateTime))
GO
INSERT [dbo].[OrderType] ([Id], [OrderType], [OrderTypeCode], [CreatedOn], [UpdatedOn]) VALUES (2, N'Limit', N'LIM', CAST(N'2022-02-27T21:16:52.683' AS DateTime), CAST(N'2022-02-27T21:16:52.683' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[OrderType] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionType] ON 
GO
INSERT [dbo].[TransactionType] ([Id], [TransactionType], [TransactionCode], [UpdatedOn], [CreatedOn]) VALUES (1, N'DEPOSIT', N'DESP', CAST(N'2022-03-08T00:00:41.377' AS DateTime), CAST(N'2022-03-08T00:00:41.377' AS DateTime))
GO
INSERT [dbo].[TransactionType] ([Id], [TransactionType], [TransactionCode], [UpdatedOn], [CreatedOn]) VALUES (2, N'WITHDRAWL', N'WDRL', CAST(N'2022-03-08T00:01:06.937' AS DateTime), CAST(N'2022-03-08T00:01:06.937' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[TransactionType] OFF
GO
INSERT [dbo].[Users] ([UserId], [UserName], [UpdatedOn], [CreatedOn]) VALUES (N'896a302c-b2e0-435f-b403-08d9fecc2d7f', N'abhi', CAST(N'2022-03-07T14:15:40.900' AS DateTime), CAST(N'2022-03-07T14:15:40.900' AS DateTime))
GO
USE [master]
GO
ALTER DATABASE [StockMarket] SET  READ_WRITE 
GO
