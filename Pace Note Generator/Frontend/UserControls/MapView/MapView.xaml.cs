using GMap.NET;
using GMap.NET.MapProviders;
using System.Windows;
using System.Windows.Controls;

namespace Pace_Note_Generator.Frontend.UserControls.MapView
{
    public partial class MapView : UserControl
    {
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
        }
    }

}
