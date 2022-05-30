CREATE TABLE [dbo].[Courses]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] VARCHAR(100) NOT NULL, 
    [DurationSeconds] INT NOT NULL DEFAULT 0, 
    [Description] VARCHAR(MAX) NULL, 
    [DurationCompleted] INT NOT NULL DEFAULT 0
)
