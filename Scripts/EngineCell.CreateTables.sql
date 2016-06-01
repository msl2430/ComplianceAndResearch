IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'Cell')
DROP TABLE [dbo].[Cell]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cell](
	[CellId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[IpAddress] [nvarchar](32) NOT NULL,
	[PortNumber] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Cell] PRIMARY KEY CLUSTERED 
([CellId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'Point')
DROP TABLE [dbo].[Point]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Point](
	[PointId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[IsInput] [bit] NOT NULL,
	[IsAnalog] [bit] NOT NULL,
	[PointGroupId] [int] NOT NULL,
 CONSTRAINT [PK_Point] PRIMARY KEY CLUSTERED 
([PointId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'PointGroup')
DROP TABLE [dbo].[PointGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PointGroup](
	[PointGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_PointGroup] PRIMARY KEY CLUSTERED 
([PointGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'Cell_Point')
DROP TABLE [dbo].[Cell_Point]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cell_Point](
	[CellPointId] [int] IDENTITY(1,1) NOT NULL,
	[CellId] [int] NOT NULL,
	[PointId] [int] NOT NULL,
	[CustomName] [nvarchar](256) NOT NULL,
	[IncludeInStripChart] [bit] NOT NULL,
	[StripChartScale] [decimal](14,6) NULL,
	[UpdateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Cell_Point] PRIMARY KEY CLUSTERED 
([CellPointId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellPointAlarm')
DROP TABLE [dbo].[CellPointAlarm]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellPointAlarm](
	[CellPointAlarmId] [int] IDENTITY(1,1) NOT NULL,
	[CellPointId] [int] NOT NULL,
	[LowShutdownWarning] [decimal](14,6) NULL,
	[LowShutdownExecute] [decimal](14,6) NULL,
	[HighShutdownWarning] [decimal](14,6) NULL,
	[HighShutdownExecute] [decimal](14,6) NULL,
	[UpdateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_PointAlarm] PRIMARY KEY CLUSTERED 
([CellPointAlarmId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTest')
DROP TABLE [dbo].[CellTest]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTest](
	[CellTestId] [int] IDENTITY(1,1) NOT NULL,
	[CellId] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [PK_CellTest] PRIMARY KEY CLUSTERED 
([CellTestId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPointData')
DROP TABLE [dbo].[CellTestPointData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPointData](
	[CellTestPointDataId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestId] [int] NOT NULL,
	[CellPointId] [int] NOT NULL,
	[Data] [decimal](14,4) NOT NULL,
	[CaptureTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TestPointData] PRIMARY KEY CLUSTERED 
([CellTestPointDataId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO