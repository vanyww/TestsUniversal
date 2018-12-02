using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestsUniversal.View.Layouts
{
    /// <summary>
    /// Логика взаимодействия для MessageLayout.xaml
    /// </summary>
    public partial class MessageLayout : UserControl, INotifyPropertyChanged
    {
        public MessageLayout()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(nameof(ButtonContent), typeof(Object), typeof(MessageLayout), new PropertyMetadata(null));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(String), typeof(MessageLayout), new PropertyMetadata(String.Empty, TextChanged));
        public static readonly DependencyProperty IsProgressProperty =
            DependencyProperty.Register(nameof(IsProgress), typeof(Boolean), typeof(MessageLayout), new PropertyMetadata(false));
        public static readonly DependencyProperty HasButtonProperty =
            DependencyProperty.Register(nameof(HasButton), typeof(Boolean), typeof(MessageLayout), new PropertyMetadata(false));
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(MessageLayout), new PropertyMetadata(null));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newText = (String)e.NewValue;
            if (newText == String.Empty || newText is null) return;

            var layout = (MessageLayout)d;

            layout.TextFirstLetter = newText[0];
            layout.TextWithoutFirstLetter = newText.Remove(0, 1);
        }

        public String Text
        {
            get => (String)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Boolean IsProgress
        {
            get => (Boolean)GetValue(IsProgressProperty);
            set => SetValue(IsProgressProperty, value);
        }

        public Boolean HasButton
        {
            get => (Boolean)GetValue(HasButtonProperty);
            set => SetValue(HasButtonProperty, value);
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public Char TextFirstLetter
        {
            get => m_textFirstLetter;
            set
            {
                m_textFirstLetter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextFirstLetter)));
            }
        }

        public String TextWithoutFirstLetter
        {
            get => m_textWithoutFirstLetter;
            set
            {
                m_textWithoutFirstLetter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextWithoutFirstLetter)));
            }
        }

        public Object ButtonContent
        {
            get => GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        private Char m_textFirstLetter;
        private String m_textWithoutFirstLetter;
    }
}
