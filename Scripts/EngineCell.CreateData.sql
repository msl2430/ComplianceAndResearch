IF (SELECT COUNT(*) FROM PointGroup) = 0 
BEGIN
	INSERT INTO PointGroup VALUES ('ThermoCouple')
	INSERT INTO PointGroup VALUES ('Volt')
	INSERT INTO PointGroup VALUES ('MilliAmp')
	INSERT INTO PointGroup VALUES ('HART')
	INSERT INTO PointGroup VALUES ('AnalogOutput')
	INSERT INTO PointGroup VALUES ('DigitalOut')
	INSERT INTO PointGroup VALUES ('DigitalIn')
	INSERT INTO PointGroup VALUES ('Frequency')
END

IF (SELECT COUNT(*) FROM Point) = 0 
BEGIN
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

	INSERT INTO Point VALUES ('AO10Bipolar',0,1,5)
	INSERT INTO Point VALUES ('AO10',0,1,5)

	INSERT INTO Point VALUES ('AO5Bipolar',0,1,5)
	INSERT INTO Point VALUES ('AO5',0,1,5)

	INSERT INTO Point VALUES ('AO4_20_0',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_1',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_2',0,1,5)
	INSERT INTO Point VALUES ('AO4_20_3',0,1,5)

	INSERT INTO Point VALUES ('DigitalIn0',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn1',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn2',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn3',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn4',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn5',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn6',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn7',0,0,6)
	INSERT INTO Point VALUES ('DigitalIn8',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn9',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn10',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn11',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn12',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn13',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn14',0,0,7)
	INSERT INTO Point VALUES ('DigitalIn15',0,0,7)

	INSERT INTO Point VALUES ('DigitalIn16',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn17',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn18',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn19',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn20',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn21',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn22',0,0,8)
	INSERT INTO Point VALUES ('DigitalIn23',0,0,8)

	INSERT INTO Point VALUES ('DigitalIn24',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn25',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn26',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn27',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn28',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn29',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn30',0,0,9)
	INSERT INTO Point VALUES ('DigitalIn31',0,0,9)

	INSERT INTO Point VALUES ('DigitalOut0',1,0,10)
	INSERT INTO Point VALUES ('DigitalOut1',1,0,10)
	INSERT INTO Point VALUES ('DigitalOut2',1,0,10)
	INSERT INTO Point VALUES ('DigitalOut3',1,0,10)
END

IF (SELECT COUNT(*) FROM Cell WHERE CellId = 1) = 0 
BEGIN
	INSERT INTO Cell VALUES ('Engine Cell 1', '98.109.58.113', 22001, 'Engine Cell 1')
	INSERT INTO Cell_Point
	SELECT 1, p.PointId, p.Name, 0, 0, NULL, 0, NULL, GETDATE()
	FROM Point p
END


IF NOT EXISTS (SELECT 1 FROM Widget WHERE WidgetId IN (1,2,3))
BEGIN
	INSERT INTO Widget (Name) VALUES
	('Ventilation Control 1'),
	('Ventilation Control 2'),
	('DynoPid')
END

IF NOT EXISTS (SELECT 1 FROM WidgetSetting WHERE WidgetId IN (1,2,3)) 
BEGIN
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
	(3, 'Dyno PID Setpoint')
END