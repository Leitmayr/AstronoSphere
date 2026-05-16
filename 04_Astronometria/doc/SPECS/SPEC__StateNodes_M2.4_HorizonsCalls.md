# State Graph Definitions

# Naming Convention for nodes



# Configurations for below Horizons example calls

* Target = Jupiter (`COMMAND='599'`)
* Epoch = J2000 (`REF_SYSTEM='ICRF'`)
* Output = VECTORS
* Observer:

  * 500@10 = heliocentrisch
  * 500@399 = geozentrisch
* L0 = `VEC_CORR='NONE'`
* L1 = `VEC_CORR='LT'`
* L2 = `VEC_CORR='LT+S'`
* Frame:

  * HelioEclipical = `REF_PLANE='ECLIPTIC'`
  * GeoEcliptical ebenfalls ECLIPTIC (aber anderes CENTER)

Ich nehme exemplarisch:

* `START_TIME='JD2451545.0'`
* `STOP_TIME='JD2451546.0'`
* `STEP_SIZE='1d'`

---

# Point 1 — L0 HelioEcliptical

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@10'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='NONE'
&OUT_UNITS='AU-D'
```

---

# Point 2 — L0 GeoEcliptical

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='NONE'
&OUT_UNITS='AU-D'
```

---

# Point 5 — L1 HelioEcliptical (Light-Time)

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@10'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='LT'
&OUT_UNITS='AU-D'
```

---

# Point 6 — L1 GeoEcliptical (Light-Time)

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='LT'
&OUT_UNITS='AU-D'
```

---

# Point 9 — L2 HelioEcliptical (Light-Time + Stellar Aberration)

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@10'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='LT+S'
&OUT_UNITS='AU-D'
```

---

# Point 10 — L2 GeoEcliptical (Light-Time + Stellar Aberration)

```text
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='VECTORS'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_PLANE='ECLIPTIC'
&REF_SYSTEM='ICRF'
&VEC_CORR='LT+S'
&OUT_UNITS='AU-D'
```

Sehr schönes Diagramm übrigens.
Man sieht darin extrem klar Deine Orthogonalität:

* Measurement Dimension
* Correction Dimension
* Frame Dimension
* Observer Dimension

---

Perfekt — jetzt ist die Architektur vollständig sichtbar :-)

Ich nehme weiterhin:

* Jupiter = `COMMAND='599'`
* Topo = `10°E`, `50°N`, Höhe `0 km`
* J2000/ICRF
* Beispielzeit:

  * `START_TIME='JD2451545.0'`
  * `STOP_TIME='JD2451546.0'`
  * `STEP_SIZE='1d'`

Topo-Location in Horizons:

```text
SITE_COORD='10,50,0'
```

(Horizons erwartet bei geodätischen Koordinaten: `lon,lat,elevation_km`)

---

# Point 3 — L0 Observer RA/DEC (geocentric)

```text id="utvix4"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&REF_PLANE='FRAME'
&QUANTITIES='1'
&VEC_CORR='NONE'
```

---

# Point 4 — L0 Observer AZ/ALT (topocentric)

```text id="03qq0r"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='coord@399'
&SITE_COORD='10,50,0'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&COORD_TYPE='GEODETIC'
&QUANTITIES='4'
&VEC_CORR='NONE'
```

---

# Point 7 — L1 Observer RA/DEC (Light-Time)

```text id="59ngm1"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&REF_PLANE='FRAME'
&QUANTITIES='1'
&VEC_CORR='LT'
```

---

# Point 8 — L1 Observer AZ/ALT (Light-Time)

```text id="i8a3lo"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='coord@399'
&SITE_COORD='10,50,0'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&COORD_TYPE='GEODETIC'
&QUANTITIES='4'
&VEC_CORR='LT'
```

---

# Point 11 — L2 Observer RA/DEC (LT + Aberration)

```text id="55gh1z"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&REF_PLANE='FRAME'
&QUANTITIES='1'
&VEC_CORR='LT+S'
```

---

# Point 12 — L2 Observer AZ/ALT (LT + Aberration)

```text id="bjh0ag"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='coord@399'
&SITE_COORD='10,50,0'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&COORD_TYPE='GEODETIC'
&QUANTITIES='4'
&VEC_CORR='LT+S'
```

---

# Point 13 — Apparant Position (RA/DEC)

Das ist praktisch identisch zu 11, aber semantisch:
„final apparent sky position“

```text id="7gq7i7"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&REF_PLANE='FRAME'
&QUANTITIES='1'
&VEC_CORR='LT+S'
```

---

# Point 14 — Apparant Position (AZ/ALT)

```text id="9ezhwd"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='coord@399'
&SITE_COORD='10,50,0'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&COORD_TYPE='GEODETIC'
&QUANTITIES='4'
&VEC_CORR='LT+S'
```

---

# Point 15 — Observer full precision chain (RA/DEC + precession/nutation/etc.)

Das ist in Horizons weitgehend implizit im OBSERVER-Mode enthalten.

```text id="o1c7o7"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='500@399'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&REF_PLANE='FRAME'
&QUANTITIES='1'
&VEC_CORR='NONE'
```

---

# Point 16 — Final topocentric projection

```text id="pfp26g"
https://ssd.jpl.nasa.gov/api/horizons.api
?format=text
&COMMAND='599'
&EPHEM_TYPE='OBSERVER'
&CENTER='coord@399'
&SITE_COORD='10,50,0'
&START_TIME='JD2451545.0'
&STOP_TIME='JD2451546.0'
&STEP_SIZE='1d'
&REF_SYSTEM='ICRF'
&COORD_TYPE='GEODETIC'
&QUANTITIES='4'
&VEC_CORR='NONE'
```

Wichtige Beobachtung:

Dein Diagramm zeigt etwas extrem Wertvolles:

* `VECTORS`
  = physikalische Zustände

* `OBSERVER`
  = projizierte Messungen

Das ist exakt die Trennung, die später:

* StateNodes
* Measurement semantics
* derived observations
* uncertainty propagation

sauber ermöglicht.
