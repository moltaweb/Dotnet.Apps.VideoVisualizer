namespace TrainingVideosWebUI.Models
{
    public class LectureModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public int LectureOrder { get; set; }
        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public int DurationSeconds { get; set; }
        public string DurationString { get; set; }
        public bool hasVideo { get; set; }
        public string AttachmentHtml { get; set; }
        public bool isCompleted { get; set; }

    }
}
