import json

with open('india_cities.geojson', 'r') as f:
    data = json.load(f)

print("Google Maps Links:\n")
for feature in data['features']:
    coords = feature['geometry']['coordinates']
    props = feature['properties']
    lat, lon = coords[1], coords[0]
    
    google_link = f"https://www.google.com/maps?q={lat},{lon}"
    print(f"{props['city']}, {props['state']}")
    print(f"  {google_link}\n")
