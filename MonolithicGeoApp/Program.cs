using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace MonolithicGeoApp
{
    class Program
    {
        static GeometryFactory geometryFactory = new GeometryFactory();
        static List<City> cities = new List<City>();

        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║  Monolithic Geospatial Application    ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            InitializeData();
            MainMenu();
        }

        static void InitializeData()
        {
            cities = new List<City>
            {
                new City("New York", -74.006, 40.7128, 8336817),
                new City("London", -0.1276, 51.5074, 8982000),
                new City("Tokyo", 139.6917, 35.6895, 13960000),
                new City("Mumbai", 72.8777, 19.0760, 20411000),
                new City("Sydney", 151.2093, -33.8688, 5312000),
                new City("Cairo", 31.2357, 30.0444, 20076000),
                new City("São Paulo", -46.6333, -23.5505, 12325000),
                new City("Moscow", 37.6173, 55.7558, 12537000),
                new City("Beijing", 116.4074, 39.9042, 21540000),
                new City("Delhi", 77.1025, 28.7041, 30291000)
            };
            Console.WriteLine($"✓ Loaded {cities.Count} cities\n");
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n═══ MENU ═══");
                Console.WriteLine("1. List all cities");
                Console.WriteLine("2. Calculate distance between cities");
                Console.WriteLine("3. Find nearest city");
                Console.WriteLine("4. Add new city");
                Console.WriteLine("5. Show city details");
                Console.WriteLine("6. Export to GeoJSON");
                Console.WriteLine("7. Calculate bounding box");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": ListCities(); break;
                    case "2": CalculateDistance(); break;
                    case "3": FindNearest(); break;
                    case "4": AddCity(); break;
                    case "5": ShowCityDetails(); break;
                    case "6": ExportGeoJSON(); break;
                    case "7": CalculateBoundingBox(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice"); break;
                }
            }
        }

        static void ListCities()
        {
            Console.WriteLine("\n═══ CITIES ═══");
            for (int i = 0; i < cities.Count; i++)
            {
                var city = cities[i];
                Console.WriteLine($"{i + 1}. {city.Name,-15} | Lon: {city.Longitude,8:F2} | Lat: {city.Latitude,7:F2} | Pop: {city.Population:N0}");
            }
        }

        static void CalculateDistance()
        {
            ListCities();
            Console.Write("\nEnter first city number: ");
            if (!int.TryParse(Console.ReadLine(), out int idx1) || idx1 < 1 || idx1 > cities.Count)
            {
                Console.WriteLine("Invalid city number");
                return;
            }

            Console.Write("Enter second city number: ");
            if (!int.TryParse(Console.ReadLine(), out int idx2) || idx2 < 1 || idx2 > cities.Count)
            {
                Console.WriteLine("Invalid city number");
                return;
            }

            var city1 = cities[idx1 - 1];
            var city2 = cities[idx2 - 1];

            var point1 = geometryFactory.CreatePoint(new Coordinate(city1.Longitude, city1.Latitude));
            var point2 = geometryFactory.CreatePoint(new Coordinate(city2.Longitude, city2.Latitude));

            double distance = point1.Distance(point2);
            double distanceKm = distance * 111.32; // Approximate conversion

            Console.WriteLine($"\n✓ Distance from {city1.Name} to {city2.Name}:");
            Console.WriteLine($"  Euclidean: {distance:F4} degrees");
            Console.WriteLine($"  Approx: {distanceKm:F2} km");
        }

        static void FindNearest()
        {
            ListCities();
            Console.Write("\nEnter city number to find nearest: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > cities.Count)
            {
                Console.WriteLine("Invalid city number");
                return;
            }

            var targetCity = cities[idx - 1];
            var targetPoint = geometryFactory.CreatePoint(new Coordinate(targetCity.Longitude, targetCity.Latitude));

            var nearest = cities
                .Where(c => c != targetCity)
                .Select(c => new
                {
                    City = c,
                    Distance = targetPoint.Distance(geometryFactory.CreatePoint(new Coordinate(c.Longitude, c.Latitude)))
                })
                .OrderBy(x => x.Distance)
                .First();

            Console.WriteLine($"\n✓ Nearest city to {targetCity.Name}: {nearest.City.Name}");
            Console.WriteLine($"  Distance: {nearest.Distance * 111.32:F2} km");
        }

        static void AddCity()
        {
            Console.Write("\nCity name: ");
            string name = Console.ReadLine() ?? "";
            
            Console.Write("Longitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lon))
            {
                Console.WriteLine("Invalid longitude");
                return;
            }

            Console.Write("Latitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lat))
            {
                Console.WriteLine("Invalid latitude");
                return;
            }

            Console.Write("Population: ");
            if (!int.TryParse(Console.ReadLine(), out int pop))
            {
                Console.WriteLine("Invalid population");
                return;
            }

            cities.Add(new City(name, lon, lat, pop));
            Console.WriteLine($"✓ Added {name}");
        }

        static void ShowCityDetails()
        {
            ListCities();
            Console.Write("\nEnter city number: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > cities.Count)
            {
                Console.WriteLine("Invalid city number");
                return;
            }

            var city = cities[idx - 1];
            var point = geometryFactory.CreatePoint(new Coordinate(city.Longitude, city.Latitude));

            Console.WriteLine($"\n═══ {city.Name.ToUpper()} ═══");
            Console.WriteLine($"Coordinates: {city.Longitude:F4}, {city.Latitude:F4}");
            Console.WriteLine($"Population: {city.Population:N0}");
            Console.WriteLine($"Geometry Type: {point.GeometryType}");
            Console.WriteLine($"WKT: {point.AsText()}");
        }

        static void ExportGeoJSON()
        {
            var features = cities.Select(c => new
            {
                type = "Feature",
                properties = new { name = c.Name, population = c.Population },
                geometry = new
                {
                    type = "Point",
                    coordinates = new[] { c.Longitude, c.Latitude }
                }
            });

            var geoJson = new
            {
                type = "FeatureCollection",
                features = features
            };

            string json = JsonConvert.SerializeObject(geoJson, Formatting.Indented);
            string filename = "cities_export.geojson";
            System.IO.File.WriteAllText(filename, json);
            Console.WriteLine($"\n✓ Exported to {filename}");
        }

        static void CalculateBoundingBox()
        {
            var points = cities.Select(c => geometryFactory.CreatePoint(new Coordinate(c.Longitude, c.Latitude))).ToArray();
            var multiPoint = geometryFactory.CreateMultiPoint(points);
            var envelope = multiPoint.EnvelopeInternal;

            Console.WriteLine("\n═══ BOUNDING BOX ═══");
            Console.WriteLine($"Min Longitude: {envelope.MinX:F4}");
            Console.WriteLine($"Max Longitude: {envelope.MaxX:F4}");
            Console.WriteLine($"Min Latitude: {envelope.MinY:F4}");
            Console.WriteLine($"Max Latitude: {envelope.MaxY:F4}");
            Console.WriteLine($"Width: {envelope.Width:F4} degrees");
            Console.WriteLine($"Height: {envelope.Height:F4} degrees");
        }
    }

    public class City
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Population { get; set; }

        public City(string name, double longitude, double latitude, int population)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            Population = population;
        }
    }
}
