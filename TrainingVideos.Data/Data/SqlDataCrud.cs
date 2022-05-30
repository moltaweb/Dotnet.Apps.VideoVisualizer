using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingVideos.Data.Database;
using TrainingVideos.Data.Models;
using TrainingVideosWebUI.Data;
using TrainingVideosWebUI.Models;

namespace TrainingVideos.Data.Data
{
    public class SqlDataCrud : IDataCrud
    {
        private readonly ISqlDatabaseAccess _db;
        private const string connectionStringName = "SqlServer";

        public SqlDataCrud(ISqlDatabaseAccess db)
        {
            _db = db;
        }

        public List<LectureModel> GetAllLectures()
        {
            throw new NotImplementedException();
        }

        public List<CourseModel> GetCoursesList()
        {
            string sql = "SELECT Id, Name, LectureOrder, SectionId, DurationSeconds, hasVideo, AttachmentHtml, isCompleted, " +
                         "RIGHT('0' + CAST(DurationSeconds / 60 AS VARCHAR),2) + ':' + RIGHT('0' + CAST(DurationSeconds % 60 AS VARCHAR), 2) AS DurationString " +
                         "FROM dbo.Lectures " +
                         "ORDER BY Id";

            List<LectureModel> lectures = _db.LoadData<LectureModel, dynamic>(sql, new { }, connectionStringName);

            sql = "SELECT Id, Title, [Order], CourseId " +
                  "FROM dbo.Sections " +
                  "ORDER BY Id";

            List<SectionModel> sections = _db.LoadData<SectionModel, dynamic>(sql, new { }, connectionStringName);

            sql = "SELECT Id, Title, DurationSeconds, DurationCompleted, Description, " +
                  "RIGHT('0' + CAST(DurationSeconds / 3660 AS VARCHAR),2) + ':' + RIGHT('0' + CAST((DurationSeconds /60) % 60 AS VARCHAR), 2) AS DurationString " +
                  "FROM dbo.Courses " +
                  "ORDER BY Id";

            List<CourseModel> courses = _db.LoadData<CourseModel, dynamic>(sql, new { }, connectionStringName);

            foreach(var lecture in lectures)
            {
                var section = sections.Where(s => s.Id == lecture.SectionId).First();
                section.Lectures.Add(lecture);                
            }

            foreach (var section in sections)
            {
                var course = courses.Where(c => c.Id == section.CourseId).First();
                course.Sections.Add(section);
            }

            return courses;

        }

        public StatusModel GetStatusLast()
        {
            string sqlStatement = @"select CourseId, LectureId from Status where IsLast=1";

            var output = _db.LoadData<StatusModel, dynamic>(sqlStatement,
                                                 new { },
                                                 connectionStringName).FirstOrDefault();

            return output;
        }

        public LectureModel GetLectureById(int id)
        {
            string sqlStatement = @"select * from Lectures where Id=@id";

            var output = _db.LoadData<LectureModel, dynamic>(sqlStatement,
                                                 new { id },
                                                 connectionStringName).First();

            return output;
        }

        public void ChangeCompletedStatus(int lectureId, int newValue)
        {
            string sqlStatement = "UPDATE dbo.Lectures SET isCompleted=@newValue WHERE Id=@lectureId";

            _db.SaveData<dynamic>(sqlStatement, new { lectureId, newValue}, connectionStringName);           

            _db.SaveData<dynamic>("spUpdateCourseCompletedPercentage", new { lectureId }, connectionStringName, true);

            if (newValue > 0)
            {
                _db.SaveData<dynamic>("spUpdateCourseViewStatus", new { lectureId }, connectionStringName, true);
            }

        }
    }
}
