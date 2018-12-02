using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskParametersDescription
    {
        [YamlMember(Alias = "Input")]
        public TaskParameterDescription[] InputDataTypes { get; set; }

        [YamlMember(Alias = "Output")]
        public TaskParameterDescription[] OutputDataTypes { get; set; }
    }
}
