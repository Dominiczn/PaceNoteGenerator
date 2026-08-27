using System.ComponentModel;
using System.Windows;

namespace Pace_Note_Generator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}