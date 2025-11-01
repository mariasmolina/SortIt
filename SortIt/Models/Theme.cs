namespace SortIt.Models
{
    public class Theme
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }
        public string GradientStart { get; set; }
        public string GradientEnd { get; set; }
        public string TextColor { get; set; }
        public string TitleTextColor { get; set; }
        public string FontFamily { get; set; }
        public string ProgressBarColor { get; set; }
        public string TabBarColor { get; set; }
        public string TabBarBackgroundColor { get; set; }
        public string TabBarUnselectedColor { get; set; }

        public Theme(string name, 
                     string gradientStart, 
                     string  gradientEnd, 
                     Color backgroundColor,
                     string textColor,
                     string titleTextColor, 
                     string font,
                     string progressBarColor,
                     string tabBarColor,
                     string tabBarBackgroundColor,
                     string tabBarUnselectedColor)
        {
            Name = name;
            BackgroundColor = backgroundColor;
            GradientStart = gradientStart;
            GradientEnd = gradientEnd;
            TextColor = textColor;
            TitleTextColor = titleTextColor;
            FontFamily = font;
            ProgressBarColor = progressBarColor;
            TabBarColor = tabBarColor;
            TabBarBackgroundColor = tabBarBackgroundColor;
            TabBarUnselectedColor = tabBarUnselectedColor;
        }
        public override string ToString() => Name;

        [Obsolete]
        public void Apply(ContentPage page)
        {
            
            var start = Color.FromArgb(GradientStart);
            var end = Color.FromArgb(GradientEnd);
            var textC = Color.FromArgb(TextColor);
            var titleTextC = Color.FromArgb(TitleTextColor);
            var progressC = Color.FromArgb(ProgressBarColor);
            var tabBarC = Color.FromArgb(TabBarColor);
            var tabBarBgC = Color.FromArgb(TabBarBackgroundColor);
            var tabBarUnC = Color.FromArgb(TabBarUnselectedColor);

            Application.Current.Resources["PageBackgroundBrush"] = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = start, Offset = 0f },
                    new GradientStop { Color = end, Offset = 1f }
                }
            };

            Application.Current.Resources["GlobalLabelStyle"] = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = FontFamily },
                    new Setter { Property = Label.TextColorProperty, Value = textC }
                }
            };

            Application.Current.Resources["GlobalButtonStyle"] = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.FontFamilyProperty, Value = FontFamily },
                    new Setter { Property = Button.TextColorProperty, Value = BackgroundColor }
                }
            };
            Application.Current.Resources["GlobalTitleLabelStyle"] = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = FontFamily },
                    new Setter { Property = Label.TextColorProperty, Value = titleTextC }
                }
            };

            Application.Current.Resources["GlobalFrameStyle"] = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter { Property = Frame.BackgroundColorProperty, Value = BackgroundColor }
                }
            };

            Application.Current.Resources["GlobalProgressBarStyle"] = new Style(typeof(ProgressBar))
            {
                Setters =
                {
                    new Setter { Property = ProgressBar.ProgressColorProperty, Value = progressC }
                }
            };

            Application.Current.Resources["GlobalTabBarStyle"] = new Style(typeof(TabBar))
            {
                Setters =
                {
                    new Setter { Property = Shell.TabBarForegroundColorProperty, Value = tabBarC },
                    new Setter { Property = Shell.TabBarTitleColorProperty, Value = tabBarC },
                    new Setter { Property = Shell.TabBarBackgroundColorProperty, Value = tabBarBgC },
                    new Setter { Property = Shell.TabBarUnselectedColorProperty, Value = tabBarUnC }
                }
            };

            Application.Current.Resources["GlobalBorderStyle"] = new Style(typeof(Border))
            {
                Setters =
                {
                    new Setter { Property = Border.BackgroundColorProperty, Value = BackgroundColor }
                }
            };

            Application.Current.Resources["GlobalEntryStyle"] = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter { Property = Entry.BackgroundColorProperty, Value = BackgroundColor },
                    new Setter { Property = Entry.FontFamilyProperty, Value = FontFamily },
                    new Setter { Property = Entry.TextColorProperty, Value = titleTextC }
                }
            };

            Application.Current.Resources["GlobalSwitchStyle"] = new Style(typeof(Switch))
            {
                Setters =
                {
                    new Setter { Property = Switch.OnColorProperty, Value = progressC }
                }
            };
        }
    }
}
