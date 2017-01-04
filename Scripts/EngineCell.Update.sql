TRUNCATE TABLE WidgetSetting
INSERT INTO WidgetSetting (WidgetId, Setting) VALUES 
(1, 'Active'),
(1, 'Inside Thermo Couple'),
(1, 'Outside Thermo Couple'),
(1, 'Analog Output'),
(1, 'Gain'),
(1, 'Set Point'),
(2, 'Active'),
(2, 'Inside Thermo Couple'),
(2, 'Outside Thermo Couple'),
(2, 'Analog Output'),
(2, 'Gain'),
(2, 'Set Point'),
(3, 'Dyno PID Mode'),
(3, 'Dyno PID Measurement'),
(3, 'Dyno PID Setpoint'),
(3, 'Active'),
(4, 'Initial Crank Time'),
(4, 'Start Parameter'),
(4, 'Additional Crank Time'),
(4, 'Active')

TRUNCATE TABLE Widget
INSERT INTO Widget (Name) VALUES
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
	[WidgetSettingId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_CellTestPhaseWidget]PRIMARY KEY CLUSTERED 
([CellTestPhaseWidgetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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


