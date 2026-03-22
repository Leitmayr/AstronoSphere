using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




public class StarCat
{
	public const int C_MAX_STARCAT_ELEMENTS = 6846;

	 public struct starCatalogElement
	{
		// structure derived from an export of the
		// VizieR V/50/catalog: https://vizier.cds.unistra.fr/viz-bin/VizieR-3?-source=V/50/catalog&-out.add=.

		//full info
		string fullInfo;

		//ID Block --------------------------------------------------------------
		//HarvardRevisedNumber
		string HRN;

		//Name
		string Name;        // unknown name: default is "-"

		//HenryDraperCatalog
		string HD;

		//AitkenDoubleStarCatalog
		string ADS;         // no double star: default is "-1"

		//VariableStarID
		//string VarID;       // no variable star: default is "-"
		//endof ID Block --------------------------------------------------------




		////Magnitude Block -------------------------------------------------------
		////VisualMagnitude
		//double Vmag;

		////Color in UBV
		//double UBV;         // no UBV value? -> default is "-100"
		////endof Magnitude Block -------------------------------------------------




		////Spectral Type Block ---------------------------------------------------
		////SpectralTypeLong
		//string SpTypeLong;

		////SpectralTypeShort
		//string SpTypeShort;
		////endof Spectral Type Block ---------------------------------------------




		////Constellation Block ---------------------------------------------------
		////LatinConstellationNameShort
		//string ConstellationLatS;

		////LatinConstellationNameLong
		//string ConstellationLatL;

		////GermanConstellationNameLong
		//string ConstellationGerL;
		////endof Constellation Block ---------------------------------------------




		////Alternative ID-Block --------------------------------------------------
		////StarNumberingInConstellation
		//string StarIDNrInConst;

		////GreekLetterInConstellation
		//string StarIDGreekLetterInConst;    // no Greek letter? -> default is "-"
		////endof Alternative ID-Block --------------------------------------------




		////Coordinates Block -----------------------------------------------------
		////Original coordinates
		////RA_h:m:s_J2000.0
		//string OriginalRA_hms;

		////DEC_d:m:s_J2000.0
		//string OriginalDEC_dms;

		////Processed
		////RA in double
		//double RA_double;

		////RA hours
		//int RA_hour;

		////RA min
		//int RA_min;

		////RA sec
		//double RA_sec;

		////RA in deg
		//double RA_double_deg;

		////AlgebraicSignOfDEC
		//int DEC_AlgSign;

		////DECin double
		//double DEC_double;

		////DEC degrees
		//int DEC_deg;

		////DEC min
		//int DEC_min;

		////DEC sec
		//double DEC_sec;
		////endof Coordinates Block -------------------------------------------
	};




	//public starCatalogElement StarCatalog = new starCatalogElement { "6846", "9110", "-", "225289", "-1" };

	};



