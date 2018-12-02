using System;
using System.Windows;

namespace TestsUniversal.View.Attached
{
    public sealed class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() =>
            new BindingProxy();

        public Object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
