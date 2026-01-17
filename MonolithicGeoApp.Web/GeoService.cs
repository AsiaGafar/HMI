using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace MonolithicGeoApp.Web
{
    public class GeoService
    {
        private readonly GeometryFactory _geometryFactory = new GeometryFactory();
        public List<City> Cities { get; private set; }

        public GeoService()
        {
            Cities = new List<City>
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
        }

        public double CalculateDistance(City city1, City city2)
        {
            var point1 = _geometryFactory.CreatePoint(new Coordinate(city1.Longitude, city1.Latitude));
            var point2 = _geometryFactory.CreatePoint(new Coordinate(city2.Longitude, city2.Latitude));
            return point1.Distance(point2) * 111.32;
        }

        public City? FindNearest(City target)
        {
            var targetPoint = _geometryFactory.CreatePoint(new Coordinate(target.Longitude, target.Latitude));
            return Cities
                .Where(c => c != target)
                .OrderBy(c => targetPoint.Distance(_geometryFactory.CreatePoint(new Coordinate(c.Longitude, c.Latitude))))
                .FirstOrDefault();
        }

        public void AddCity(City city) => Cities.Add(city);
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
