using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using TestsUniversal.Model.Examination;
using TestsUniversal.Model.Examination.Data;
using TestsUniversal.Model.Examination.Data.Yaml;
using TestsUniversal.ViewModel.Commands;
using TestsUniversal.ViewModel.Navigation;
using YamlDotNet.Core;

namespace TestsUniversal.ViewModel.Examination
{
    public sealed class ExamSolveViewModel : INotifyPropertyChanged
    {
        static ExamSolveViewModel() => logger = LogManager.GetCurrentClassLogger();

        public ExamSolveViewModel()
        {
            BackCommand = new UniversalCommand(async obj =>
            {
                if (TestStarted)
                {
                    var mainWindow = (MetroWindow)App.Current.MainWindow;
                    var result = await mainWindow.ShowMessageAsync("Возврат к выбору теста",
                        "Для того чтобы вернуться к выбору теста, подтвердите отмену решения текущего задания.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        new MetroDialogSettings { AffirmativeButtonText = "Подтверждение", NegativeButtonText = "Отмена" });
                    if (result == MessageDialogResult.Negative) return;
                }
                m_timer.Stop();
                NavigationWorker.Instance.SetLayout(Layout.StartupLayout);
                TestStarted = false;
            });
            AcceptAnswer = new UniversalCommand((obj) => AcceptTaskAnswer());
            StartTask = new UniversalCommand((obj) =>
            {
                TestStarted = true;
                m_timer.Start();
            });

            TimePassed = TimeSpan.Zero;
            m_timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (sender, e) =>
                TimePassed += TimeSpan.FromSeconds(1), Application.Current.Dispatcher)
            { IsEnabled = false };

            m_testStarted = false;
        }

        private void AcceptTaskAnswer()
        {
            m_timer.Stop();
            TaskResult = m_outParameters.All(param => param.Value.Equals(param.ValidValue));
            TestStarted = false;

            NavigationWorker.Instance.SetLayout(Layout.TaskResultLayout, this);
        }

        public void SetTaskData(TaskDescription taskDescription, TaskVariantDescription selectedVariant)
        {
            LoadStatus = LoadingStatus.Loading;
            TaskData taskData;
            try
            {
                taskData = LocalExamTaskLoader.ReadTaskData(taskDescription, selectedVariant);

                InputVariantParameters = taskData.InputVariantParameters.Where(param => param.Value != null);
                Document = Application.Current.Dispatcher.Invoke(() => taskData.TaskDocument.GetFixedDocumentSequence());
                TaskName = taskData.TaskDescription.Name;
                TaskDescription = taskData.Title;
                var validResult = taskData.TaskValidResult.Select(val => (val is Double dVal) ?
                    Math.Round(dVal, ExamSettings.Default.SignesNumber) : val);
                OutputVariantParameters = taskData.OutputVariantParameters.OrderBy(param => param.Position).
                    Zip(validResult, (param, res) => new { ValidResult = res, Parameter = param }).
                    Where(rp => rp.ValidResult != null).
                    Select(rp => new OutputTaskParameterViewModel
                    {
                        Type = rp.Parameter.DataType,
                        Label = rp.Parameter.Label,
                        Value = String.Empty,
                        ValidValue = rp.ValidResult
                    }).ToArray();
                SelectedVariant = selectedVariant.Id;

                LoadStatus = LoadingStatus.Loaded;
            }
            catch (FileNotFoundException e)
            {
                LoadStatus = LoadingStatus.Error;
                logger.Error(e, "Task data file set is not full (some files do not exist): {0}.", taskDescription.Name);
            }
            catch (YamlException e)
            {
                LoadStatus = LoadingStatus.Error;
                logger.Error(e, "Error occured, while trying to load task data: {0}.", taskDescription.Name);
            }
            catch (KeyNotFoundException e)
            {
                LoadStatus = LoadingStatus.Error;
                logger.Error(e, "One or more predefined arguments are not declared in variant file: {0}.", selectedVariant.Path);
            }
            catch (FormatException e)
            {
                LoadStatus = LoadingStatus.Error;
                logger.Error(e, "Variant file contains argument(s) with wrong formatting for it's type.", selectedVariant.Path);
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Fatal error occured, while trying to load task data: {0}.", taskDescription.Name);
                throw e;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<OutputTaskParameterViewModel> OutputVariantParameters
        {
            get => m_outParameters;
            set
            {
                m_outParameters = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputVariantParameters)));
            }
        }

        public IEnumerable<TaskParameterInfo> InputVariantParameters
        {
            get => m_inParameters;
            set
            {
                m_inParameters = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputVariantParameters)));
            }
        }

        public String TaskName
        {
            get => m_taskName;
            set
            {
                m_taskName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskName)));
            }
        }

        public String TaskDescription
        {
            get => m_taskDescription;
            set
            {
                m_taskDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskDescription)));
            }
        }

        public FixedDocumentSequence Document
        {
            get => m_document;
            set
            {
                m_document = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Document)));
            }
        }

        public LoadingStatus LoadStatus
        {
            get => m_loadStatus;
            set
            {
                m_loadStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadStatus)));
            }
        }

        public TimeSpan TimePassed
        {
            get => m_timePassed;
            set
            {
                m_timePassed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimePassed)));
            }
        }

        public Boolean TestStarted
        {
            get => m_testStarted;
            set
            {
                m_testStarted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestStarted)));
            }
        }

        public Int32 SelectedVariant
        {
            get => m_selectedVariant;
            set
            {
                m_selectedVariant = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVariant)));
            }
        }

        public Func<Double, String> Formatter { get; set; }
        public Boolean TaskResult { get; set; }
        public UniversalCommand AcceptAnswer { get; set; }
        public UniversalCommand BackCommand { get; set; }
        public UniversalCommand StartTask { get; set; }

        private static readonly Logger logger;

        private DispatcherTimer m_timer;

        private FixedDocumentSequence m_document;
        private IEnumerable<OutputTaskParameterViewModel> m_outParameters;
        private IEnumerable<TaskParameterInfo> m_inParameters;
        private LoadingStatus m_loadStatus;
        private Boolean m_testStarted;
        private TimeSpan m_timePassed;
        private Int32 m_selectedVariant;
        private String m_taskName, m_taskDescription;
    }
}
