using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Backend.Database;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Pace_Note_Generator.Frontend.UserControls.Pacenotes
{
    public partial class PacenotesView : UserControl
    {
        private List<Pacenote> pacenotes = new List<Pacenote>();
        private int currentIndex = 0;
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly DispatcherTimer hazardTimer = new DispatcherTimer();

        private readonly Dictionary<Key, string> hazardKeys = new Dictionary<Key, string>()
        {
            {Key.D1, "Blind" },
            {Key.D2, "Crest" },
            {Key.D3, "Jump" },
            {Key.D4, "Narrows" },
            {Key.D5, "Junction" },
            {Key.D6, "Off Camber" },
            {Key.D7, "Gravel" },
            {Key.D8, "Tarmac" },
            {Key.D9, "Cut" },
            {Key.D0, "Dont Cut" },
            {Key.Back, "Unconfirmed" },
        };

        public PacenotesView(List<Pacenote> pacenotes)
        {
            InitializeComponent();
            this.pacenotes = pacenotes;

            Focusable = true;
            Loaded += (sender, e) => Focus();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            TxtTimer.Text = "00:00";

            hazardTimer.Tick += HazardTimer_Tick;
            hazardTimer.Interval = TimeSpan.FromSeconds(1.5);

            PbProgressBar.Maximum = pacenotes.Count;
            UpdateDisplay();
        }

        private void ShowHazardCard(string hazard)
        {
            hazardCardText.Text = hazard;
            hazardCard.Visibility = Visibility.Visible;

            hazardTimer.Stop();
            hazardTimer.Start();
        }

        private void HazardTimer_Tick(object? sender, EventArgs e)
        {
            hazardCard.Visibility = Visibility.Collapsed;
            hazardTimer.Stop();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            TxtTimer.Text = stopwatch.Elapsed.ToString(@"mm\:ss");
        }

        private void DisplayCard(PacenoteCard card, int index)
        {
            if (index < 0 || index >= pacenotes.Count)
            {
                card.Visibility = Visibility.Hidden;
                return;
            }

            card.Visibility = Visibility.Visible;
            card.DisplayPacenote(pacenotes[index]);
        }

        private void UpdateDisplay()
        {
            DisplayCard(FarLeftPacenote, currentIndex - 2);
            DisplayCard(MiddleLeftPacenote, currentIndex - 1);
            DisplayCard(MiddlePacenote, currentIndex);
            DisplayCard(MiddleRightPacenote, currentIndex + 1);
            DisplayCard(FarRightPacenote, currentIndex + 2);

            TxtProgressText.Text = $"{currentIndex + 1}/{pacenotes.Count}";
            PbProgressBar.Value = currentIndex + 1;
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                if (currentIndex < pacenotes.Count - 1)
                {
                    currentIndex++;
                    UpdateDisplay();
                }
                e.Handled = true;
            }

            else if (e.Key == Key.Left)
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    UpdateDisplay();
                }
                e.Handled = true;
            }

            else if (e.Key == Key.Space)
            {
                if (stopwatch.IsRunning)
                {
                    stopwatch.Stop();
                    dispatcherTimer.Stop();
                }
                else
                {
                    stopwatch.Start();
                    dispatcherTimer.Start();
                }

                e.Handled = true;
            }

            else if (hazardKeys.ContainsKey(e.Key))
            {
                string hazard = hazardKeys[e.Key];
                ShowHazardCard(hazardKeys[e.Key]);

                DataAccess.AddHazard(hazard, pacenotes[currentIndex].PacenoteId);

                e.Handled = true;
            }
        }
    }
}
