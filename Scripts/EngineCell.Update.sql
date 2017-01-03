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

