CREATE PROCEDURE [dbo].[spUpdateCourseCompletedPercentage]
	@lectureId int = 0
AS
	
BEGIN
	SET NOCOUNT ON;

	DECLARE @courseId INT;
	DECLARE @durationCompleted INT;

	SELECT @courseId = s.CourseId 
	FROM Sections s INNER JOIN Lectures l on s.Id = l.SectionId
	WHERE l.Id=@lectureId;

	IF EXISTS (SELECT 1 FROM Lectures WHERE isCompleted=1 AND SectionId IN (SELECT Id FROM Sections WHERE Sections.CourseId=@courseId))
		BEGIN
			SELECT @durationCompleted = SUM(DurationSeconds) 
			FROM Lectures 
			WHERE isCompleted=1 AND SectionId IN (SELECT Id FROM Sections WHERE Sections.CourseId=@courseId);

			UPDATE Courses 
			SET DurationCompleted = @durationCompleted
			WHERE Id=@courseId;
		END
	ELSE
		UPDATE Courses 
			SET DurationCompleted = 0
			WHERE Id=@courseId;

END

RETURN 0
