using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace movieRateGuess.views.elements
{
    /// <summary>
    /// Логика взаимодействия для v_helpWindow.xaml
    /// </summary>
    public partial class v_helpWindow : UserControl
    {
        public v_helpWindow()
        {
            InitializeComponent();
        }

        public string helpText
        {
            get => (string)GetValue(helpTextProperty);
            set => SetValue(helpTextProperty, value);
        }
        public static readonly DependencyProperty helpTextProperty = DependencyProperty.Register("helpText", typeof(string), typeof(v_helpWindow), new PropertyMetadata(default));

        public ICommand closeCommand
        {
            get => (ICommand)GetValue(closeCommandProperty);
            set => SetValue(closeCommandProperty, value);
        }
        public static readonly DependencyProperty closeCommandProperty = DependencyProperty.Register("closeCommand", typeof(ICommand), typeof(v_helpWindow), new PropertyMetadata(default));
    }
}
