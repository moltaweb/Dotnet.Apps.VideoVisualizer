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
    public class SqliteDataCrud : IDataCrud
    {
        private readonly ISqlDatabaseAccess _db;
        private const string connectionStringName = "SqliteDB";

        public SqliteDataCrud(ISqlDatabaseAccess db)
        {
            _db = db;
        }

        public List<LectureModel> GetAllLectures()
        {
            throw new NotImplementedException();
        }

        public List<CourseModel> GetCoursesList()
        {
            throw new NotImplementedException();
        }

        public LectureModel GetLectureById(int id)
        {
            string sqlStatement = @"select * from Lectures where Id=@id";

            var output = _db.LoadData<LectureModel, dynamic>(sqlStatement,
                                                 new { id },
                                                 connectionStringName).First();

            return output;
        }

        public void SaveAllLectures()
        {

        }

        public void ChangeCompletedStatus(int lectureId, int newValue)
        {
            throw new NotImplementedException();
        }

        public StatusModel GetStatusLast()
        {
            throw new NotImplementedException();
        }
    }
}
