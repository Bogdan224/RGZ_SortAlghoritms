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
using System.Windows.Shapes;

namespace RGZ_SortAlghoritms.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControls.QuickSortUserControl quickSortUC;

        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
            InitializeComponent();

            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            quickSortUC = new UserControls.QuickSortUserControl();

            currentUC.Content = quickSortUC;
        }

        
    }
}
