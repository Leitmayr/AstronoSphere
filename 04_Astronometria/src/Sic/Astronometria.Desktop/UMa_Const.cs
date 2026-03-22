using System;
using System.Collections.Generic;
using System.Text;


static class UMaBrightStars
{

	public const int C_UMa_MaxNumberOfStars = 8;
	public const int C_UMa_NumberOfConnectionLines = 7;


	public static readonly string[] UMaStarSpectralType =
	{
        //	visual magnitude		
			"B3",	//	eta
			"A1",   //	zeta
			"A1",   //	zeta2 (Reiterlein)
			"A0",	//	epsilon
	        "A3",	//	delta
	        "A0",	//	gamma
	        "A1",	//	beta
	    	"K0"	//	alpha

    };

	public static readonly string[] UMaStarNames =
	{
		"eta",
		"zeta",
		"zeta2", // (Reiterlein)
		"epsilon",
		"delta",
		"gamma",
		"beta",
		"alpha"

	};


	public static readonly double[] UMaStarMagnitudes =
{
        //	visual magnitude		
			1.86,	//	eta
			2.27,   //	zeta
			3.95,   //	zeta2 (Reiterlein)
			1.77,	//	epsilon
	        3.31,	//	delta
	        2.44,	//	gamma
	        2.37,	//	beta
	    	1.79	//	alpha

    };


	public static readonly double[,] UMaStarCoordinates =
    {
        //	RA		DEC			
        { 206.88500   ,   49.31 }   ,	//	eta
	    { 200.98125   ,   54.93 }   ,   //	zeta
		{ 200.98500   ,   54.92167 }   ,   //	zeta2 (Reiterlein)
	    { 193.50708   ,   55.96 }   ,	//	epsilon
	    { 183.85667   ,   57.03 }   ,	//	delta
	    { 178.45750   ,   53.69 }   ,	//	gamma
	    { 165.46042   ,   56.38 }   ,	//	beta
	    { 165.93208   ,   61.75 }		//	alpha

    };


	public static readonly double[,] UMaConnectStart = 
	{
		//	Start Coordinates			
		{ 206.88500   ,	49.31   }   ,	//	eta	-> zeta
		{ 200.98125   ,	54.93   }   ,	//	zeta -> epsilon
		{ 193.50708   ,	55.96   }   ,	//	epsilon	-> delta
		{ 183.85667   ,	57.03   }   ,	//	delta -> alpha
		{ 183.85667   ,	57.03   }   ,	//	delta -> gamma
		{ 178.45750   ,	53.69   }   ,	//	gamma -> beta
		{ 165.46042   ,	56.38   }		//	beta -> alpha

	};

	public static readonly double[,] UMaConnectEnd =
	{
		{   200,98125   ,   54,93   }   ,	//	eta	-> zeta
		{   193,50708   ,   55,96   }   ,	//	zeta -> epsilon
		{   183,85667   ,   57,03   }   ,	//	epsilon -> delta
		{   165,93208   ,   61,75   }   ,	//	delta -> alpha
		{   178,45750   ,   53,69   }   ,	//	delta -> gamma
		{   165,46042   ,   56,38   }   ,	//	gamma -> beta
		{   165,93208   ,   61,75   }		//	beta -> alpha
	};
}


