using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskVariantDescription
    {
        [YamlMember(Alias = "Id")]
        public Int32 Id { get; set; }

        [YamlMember(Alias = "Path")]
        public String Path { get; set; }
    }
}
