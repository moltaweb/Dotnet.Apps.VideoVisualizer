using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingVideosWebUI.Models;

namespace TrainingVideos.Wpf.Models
{
    public class LectureModelUI : LectureModel
    {
        public string VideoURI { get; set; } = "default";
        public string AttachmentImage { get; set; } = "";
        public string IsCompletedText { get; set; } = "";

        public LectureModelUI(LectureModel parent, string lectureContentPath, string videoExtension)
        {
            this.Id = parent.Id;
            this.Name = parent.Name;
            this.DurationSeconds = parent.DurationSeconds;
            this.DurationString = parent.DurationString;
            this.AttachmentHtml = parent.AttachmentHtml;
            this.isCompleted = parent.isCompleted;
            this.hasVideo = parent.hasVideo;

            if (this.hasVideo)
            {
                string videoFileName = parent.Id + videoExtension;
                this.VideoURI = Path.Combine(lectureContentPath, "videos", videoFileName);
            }

            if(!string.IsNullOrEmpty(this.AttachmentHtml))
            {
                this.AttachmentImage = "/Images/clip.png";
            }

            if (this.isCompleted)
            {
                IsCompletedText = " x ";
            }
    }
    }
}
