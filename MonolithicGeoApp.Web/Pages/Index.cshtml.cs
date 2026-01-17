using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MonolithicGeoApp.Web.Pages
{
    public class IndexModel : PageModel
    {
        public GeoService GeoService { get; }
        public string Result { get; set; } = "";

        public IndexModel(GeoService geoService)
        {
            GeoService = geoService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostDistance()
        {
            if (GeoService.Cities.Count >= 2)
            {
                var city1 = GeoService.Cities[0];
                var city2 = GeoService.Cities[1];
                var distance = GeoService.CalculateDistance(city1, city2);
                Result = $"Distance from {city1.Name} to {city2.Name}: {distance:F2} km";
            }
            return Page();
        }

        public IActionResult OnPostNearest()
        {
            if (GeoService.Cities.Count > 0)
            {
                var target = GeoService.Cities[0];
                var nearest = GeoService.FindNearest(target);
                if (nearest != null)
                {
                    var distance = GeoService.CalculateDistance(target, nearest);
                    Result = $"Nearest to {target.Name}: {nearest.Name} ({distance:F2} km)";
                }
            }
            return Page();
        }

        public IActionResult OnPostAddRandom()
        {
            var random = new Random();
            var name = $"City{random.Next(100, 999)}";
            var lon = random.NextDouble() * 360 - 180;
            var lat = random.NextDouble() * 180 - 90;
            var pop = random.Next(100000, 10000000);
            
            GeoService.AddCity(new City(name, lon, lat, pop));
            Result = $"Added: {name} at ({lon:F2}, {lat:F2})";
            return Page();
        }
    }
}
