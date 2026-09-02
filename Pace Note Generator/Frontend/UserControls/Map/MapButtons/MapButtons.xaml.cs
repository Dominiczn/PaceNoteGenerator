using Pace_Note_Generator.Frontend.UserControls.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pace_Note_Generator.Frontend.UserControls.MapButtons
{
    public partial class MapButtons : UserControl
    {
        public MapButtons()
        {
            InitializeComponent();
        }

        private void BtnAddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            AddCheckpointChecker.CanPlaceCheckpoint = true;
        }

        private void BtnCalculateRoute_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
