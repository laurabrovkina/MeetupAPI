USE [master]
GO
/****** Object:  Database [MeetupDb]    Script Date: 28/09/2023 2:09:47 pm ******/
CREATE DATABASE [MeetupDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MeetupDb', FILENAME = N'C:\Users\Admin\MeetupDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MeetupDb_log', FILENAME = N'C:\Users\Admin\MeetupDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MeetupDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MeetupDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MeetupDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MeetupDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MeetupDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MeetupDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MeetupDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [MeetupDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MeetupDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MeetupDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MeetupDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MeetupDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MeetupDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MeetupDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MeetupDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MeetupDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MeetupDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MeetupDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MeetupDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MeetupDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MeetupDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MeetupDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MeetupDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MeetupDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MeetupDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MeetupDb] SET  MULTI_USER 
GO
ALTER DATABASE [MeetupDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MeetupDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MeetupDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MeetupDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MeetupDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MeetupDb] SET QUERY_STORE = OFF
GO
USE [MeetupDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [MeetupDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 28/09/2023 2:09:47 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lectures]    Script Date: 28/09/2023 2:09:47 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lectures](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Author] [nvarchar](max) NULL,
	[Topic] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[MeetupId] [int] NOT NULL,
 CONSTRAINT [PK_Lectures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 28/09/2023 2:09:48 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[City] [nvarchar](max) NULL,
	[Street] [nvarchar](max) NULL,
	[PostCode] [nvarchar](max) NULL,
	[MeetupId] [int] NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meetups]    Script Date: 28/09/2023 2:09:48 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meetups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Organizer] [nvarchar](max) NULL,
	[Date] [datetime2](7) NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[CreatedById] [int] NULL,
 CONSTRAINT [PK_Meetups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 28/09/2023 2:09:48 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28/09/2023 2:09:48 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Nationality] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190818100405_Init', N'3.1.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220504033440_AddUsers', N'3.1.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220721035238_MeetupCreatedById-add', N'3.1.3')
SET IDENTITY_INSERT [dbo].[Lectures] ON 

INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1, N'Bob Clark', N'Modern browsers', N'Deep dive into V8', 1)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2, N'Will Smith', N'React.js', N'Redux introduction', 2)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (3, N'John Cena', N'Angular store', N'Ngxs in practice', 2)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (4, N'Nick Chapsas', N'MediatR', N'Getting Started with MediatR and Vertical Slices in .NET', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (5, N'Nick Chapsas', N'MediatR Part 2', N'Getting Started with MediatR and Vertical Slices in .NET', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (7, N'Nick Chapsas', N'MediatR Part 3', N'Getting Started with MediatR and Vertical Slices in .NET', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (8, N'Nick C', N'MediatR', N'Getting Started with MediatR and Vertical Slices in .NET', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1004, N'string', N'string', NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1005, N'string', N'qw', NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1006, N'string', N'qw', NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1007, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1008, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1009, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1010, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1011, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1012, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1013, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1014, N'string', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1015, N'1', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1016, N'1', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1017, N'1', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1018, N'1', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1019, NULL, NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1020, N'Matt', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (1021, N'Matt', NULL, NULL, 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2011, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2012, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2013, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2014, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2015, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2016, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2017, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2018, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2019, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2020, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2021, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2022, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2023, N'string', N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2024, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2025, NULL, N'string', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2026, NULL, N'111', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2027, NULL, N'111', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2028, NULL, N'111', N'string', 3)
INSERT [dbo].[Lectures] ([Id], [Author], [Topic], [Description], [MeetupId]) VALUES (2029, NULL, N'111', N'string', 3)
SET IDENTITY_INSERT [dbo].[Lectures] OFF
SET IDENTITY_INSERT [dbo].[Locations] ON 

INSERT [dbo].[Locations] ([Id], [City], [Street], [PostCode], [MeetupId]) VALUES (1, N'Krakow', N'Szeroka 33/5', N'31-337', 1)
INSERT [dbo].[Locations] ([Id], [City], [Street], [PostCode], [MeetupId]) VALUES (2, N'Warszawa', N'Chmielna 33/5', N'00-007', 2)
SET IDENTITY_INSERT [dbo].[Locations] OFF
SET IDENTITY_INSERT [dbo].[Meetups] ON 

INSERT [dbo].[Meetups] ([Id], [Name], [Organizer], [Date], [IsPrivate], [CreatedById]) VALUES (1, N'Web summit', N'Microsoft', CAST(N'2023-08-09T13:29:37.6864843' AS DateTime2), 0, NULL)
INSERT [dbo].[Meetups] ([Id], [Name], [Organizer], [Date], [IsPrivate], [CreatedById]) VALUES (2, N'4Devs', N'KGD', CAST(N'2023-08-09T13:29:37.6871050' AS DateTime2), 0, NULL)
INSERT [dbo].[Meetups] ([Id], [Name], [Organizer], [Date], [IsPrivate], [CreatedById]) VALUES (3, N'MediatR', N'Nick Chapsas', CAST(N'2024-08-04T01:56:15.6490000' AS DateTime2), 0, 2)
SET IDENTITY_INSERT [dbo].[Meetups] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (1, N'User')
INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (2, N'Moderator')
INSERT [dbo].[Roles] ([Id], [RoleName]) VALUES (3, N'Admin')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Email], [FirstName], [LastName], [Nationality], [DateOfBirth], [PasswordHash], [RoleId]) VALUES (1, N'star@gmail.com', N'Ringo', N'Starr', N'UK', CAST(N'1937-07-10T23:28:53.7780000' AS DateTime2), N'AQAAAAIAAYagAAAAEEeS3AVpxauYWh23+ieWpnhkxfGuEhjqBu0NBfBXMLNFUsLG0Sic97rH4Stcl7+6qg==', 1)
INSERT [dbo].[Users] ([Id], [Email], [FirstName], [LastName], [Nationality], [DateOfBirth], [PasswordHash], [RoleId]) VALUES (2, N'nick@youtube.com', NULL, NULL, N'Argentinian', CAST(N'2023-08-04T01:57:47.7410000' AS DateTime2), N'AQAAAAEAACcQAAAAEKwXx0VHzLHeR+IfpskZhNk6K1X1+TUKEgseKGgAiyxURXMa8uyKWJnYAVz++tsvDQ==', 3)
INSERT [dbo].[Users] ([Id], [Email], [FirstName], [LastName], [Nationality], [DateOfBirth], [PasswordHash], [RoleId]) VALUES (1003, N'test@test.com', NULL, NULL, N'string', CAST(N'2023-08-29T04:33:55.0430000' AS DateTime2), N'AQAAAAEAACcQAAAAEAgNErRr63bV4KB0j1MAZGFL5ZQQtd9uOCouyMVbWAJ9LVwSKdPtMhT8cDU0sg8WCg==', 3)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [IX_Lectures_MeetupId]    Script Date: 28/09/2023 2:09:48 pm ******/
CREATE NONCLUSTERED INDEX [IX_Lectures_MeetupId] ON [dbo].[Lectures]
(
	[MeetupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Locations_MeetupId]    Script Date: 28/09/2023 2:09:48 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Locations_MeetupId] ON [dbo].[Locations]
(
	[MeetupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Meetups_CreatedById]    Script Date: 28/09/2023 2:09:48 pm ******/
CREATE NONCLUSTERED INDEX [IX_Meetups_CreatedById] ON [dbo].[Meetups]
(
	[CreatedById] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_RoleId]    Script Date: 28/09/2023 2:09:48 pm ******/
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Lectures]  WITH CHECK ADD  CONSTRAINT [FK_Lectures_Meetups_MeetupId] FOREIGN KEY([MeetupId])
REFERENCES [dbo].[Meetups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lectures] CHECK CONSTRAINT [FK_Lectures_Meetups_MeetupId]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Meetups_MeetupId] FOREIGN KEY([MeetupId])
REFERENCES [dbo].[Meetups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Meetups_MeetupId]
GO
ALTER TABLE [dbo].[Meetups]  WITH CHECK ADD  CONSTRAINT [FK_Meetups_Users_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Meetups] CHECK CONSTRAINT [FK_Meetups_Users_CreatedById]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles_RoleId]
GO
USE [master]
GO
ALTER DATABASE [MeetupDb] SET  READ_WRITE 
GO
