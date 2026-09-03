using System.Windows;
using System.Windows.Controls;

namespace Pace_Note_Generator.Frontend.UserControls.Map
{
    public partial class MapButtons : UserControl
    {
        public MapButtons()
        {
            InitializeComponent();
        }

        public event EventHandler? CheckpointAdded;
        public event EventHandler? CheckpointRemoved;

        private void BtnAddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            CheckpointAdded?.Invoke(this, EventArgs.Empty);
        }

        private void BtnRemoveCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            CheckpointRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void BtnCalculateRoute_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
