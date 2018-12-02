using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskParameterDescription
    {
        [YamlMember(Alias = "Name")]
        public String Name { get; set; }

        [YamlMember(Alias = "Position")]
        public Int32 Position { get; set; }

        [YamlMember(Alias = "Type")]
        public ExamTaskDataType DataType { get; set; }

        [YamlMember(Alias = "Label")]
        public String Label { get; set; }
    }
}
