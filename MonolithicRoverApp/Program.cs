using NetTopologySuite.Geometries;
using System.Text.Json;
using System.Collections.Generic;

// ============ EMBEDDED DATA & LOGIC ============

public class RoverService
{
    private readonly List<Rover> _rovers = new();
    private readonly List<Measurement> _measurements = new();
    private readonly GeometryFactory _geoFactory = new();
    private int _sequence = 0;

    public RoverService()
    {
        // Initialize rovers with embedded data
        _rovers.Add(new Rover(1, "Rover Alpha", -74.006, 40.7128, "#FF0000"));
        _rovers.Add(new Rover(2, "Rover Beta", -0.1276, 51.5074, "#00FF00"));
        _rovers.Add(new Rover(3, "Rover Gamma", 139.6917, 35.6895, "#0000FF"));
    }

    public List<Rover> GetRovers() => _rovers;
    
    public List<Measurement> GetMeasurements() => _measurements;

    public void SimulateMovement()
    {
        var random = new Random();
        foreach (var rover in _rovers)
        {
            // Move rover randomly
            rover.Longitude += (random.NextDouble() - 0.5) * 0.01;
            rover.Latitude += (random.NextDouble() - 0.5) * 0.01;

            // Create measurement
            var measurement = new Measurement
            {
                Sequence = ++_sequence,
                RoverId = rover.Id,
                RoverName = rover.Name,
                Longitude = rover.Longitude,
                Latitude = rover.Latitude,
                WindSpeed = random.Next(0, 20),
                WindDirection = random.Next(0, 360),
                Timestamp = DateTime.UtcNow
            };

            _measurements.Add(measurement);

            // Keep only last 100 measurements per rover
            if (_measurements.Count > 300)
                _measurements.RemoveAt(0);
        }
    }
}

public class Rover
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Color { get; set; }

    public Rover(int id, string name, double lon, double lat, string color)
    {
        Id = id;
        Name = name;
        Longitude = lon;
        Latitude = lat;
        Color = color;
    }
}

public class Measurement
{
    public int Sequence { get; set; }
    public int RoverId { get; set; }
    public string RoverName { get; set; } = "";
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int WindSpeed { get; set; }
    public int WindDirection { get; set; }
    public DateTime Timestamp { get; set; }
}

// ============ EMBEDDED HTML UI ============

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<RoverService>();
        var app = builder.Build();

        app.MapGet("/", () => Results.Content(GetHtml(), "text/html"));
        app.MapGet("/api/rovers", (RoverService service) => service.GetRovers());
        app.MapGet("/api/measurements", (RoverService service) => service.GetMeasurements());
        app.MapPost("/api/simulate", (RoverService service) => 
        {
            service.SimulateMovement();
            return Results.Ok();
        });

        app.Run();
    }

    static string GetHtml() => @"
<!DOCTYPE html>
<html>
<head>
    <title>Monolithic Rover Tracker</title>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: Arial, sans-serif; }
        .header { background: #2c3e50; color: white; padding: 15px; }
        .container { display: grid; grid-template-columns: 3fr 1fr; height: calc(100vh - 60px); }
        #map { height: 100%; }
        .sidebar { background: #ecf0f1; padding: 20px; overflow-y: auto; }
        .rover-card { background: white; padding: 15px; margin: 10px 0; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .rover-name { font-weight: bold; font-size: 18px; margin-bottom: 10px; }
        .stat { margin: 5px 0; font-size: 14px; }
        button { width: 100%; padding: 12px; margin: 10px 0; background: #3498db; color: white; border: none; border-radius: 4px; cursor: pointer; font-size: 16px; }
        button:hover { background: #2980b9; }
        .measurements { max-height: 300px; overflow-y: auto; font-size: 12px; background: #f8f9fa; padding: 10px; border-radius: 4px; }
        .measurement { padding: 5px; border-bottom: 1px solid #ddd; }
    </style>
</head>
<body>
    <div class='header'>
        <h1>🤖 Monolithic Rover Tracking System</h1>
    </div>
    <div class='container'>
        <div id='map'></div>
        <div class='sidebar'>
            <button onclick='simulate()'>▶️ Simulate Movement</button>
            <button onclick='autoSimulate()'>🔄 Auto Simulate (5s)</button>
            <div id='rovers'></div>
            <h3>Recent Measurements</h3>
            <div id='measurements' class='measurements'></div>
        </div>
    </div>

    <script>
        const map = L.map('map').setView([20, 0], 2);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap'
        }).addTo(map);

        const markers = {};
        const trails = {};
        let autoInterval = null;

        async function loadData() {
            const rovers = await fetch('/api/rovers').then(r => r.json());
            const measurements = await fetch('/api/measurements').then(r => r.json());

            // Update rovers
            document.getElementById('rovers').innerHTML = rovers.map(r => `
                <div class='rover-card'>
                    <div class='rover-name' style='color: ${r.color}'>${r.name}</div>
                    <div class='stat'>📍 Lon: ${r.longitude.toFixed(4)}</div>
                    <div class='stat'>📍 Lat: ${r.latitude.toFixed(4)}</div>
                </div>
            `).join('');

            // Update map markers
            rovers.forEach(rover => {
                if (!markers[rover.id]) {
                    markers[rover.id] = L.circleMarker([rover.latitude, rover.longitude], {
                        color: rover.color,
                        fillColor: rover.color,
                        fillOpacity: 0.8,
                        radius: 8
                    }).addTo(map).bindPopup(rover.name);

                    trails[rover.id] = L.polyline([], {
                        color: rover.color,
                        weight: 2,
                        opacity: 0.5
                    }).addTo(map);
                } else {
                    markers[rover.id].setLatLng([rover.latitude, rover.longitude]);
                }
            });

            // Update trails
            const roverMeasurements = {};
            measurements.forEach(m => {
                if (!roverMeasurements[m.roverId]) roverMeasurements[m.roverId] = [];
                roverMeasurements[m.roverId].push([m.latitude, m.longitude]);
            });

            Object.keys(roverMeasurements).forEach(roverId => {
                if (trails[roverId]) {
                    trails[roverId].setLatLngs(roverMeasurements[roverId]);
                }
            });

            // Update measurements list
            document.getElementById('measurements').innerHTML = measurements.slice(-20).reverse().map(m => `
                <div class='measurement'>
                    <strong>${m.roverName}</strong> - 
                    Wind: ${m.windSpeed}m/s @ ${m.windDirection}° - 
                    ${new Date(m.timestamp).toLocaleTimeString()}
                </div>
            `).join('');
        }

        async function simulate() {
            await fetch('/api/simulate', { method: 'POST' });
            await loadData();
        }

        function autoSimulate() {
            if (autoInterval) {
                clearInterval(autoInterval);
                autoInterval = null;
            } else {
                autoInterval = setInterval(simulate, 5000);
                simulate();
            }
        }

        loadData();
        setInterval(loadData, 2000);
    </script>
</body>
</html>
";
}
