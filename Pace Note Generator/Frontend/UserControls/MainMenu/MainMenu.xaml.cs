using System.Windows;
using System.Windows.Controls;

namespace Pace_Note_Generator.Frontend.UserControls.MainMenu
{
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public event EventHandler? NewStageRequested;
        public event EventHandler? OpenStageRequested;

        private void BtnNewStage_Click(object sender, RoutedEventArgs e)
        {
            NewStageRequested?.Invoke(this, EventArgs.Empty);
        }
        private void BtnOpenStage_Click(object sender, RoutedEventArgs e)
        {
            OpenStageRequested?.Invoke(this, EventArgs.Empty);
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
