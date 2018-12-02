using System;

namespace TestsUniversal.Model.Examination.Data
{
    public sealed class TaskParameterInfo
    {
        public TaskParameterInfo(String name, String label, Object value, Int32 position)
        {
            Name = name;
            Label = label;
            Value = value;
            Position = position;
        }

        public String Name { get; }
        public String Label { get; }
        public Object Value { get; }
        public Int32 Position { get; }
    }
}
