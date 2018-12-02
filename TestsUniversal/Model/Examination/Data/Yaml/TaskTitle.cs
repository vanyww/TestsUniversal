using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskTitle
    {
        [YamlMember(Alias = "Id")]
        public Int32 Id { get; set; }

        [YamlMember(Alias = "Title")]
        public String Title { get; set; }
    }
}
