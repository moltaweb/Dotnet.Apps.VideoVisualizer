using System.Collections.Generic;

namespace TrainingVideosWebUI.Models
{
    public class SectionModel
    {
        public int Id { get; set; }        
        public string Title { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
        public List<LectureModel> Lectures { get; set; } = new List<LectureModel>();
    }
}
