using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TrainingVideos.Data.Data;
using TrainingVideos.Data.Database;
using TrainingVideosWebUI.Data;

namespace TrainingVideos.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Dependency Injection
            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<AttachmentsWindow>();

            services.AddTransient<ISqlDatabaseAccess, SqlDatabaseAccess>();
            services.AddTransient<IDataCrud, SqlDataCrud>();

            // Configurations
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();            
            services.AddSingleton(config);

            serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
