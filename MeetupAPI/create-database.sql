/****** Object:  Database [MeetupDb]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE DATABASE [MeetupDb]
GO

USE [MeetupDb]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[__EFMigrationsHistory]
(
    [MigrationId] [nvarchar](150) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Roles]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[Roles]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] [nvarchar](max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[Users]
(
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

/****** Object:  Table [dbo].[Meetups]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[Meetups]
(
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

/****** Object:  Table [dbo].[Locations]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[Locations]
(
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

/****** Object:  Table [dbo].[Lectures]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE TABLE [dbo].[Lectures]
(
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

/****** Object:  Index [IX_Users_RoleId]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Meetups_CreatedById]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE NONCLUSTERED INDEX [IX_Meetups_CreatedById] ON [dbo].[Meetups]
(
	[CreatedById] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Locations_MeetupId]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Locations_MeetupId] ON [dbo].[Locations]
(
	[MeetupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Lectures_MeetupId]    Script Date: 4/10/2023 10:31:06 am ******/
CREATE NONCLUSTERED INDEX [IX_Lectures_MeetupId] ON [dbo].[Lectures]
(
	[MeetupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Meetups] ADD CONSTRAINT [FK_Meetups_Users_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Locations] ADD CONSTRAINT [FK_Locations_Meetups_MeetupId] FOREIGN KEY([MeetupId])
REFERENCES [dbo].[Meetups] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Lectures] ADD CONSTRAINT [FK_Lectures_Meetups_MeetupId] FOREIGN KEY([MeetupId])
REFERENCES [dbo].[Meetups] ([Id])
ON DELETE CASCADE
GO

-- Insert default roles
INSERT INTO [dbo].[Roles]
    ([RoleName])
VALUES
    ('User'),
    ('Moderator'),
    ('Admin')
GO

-- Insert default admin user
INSERT INTO [dbo].[Users]
    ([Email], [RoleId], [PasswordHash])
VALUES
    ('test-user@test.com', 3, 'AQAAAAEAACcQAAAAEEuVPeU4eVNRfWh0cxWUhwGXXUKZkYFVQhgS0O0EqwZIzGtQBxZGxaqhQyHtQNhwQw==')
GO 