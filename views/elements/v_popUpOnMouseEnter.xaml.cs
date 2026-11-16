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
    /// Логика взаимодействия для v_popUpOnMouseEnter.xaml
    /// </summary>
    public partial class v_popUpOnMouseEnter : UserControl
    {
        public v_popUpOnMouseEnter()
        {
            InitializeComponent();
        }
        // создаем зависимость(передаваемая переменная) с данными подсказки
        public string ToolTipText
        {
            get => (string)GetValue(ToolTipTextProperty);
            set => SetValue(ToolTipTextProperty, value);
        }
        public static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register("ToolTipText", typeof(string), typeof(v_popUpOnMouseEnter), new PropertyMetadata(default));

        public int ToolTipWidth
        {
            get => (int)GetValue(ToolTipWidthProperty);
            set => SetValue(ToolTipWidthProperty, value);
        }
        public static readonly DependencyProperty ToolTipWidthProperty = DependencyProperty.Register("ToolTipWidth", typeof(int), typeof(v_popUpOnMouseEnter), new PropertyMetadata(default));

        public int ToolTipHeight
        {
            get => (int)GetValue(ToolTipHeightProperty);
            set => SetValue(ToolTipHeightProperty, value);
        }
        public static readonly DependencyProperty ToolTipHeightProperty = DependencyProperty.Register("ToolTipHeight", typeof(int), typeof(v_popUpOnMouseEnter), new PropertyMetadata(default));

        public int ToolTipHorizontalOffset
        {
            get => (int)GetValue(ToolTipHorizontalOffsetProperty);
            set => SetValue(ToolTipHorizontalOffsetProperty, value);
        }
        public static readonly DependencyProperty ToolTipHorizontalOffsetProperty = DependencyProperty.Register("ToolTipHorizontalOffset", typeof(int), typeof(v_popUpOnMouseEnter), new PropertyMetadata(default));
        public int ToolTipVerticalOffset
        {
            get => (int)GetValue(ToolTipVerticalOffsetProperty);
            set => SetValue(ToolTipVerticalOffsetProperty, value);
        }
        public static readonly DependencyProperty ToolTipVerticalOffsetProperty = DependencyProperty.Register("ToolTipVerticalOffset", typeof(int), typeof(v_popUpOnMouseEnter), new PropertyMetadata(default));

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip.IsOpen = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ToolTip.IsOpen = false;
        }
    }
}
