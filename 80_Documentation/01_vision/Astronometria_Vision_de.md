# Vision and Scope of Astronometria

Astronometria ist die Astro Engine des Frameworks AstronoSphere.

Die Vision ist ein umfassendes Astronomie-Programm zu entwickeln, und ich bin mir bewusst, dass das nicht in vier Wochen erfolgen kann. Der Zeithorizont für das Projekt ist 2-3 Jahre. Es ist ein Hobby-Projekt und ich werde Zeiten haben, in denen ich mehr daran arbeite und andere, in denen weniger Zeit da ist.

Inhaltlich möchte ich den Sternhimmel in unterschiedlichen Projektionen darstellen können. Vorlagen sind für mich die Monatsdarstellungen in „Sterne und Weltraum“ (horizontale Karten, heliozentrische Darstellung des Planetenlaufs, Merkatorprojektion der Himmelsregion um die Ekliptik). Außerdem eine Bebilderung der Himmelsrichtungen, wie im Buch von Kosmos „Welcher Stern ist das?“, Auflage von 1985. 

Darüber hinaus sollen Konstellationen und Ereignisse über längere Zeiträume genau berechnet und dargestellt werden. Daraus resultiert, dass die verwendeten Daten und Algorithmen sehr genau sein müssen. Die Berechnung der Ereignisse und Konstellationen sowie deren Darstellung erfordert eine Simulation Engine.

Ich möchte in der Lage sein, ein astronomisches Jahresbuch über Berichte zusammenzustellen (Vorlage: Kosmos "Das Himmelsjahr"). Das bedeutet, dass ich monatliche Berichte erzeugen möchte, die entsprechend bebildert werden können.

Es sollen Teilprojekte integrierbar sein wie zum Beispiel die Darstellung und Simulation von Jupiter mit seinen vier großen Jupitermonde oder ähnliches für das Saturnsystem. Beim Saturn zusätzlich die Öffnung des Ringsystems.

Viele der notwendigen Algorithmen liegen bereits in C++ oder C# vor, z.B. in C++ die Berechnung der Planetenpositionen, der Mondposition inkl. Mondphasen, etc. Auch die zugrundeliegende Himmelsmechanik mit Präzession, Ekliptikschiefe, Aberration und die meisten Koordinatentransformationen sind programmiert und teils gut getestet. Diese Algorithmen möchte ich weiterverwenden, weil mein Ziel nicht ist: „ChatGPT – erstelle mir mal alles, was ich Dir gerade gesagt habe“.

Als Basis für meine bisherigen Berechnungen habe ich das Buch „Astronomical Algorithms“, 2. Auflage, von Jean Meeus verwendet. 
Vorhandene Daten stammen aus dem Internet und müssten über eine Parserschicht eingelesen werden. Zu VSOP sind die Daten z.B. direkt vom FTP-Server des IMCCE entnommen. Außerdem habe ich für die vorhandenen Sternkarten eine modifizierte Version des Bright Stars Catalog von VizieR verwendet (z.B. Einschränkung auf den Himmel bei +47°Nord und nördlicher).
Das Projekt soll so erstellt werden, dass es auf unterschiedlichen Plattformen lauffähig ist. Ich möchte es auf einen Rhaspberry Pi 4 portieren können und über INDI mit einem Teleskop koppeln. Ziel ist dann, aus meinem Programm heraus mit dem Teleskop zu interagieren. Wir werden auch etwas wie den „Beobachtungsmodus“ integrieren müssen. 

