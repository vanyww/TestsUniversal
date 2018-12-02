using MSScriptControl;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Xps.Packaging;
using TestsUniversal.Model.Examination.Data;
using TestsUniversal.Model.Examination.Data.Yaml;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination
{
    public static class LocalExamTaskLoader
    {
        private const String YamlExtension = "*.yaml",
                             VBSName = "VBScript";

        static LocalExamTaskLoader()
        {
            logger = LogManager.GetCurrentClassLogger();

            deserializer = new Deserializer();
            scriptControl = new ScriptControl { Language = VBSName };
        }

        public static IEnumerable<TaskDescription> ReadTasks(String dir)
        {
            var files = Directory.GetFiles(dir, YamlExtension, SearchOption.TopDirectoryOnly);
            if (files.Length == 0) return null;

            var tasks = new HashSet<TaskDescription>();
            for (Int32 i = 0; i < files.Length; i++)
                try
                {
                    using (var fileStream = new StreamReader(File.Open(files[i], FileMode.Open, FileAccess.Read)))
                        try
                        {
                            var task = deserializer.Deserialize<TaskDescription>(fileStream);
                            if (!tasks.Add(task))
                                logger.Warn("File duplicates another, that has the same Id: {0}.", files[i]);
                        }
                        catch (YamlException e) { logger.Error(e, "File has wrong markup: {0}.", files[i]); }
                }
                catch (FileNotFoundException e) { logger.Error(e, "File was not found: {0}.", files[i]); }

            tasks.TrimExcess();
            return tasks;
        }

        public static TaskData ReadTaskData(TaskDescription task, TaskVariantDescription variant)
        {
            TaskParametersDescription dataDescription = null;
            using (var fileStream = new StreamReader(File.Open(task.DataPath, FileMode.Open, FileAccess.Read)))
                dataDescription = deserializer.Deserialize<TaskParametersDescription>(fileStream);

            TaskVariant variantData = null;
            using (var fileStream = new StreamReader(File.Open(variant.Path, FileMode.Open, FileAccess.Read)))
                variantData = deserializer.Deserialize<TaskVariant>(fileStream);

            TaskTitle[] titles = null;
            using (var fileStream = new StreamReader(File.Open(task.TitlesPath, FileMode.Open, FileAccess.Read)))
                titles = deserializer.Deserialize<TaskTitle[]>(fileStream);

            var parsedVariantParameters = ParameterParser.ParseVariantData(dataDescription.InputDataTypes, variantData.Parameters);

            scriptControl.Reset();
            var script = File.ReadAllText(task.ScriptPath);
            scriptControl.AddCode(script);
            var parameters = parsedVariantParameters.Select(param => param.Value).
                ToArray();
            var scriptResult = (Object[])scriptControl.Run(variantData.ComputeFunctionName, parameters);

            XpsDocument document = null;
            try
            {
                document = new XpsDocument(task.DocumentPath, FileAccess.Read);
                if (document is null)
                    logger.Warn("XPS document error, path: {0}.", task.DocumentPath);
            }
            catch (FileNotFoundException e) { logger.Warn(e, "XPS file not found, path: {0}.", task.DocumentPath); }

            return new TaskData
            (
                scriptResult,
                parsedVariantParameters.ToArray(),
                dataDescription.OutputDataTypes,
                document,
                task,
                titles.SingleOrDefault(title => title.Id == variantData.TitleId).Title
            );
        }

        private static readonly IDeserializer deserializer;
        private static readonly Logger logger;
        private static readonly ScriptControl scriptControl;
    }
}
