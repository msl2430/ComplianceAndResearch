TRUNCATE TABLE WidgetSetting
INSERT INTO WidgetSetting (WidgetId, Setting) VALUES 
(1, 'Test Schedule File'),
(2, 'Inside Thermo Couple'),
(2, 'Outside Thermo Couple'),
(2, 'Analog Output'),
(2, 'Gain'),
(2, 'Set Point'),
(3, 'Inside Thermo Couple'),
(3, 'Outside Thermo Couple'),
(3, 'Analog Output'),
(3, 'Gain'),
(3, 'Set Point'),
(4, 'Dyno PID Mode'),
(4, 'Dyno PID Measurement'),
(4, 'Dyno PID Setpoint'),
(5, 'Initial Crank Time'),
(5, 'Start Parameter'),
(5, 'Additional Crank Time')

TRUNCATE TABLE Widget
INSERT INTO Widget (Name) VALUES
('TestSchedule'),
('Ventilation Control 1'),
('Ventilation Control 2'),
('DynoPid'),
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

IF EXISTS (SELECT 1 FROM sys.tables WHERE name LIKE 'CellTestPhaseWidget_PhaseEndSetting')
DROP TABLE [dbo].[CellTestPhaseWidget_PhaseEndSetting]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CellTestPhaseWidget_PhaseEndSetting](
	[CellTestPhaseWidgetPhaseEndSettingId] [int] IDENTITY(1,1) NOT NULL,
	[CellTestPhaseWidgetId] [int] NOT NULL,
	[PhaseEndSettingId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_CellTestPhaseWidget_PhaseEndSetting]PRIMARY KEY CLUSTERED 
([CellTestPhaseWidgetPhaseEndSettingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
	[CellTestTypeId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](2056) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [PK_CellTest] PRIMARY KEY CLUSTERED 
([CellTestId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
