using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using TestsUniversal.Model.Examination.Data.Yaml;
using TestsUniversal.View.Dialogs;

namespace TestsUniversal.ViewModel.Navigation
{
    public class NavigationWorker : INotifyPropertyChanged
    {
        private NavigationWorker() { }

        public NavigationWorker Initialize(NavigationDictionary layouts, Layout currentLayout)
        {
            Layouts = layouts;
            CurrentLayout = layouts[currentLayout];
            return this;
        }

        public async Task<VariantChooseDialog> ShowVariantChooseDialog(IEnumerable<TaskVariantDescription> variants, String title)
        {
            var dialog = new VariantChooseDialog(variants, title);
            var mainWindow = (MetroWindow)App.Current.MainWindow;
            await mainWindow.ShowMetroDialogAsync(dialog);
            await dialog.WaitUntilUnloadedAsync();
            return dialog;
        }

        public void SetLayout(Layout layout, Object context = null)
        {
            var element = Layouts[layout];
            if (element.DataContext != null)
                element.DataContext = null;
            element.DataContext = context;
            CurrentLayout = element;
        }

        public FrameworkElement CurrentLayout
        {
            get => m_currentLayout;
            private set
            {
                m_currentLayout = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLayout)));
            }
        }

        public NavigationDictionary Layouts { get; private set; }
        public static NavigationWorker Instance => m_instance ?? (m_instance = new NavigationWorker());

        public event PropertyChangedEventHandler PropertyChanged;

        private FrameworkElement m_currentLayout;
        private static NavigationWorker m_instance;
    }
}
