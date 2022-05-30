using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrainingVideos.Wpf.Models;

namespace TrainingVideos.Wpf
{
    /// <summary>
    /// Interaction logic for Attachments.xaml
    /// </summary>
    public partial class AttachmentsWindow : Window
    {
        public AttachmentsWindow()
        {
            InitializeComponent();
        }

        public void PopulateAttachmentsInfo(LectureModelUI model)
        {
            if (!string.IsNullOrEmpty(model.AttachmentHtml)) { 
                attachmentInfo.Text = model.AttachmentHtml;
                attachmentInfoHTML.NavigateToString(model.AttachmentHtml);
            }
        }
    }
}
