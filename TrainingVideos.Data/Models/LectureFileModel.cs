namespace TrainingVideosWebUI.Models
{
    public class LectureFileModel
    {
        public string LectureId { get; set; }
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int CoursePriority { get; set; }
        public string SectionTitle { get; set; }
        public string LectureWistiaId { get; set; }
        public string LectureName { get; set; }
        public string LectureDuration { get; set; }
        public string LectureUrl { get; set; }
        public string LectureVideoUrl { get; set; }
        public bool LectureIsVideoDownloaded { get; set; }
        public string AttachmentsHtml { get; set; }
    }
}
