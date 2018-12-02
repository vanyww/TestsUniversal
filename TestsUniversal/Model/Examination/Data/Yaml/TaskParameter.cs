using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskParameter<T>
    {
        [YamlMember(Alias = "Name")]
        public String Name { get; set; }

        [YamlMember(Alias = "Value")]
        public T Value { get; set; }
    }
}
