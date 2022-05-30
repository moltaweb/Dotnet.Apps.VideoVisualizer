CREATE TABLE [dbo].[Status]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CourseId] INT NOT NULL, 
    [LectureId] INT NOT NULL, 
    [IsLast] BIT NOT NULL
)
