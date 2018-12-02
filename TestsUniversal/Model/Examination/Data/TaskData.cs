using System;
using System.Windows.Xps.Packaging;
using TestsUniversal.Model.Examination.Data.Yaml;

namespace TestsUniversal.Model.Examination.Data
{
    public sealed class TaskData
    {
        public TaskData(Object[] taskValidResult,
                        TaskParameterInfo[] inputVariantParameters,
                        TaskParameterDescription[] outputVariantParameters,
                        XpsDocument taskDocument,
                        TaskDescription taskDescription,
                        String title)
        {
            TaskValidResult = taskValidResult;
            InputVariantParameters = inputVariantParameters;
            OutputVariantParameters = outputVariantParameters;
            TaskDocument = taskDocument;
            TaskDescription = taskDescription;
            Title = title;
        }

        public Object[] TaskValidResult { get; }
        public TaskParameterInfo[] InputVariantParameters { get; }
        public TaskParameterDescription[] OutputVariantParameters { get; }
        public XpsDocument TaskDocument { get; }
        public TaskDescription TaskDescription { get; }
        public String Title { get; }
    }
}
