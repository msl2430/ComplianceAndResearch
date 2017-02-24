IF NOT EXISTS (SELECT 1 FROM Cell WHERE CellId = 2) 
BEGIN
	INSERT INTO Cell VALUES ('John Deere Genset', '98.109.58.113', '22001', 'John Deere GenSet')
END

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



UPDATE cell SET name = 'Engine Cell 5', Description = 'Engine Cell 5' WHERE CellId = 1

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





IF NOT EXISTS (SELECT 1 FROM Cell_Point) 
BEGIN
INSERT INTO Cell_Point (CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, IsActive, UpdateDateTime)
SELECT 1, PointId, Name, 0, 0, NULL, 0, NULL, 1, GETDATE()
FROM Point

END

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



IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellPointAlarm')
BEGIN
	DROP TABLE CellPointAlarm
END

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'Cell_Point2')
DROP TABLE [dbo].[Cell_Point2]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cell_Point2](
	[CellPointId] [int] IDENTITY(1,1) NOT NULL,
	[CellId] [int] NOT NULL,
	[PointId] [int] NOT NULL,
	[CustomName] [nvarchar](256) NOT NULL,
	[IsRecord] [bit] NOT NULL,
	[IsAverage] [bit] NOT NULL,
	[AverageSeconds] [int] NULL,
	[IncludeInStripChart] [bit] NOT NULL,
	[StripChartScale] [decimal](14,6) NULL,
	[IsActive] [bit] NOT NULL,
	[UpdateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Cell_Point] PRIMARY KEY CLUSTERED 
([CellPointId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


