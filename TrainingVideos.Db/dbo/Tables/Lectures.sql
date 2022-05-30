CREATE TABLE [dbo].[Lectures]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NOT NULL, 
    [LectureOrder] INT NOT NULL,     
    [SectionId] INT NOT NULL, 
    [DurationSeconds] INT NOT NULL DEFAULT 0, 
    [hasVideo] BIT NOT NULL DEFAULT 0, 
    [AttachmentHtml] VARCHAR(MAX) NULL, 
    [isCompleted] BIT NOT NULL DEFAULT 0
)
