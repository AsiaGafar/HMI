using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetTopologySuite.Geometries;

namespace MonolithicGeoApp.WinForms
{
    public class MainForm : Form
    {
        private ListBox cityListBox;
        private Button calculateButton;
        private Label resultLabel;
        private Panel mapPanel;
        private GeometryFactory geometryFactory = new GeometryFactory();
        private List<City> cities;

        public MainForm()
        {
            InitializeData();
            InitializeUI();
        }

        private void InitializeData()
        {
            cities = new List<City>
            {
                new City("New York", -74.006, 40.7128),
                new City("London", -0.1276, 51.5074),
                new City("Tokyo", 139.6917, 35.6895),
                new City("Mumbai", 72.8777, 19.0760),
                new City("Sydney", 151.2093, -33.8688),
                new City("Cairo", 31.2357, 30.0444),
                new City("São Paulo", -46.6333, -23.5505),
                new City("Moscow", 37.6173, 55.7558)
            };
        }

        private void InitializeUI()
        {
            Text = "Monolithic Geospatial App";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            cityListBox = new ListBox
            {
                Location = new Point(20, 20),
                Size = new Size(200, 400),
                SelectionMode = SelectionMode.MultiSimple
            };
            foreach (var city in cities)
                cityListBox.Items.Add(city.Name);

            calculateButton = new Button
            {
                Location = new Point(20, 430),
                Size = new Size(200, 30),
                Text = "Calculate Distance"
            };
            calculateButton.Click += CalculateButton_Click;

            resultLabel = new Label
            {
                Location = new Point(20, 470),
                Size = new Size(200, 80),
                Text = "Select 2 cities"
            };

            mapPanel = new Panel
            {
                Location = new Point(240, 20),
                Size = new Size(540, 530),
                BorderStyle = BorderStyle.FixedSingle
            };
            mapPanel.Paint += MapPanel_Paint;

            Controls.AddRange(new Control[] { cityListBox, calculateButton, resultLabel, mapPanel });
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (cityListBox.SelectedIndices.Count != 2)
            {
                resultLabel.Text = "Select exactly 2 cities";
                return;
            }

            var city1 = cities[cityListBox.SelectedIndices[0]];
            var city2 = cities[cityListBox.SelectedIndices[1]];

            var point1 = geometryFactory.CreatePoint(new Coordinate(city1.Longitude, city1.Latitude));
            var point2 = geometryFactory.CreatePoint(new Coordinate(city2.Longitude, city2.Latitude));

            double distance = point1.Distance(point2) * 111.32;

            resultLabel.Text = $"{city1.Name} to {city2.Name}\n\nDistance:\n{distance:F0} km";
            mapPanel.Invalidate();
        }

        private void MapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            var minLon = cities.Min(c => c.Longitude);
            var maxLon = cities.Max(c => c.Longitude);
            var minLat = cities.Min(c => c.Latitude);
            var maxLat = cities.Max(c => c.Latitude);

            foreach (var city in cities)
            {
                var x = (int)((city.Longitude - minLon) / (maxLon - minLon) * (mapPanel.Width - 40) + 20);
                var y = (int)((maxLat - city.Latitude) / (maxLat - minLat) * (mapPanel.Height - 40) + 20);

                g.FillEllipse(Brushes.Red, x - 5, y - 5, 10, 10);
                g.DrawString(city.Name, Font, Brushes.Black, x + 8, y - 8);
            }

            if (cityListBox.SelectedIndices.Count == 2)
            {
                var city1 = cities[cityListBox.SelectedIndices[0]];
                var city2 = cities[cityListBox.SelectedIndices[1]];

                var x1 = (int)((city1.Longitude - minLon) / (maxLon - minLon) * (mapPanel.Width - 40) + 20);
                var y1 = (int)((maxLat - city1.Latitude) / (maxLat - minLat) * (mapPanel.Height - 40) + 20);
                var x2 = (int)((city2.Longitude - minLon) / (maxLon - minLon) * (mapPanel.Width - 40) + 20);
                var y2 = (int)((maxLat - city2.Latitude) / (maxLat - minLat) * (mapPanel.Height - 40) + 20);

                g.DrawLine(new Pen(Color.Blue, 2), x1, y1, x2, y2);
            }
        }
    }

    public class City
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public City(string name, double longitude, double latitude)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
