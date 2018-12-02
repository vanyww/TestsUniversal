using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using TestsUniversal.Model.Examination.Data.Yaml;

namespace TestsUniversal.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для VariantChooseDialog.xaml
    /// </summary>
    public partial class VariantChooseDialog : CustomDialog
    {
        public VariantChooseDialog(IEnumerable<TaskVariantDescription> variants, String title)
        {
            Variants = variants;
            Title = title;
            DataContext = this;
            InitializeComponent();
        }

        private void CancelClicked(Object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            ((MetroWindow)App.Current.MainWindow).HideMetroDialogAsync(this);
        }

        private void OkClicked(Object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Ok;
            ((MetroWindow)App.Current.MainWindow).HideMetroDialogAsync(this);
        }

        public IEnumerable<TaskVariantDescription> Variants { get; }
        public TaskVariantDescription SelectedVariant { get; set; }
        public DialogResult DialogResult { get; private set; }
    }
}
