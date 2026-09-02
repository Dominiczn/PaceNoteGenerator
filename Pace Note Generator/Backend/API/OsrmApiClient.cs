using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Channels;
using System.Text.Json;

namespace Pace_Note_Generator.Backend.API
{
    internal class OsrmApiClient : ApiClient
    {
         public async Task<List<Node>> FetchRoute(List<Waypoint> waypoints)
        {
            string coordinates = string.Join(";", waypoints.Select(w => $"{w.Longitude},{w.Latitude}"));
            string url = $"http://router.project-osrm.org/route/v1/driving/{coordinates}?overview=full&geometries=geojson";

            string json = await SendRequest(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            var coordinatesArray = doc.RootElement.GetProperty("routes")[0].GetProperty("geometry").GetProperty(coordinates);

            List<Node> routeNodes = new List<Node>();

            foreach(var coord in coordinatesArray.EnumerateArray())
            {
                routeNodes.Add(new Node(coord[1].GetDouble(), coord[0].GetDouble()));
            }

            return routeNodes;
        }
    }
}
