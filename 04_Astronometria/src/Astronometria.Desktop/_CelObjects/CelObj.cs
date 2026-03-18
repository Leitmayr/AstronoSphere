using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ObjID { PLANET, SUN, MOON, MINOR, COMET, STELLAR, UNKNOWN };
/**
 * \enum pname
 * \brief enthaelt die Planetennamen als enums
 */
public enum pname { MERCURY, VENUS, EARTH, MARS, JUPITER, SATURN, URANUS, NEPTUNE, PLUTO };



public class CelObj
{

	public double _RightAscension;      // Right Ascension in range 0...360°
	public double _Declination;         // Declination in range -90...+90°
	double _magnitude;					// magnitude of the celestial object

	public double get_Declination
	{
		get { return _Declination; }
	}

	public double get_RightAscension
	{
		get { return _RightAscension; }
	}






	public CelObj(double arg_GALong, double arg_GALat)
	{
		_RightAscension = arg_GALong;
		_Declination = arg_GALat;


	}
}