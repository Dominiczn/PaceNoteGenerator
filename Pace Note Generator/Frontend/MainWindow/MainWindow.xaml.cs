using Pace_Note_Generator.Frontend.UserControls.MainMenu;
using Pace_Note_Generator.Frontend.UserControls.MapView;
using System.ComponentModel;
using System.Windows;

namespace Pace_Note_Generator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainMenu = new MainMenu();

            mainMenu.NewStageRequested += MainMenu_NewStageRequested;
            mainMenu.OpenStageRequested += MainMenu_OpenStageRequested;
            MainContent.Content = mainMenu;
        }

        private void MainMenu_NewStageRequested(object? sender, EventArgs e)
        {
            MainContent.Content = new MapView();
        }

        private void MainMenu_OpenStageRequested(object? sender, EventArgs e)
        {
            // will eventually load an existing stage into MapView
        }
    }
} 