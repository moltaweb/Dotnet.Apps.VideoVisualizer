using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingVideos.Data.Models
{
    public class StatusModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int LectureId { get; set; }
        public bool IsLast { get; set; }
    }
}

