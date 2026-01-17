window.initMap = function() {
    var map = L.map('map').setView([20, 0], 2);
    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);
    
    fetch('/data/countries.geojson')
        .then(response => response.json())
        .then(data => {
            L.geoJSON(data, {
                style: {
                    color: '#3388ff',
                    weight: 1,
                    fillOpacity: 0.3
                },
                onEachFeature: function(feature, layer) {
                    if (feature.properties && feature.properties.ADMIN) {
                        layer.bindPopup(feature.properties.ADMIN);
                    }
                }
            }).addTo(map);
        });
}
