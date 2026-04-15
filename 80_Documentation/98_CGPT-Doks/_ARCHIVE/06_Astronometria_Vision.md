# Vision and Scope of Astronometria

Astronometria is the astro engine of the AstronoSphere framework.

The vision is to develop a comprehensive astronomy program, and I'm aware that this can't be accomplished in four weeks. The timeframe for the project is 2-3 years. It's a hobby project, and I'll have periods when I dedicate more time to it and others when I have less.

In terms of content, I want to be able to display the night sky in different projections. My models are the monthly charts in "Stars and Space" (horizontal maps, heliocentric representation of planetary motion, Mercator projection of the celestial region around the ecliptic). I'll also use illustrations of the cardinal directions, as in the Kosmos book "Which Star Is That?", 1985 edition.

Furthermore, constellations and events should be accurately calculated and displayed over longer periods. This means that the data and algorithms used must be very precise. Calculating and visualizing the events and constellations requires a simulation engine.

I want to be able to compile an astronomical yearbook of reports (using the Kosmos "The Sky Yearbook" as a template). This means I want to generate monthly reports that can be illustrated.

Sub-projects should be integrable, such as the visualization and simulation of Jupiter with its four large moons, or something similar for the Saturnian system. For Saturn, this would also include the opening of the ring system.

Many of the necessary algorithms already exist in C++ or C#, for example, the calculation of planetary positions, the moon's position including its phases, etc., in C++. The underlying celestial mechanics, including precession, obliquity of the ecliptic, aberration, and most coordinate transformations, are also programmed and, in some cases, well-tested. I want to continue using these algorithms because my goal isn't simply to "ChatGPT – create everything I just told you to do."

As a basis for my calculations so far, I've used the book "Astronomical Algorithms," 2nd edition, by Jean Meeus.


I've used the book "Astronomical Algorithms," 2nd edition, by Jean Meeus as a guide.

Many of the necessary algorithms already exist in C++ or C#, for example, the calculation of planetary positions, the moon's position including its phases, etc., in C++. Existing data is sourced from the internet and would need to be parsed. For VSOP, for example, the data was taken directly from the IMCCE FTP server. I also used a modified version of VizieR's Bright Stars Catalog for the existing star charts (e.g., restricting the data to the sky at +47° North and north).

The project is designed to run on different platforms. I want to be able to port it to a Raspberry Pi 4 and connect it to a telescope via INDI. The goal is to then be able to interact with the telescope from within my program. We will also need to integrate something like an "observation mode."