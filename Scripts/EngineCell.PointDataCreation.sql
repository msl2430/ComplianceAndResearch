TRUNCATE TABLE WidgetSetting
INSERT INTO WidgetSetting (WidgetId, Setting) VALUES 
(1, 'Test Schedule File'),
(1, 'Test Schedule Timeout'),
(2, 'Dyno Ramp Setpoint'),
(2, 'Dyno Ramp Time'),
(2, 'Dyno Ramp Mode'),
(3, 'Throttle Ramp Setpoint'),
(3, 'Throttle Ramp Time'),
(3, 'Throttle Ramp Mode'),
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
(11, 'Crank Time'),
(11, 'Start RPM'),
(11, 'Time Between Tries'),
(11, 'Number of Tries'),
(11, 'Time at RPM'),
(11, 'Timeout'),
(12, 'Expiration')

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
('Starter'),
('Timer'),
('AIRate')


TRUNCATE TABLE PointGroup
	INSERT INTO PointGroup VALUES ('ThermoCouple')
	INSERT INTO PointGroup VALUES ('Volt')
	INSERT INTO PointGroup VALUES ('MilliAmp')
	INSERT INTO PointGroup VALUES ('HART')
	INSERT INTO PointGroup VALUES ('AnalogOutput')
	INSERT INTO PointGroup VALUES ('DigitalOut')
	INSERT INTO PointGroup VALUES ('DigitalIn')
	INSERT INTO PointGroup VALUES ('Frequency')
	INSERT INTO PointGroup VALUES ('LoadCell')

