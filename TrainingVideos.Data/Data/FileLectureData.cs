using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TrainingVideos.Data.Models;
using TrainingVideosWebUI.Models;

namespace TrainingVideosWebUI.Data
{
    public class FileLectureData : IDataCrud
    {
        private readonly IConfiguration _config;

        public FileLectureData(IConfiguration config)
        {
            _config = config;
        }

        public List<LectureModel> GetAllLectures()
        {
            throw new System.NotImplementedException();
        }

        public LectureModel GetLectureById(int id)
        {
            LectureModel output = new LectureModel();
            
            // Retrieve data from file
            string dbFileName = _config.GetSection("dbFile").Value;
            var lines = File.ReadLines(dbFileName);
            foreach (var line in lines)
            {
                string[] parts = line.Split(";");

                if (parts[0] == id.ToString())
                {                          
                    output.Id = int.Parse(parts[0]);
                    output.CourseId = int.Parse(parts[1]);                    
                    output.Name = parts[5];
                    output.DurationString = parts[6];
                    output.hasVideo = (parts[9] == "True") ? true : false;
                    output.AttachmentHtml = parts[10];
                }

            }

            return output;
        }

        public List<CourseModel> GetCoursesList()
        {
            List<CourseModel> coursesList = new List<CourseModel>();
            List<SectionModel> sectionsList = new List<SectionModel>();

            List<LectureFileModel> lecturesList = new List<LectureFileModel>();

            // Retrieve data from file
            string dbFileName = _config.GetSection("dbFile").Value;
            var lines = File.ReadLines(dbFileName);
            foreach (var line in lines)
            {
                string[] parts = line.Split(";");

                LectureFileModel model = new LectureFileModel();
                model.LectureId = parts[0];
                model.CourseId = parts[1];
                model.CourseTitle = parts[2];
                model.CoursePriority = int.Parse(parts[3]);
                model.SectionTitle = parts[4];
                model.LectureName = parts[5];
                model.LectureDuration = parts[6];
                model.LectureWistiaId = parts[7];
                model.LectureVideoUrl = parts[8];
                model.LectureIsVideoDownloaded = (parts[9] == "True") ? true : false;
                model.AttachmentsHtml = parts[10];

                lecturesList.Add(model);
            }

            // Create models based on retrieved data
            // We Loop through every lecture stored in DB file
            // Then through every course in output courseList to check if the course exists already
            // and if not, we create it
            // same for sections
            // once created, add the lecture to it

            // Loop through every lecture stored in DB file
            foreach (var item in lecturesList)
            {
                // This is the lecture we will add to the courseList
                LectureModel lecture = new LectureModel();

                lecture.Id = int.Parse(item.LectureId);
                //lecture.CourseId = int.Parse(item.CourseId);
                //lecture.SectionTitle = item.SectionTitle;
                lecture.Name = item.LectureName;
                lecture.DurationString = item.LectureDuration;                
                lecture.hasVideo = item.LectureIsVideoDownloaded;
                lecture.AttachmentHtml = item.AttachmentsHtml;


                // check if the course exists already
                int courseIndex = coursesList.FindIndex(c => c.Id == int.Parse(item.CourseId));

                if (courseIndex == -1)
                {
                    // the course does not exist, so we create it along with the section and add the lecture
                    CourseModel course = new CourseModel();

                    course.Id = int.Parse(item.CourseId);
                    course.Title = item.CourseTitle;

                    SectionModel section = new SectionModel();

                    section.CourseId = int.Parse(item.CourseId);
                    section.Title = item.SectionTitle;                    
                    section.Order = 1;

                    if (sectionsList.Count > 0)
                    {
                        section.Id = sectionsList.Max(r => r.Id) + 1;
                    }
                    else
                    {
                        section.Id = 1;
                    }
                    
                    lecture.SectionId = section.Id;

                    section.Lectures.Add(lecture);
                    course.Sections.Add(section);
                    sectionsList.Add(section);
                    coursesList.Add(course);

                }
                else
                {
                    // the course already exists

                    // check if the section exists already
                    int sectionIndex = coursesList[courseIndex].Sections.FindIndex(s => s.Title == item.SectionTitle);

                    if (sectionIndex == -1)
                    {
                        // the section does not exist, so we create it and add the lecture                        
                        SectionModel section = new SectionModel();
                        section.CourseId = int.Parse(item.CourseId);
                        section.Title = item.SectionTitle;
                        section.Order = 1;

                        if (sectionsList.Count > 0)
                        {
                            section.Id = sectionsList.Max(r => r.Id) + 1;
                        }
                        else
                        {
                            section.Id = 1;
                        }

                        lecture.SectionId = section.Id;

                        section.Lectures.Add(lecture);
                        coursesList[courseIndex].Sections.Add(section);
                        sectionsList.Add(section);
                    }
                    else
                    {
                        // the section already exists
                        lecture.SectionId = sectionsList[sectionIndex].Id;

                        coursesList[courseIndex].Sections[sectionIndex].Lectures.Add(lecture);
                    }

                }

            }

            return coursesList;
        }

        public void ChangeCompletedStatus(int lectureId, int newValue)
        {
            throw new System.NotImplementedException();
        }

        public StatusModel GetStatusLast()
        {
            throw new System.NotImplementedException();
        }
    }
}
