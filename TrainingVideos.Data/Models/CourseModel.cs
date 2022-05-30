using System.Collections.Generic;

namespace TrainingVideosWebUI.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DurationSeconds { get; set; }
        public string DurationString { get; set; }
        public double DurationCompleted { get; set; }
        public List<SectionModel> Sections { get; set; } = new List<SectionModel>();
        public string Description { get; internal set; } = "Default description";
    }
}
