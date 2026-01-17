import csv
import json

def csv_to_geojson(csv_file, output_file):
    features = []
    
    with open(csv_file, 'r') as f:
        reader = csv.DictReader(f)
        for row in reader:
            feature = {
                "type": "Feature",
                "geometry": {
                    "type": "Point",
                    "coordinates": [float(row['longitude']), float(row['latitude'])]
                },
                "properties": {
                    "city": row['city'],
                    "state": row['state'],
                    "population": int(row['population'])
                }
            }
            features.append(feature)
    
    geojson = {
        "type": "FeatureCollection",
        "features": features
    }
    
    with open(output_file, 'w') as f:
        json.dump(geojson, f, indent=2)
    
    print(f"Converted {len(features)} features to GeoJSON")

csv_to_geojson('sample_cities.csv', 'india_cities.geojson')
