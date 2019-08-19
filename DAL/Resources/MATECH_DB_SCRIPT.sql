USE [master]
GO
/****** Object:  Database [MATECH]    Script Date: 7/4/2019 2:24:20 AM ******/
CREATE DATABASE [MATECH]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MATECH', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\MATECH.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MATECH_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\MATECH_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MATECH] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MATECH].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MATECH] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MATECH] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MATECH] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MATECH] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MATECH] SET ARITHABORT OFF 
GO
ALTER DATABASE [MATECH] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MATECH] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MATECH] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MATECH] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MATECH] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MATECH] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MATECH] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MATECH] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MATECH] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MATECH] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MATECH] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MATECH] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MATECH] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MATECH] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MATECH] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MATECH] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MATECH] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MATECH] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MATECH] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MATECH] SET  MULTI_USER 
GO
ALTER DATABASE [MATECH] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MATECH] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MATECH] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MATECH] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [MATECH]
GO
/****** Object:  Table [dbo].[tblActionEndPoint]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblActionEndPoint](
	[EndPointID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionActionID] [int] NOT NULL,
	[APIController] [varchar](50) NULL,
	[APIName] [varchar](50) NULL,
 CONSTRAINT [PK_tblActionEndPoint] PRIMARY KEY CLUSTERED 
(
	[EndPointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblAppClient]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppClient](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppName] [nvarchar](50) NULL,
	[AppID] [nvarchar](50) NULL,
	[AppSecret] [nvarchar](150) NULL,
	[CallBackUrl] [nvarchar](250) NULL,
	[Created] [datetime2](7) NULL,
 CONSTRAINT [PK_tblAppClient] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblModule]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblModule](
	[ModuleID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[ModuleOrder] [int] NULL,
	[ModuleIcon] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[IsChargeable] [bit] NOT NULL,
	[Charges] [decimal](18, 0) NULL,
 CONSTRAINT [PK_tblModule] PRIMARY KEY CLUSTERED 
(
	[ModuleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblPage]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPage](
	[PageID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[PageName] [varchar](50) NULL,
	[PageUrl] [varchar](50) NULL,
	[PageOrder] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[PageIcon] [varchar](50) NULL,
	[ShowOnMenu] [bit] NOT NULL,
	[Controller] [varchar](50) NULL,
 CONSTRAINT [PK_tblPages] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblPermission]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPermission](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[Permission] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblPermission] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblPermissionActionJunc]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPermissionActionJunc](
	[PermissionActionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[Action] [varchar](100) NOT NULL,
	[IsLandingAction] [int] NULL,
 CONSTRAINT [PK_tblPermissionActionJunc] PRIMARY KEY CLUSTERED 
(
	[PermissionActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblProduct]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblProduct](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ProductCode] [varchar](10) NULL,
	[ProductTypeID] [int] NULL,
	[SKU] [varchar](50) NULL,
	[Price] [decimal](18, 2) NULL,
	[Description] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[Created] [datetime2](7) NULL,
	[Updated] [datetime2](7) NULL,
	[Deleted] [datetime2](7) NULL,
 CONSTRAINT [PK_tblProduct] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblProductType]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblProductType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NULL,
	[Initials] [varchar](10) NULL,
 CONSTRAINT [PK_tblProductType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblRole]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblRole](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[Created] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[Updated] [datetime2](7) NULL,
	[DeletedBy] [int] NULL,
	[Deleted] [datetime2](7) NULL,
 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblRolePermissionJunc]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRolePermissionJunc](
	[RolePermissionJuncID] [bigint] IDENTITY(1,1) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_tblRolePermissionJunc] PRIMARY KEY CLUSTERED 
(
	[RolePermissionJuncID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserLoginID] [varchar](50) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[Password] [varchar](100) NULL,
	[ReportingTo] [int] NULL,
	[UserType] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreateBy] [int] NULL,
	[UpdateBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[Deleted] [datetime2](7) NULL,
	[Closed] [datetime2](7) NULL,
	[ClosedBy] [int] NULL,
	[LastPasswordChanged] [datetime2](7) NULL,
	[LandingPage] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblUserRole]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_tblUserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUserTokenExpire]    Script Date: 7/4/2019 2:24:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserTokenExpire](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Token] [nvarchar](300) NULL,
	[TokenGenerated] [datetime2](7) NULL,
	[Created] [datetime2](7) NULL,
 CONSTRAINT [PK_tblUserTokenExpire] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[tblActionEndPoint] ON 

GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (1, 1, N'Role', N'getAsyncRoles')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (2, 2, N'Role', N'getRoles')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (3, 3, N'Role', N'createRole')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (4, 4, N'Role', N'updateRole')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (5, 5, N'Role', N'deleteRole')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (6, 6, N'Role', N'getAllPermissions')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (7, 7, N'Role', N'updateRolePermission')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (8, 8, N'User', N'getUsers')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (9, 9, N'User', N'getUsers')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (10, 10, N'User', N'createUser')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (11, 11, N'User', N'updateUser')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (12, 12, N'User', N'deleteUser')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (13, 13, N'User', N'isUsernameAailable')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (14, 14, N'Product', N'getProducts')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (15, 15, N'Product', N'getProducts')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (18, 18, N'Product', N'createProduct')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (19, 19, N'Product', N'updateProduct')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (20, 20, N'Product', N'deleteProduct')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (21, 18, N'Product', N'getProductTypeList')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (22, 19, N'Product', N'getProductTypeList')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (23, 10, N'Role', N'getAsyncRoles')
GO
INSERT [dbo].[tblActionEndPoint] ([EndPointID], [PermissionActionID], [APIController], [APIName]) VALUES (24, 11, N'Role', N'getAsyncRoles')
GO
SET IDENTITY_INSERT [dbo].[tblActionEndPoint] OFF
GO
SET IDENTITY_INSERT [dbo].[tblAppClient] ON 

GO
INSERT [dbo].[tblAppClient] ([ID], [AppName], [AppID], [AppSecret], [CallBackUrl], [Created]) VALUES (1, N'MATECH Portal', N'MATECHapp', N'MATECHappsecret', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblAppClient] OFF
GO
SET IDENTITY_INSERT [dbo].[tblModule] ON 

GO
INSERT [dbo].[tblModule] ([ModuleID], [ModuleName], [IsActive], [ModuleOrder], [ModuleIcon], [ParentID], [IsChargeable], [Charges]) VALUES (1, N'Setup', 1, 1, NULL, 0, 0, CAST(0 AS Decimal(18, 0)))
GO
INSERT [dbo].[tblModule] ([ModuleID], [ModuleName], [IsActive], [ModuleOrder], [ModuleIcon], [ParentID], [IsChargeable], [Charges]) VALUES (2, N'Product', 1, 2, NULL, 0, 0, CAST(0 AS Decimal(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[tblModule] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPage] ON 

GO
INSERT [dbo].[tblPage] ([PageID], [ModuleID], [PageName], [PageUrl], [PageOrder], [IsActive], [PageIcon], [ShowOnMenu], [Controller]) VALUES (1, 1, N'Role', N'/Role', 1, 1, NULL, 1, N'Role')
GO
INSERT [dbo].[tblPage] ([PageID], [ModuleID], [PageName], [PageUrl], [PageOrder], [IsActive], [PageIcon], [ShowOnMenu], [Controller]) VALUES (2, 1, N'User', N'/User', 2, 1, NULL, 1, N'User')
GO
INSERT [dbo].[tblPage] ([PageID], [ModuleID], [PageName], [PageUrl], [PageOrder], [IsActive], [PageIcon], [ShowOnMenu], [Controller]) VALUES (4, 2, N'Product', N'/Product', 1, 1, NULL, 1, N'Product')
GO
SET IDENTITY_INSERT [dbo].[tblPage] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPermission] ON 

GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (1, 1, N'View Role List', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (2, 1, N'View Role Detail', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (3, 1, N'Create Role', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (4, 1, N'Update Role', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (5, 1, N'Delete Role', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (6, 1, N'View Role Permission', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (7, 1, N'Update Role Permission', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (8, 2, N'View User List', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (9, 2, N'View User Detail', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (10, 2, N'Create User', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (11, 2, N'Update User', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (12, 2, N'Delete User', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (13, 4, N'View Product List', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (14, 4, N'View Product Detail', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (16, 4, N'Create Product', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (17, 4, N'Update Product', 1)
GO
INSERT [dbo].[tblPermission] ([PermissionID], [PageID], [Permission], [IsActive]) VALUES (18, 4, N'Delete Product', 1)
GO
SET IDENTITY_INSERT [dbo].[tblPermission] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPermissionActionJunc] ON 

GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (1, 1, N'Index', 1)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (2, 2, N'Detail', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (3, 3, N'Save', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (4, 4, N'Update', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (5, 5, N'Delete', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (6, 6, N'RolePermission', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (7, 7, N'SavePermission', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (8, 8, N'Index', 1)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (9, 9, N'Detail', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (10, 10, N'Save', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (11, 11, N'Update', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (12, 12, N'Delete', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (13, 10, N'isUsernameAailable', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (14, 13, N'Index', 1)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (15, 14, N'Detail', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (18, 16, N'Save', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (19, 17, N'Update', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (20, 18, N'Delete', NULL)
GO
INSERT [dbo].[tblPermissionActionJunc] ([PermissionActionID], [PermissionID], [Action], [IsLandingAction]) VALUES (21, 10, N'getLandingPageList', NULL)
GO
SET IDENTITY_INSERT [dbo].[tblPermissionActionJunc] OFF
GO
SET IDENTITY_INSERT [dbo].[tblProductType] ON 

GO
INSERT [dbo].[tblProductType] ([ID], [TypeName], [Initials]) VALUES (1, N'A', N'ABC')
GO
INSERT [dbo].[tblProductType] ([ID], [TypeName], [Initials]) VALUES (2, N'B', N'BCD')
GO
INSERT [dbo].[tblProductType] ([ID], [TypeName], [Initials]) VALUES (3, N'C', N'CDE')
GO
SET IDENTITY_INSERT [dbo].[tblProductType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblRole] ON 

GO
INSERT [dbo].[tblRole] ([RoleID], [RoleName], [IsActive], [CreatedBy], [Created], [UpdatedBy], [Updated], [DeletedBy], [Deleted]) VALUES (1, N'Admin', 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblRole] ([RoleID], [RoleName], [IsActive], [CreatedBy], [Created], [UpdatedBy], [Updated], [DeletedBy], [Deleted]) VALUES (2, N'Matech', 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblRole] OFF
GO
SET IDENTITY_INSERT [dbo].[tblRolePermissionJunc] ON 

GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (145, 1, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (146, 2, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (147, 3, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (148, 4, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (149, 5, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (150, 6, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (151, 7, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (152, 8, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (153, 9, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (154, 10, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (155, 11, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (156, 12, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (157, 13, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (158, 14, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (159, 16, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (160, 17, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (161, 18, 1)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (162, 8, 2)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (163, 13, 2)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (164, 14, 2)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (165, 16, 2)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (166, 17, 2)
GO
INSERT [dbo].[tblRolePermissionJunc] ([RolePermissionJuncID], [PermissionID], [RoleID]) VALUES (167, 18, 2)
GO
SET IDENTITY_INSERT [dbo].[tblRolePermissionJunc] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUser] ON 

GO
INSERT [dbo].[tblUser] ([UserID], [UserLoginID], [FirstName], [LastName], [Phone], [Email], [Password], [ReportingTo], [UserType], [IsActive], [CreateBy], [UpdateBy], [DeletedBy], [CreateDate], [UpdateDate], [Deleted], [Closed], [ClosedBy], [LastPasswordChanged], [LandingPage]) VALUES (1, N'admin', N'MATECH', N'Admin', NULL, N'admin@admin.com', N'UOyOp30cJEJZwh29p00sEGdvbGRMZWFm', NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'/Role')
GO
INSERT [dbo].[tblUser] ([UserID], [UserLoginID], [FirstName], [LastName], [Phone], [Email], [Password], [ReportingTo], [UserType], [IsActive], [CreateBy], [UpdateBy], [DeletedBy], [CreateDate], [UpdateDate], [Deleted], [Closed], [ClosedBy], [LastPasswordChanged], [LandingPage]) VALUES (3, N'matech', N'matech', N'user', N'033333122312', N'secondary@admin.com', N'hZPugKfCs8gl3see0H1ttGdvbGRMZWFm', NULL, 1, 1, 1, NULL, NULL, CAST(0x070BB1213B08DB3F0B AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'/User')
GO
SET IDENTITY_INSERT [dbo].[tblUser] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUserRole] ON 

GO
INSERT [dbo].[tblUserRole] ([UserRoleID], [UserID], [RoleID]) VALUES (1, 1, 1)
GO
INSERT [dbo].[tblUserRole] ([UserRoleID], [UserID], [RoleID]) VALUES (2, 3, 2)
GO
SET IDENTITY_INSERT [dbo].[tblUserRole] OFF
GO
ALTER TABLE [dbo].[tblActionEndPoint]  WITH CHECK ADD  CONSTRAINT [FK_tblActionEndPoint_tblPermissionActionJunc] FOREIGN KEY([PermissionActionID])
REFERENCES [dbo].[tblPermissionActionJunc] ([PermissionActionID])
GO
ALTER TABLE [dbo].[tblActionEndPoint] CHECK CONSTRAINT [FK_tblActionEndPoint_tblPermissionActionJunc]
GO
ALTER TABLE [dbo].[tblPage]  WITH CHECK ADD  CONSTRAINT [FK_tblPage_tblModule] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[tblModule] ([ModuleID])
GO
ALTER TABLE [dbo].[tblPage] CHECK CONSTRAINT [FK_tblPage_tblModule]
GO
ALTER TABLE [dbo].[tblPermission]  WITH CHECK ADD  CONSTRAINT [FK_tblPermission_tblPage] FOREIGN KEY([PageID])
REFERENCES [dbo].[tblPage] ([PageID])
GO
ALTER TABLE [dbo].[tblPermission] CHECK CONSTRAINT [FK_tblPermission_tblPage]
GO
ALTER TABLE [dbo].[tblPermissionActionJunc]  WITH CHECK ADD  CONSTRAINT [FK_tblPermissionActionJunc_tblPermission] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[tblPermission] ([PermissionID])
GO
ALTER TABLE [dbo].[tblPermissionActionJunc] CHECK CONSTRAINT [FK_tblPermissionActionJunc_tblPermission]
GO
ALTER TABLE [dbo].[tblProduct]  WITH CHECK ADD  CONSTRAINT [FK_tblProduct_tblProductType] FOREIGN KEY([ProductTypeID])
REFERENCES [dbo].[tblProductType] ([ID])
GO
ALTER TABLE [dbo].[tblProduct] CHECK CONSTRAINT [FK_tblProduct_tblProductType]
GO
ALTER TABLE [dbo].[tblRolePermissionJunc]  WITH CHECK ADD  CONSTRAINT [FK_tblRolePermissionJunc_tblPermission] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[tblPermission] ([PermissionID])
GO
ALTER TABLE [dbo].[tblRolePermissionJunc] CHECK CONSTRAINT [FK_tblRolePermissionJunc_tblPermission]
GO
ALTER TABLE [dbo].[tblRolePermissionJunc]  WITH CHECK ADD  CONSTRAINT [FK_tblRolePermissionJunc_tblRole] FOREIGN KEY([RoleID])
REFERENCES [dbo].[tblRole] ([RoleID])
GO
ALTER TABLE [dbo].[tblRolePermissionJunc] CHECK CONSTRAINT [FK_tblRolePermissionJunc_tblRole]
GO
ALTER TABLE [dbo].[tblUserRole]  WITH CHECK ADD  CONSTRAINT [FK_tblUserRole_tblRole] FOREIGN KEY([RoleID])
REFERENCES [dbo].[tblRole] ([RoleID])
GO
ALTER TABLE [dbo].[tblUserRole] CHECK CONSTRAINT [FK_tblUserRole_tblRole]
GO
ALTER TABLE [dbo].[tblUserRole]  WITH CHECK ADD  CONSTRAINT [FK_tblUserRole_tblUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUser] ([UserID])
GO
ALTER TABLE [dbo].[tblUserRole] CHECK CONSTRAINT [FK_tblUserRole_tblUser]
GO
USE [master]
GO
ALTER DATABASE [MATECH] SET  READ_WRITE 
GO
