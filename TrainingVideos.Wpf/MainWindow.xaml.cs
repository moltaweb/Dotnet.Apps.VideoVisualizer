using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TrainingVideos.Data.Models;
using TrainingVideos.Wpf.Models;
using TrainingVideosWebUI.Data;
using TrainingVideosWebUI.Models;

namespace TrainingVideos.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataCrud _db;
        private readonly IConfiguration _config;
        private CourseModel? SelectedCourse;

        public LectureModel? SelectedLecture;
        //public List<LectureModel>? CourseLectures;

        public MainWindow(IDataCrud db, IConfiguration config)
        {
            InitializeComponent();
            _db = db;
            _config = config;
            videoControlsCollapsed.Visibility = Visibility.Collapsed;
            LoadCourseList();

        }        

        private void courseList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (courseList.SelectedItem == null)
            {
                return;
            }

            SelectedCourse = courseList.SelectedItem as CourseModel;

            if (SelectedCourse != null)
            {
                List<LectureModelUI> Lectures = new List<LectureModelUI>();
                string lectureContentPath = _config.GetSection("content").GetSection("path").Value;
                string videoExtension = _config.GetSection("content").GetSection("videoExtension").Value;

                foreach (var section in SelectedCourse.Sections)
                {
                    foreach (var lecture in section.Lectures)
                    {
                        Lectures.Add(new LectureModelUI(lecture, lectureContentPath, videoExtension));
                    }
                }
                courseContent.DataContext = Lectures;

                int percentageCompleted = (int)(SelectedCourse.DurationCompleted / SelectedCourse.DurationSeconds * 100);
                lectureDescription.Text = $"{SelectedCourse.Title} ({SelectedCourse.DurationString}) - {percentageCompleted}% completed";
                this.Title = $"Training Videos - {SelectedCourse.Title}";
            }
        }


        private void ListViewItem_Change(object sender, SelectionChangedEventArgs e)
        {
            SelectedLecture = courseContent.SelectedItem as LectureModelUI;
            
            if (SelectedLecture != null)
            {
                lectureDescription.Text = $"{SelectedLecture.Name} ({SelectedLecture.DurationString}) [Id: {SelectedLecture.Id}]";
            }
        }


        // Media Events

        private void OnMouseDownPlayMedia_Click(object sender, RoutedEventArgs e)
        {
            var selectedLectureUI = courseContent.SelectedItem as LectureModelUI;
            if (selectedLectureUI.hasVideo && !File.Exists(selectedLectureUI.VideoURI))
            {
                MessageBox.Show($"Video not found. Please check the filePath:\n{selectedLectureUI.VideoURI}");
                return;
            }

            // Initialize the MediaElement property values.
            InitializePropertyValues();

            // The Play method will begin the media if it is not currently active or
            // resume media if it is paused. This has no effect if the media is
            // already running.
            videoElement.Play();            
        }

        private void OnMouseDownPauseMedia_Click(object sender, RoutedEventArgs e)
        {
            // The Pause method pauses the media if it is currently running.
            // The Play method can be used to resume.
            videoElement.Pause();
        }

        private void OnMouseDownStopMedia_Click(object sender, RoutedEventArgs e)
        {
            // The Stop method stops and resets the media to be played from
            // the beginning.
            videoElement.Stop();
            timelineSlider.Value = 0;
            //videoElement.Play();
            //videoElement.Stop();
        }

        private void SeekToMediaPosition_Drag(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //    SeekToMediaPosition_Change(((Slider)sender).Value);
            int SliderValue = (int)timelineSlider.Value;

            int duration = (int)videoElement.NaturalDuration.TimeSpan.TotalSeconds;

            double newValue = SliderValue / 10.0 * duration;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with seconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, (int)newValue, 0);
            videoElement.Position = ts;

        }


        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.            
            // videoElement.SpeedRatio = (double)speedRatioSlider.Value;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            SetSpeedRatio();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (videoElement.Source != null)
            {
                if (videoElement.NaturalDuration.HasTimeSpan)
                    lblStatus.Text = String.Format("{0} / {1}", videoElement.Position.ToString(@"mm\:ss"), videoElement.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Text = "No file selected...";
        }

        private void Rewind_Click(object sender, RoutedEventArgs e)
        {
            //    SeekToMediaPosition_Change(((Slider)sender).Value);
            TimeSpan currentTime = videoElement.Position;

            int newPosition = (int)currentTime.TotalSeconds - 10;

            TimeSpan ts = new TimeSpan(0, 0, 0, newPosition, 0);
            videoElement.Position = ts;
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            //    SeekToMediaPosition_Change(((Slider)sender).Value);
            TimeSpan currentTime = videoElement.Position;

            int newPosition = (int)currentTime.TotalSeconds + 10;

            TimeSpan ts = new TimeSpan(0, 0, 0, newPosition, 0);
            videoElement.Position = ts;
        }



        private void SetSpeedRatio()
        {
            var item = speedRatio.SelectedValue.ToString();
            if (item != null)
            {
                double value = Double.Parse(item) / 100;
                videoElement.SpeedRatio = value;
            }
        }

        private void SpeedRatio_Change(object sender, SelectionChangedEventArgs e)
        {
            var item = speedRatio.SelectedValue.ToString();
            if (item != null && videoElement != null)
            {
                double value = Double.Parse(item) / 100;
                videoElement.SpeedRatio = value;
            }

        }


        private void BtnViewAttachments_Click(object sender, RoutedEventArgs e)
        {
            var attachmentWindow = App.serviceProvider.GetService<AttachmentsWindow>();

            var selectedLecture = (LectureModelUI)courseContent.SelectedItem;
            //var model = (LectureModelUI)((Button)e.Source).DataContext;

            attachmentWindow.PopulateAttachmentsInfo(selectedLecture);

            attachmentWindow.Show();
        }

        private void BtnMarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)courseContent.SelectedItems;

            var selectedLectures = items.Cast<LectureModelUI>();

            foreach (var selectedLecture in selectedLectures)
            {
                if (selectedLecture.Id > 0)
                {
                    _db.ChangeCompletedStatus(selectedLecture.Id, 1);
                }
            }

            LoadCourseList();

        }

        private void BtnMarkUncompleted_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)courseContent.SelectedItems;

            var selectedLectures = items.Cast<LectureModelUI>();

            foreach (var selectedLecture in selectedLectures)
            {
                if (selectedLecture.Id > 0)
                {
                    _db.ChangeCompletedStatus(selectedLecture.Id, 0);
                }
            }

            LoadCourseList();
        }

        private void LoadCourseList()
        {
            List<CourseModel> Courses = _db.GetCoursesList();
            courseList.ItemsSource = Courses;

            StatusModel status = _db.GetStatusLast();

            if (status != null)
            {
                int index = Courses.FindIndex(c => c.Id == status.CourseId);
                courseList.SelectedIndex = index;
            }
            else
            {
                courseList.SelectedIndex = 0;
            }
        }

        private void BtnCompleteLectureAndContinue_Click(object sender, RoutedEventArgs e)
        {
            int currentLecture = courseContent.SelectedIndex;

            BtnMarkCompleted_Click(sender, e);

            courseContent.SelectedIndex = currentLecture + 1;
        }

        private void BtnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (statusBar.Visibility == Visibility.Collapsed)
            {
                // Exit FullScreen Mode
                statusBar.Visibility = Visibility.Visible;
                lectureInfo.Visibility = Visibility.Visible;
                sidebar.Visibility = Visibility.Visible;
                videoControls.Visibility = Visibility.Visible;
                videoControlsCollapsed.Visibility = Visibility.Collapsed;

                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                // Enter FullScreen Mode
                statusBar.Visibility = Visibility.Collapsed;
                lectureInfo.Visibility = Visibility.Collapsed;
                sidebar.Visibility = Visibility.Collapsed;
                videoControls.Visibility = Visibility.Collapsed;
                videoControlsCollapsed.Visibility = Visibility.Visible;

                this.WindowStyle = WindowStyle.None;
            }
            
            //statusBar.Height = 0;
            
            
        }


        private void VideoControlsDisplay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            videoControlsDisplay.Visibility = Visibility.Visible;
        }

        private void BtnVideoControlsDisplay_Click(object sender, RoutedEventArgs e)
        {
            if ((string)videoControlsDisplay.Content == "PLAY")
            {
                videoControlsDisplay.Content = "PAUSE";
                videoElement.Play();                
            }
            else
            {
                videoControlsDisplay.Content = "PLAY";
                videoElement.Pause();
            }
            videoControlsDisplay.Visibility = Visibility.Hidden;
        }
    } 
}
