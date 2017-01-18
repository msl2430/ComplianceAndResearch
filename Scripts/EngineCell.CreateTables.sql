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
	[IsRecord] [bit] NOT NULL,
	[IsAverage] [bit] NOT NULL,
	[AverageSeconds] [int] NULL,
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
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](2056) NULL,
	[CreationDateTime] [datetime] NOT NULL DEFAULT(GETDATE()),
	[StartTime] [datetime] NULL,
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

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'Widget')
DROP TABLE [dbo].[Widget]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Widget](
	[WidgetId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Widget] PRIMARY KEY CLUSTERED 
([WidgetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'WidgetSetting')
DROP TABLE [dbo].[WidgetSetting]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WidgetSetting](
	[WidgetSettingId] [int] IDENTITY(1,1) NOT NULL,
	[WidgetId] [int] NOT NULL,
	[Setting] [nvarchar](256) NOT NULL
 CONSTRAINT [PK_WidgetSetting] PRIMARY KEY CLUSTERED 
([WidgetSettingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'WidgetSettingValue')
DROP TABLE [dbo].[WidgetSettingValue]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WidgetSettingValue](
	[WidgetSettingValueId] [int] IDENTITY(1,1) NOT NULL,
	[WidgetId] [int] NOT NULL,
	[CellId] [int] NOT NULL,
	[WidgetSettingId] int NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_WidgetSettingValue] PRIMARY KEY CLUSTERED 
([WidgetSettingValueId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPhase')
DROP TABLE [dbo].[CellTestPhase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPhase](
	[CellTestPhaseId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestId] [int] NOT NULL,
	[PhaseOrder] [int] NOT NULL,
	[Name] [nvarchar](256) NULL,
 CONSTRAINT [PK_CellTestPhase] PRIMARY KEY CLUSTERED 
([CellTestPhaseId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPhaseWidget')
DROP TABLE [dbo].[CellTestPhaseWidget]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPhaseWidget](
	[CellTestPhaseWidgetId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestPhaseId] [int] NOT NULL,
	[WidgetId] [int] NOT NULL,	
 CONSTRAINT [PK_CellTestPhaseWidget]PRIMARY KEY CLUSTERED 
([CellTestPhaseWidgetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPhaseWidget_Setting')
DROP TABLE [dbo].[CellTestPhaseWidget_Setting]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPhaseWidget_Setting](
	[CellTestPhaseWidgetSettingId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestPhaseWidgetId] [int] NOT NULL,
	[WidgetSettingId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_CellTestPhaseWidget_Setting]PRIMARY KEY CLUSTERED 
([CellTestPhaseWidgetSettingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPhaseTrigger')
DROP TABLE [dbo].[CellTestPhaseTrigger]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPhaseTrigger](
	[CellTestPhaseTriggerId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestPhaseId] [int] NOT NULL,
	[CellPointId] [int] NOT NULL,
	[LowValue] [decimal](14,4) NULL,
	[HighValue] [decimal](14,4) NULL,
	[SecondsThreshold] [int] NULL,
	[ResultTypeId] [int] NOT NULL,
	[ResultTypeParameter] [nvarchar](1024) NULL,
	[IsEmail] [bit] NOT NULL,
 CONSTRAINT [PK_CellTestPhaseTrigger]PRIMARY KEY CLUSTERED 
([CellTestPhaseTriggerId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'TriggerResultType')
DROP TABLE [dbo].[TriggerResultType]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TriggerResultType](
	[TriggerResultTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TriggerResultType]PRIMARY KEY CLUSTERED 
([TriggerResultTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
