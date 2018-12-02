using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TestsUniversal.Model.Themes;
using TestsUniversal.ViewModel.Commands;

namespace TestsUniversal.ViewModel.Themes
{
    public sealed class ThemeWorkerViewModel : INotifyPropertyChanged
    {
        private const String AccentColorKey = "AccentColorBrush",
                             ThemeColorKey = "WhiteColorBrush";

        public ThemeWorkerViewModel()
        {
            m_accentColors = ThemeManager.Accents.Select(accent => new NamedBrush(accent.Name,
                                                                                  (Brush)accent.Resources[AccentColorKey]));
            m_appThemes = ThemeManager.AppThemes.Select(theme => new NamedBrush(theme.Name,
                                                                                (Brush)theme.Resources[ThemeColorKey]));
            ChangeAccentCommand = new UniversalCommand(e => ChangeAccent((String)e));
            ChangeThemeCommand = new UniversalCommand(e => ChangeTheme((String)e));
            ToggleThemeChangeFlyout = new UniversalCommand(e =>
            {
                ThemeChangeFlyoutOpened = !ThemeChangeFlyoutOpened;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThemeChangeFlyoutOpened)));
            });
            ThemeChangeFlyoutOpened = false;

            ThemeManager.IsThemeChanged += ThemeChanged;
            SetCurrentTheme();

            void SetCurrentTheme()
            {
                (AppTheme appTheme, Accent accent) = ThemeManager.DetectAppStyle();
                m_currentAcccent = accent;
                m_currentAppTheme = appTheme;

                if (ThemeSettings.Default.Accent != String.Empty)
                    if(m_accentColors.Any(i_accent =>  i_accent.Name == ThemeSettings.Default.Accent))
                        ChangeAccent(ThemeSettings.Default.Accent);

                if (ThemeSettings.Default.AppTheme != String.Empty)
                    if (m_appThemes.Any(i_appTheme => i_appTheme.Name == ThemeSettings.Default.AppTheme))
                        ChangeTheme(ThemeSettings.Default.AppTheme);
            }
        }

        private void ChangeTheme(String name)
        {
            (_, Accent accent) = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
        }

        private void ChangeAccent(String name)
        {
            (AppTheme appTheme, _) = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
        }

        private void ThemeChanged(Object sender, OnThemeChangedEventArgs e)
        {
            if (m_currentAppTheme != e.AppTheme)
            {
                m_currentAppTheme = e.AppTheme;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailableAppThemes)));
                ThemeSettings.Default.AppTheme = e.AppTheme.Name;
            }

            if (m_currentAcccent != e.Accent)
            {
                m_currentAcccent = e.Accent;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailableAccents)));
                ThemeSettings.Default.Accent = e.Accent.Name;
            }

            ThemeSettings.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<NamedBrush> AvailableAppThemes => m_appThemes.Where(theme => theme.Name != m_currentAppTheme.Name);
        public IEnumerable<NamedBrush> AvailableAccents => m_accentColors.Where(accent => accent.Name != m_currentAcccent.Name);

        public Boolean ThemeChangeFlyoutOpened { get; set; }

        public UniversalCommand ChangeThemeCommand { get; }
        public UniversalCommand ChangeAccentCommand { get; }
        public UniversalCommand ToggleThemeChangeFlyout { get; }

        private IEnumerable<NamedBrush> m_accentColors,
                                        m_appThemes;
        private AppTheme m_currentAppTheme;
        private Accent m_currentAcccent;
    }
}
