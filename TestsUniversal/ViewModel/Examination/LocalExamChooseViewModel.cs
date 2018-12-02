using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestsUniversal.Model.Examination;
using TestsUniversal.Model.Examination.Data.Yaml;
using TestsUniversal.View.Dialogs;
using TestsUniversal.ViewModel.Commands;
using TestsUniversal.ViewModel.Navigation;

namespace TestsUniversal.ViewModel.Examination
{
    public sealed class LocalExamChooseViewModel : INotifyPropertyChanged
    {
        static LocalExamChooseViewModel() => logger = LogManager.GetCurrentClassLogger();

        public LocalExamChooseViewModel()
        {
            m_loadStatus = LoadingStatus.NotLoaded;
            m_solvingViewModel = new ExamSolveViewModel();
            m_random = new Random();
            m_filterLock = new Object();
            m_refreshThreadControl = 0;

            StartInitializationCommand = new UniversalCommand(obj =>
                {
                    if (m_loadStatus != LoadingStatus.NotLoaded)
                        return;
                    Task.Run((Action)RefreshTasks);
                });
            FilterTasksCommand = new UniversalCommand(obj =>
                {
                    Task.Run(() =>
                    {
                        var text = (String)obj;
                        if (text == String.Empty)
                            Tasks = m_loadedTasks;
                        var tempTasks =
                            m_loadedTasks.Where(task => task.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
                        if (Int32.TryParse(text, out Int32 id))
                        {
                            var taskById = m_loadedTasks.FirstOrDefault(task => task.Id == id);
                            if (taskById != null && !tempTasks.Contains(taskById))
                                tempTasks = tempTasks.Concat(new TaskDescription[] { taskById }).OrderBy(task => task.Id);
                        }
                        lock (m_filterLock)
                            Tasks = tempTasks;
                    });
                });
            RefreshTasksCommand = new UniversalCommand(obj => Task.Run((Action)RefreshTasks));
            ChooseTaskCommand = new UniversalCommand(async obj =>
                {
                    var selectedTaskDescription = (TaskDescription)obj;
                    TaskVariantDescription selectedVariant;

                    if (ExamSettings.Default.ChooseVariant)
                    {
                        var dialog = await NavigationWorker.Instance.ShowVariantChooseDialog(
                            selectedTaskDescription.VariantPaths,
                            $"Задание {selectedTaskDescription.Id}: выбор варианта");
                        if (dialog.DialogResult == DialogResult.Cancel)
                            return;
                        selectedVariant = dialog.SelectedVariant;
                    }
                    else
                    {
                        selectedVariant = selectedTaskDescription.
                            VariantPaths[m_random.Next(0, selectedTaskDescription.VariantPaths.Length)];
                    }

                    NavigationWorker.Instance.SetLayout(Layout.TaskSolvingLayout, m_solvingViewModel);
                    await Task.Run(() => m_solvingViewModel.SetTaskData(selectedTaskDescription, selectedVariant));
                });
        }

        public void RefreshTasks()
        {
            if (Interlocked.CompareExchange(ref m_refreshThreadControl, Blocked, Free) == Blocked)
                return;

            LoadStatus = LoadingStatus.Loading;
            try
            {
                m_loadedTasks = Tasks = LocalExamTaskLoader.ReadTasks(ExamSettings.Default.TasksPath);
                if (!m_filteredTasks.Any())
                {
                    logger.Info("No local tests found in folder {0}.", ExamSettings.Default.TasksPath);
                    LoadStatus = LoadingStatus.Empty;
                    return;
                }
                LoadStatus = LoadingStatus.Loaded;
            }
            catch (ArgumentException e)
            {
                LoadStatus = LoadingStatus.Error;
                logger.Warn(e, "Local tests folder does not set.");
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Fatal error occured, while trying to load task file: {0}.", ExamSettings.Default.TasksPath);
                throw e;
            }
            finally
            {
                m_refreshThreadControl = Free;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<TaskDescription> Tasks
        {
            get => m_filteredTasks;
            private set
            {
                m_filteredTasks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
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

        public UniversalCommand StartInitializationCommand { get; }
        public UniversalCommand FilterTasksCommand { get; }
        public UniversalCommand RefreshTasksCommand { get; }
        public UniversalCommand ChooseTaskCommand { get; }

        #region CAS Operation Members

        private const Int32 Blocked = 0x01,
                            Free = 0x00;

        private Int32 m_refreshThreadControl;

        #endregion

        private static readonly Logger logger;

        private LoadingStatus m_loadStatus;
        private IEnumerable<TaskDescription> m_filteredTasks,
                                             m_loadedTasks;
        private ExamSolveViewModel m_solvingViewModel;
        private Random m_random;
        private Object m_filterLock;
    }
}
