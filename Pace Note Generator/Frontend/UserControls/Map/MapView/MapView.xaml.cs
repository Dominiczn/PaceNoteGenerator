using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using Pace_Note_Generator.Frontend.UserControls.Map;

namespace Pace_Note_Generator.Frontend.UserControls.MapView
{
    public partial class MapView : UserControl
    {
        private Point mouseDownPosition;
        private List<TextBox> waypointSlots = new();
        private int activeSlotIndex = 0;
        public MapView()
        {
            InitializeComponent();
            MapControl.MapProvider = GMapProviders.OpenStreetMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapProviders.OpenStreetMap.RefererUrl = "https://github.com/dominiczn/PaceNoteGenerator";
            GMapProvider.UserAgent = "PaceNoteGenerator/1.0";
            MapControl.MinZoom = 2;
            MapControl.MaxZoom = 18;
            MapControl.Zoom = 16;
            MapControl.Position = new PointLatLng(52.18814661958257, 0.13528958291712542);
            MapControl.CanDragMap = true;
            MapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            MapControl.ShowCenter = false;
            MapControl.DragButton = MouseButton.Left;
            MapControl.MouseLeftButtonDown += MapControl_MouseLeftButtonDown;
            MapControl.MouseLeftButtonUp += MapControl_MouseLeftButtonUp;
        }

        private void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPosition = e.GetPosition(MapControl);
        }

        private void MapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point upPosition = e.GetPosition(MapControl);
            double distance = (upPosition - mouseDownPosition).Length;

            if (distance > 5) { return; }

            PointLatLng point = MapControl.FromLocalToLatLng((int)upPosition.X, (int)upPosition.Y);

            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red,
                Stroke = Brushes.White,
                StrokeThickness = 1.5,
            };

            if (AddCheckpointChecker.CanPlaceCheckpoint)
            {
                MapControl.Markers.Add(marker);
                AddCheckpointChecker.CanPlaceCheckpoint = false;
            }
            

        }
    }

}
