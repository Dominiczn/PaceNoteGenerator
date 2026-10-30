using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Backend.Analysis;
using Pace_Note_Generator.Backend.API;
using Pace_Note_Generator.Backend.Enums_and_Structs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pace_Note_Generator.Frontend.UserControls.Map
{
    public partial class MapView : UserControl
    {
        private Point mouseDownPosition;
        private bool isPlacingMarker = false;
        private List<Waypoint> waypoints = new List<Waypoint>();
        private List<Node> nodes = new List<Node>();

        public MapView()
        {
            InitializeComponent();

            //Setting up the map
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
            MapButtonsPanel.CalculateRoute += CalculateRoute;
            MapButtonsPanel.GeneratePacenotes += GeneratePacenotes;
        }

        //uses the PathAnalyser class to create pacenotes for the provided route. Whether the a straight is displayed is decided by its length being greater than 100m. This is then printed to the debug output
        private void GeneratePacenotes(object? sender, EventArgs e)
        {
            if (nodes.Count == 0) { return; }
            PathAnalyser path = new PathAnalyser(nodes);
            {

                List<Pacenote> pacenotes = path.AnalysePath();
                for (int i = 0; i < pacenotes.Count; i++)
                {
                    if (pacenotes[i].Type == PacenoteType.Straight && pacenotes[i].StraightLength < 100) { continue; }
                    Debug.WriteLine($"{pacenotes[i].CornerSeverity} {pacenotes[i].Direction}");
                }
            }
        }

        //Checks if the user is placing a marker or if there is enough waypoints to calculate a route and if there is, the OsrmApiClient class is used to make a list of nodes
        //If the API call fails the error message is printed to the debug output and the method is returned
        //Any existing route is then cleared and nodes is set to the nodes returned from the OSRM Api
        //
        private async void CalculateRoute(object? sender, EventArgs e)
        {
            if (isPlacingMarker) { return; }
            if (waypoints.Count < 2) { return; }
            var routingAPI = new OsrmApiClient();
            List<Node> newNodes;
            try
            {
                newNodes = await routingAPI.FetchRoute(waypoints);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Calculating route failed due to {ex}");
                return;
            }

            ClearRoute();

            nodes = newNodes;

            List<PointLatLng> nodeCoordinates = new List<PointLatLng>();
            for(int i = 0; i < nodes.Count; i++)
            {
                nodeCoordinates.Add(new PointLatLng(nodes[i].Latitude, nodes[i].Longitude));
                
                if (nodeCoordinates.Count > 1)
                {
                    List<PointLatLng> groupsOfNodes = new List<PointLatLng>();
                    groupsOfNodes.Add(nodeCoordinates[i - 1]);
                    groupsOfNodes.Add(nodeCoordinates[i]);

                    GMapPolygon polygon = new GMapPolygon(groupsOfNodes);
                    MapControl.RegenerateShape(polygon);

                    (polygon.Shape as Path)!.Stroke = Brushes.DarkBlue;
                    (polygon.Shape as Path)!.StrokeThickness = 5;
                    (polygon.Shape as Path)!.Effect = null;

                    MapControl.Markers.Add(polygon);


                    groupsOfNodes.Clear();
                }
                
            }

        }

        private void ClearRoute()
        {
            foreach (var poly in MapControl.Markers.OfType<GMapPolygon>().ToList())
            {
                MapControl.Markers.Remove(poly);
            }

            nodes.Clear();
        }

        private void RemoveMarker_Checkpoint(object? sender, EventArgs e)
        {
            if (isPlacingMarker) { return; }
            if (waypoints.Count == 0) { return; }

            ClearRoute();

            MapControl.Markers.Remove(waypoints[^1].Marker);

            waypoints.RemoveAt(waypoints.Count - 1);
            StcPnlButtonsHolder.Children.RemoveAt(StcPnlButtonsHolder.Children.Count - 1);

            if (waypoints.Count > 1)
            {
                waypoints[^1].Type = WaypointType.End;
                ((Ellipse)waypoints[^1].Marker!.Shape).Fill = Brushes.Red;
                ((MarkerCoordinates)StcPnlButtonsHolder.Children[^1]).BorderBrush = Brushes.Red;
            }

        }

        private void AddMarker_Checkpoint(object? sender, EventArgs e)
        {
            if (isPlacingMarker) { isPlacingMarker = false; }
            else { isPlacingMarker = true; }
        }

        private void MapControl_MouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
        {
            mouseDownPosition = e.GetPosition(MapControl);
        }

        //Incharge of placing markers
        private void MapControl_MouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
        {
            if (!isPlacingMarker) { return; }

            Point upPosition = e.GetPosition(MapControl);
            double distance = (upPosition - mouseDownPosition).Length;

            if (distance > 5) { return; }

            ClearRoute();

            WaypointType type;
            var markerCoordinates = new MarkerCoordinates();
            markerCoordinates.BorderThickness = new Thickness(5);

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
                    ((Ellipse)waypoints[^1].Marker!.Shape).Fill = Brushes.DarkBlue;
                    ((MarkerCoordinates)StcPnlButtonsHolder.Children[^1]).BorderBrush = Brushes.DarkBlue;
                }
                type = WaypointType.End; 
                ((Ellipse)marker.Shape).Fill = Brushes.Red;
                markerCoordinates.BorderBrush = Brushes.Red;
            }
            Waypoint waypoint = new Waypoint(point.Lat, point.Lng, type);
            waypoint.Marker = marker;
            waypoints.Add(waypoint);
            MapControl.Markers.Add(marker);
            markerCoordinates.Content = $"{waypoint.Latitude}, {waypoint.Longitude}";
            markerCoordinates.Foreground = Brushes.White;
            StcPnlButtonsHolder.Children.Add(markerCoordinates);
            isPlacingMarker = false;
        }
    }

}
