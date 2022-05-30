CREATE TABLE [dbo].[Sections]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] VARCHAR(100) NOT NULL, 
    [Order] INT NOT NULL, 
    [CourseId] INT NOT NULL
)
