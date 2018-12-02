using System;
using YamlDotNet.Serialization;

namespace TestsUniversal.Model.Examination.Data.Yaml
{
    public sealed class TaskDescription
    {
        public override Int32 GetHashCode() => Id;

        public override Boolean Equals(Object obj) =>
            obj != null && obj is TaskDescription otherTask && Id.Equals(otherTask.Id);

        [YamlMember(Alias = "Id")]
        public Int32 Id { get; set; }

        [YamlMember(Alias = "Name")]
        public String Name { get; set; }

        [YamlMember(Alias = "TaskTitlesPath")]
        public String TitlesPath { get; set; }

        [YamlMember(Alias = "ScriptPath")]
        public String ScriptPath { get; set; }

        [YamlMember(Alias = "DataDescPath")]
        public String DataPath { get; set; }

        [YamlMember(Alias = "DocPath")]
        public String DocumentPath { get; set; }

        [YamlMember(Alias = "Variants")]
        public TaskVariantDescription[] VariantPaths { get; set; }
    }
}