TRUNCATE TABLE Point
	INSERT INTO Point VALUES ('ThermoCouple0',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple1',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple2',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple3',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple4',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple5',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple6',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple7',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple8',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple9',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple10',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple11',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple12',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple13',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple14',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple15',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple16',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple17',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple18',1,1,1)
	INSERT INTO Point VALUES ('ThermoCouple19',1,1,1)

	INSERT INTO Point VALUES ('Volt0',1,1,2)
	INSERT INTO Point VALUES ('Volt1',1,1,2)
	INSERT INTO Point VALUES ('Volt2',1,1,2)
	INSERT INTO Point VALUES ('Volt3',1,1,2)
	INSERT INTO Point VALUES ('Volt4',1,1,2)
	INSERT INTO Point VALUES ('Volt5',1,1,2)

	INSERT INTO Point VALUES ('MilliAmp0',1,1,3)
	INSERT INTO Point VALUES ('MilliAmp1',1,1,3)

	INSERT INTO Point VALUES ('HART0',1,1,4)
	INSERT INTO Point VALUES ('HART1',1,1,4)

	INSERT INTO Point VALUES ('AO10_0',0,1,5)
	INSERT INTO Point VALUES ('AO10_1',0,1,5)

	INSERT INTO Point VALUES ('AO5_0',0,1,5)
	INSERT INTO Point VALUES ('AO5_0',0,1,5)

	INSERT INTO Point VALUES ('AO4_20_0',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_1',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_2',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_3',0,1,5)

	INSERT INTO Point VALUES ('DigitalOut0',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut1',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut2',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut3',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut4',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut5',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut6',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut7',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut8',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut9',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut10',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut11',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut12',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut13',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut14',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut15',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut16',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut17',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut18',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut19',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut20',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut21',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut22',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut23',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut24',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut25',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut26',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut27',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut28',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut29',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut30',0,0,6)
	INSERT INTO Point VALUES ('DigitalOut31',0,0,6)

	INSERT INTO Point VALUES ('Freq0',1,1,8)
	INSERT INTO Point VALUES ('Freq1',1,1,8)
	--INSERT INTO Point VALUES ('Freq2',1,0,8)
	--INSERT INTO Point VALUES ('Freq3',1,0,8)
	INSERT INTO Point Values ('LC_BandStop', 1, 0, 9)
	INSERT INTO Point Values ('LC_BandPass', 1, 1, 9)
	
	INSERT INTO Point (Name, IsInput, IsAnalog, PointGroupId) VALUES
	('DigitalIn0', 1, 0, 7),
	('DigitalIn1', 1, 0, 7),
	('DigitalIn2', 1, 0, 7),
	('DigitalIn3', 1, 0, 7),
	('DigitalIn4', 1, 0, 7),
	('DigitalIn5', 1, 0, 7),
	('DigitalIn6', 1, 0, 7),
	('DigitalIn7', 1, 0, 7),
	('DigitalIn8', 1, 0, 7),
	('DigitalIn9', 1, 0, 7),
	('DigitalIn10', 1, 0, 7),
	('DigitalIn11', 1, 0, 7),
	('DigitalIn12', 1, 0, 7),
	('DigitalIn13', 1, 0, 7),
	('DigitalIn14', 1, 0, 7),
	('DigitalIn15', 1, 0, 7),
	('DigitalIn16', 1, 0, 7),
	('DigitalIn17', 1, 0, 7),
	('DigitalIn18', 1, 0, 7),
	('DigitalIn19', 1, 0, 7),
	('Rate0', 1, 1, 10),
	('Rate1', 1, 1, 10)
GO

TRUNCATE TABLE TriggerResultType
INSERT INTO TriggerResultType (Name, Description) VALUES
('GoTo Phase', 'Go to the phase specified'),
('Shutdown', 'Shutdown entire test')


TRUNCATE TABLE Cell_Point
--Engine Cell
INSERT INTO Cell_Point (CellId, PointId, CustomName, IncludeInStripChart, IsRecord, IsAverage, IsActive, UpdateDateTime) 
SELECT 1, PointId, Name, 0, 0, 0, 1, GETUTCDATE()
FROM Point
WHERE PointId NOT IN (95, 96)

--John Deere
INSERT INTO Cell_Point (CellId, PointId, CustomName, IncludeInStripChart, UpdateDateTime, IsRecord, IsAverage, IsActive) VALUES
(2, 1, 'ThermoCouple0', 0, GETUTCDATE(), 0, 0,1), 
(2, 2, 'ThermoCouple1', 0, GETUTCDATE(), 0, 0,1),
(2, 3, 'ThermoCouple2', 0, GETUTCDATE(), 0, 0,1),
(2, 4, 'ThermoCouple3', 0, GETUTCDATE(), 0, 0,1),
(2, 5, 'ThermoCouple4', 0, GETUTCDATE(), 0, 0,1),
(2, 6, 'ThermoCouple5', 0, GETUTCDATE(), 0, 0,1),
(2, 7, 'ThermoCouple6', 0, GETUTCDATE(), 0, 0,1),
(2, 8, 'ThermoCouple7', 0, GETUTCDATE(), 0, 0,1),
(2, 9, 'ThermoCouple8', 0, GETUTCDATE(), 0, 0,1),
(2, 10, 'ThermoCouple9', 0, GETUTCDATE(), 0, 0,1),
(2, 11, 'ThermoCouple10', 0, GETUTCDATE(), 0, 0,1),
(2, 12, 'ThermoCouple11', 0, GETUTCDATE(), 0, 0,1),
(2, 21, 'Volt0', 0, GETUTCDATE(), 0, 0,1),
(2, 22, 'Volt1', 0, GETUTCDATE(), 0, 0,1),
(2, 23, 'Volt2', 0, GETUTCDATE(), 0, 0,1),
(2, 24, 'Volt3', 0, GETUTCDATE(), 0, 0,1),
(2, 27, 'MilliAmp0', 0, GETUTCDATE(), 0, 0,1),
(2, 28, 'MilliAmp1', 0, GETUTCDATE(), 0, 0,1),
(2, 35, 'AO_4_20_0', 0, GETUTCDATE(), 0, 0,1),
(2, 36, 'AO_4_20_1', 0, GETUTCDATE(), 0, 0,1),
(2, 95, 'Rate0', 0, GETUTCDATE(), 0, 0,1),
(2, 96, 'Rate1', 0, GETUTCDATE(), 0, 0,1)
GO

UPDATE Cell_Point SET CustomName = 'Coolant0' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Coolant1' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple1') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'OilSump' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple2') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'OilFileter' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple3') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'DynoIn' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple4') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'DynoOut' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple5') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'HeadExchanger' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple6') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'IntakeAir' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple7') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'PreIntercooler' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple8') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'PostIntercooler' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple9') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Exhaust' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple10') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'EngineFuelPumpIn' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple11') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'FuelHeatExchangerIn' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple12') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'FuelHeatExchangerOut' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple13') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'DynoBearingFront' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple14') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'DynoBearingRear' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple15') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'IntakeManifold' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'ThermoCouple16') AND CellId = 1


UPDATE Cell_Point SET CustomName = 'OilPressure' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'Volt1') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'ManifoldPressure' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'Volt2') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'CrankCasePressue' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'Volt3') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'BarometricPressure' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'MilliAmp0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'FuelFlow' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'HART0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'StarterCrank' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'DigitalOut0') AND CellId = 1
--UPDATE Cell_Point SET CustomName = 'Dyno' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO10_0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Throttle' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO10_1') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Oil' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO4_20_0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Intercooler' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO4_20_1') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'Dyno' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO4_20_2') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'EngineRpm' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'Freq0') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'DynoCooling' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'AO4_20_3') AND CellId = 1
UPDATE Cell_Point SET CustomName = 'EngineECU' WHERE  PointId IN (SELECT PointId FROM Point WHERE Name LIKE 'DigitalOut1') AND CellId = 1


/* Manual
SET IDENTITY_INSERT Cell_Point2 ON;

INSERT INTO Cell_Point2 (CellPointId, CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, IsActive, UpdateDateTime)
SELECT CellPointId, CellId, PointId, CustomName, IsRecord, IsAverage, AverageSeconds, IncludeInStripChart, StripChartScale, 1, UpdateDateTime
FROM Cell_Point

SET IDENTITY_INSERT Cell_Point2 OFF

EXEC sp_rename 'Cell_Point', 'Cell_Point_old'
EXEC sp_rename 'Cell_Point2', 'Cell_Point'

DROP TABLE Cell_Point_old

go
*/