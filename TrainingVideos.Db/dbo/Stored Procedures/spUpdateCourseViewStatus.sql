CREATE PROCEDURE [dbo].[spUpdateCourseViewStatus]
	@lectureId int = 0

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @courseId INT;	

	SELECT @courseId = s.CourseId 
	FROM Sections s INNER JOIN Lectures l on s.Id = l.SectionId
	WHERE l.Id=@lectureId;

	UPDATE dbo.[Status] SET IsLast=0 WHERE IsLast=1

	IF NOT EXISTS (SELECT 1 FROM dbo.[Status] WHERE CourseId=@courseId)
		BEGIN		
			INSERT INTO dbo.[Status] (CourseId, LectureId, IsLast)
			VALUES (@courseId, @lectureId, 1)	;	
		END

	ELSE
		BEGIN
			UPDATE dbo.[Status] SET IsLast=1 WHERE CourseId=@courseId
			UPDATE dbo.[Status] SET LectureId=@lectureId WHERE CourseId=@courseId
		END

END

RETURN 0
