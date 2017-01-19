TRUNCATE TABLE WidgetSetting
INSERT INTO WidgetSetting (WidgetId, Setting) VALUES 
(1, 'Test Schedule File'),
(1, 'Test Schedule Timeout'),
(2, 'Dyno Ramp Setpoint'),
(2, 'Dyno Ramp Time'),
(3, 'Throttle Ramp Setpoint'),
(3, 'Throttle Ramp Time'),
(9, 'Inside Thermo Couple'),
(9, 'Outside Thermo Couple'),
(9, 'Analog Output'),
(9, 'Gain'),
(9, 'Set Point'),
(10, 'Inside Thermo Couple'),
(10, 'Outside Thermo Couple'),
(10, 'Analog Output'),
(10, 'Gain'),
(10, 'Set Point'),
(11, 'Initial Crank Time'),
(11, 'Start Parameter'),
(11, 'Additional Crank Time')

TRUNCATE TABLE Widget
INSERT INTO Widget (Name) VALUES
('TestSchedule'),
('Dyno Ramp'),
('Throttle Ramp'),
('Custom Chart 1'),
('Custom Chart 2'),
('Custom Chart 3'),
('Custom Chart 4'),
('Custom Chart 5'),
('Ventilation Control 1'),
('Ventilation Control 2'),
('Starter')

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

IF NOT EXISTS (SELECT 1 FROM Point WHERE Name Like 'DynoIn%') 
BEGIN
INSERT INTO Point (Name, IsInput, IsAnalog, PointGroupId) VALUES
('DynoIn0', 1, 0, 11),
('DynoIn1', 1, 0, 11),
('DynoIn2', 1, 0, 11),
('DynoIn3', 1, 0, 11),
('DynoIn4', 1, 0, 11),
('DynoIn5', 1, 0, 11),
('DynoIn6', 1, 0, 11),
('DynoIn7', 1, 0, 11)
END

IF NOT EXISTS (SELECT 1 FROM Point WHERE Name Like 'EngineIn%') 
BEGIN
INSERT INTO Point (Name, IsInput, IsAnalog, PointGroupId) VALUES
('EngineIn0', 1, 0, 12),
('EngineIn1', 1, 0, 12),
('EngineIn2', 1, 0, 12),
('EngineIn3', 1, 0, 12),
('EngineIn4', 1, 0, 12),
('EngineIn5', 1, 0, 12),
('EngineIn6', 1, 0, 12),
('EngineIn7', 1, 0, 12)
END

IF NOT EXISTS (SELECT 1 FROM Cell_Point WHERE PointId IN (SELECT PointId FROM Point WHERE PointGroupId IN (11,12))) 
BEGIN
INSERT INTO Cell_Point (CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, UpdateDateTime)
SELECT 1, PointId, Name, 0, 0, NULL, 0, NULL, GETDATE()
FROM Point
WHERE PointGroupId IN (11,12)
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

TRUNCATE TABLE TriggerResultType
INSERT INTO TriggerResultType (Name, Description) VALUES
('GoTo Phase', 'Go to the phase specified'),
('Shutdown', 'Shutdown entire test')

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

SET IDENTITY_INSERT Cell_Point2 ON;

INSERT INTO Cell_Point2 (CellPointId, CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, IsActive, UpdateDateTime)
SELECT CellPointId, CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, 1, UpdateDateTime
FROM Cell_Point

SET IDENTITY_INSERT Cell_Point2 OFF

EXEC sp_rename 'Cell_Point', 'Cell_Point_old'
EXEC sp_rename 'Cell_Point2', 'Cell_Point'

DROP TABLE Cell_Point_old