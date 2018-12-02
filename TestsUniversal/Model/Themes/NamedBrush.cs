using System;
using System.Windows.Media;

namespace TestsUniversal.Model.Themes
{
    public sealed class NamedBrush
    {
        public NamedBrush(String name, Brush themeBrush)
        {
            Name = name;
            Brush = themeBrush;
        }

        public String Name { get; }
        public Brush Brush { get; }
    }
}
