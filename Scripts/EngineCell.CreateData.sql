IF (SELECT COUNT(*) FROM PointGroup) = 0 
BEGIN
	INSERT INTO PointGroup VALUES ('ThermalCouple')
	INSERT INTO PointGroup VALUES ('Volt')
	INSERT INTO PointGroup VALUES ('MilliAmp')
	INSERT INTO PointGroup VALUES ('HART')
	INSERT INTO PointGroup VALUES ('AnalogOutput')
	INSERT INTO PointGroup VALUES ('Dyno')
	INSERT INTO PointGroup VALUES ('Engine')
	INSERT INTO PointGroup VALUES ('Fuel')
	INSERT INTO PointGroup VALUES ('Misc')
	INSERT INTO PointGroup VALUES ('Frequency')
END

IF (SELECT COUNT(*) FROM Point) = 0 
BEGIN
	INSERT INTO Point VALUES ('ThermalCouple0',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple1',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple2',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple3',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple4',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple5',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple6',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple7',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple8',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple9',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple10',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple11',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple12',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple13',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple14',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple15',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple16',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple17',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple18',1,1,1)
	INSERT INTO Point VALUES ('ThermalCouple19',1,1,1)

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

	INSERT INTO Point VALUES ('Dyno0',0,0,6)
	INSERT INTO Point VALUES ('Dyno1',0,0,6)
	INSERT INTO Point VALUES ('Dyno2',0,0,6)
	INSERT INTO Point VALUES ('Dyno3',0,0,6)
	INSERT INTO Point VALUES ('Dyno4',0,0,6)
	INSERT INTO Point VALUES ('Dyno5',0,0,6)
	INSERT INTO Point VALUES ('Dyno6',0,0,6)
	INSERT INTO Point VALUES ('Dyno7',0,0,6)

	INSERT INTO Point VALUES ('Engine0',0,0,7)
	INSERT INTO Point VALUES ('Engine1',0,0,7)
	INSERT INTO Point VALUES ('Engine2',0,0,7)
	INSERT INTO Point VALUES ('Engine3',0,0,7)
	INSERT INTO Point VALUES ('Engine4',0,0,7)
	INSERT INTO Point VALUES ('Engine5',0,0,7)
	INSERT INTO Point VALUES ('Engine6',0,0,7)
	INSERT INTO Point VALUES ('Engine7',0,0,7)

	INSERT INTO Point VALUES ('Fuel0',0,0,8)
	INSERT INTO Point VALUES ('Fuel1',0,0,8)
	INSERT INTO Point VALUES ('Fuel2',0,0,8)
	INSERT INTO Point VALUES ('Fuel3',0,0,8)
	INSERT INTO Point VALUES ('Fuel4',0,0,8)
	INSERT INTO Point VALUES ('Fuel5',0,0,8)
	INSERT INTO Point VALUES ('Fuel6',0,0,8)
	INSERT INTO Point VALUES ('Fuel7',0,0,8)

	INSERT INTO Point VALUES ('Misc0',0,0,9)
	INSERT INTO Point VALUES ('Misc1',0,0,9)
	INSERT INTO Point VALUES ('Misc2',0,0,9)
	INSERT INTO Point VALUES ('Misc3',0,0,9)
	INSERT INTO Point VALUES ('Misc4',0,0,9)
	INSERT INTO Point VALUES ('Misc5',0,0,9)
	INSERT INTO Point VALUES ('Misc6',0,0,9)
	INSERT INTO Point VALUES ('Misc7',0,0,9)

	INSERT INTO Point VALUES ('Freq0',1,0,10)
	INSERT INTO Point VALUES ('Freq1',1,0,10)
	INSERT INTO Point VALUES ('Freq2',1,0,10)
	INSERT INTO Point VALUES ('Freq3',1,0,10)
END

IF (SELECT COUNT(*) FROM Cell WHERE CellId = 1) = 0 
BEGIN
	INSERT INTO Cell VALUES ('Engine Cell 1', '98.109.58.113', 22001, 'Engine Cell 1')
	INSERT INTO Cell_Point
	SELECT 1, p.PointId, p.Name, 0, NULL, GETDATE()
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