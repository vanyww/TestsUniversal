using MahApps.Metro.Controls;
using NLog;
using System;
using TestsUniversal.ViewModel.Commands;

namespace TestsUniversal.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static Logger logger;
        static MainWindow() => logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            logger.Info("TestsUniversal application started.");
            InitializeComponent();
        }

        private void AppClosed(Object sender, EventArgs e)
        {
            logger.Info("TestsUniversal application ended.");
            LogManager.Shutdown();
        }
    }
}
