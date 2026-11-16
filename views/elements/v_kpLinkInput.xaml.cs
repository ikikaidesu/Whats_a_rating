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
    /// Логика взаимодействия для v_kpLinkInput.xaml
    /// </summary>
    public partial class v_kpLinkInput : UserControl
    {
        public v_kpLinkInput()
        {
            InitializeComponent();
        }

        public ICommand goToFriendModeCommand
        {
            get => (ICommand)GetValue(goToFriendModeCommandProperty);
            set => SetValue(goToFriendModeCommandProperty, value);
        }
        public static readonly DependencyProperty goToFriendModeCommandProperty = DependencyProperty.Register("goToFriendModeCommand", typeof(ICommand), typeof(v_kpLinkInput), new PropertyMetadata(default));
    }
}
