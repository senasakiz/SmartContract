USE [master]
GO
/****** Object:  Database [SMART_CONTRACT]    Script Date: 9.5.2020 18:51:01 ******/
CREATE DATABASE [SMART_CONTRACT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SMART_CONTRACT', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SMART_CONTRACT.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SMART_CONTRACT_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SMART_CONTRACT_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SMART_CONTRACT] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SMART_CONTRACT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SMART_CONTRACT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET ARITHABORT OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SMART_CONTRACT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SMART_CONTRACT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SMART_CONTRACT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SMART_CONTRACT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SMART_CONTRACT] SET  MULTI_USER 
GO
ALTER DATABASE [SMART_CONTRACT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SMART_CONTRACT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SMART_CONTRACT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SMART_CONTRACT] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SMART_CONTRACT] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SMART_CONTRACT]
GO
/****** Object:  Table [dbo].[ADDED_MONEY_IN_TL]    Script Date: 9.5.2020 18:51:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADDED_MONEY_IN_TL](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MONEY] [float] NOT NULL,
	[ADDED_TIME] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ENTITY_STATE]    Script Date: 9.5.2020 18:51:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ENTITY_STATE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ENTITY] [float] NOT NULL,
	[SIGN] [nvarchar](10) NOT NULL,
	[STATUS] [float] NOT NULL,
	[IS_ACTIVE] [bit] NOT NULL,
	[UPDATED_TIME] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EXCHANGE_CONDITION]    Script Date: 9.5.2020 18:51:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXCHANGE_CONDITION](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CURRENCY] [nvarchar](10) NOT NULL,
	[CONDITION_OPERATOR] [nvarchar](10) NOT NULL,
	[CONDITION_VALUE] [float] NOT NULL,
	[IS_ACTIVE] [bit] NOT NULL,
	[ADDED_TIME] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EXCHANGE_OPERATION]    Script Date: 9.5.2020 18:51:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXCHANGE_OPERATION](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MONEY] [nvarchar](50) NOT NULL,
	[BUY_RATE] [float] NOT NULL,
	[SELL_RATE] [float] NOT NULL,
	[CURRENT_MONEY] [nvarchar](50) NOT NULL,
	[EXCHANGED_TIME] [datetime] NOT NULL
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [SMART_CONTRACT] SET  READ_WRITE 
GO
