Best free tools to convert spatial data (2024): ---> This is from Claud4.5 
Command-line tools:
1. GDAL/OGR ⭐ Industry standard

Converts between 100+ formats

ogr2ogr -f GeoJSON output.geojson input.shp

Supports: Shapefile, KML, GeoJSON, GeoPackage, PostGIS

2. QGIS ⭐ Most popular GUI

Desktop GIS application

Drag-and-drop conversion

Visual data inspection + conversion

Python libraries:
3. GeoPandas ⭐ Best for Python

import geopandas as gpd
gdf = gpd.read_file('input.shp')
gdf.to_file('output.geojson', driver='GeoJSON')

Copy
python
4. Fiona - Lightweight alternative
5. Shapely - Geometry manipulation
6. PyProj - Coordinate transformations

Online tools:
7. geojson.io ⭐ Best web tool

Drag-and-drop files

Visual editor + instant preview

Export to multiple formats

8. MyGeodata Converter - Supports 100+ formats
9. MapShaper - Simplify + convert large files

JavaScript/Node.js:
10. Turf.js - Spatial analysis in browser
11. GDAL.js - GDAL compiled to WebAssembly

Database tools:
12. PostGIS - PostgreSQL extension
13. SpatiaLite - SQLite extension

Recommended by use case:
Quick conversion: geojson.io

Automation/scripts: GDAL/OGR

Python projects: GeoPandas

Visual work: QGIS

Microservices: Custom Python (like we did)

All are completely free and open-source