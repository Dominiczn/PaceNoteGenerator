using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Frontend.UserControls.MainMenu;
using Pace_Note_Generator.Frontend.UserControls.Map;
using Pace_Note_Generator.Frontend.UserControls.Pacenotes;
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
            //will eventually load existing stages into MapView or as a list
            var testPacenotes = new List<Pacenote>
    {
        new Pacenote { Direction = Direction.Left, CornerSeverity = CornerSeverity.Four },
        new Pacenote { Direction = Direction.Straight, CornerSeverity = CornerSeverity.Straight, StraightLength = 150 },
        new Pacenote { Direction = Direction.Right, CornerSeverity = CornerSeverity.Three, Tightens = true, Hazards = new() { "Blind" } },
        new Pacenote { Direction = Direction.Straight, CornerSeverity = CornerSeverity.Straight, StraightLength = 80, Hazards = new() { "Crest" } },
        new Pacenote { Direction = Direction.Left, CornerSeverity = CornerSeverity.Five, IsLong = true },
        new Pacenote { Direction = Direction.Right, CornerSeverity = CornerSeverity.Hairpin },
        new Pacenote { Direction = Direction.Straight, CornerSeverity = CornerSeverity.Straight, StraightLength = 300 },
        new Pacenote { Direction = Direction.Right, CornerSeverity = CornerSeverity.Six, Hazards = new() { "Jump" } },
        new Pacenote { Direction = Direction.Left, CornerSeverity = CornerSeverity.Two, Widens = true },
        new Pacenote { Direction = Direction.Right, CornerSeverity = CornerSeverity.One, Hazards = new() { "Jump", "Blind" } },
        new Pacenote { Direction = Direction.Straight, CornerSeverity = CornerSeverity.Straight, StraightLength = 50 },
        new Pacenote { Direction = Direction.Left, CornerSeverity = CornerSeverity.Four, IsLong = true, Tightens = true }
    };

            MainContent.Content = new PacenotesView(testPacenotes);
        }
    }
} 