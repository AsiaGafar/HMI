using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace HMI.GeoSpatial
{
    // Geospatial Data Model
    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
    }

    // Simple Map UI
    public class GeoMapForm : Form
    {
        private List<GeoPoint> points;
        private Panel mapPanel;

        public GeoMapForm()
        {
            InitializeUI();
            LoadSampleData();
        }

        private void InitializeUI()
        {
            Text = "Geospatial Viewer";
            Size = new Size(800, 600);
            
            mapPanel = new Panel { Dock = DockStyle.Fill };
            mapPanel.Paint += MapPanel_Paint;
            Controls.Add(mapPanel);
        }

        private void LoadSampleData()
        {
            points = new List<GeoPoint>
            {
                new GeoPoint { Latitude = 40.7128, Longitude = -74.0060, Name = "New York" },
                new GeoPoint { Latitude = 34.0522, Longitude = -118.2437, Name = "Los Angeles" },
                new GeoPoint { Latitude = 51.5074, Longitude = -0.1278, Name = "London" }
            };
        }

        private void MapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.LightBlue);

            foreach (var point in points)
            {
                int x = (int)((point.Longitude + 180) * mapPanel.Width / 360);
                int y = (int)((90 - point.Latitude) * mapPanel.Height / 180);
                
                g.FillEllipse(Brushes.Red, x - 5, y - 5, 10, 10);
                g.DrawString(point.Name, Font, Brushes.Black, x + 10, y - 10);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GeoMapForm());
        }
    }
}
