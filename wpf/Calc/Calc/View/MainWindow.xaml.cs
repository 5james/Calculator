using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Calc.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            logger.LogInfoMessage("Created");
        }

        ILogger logger = new Logger(typeof(MainWindow));

        private FontPicker picker;

        private void CreateFontAndColorPicker(object obj, RoutedEventArgs args)
        {
            if(picker == null || picker.IsLoaded == false)
            {
                logger.LogInfoMessage("Creating instance of class FontPicker (Window)");
                picker = new FontPicker();
                picker.DataContext = DataContext;
                picker.Show();
            }
            else
            {
                logger.LogWarningMessage("User wanted to create another instance of class FontPicker (Window)");
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            logger.LogInfoMessage("OnClosed - Shutting down application");

            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
