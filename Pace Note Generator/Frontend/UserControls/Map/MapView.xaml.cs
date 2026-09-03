using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Backend.Enums_and_Structs;

namespace Pace_Note_Generator.Frontend.UserControls.Map
{
    public partial class MapView : UserControl
    {
        private Point mouseDownPosition;
        private bool isPlacingMarker = false;
        private List<Waypoint> waypoints = new List<Waypoint>();
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
            MapButtonsPanel.CheckpointAdded += AddMarker_Checkpoint;
            MapButtonsPanel.CheckpointRemoved += RemoveMarker_Checkpoint;
        }

        private void RemoveMarker_Checkpoint(object? sender, EventArgs e)
        {
            int numWaypoints = waypoints.Count;
            if (numWaypoints == 0) { return; }
            if (numWaypoints != 0) { waypoints.RemoveAt(waypoints.Count - 1); }
            MapControl.Markers.RemoveAt(waypoints.Count);
        }

        private void AddMarker_Checkpoint(object? sender, EventArgs e)
        {
            isPlacingMarker = true;
        }

        private void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPosition = e.GetPosition(MapControl);
        }

        private void MapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isPlacingMarker) { return; }
            WaypointType type;
            var markerCoordinates = new MarkerCoordinates();

            Point upPosition = e.GetPosition(MapControl);
            double distance = (upPosition - mouseDownPosition).Length;

            if (distance > 5) { return; }

            PointLatLng point = MapControl.FromLocalToLatLng((int)upPosition.X, (int)upPosition.Y);

            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.DarkBlue,
                Stroke = Brushes.White,
                StrokeThickness = 1.5,
            };

            if (waypoints.Count == 0)
            {
                type = WaypointType.Start; 
                ((Ellipse)marker.Shape).Fill = Brushes.Green;
                markerCoordinates.BorderBrush = Brushes.Green;
            }
            else
            {
                if (waypoints.Count >= 2)
                {
                    waypoints[^1].Type = WaypointType.Checkpoint; 
                    ((Ellipse)waypoints[^1].Marker.Shape).Fill = Brushes.DarkBlue; 
                    markerCoordinates.BorderBrush = Brushes.DarkBlue;
                }
                type = WaypointType.End; 
                ((Ellipse)marker.Shape).Fill = Brushes.Red;
                markerCoordinates.BorderBrush = Brushes.Red;
            }
            Waypoint waypoint = new Waypoint(point.Lat, point.Lng, type);
            waypoint.Marker = marker;
            waypoints.Add(waypoint);
            MapControl.Markers.Add(marker);
            markerCoordinates.Content = $"dsfsdfdsdff";
            markerCoordinates.Foreground = Brushes.White;
            StcPnlButtonsHolder.Children.Add(markerCoordinates);
            isPlacingMarker = false;

            Debug.WriteLine(waypoints.Count);
        }
    }

}
