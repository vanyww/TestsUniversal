using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskVariant
    {
        [YamlMember(Alias = "Data")]
        public TaskParameter<String>[] Parameters { get; set; }

        [YamlMember(Alias = "TitleId")]
        public Int32 TitleId { get; set; }

        [YamlMember(Alias = "FunctionName")]
        public String ComputeFunctionName { get; set; }
    }
}
