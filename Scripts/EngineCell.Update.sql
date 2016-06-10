alter table cell_point
add [AverageSeconds] int NULL 
alter table cell_point
add [IsAverage] [bit] NOT NULL DEFAULT(0)
alter table cell_point
add [IsRecord] [bit] NOT NULL DEFAULT(0)