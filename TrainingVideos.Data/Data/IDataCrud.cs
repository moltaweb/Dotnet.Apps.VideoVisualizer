using System.Collections.Generic;
using TrainingVideos.Data.Models;
using TrainingVideosWebUI.Models;

namespace TrainingVideosWebUI.Data
{
    public interface IDataCrud
    {
        List<LectureModel> GetAllLectures();
        List<CourseModel> GetCoursesList();
        LectureModel GetLectureById(int id);
        void ChangeCompletedStatus(int lectureId, int newValue);
        StatusModel GetStatusLast();
    }
}
