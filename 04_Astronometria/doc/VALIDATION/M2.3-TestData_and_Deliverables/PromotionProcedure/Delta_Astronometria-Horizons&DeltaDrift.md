Delta1 is the deviation between VSOP from Astrometria and Horizons for the first data set of the time span
Delta2 is the deviation between VSOP from Astrometria and Horizons for the last data set of the time span

Note: Delta1/Delta2 evaluation is how much the model deviations are changing during the time span. If this is ~1, change of model quality is 0.

AS-000013:

HorizonsCall: https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&CENTER=%4010&START_TIME=JD2460940.626388889&STOP_TIME=JD2460942.626388889&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): 1.003657640850634,     -0.01714847191428989,   -3.632470532417432E-06
Astronometria (Position): 1.0036576434271152,    -0.017148103730046441,  -3.7412999870468347E-06
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[-2.5764812505713053E-9, -3.68184243447478E-7, 1.0882945462940269E-7]

Last
HorizonsState (Position): 1.003096605745708,  0.01714269741671966, 		-4.528293907833674E-06
Astronometria (Position): 1.003096595763262,  0.017143064160185503, 	-4.6299418791182529E-06
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 9.982445980938337E-9, -3.667434658426172E-7, 1.01647971284579E-7]

Deltas:
[-2.5764812505713053E-9, -3.68184243447478E-7, 1.0882945462940269E-7]
[ 9.982445980938337E-9, -3.667434658426172E-7, 1.01647971284579E-7  ]

Delta1/Delta2: is the change in the same magnitude or significan
[ -0.25810119638925605 ,  1.003928570619658, 1.0706505329527731]

> Note: Delta is constistent in all three dimensions

------------------

AS-000025:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&CENTER=%4010&START_TIME=JD2460740.078472222&STOP_TIME=JD2460742.078472222&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): 0.03373013432637644,  0.3051890308120436,  0.02184677227227520
Astronometria (Position): 0.033729961208968887, 0.30518904639117761, 0.021846831835361045
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html
[ 1.7311740755504568E-7, -1.5579133982868143E-8,  -5.956308584548209E-8]

Last
HorizonsState (Position): -0.03371410557197465,  0.3072460913610752,   0.02820094646118400
Astronometria (Position): -0.03371427876838054,  0.30724606947899447,  0.028201012770883844
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 1.7319640589069651E-7, 2.188208075848408E-8 , -6.630969984558477E-8]

Deltas:
[ 1.7311740755504568E-7, -1.5579133982868143E-8,  -5.956308584548209E-8]
[ 1.7319640589069651E-7, 2.188208075848408E-8 , -6.630969984558477E-8  ]


Delta1/Delta2: is the change in the same magnitude or significant
[  0.9995438800519874,  -0.7119585269251797, 0.8982560015229522]


> Note: Delta is extremely constistent in all three dimensions


------------------

AS-000047:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&CENTER=500%40399&START_TIME=JD2469641.24375&STOP_TIME=JD2469643.24375&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): 3.540710910888044, 	-8.263314576319296,	  	0.0002198646817418718
Astronometria (Position): 3.5407253525922626,   -8.2633075806206939,	0.00021648917975472309
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[-0.000014441704218715046, -0.000006995698601741651, 0.000003375501987148722]

Last
HorizonsState (Position): 3.520180028391305, -8.274268273594128, -0.0002390191649837472
Astronometria (Position): 3.5201932389961161,-8.2742618951225246,-0.00024194521101934325
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.00001321060481096481, -0.0000063784716033410405, 0.000002926046035596063]

Deltas:
[-0.000014441704218715046, -0.000006995698601741651, 0.000003375501987148722]
[ -0.00001321060481096481, -0.0000063784716033410405, 0.000002926046035596063]



Delta1/Delta2: is the change in the same magnitude or significant?
[  1.0931902380978367, 1.0967672252514704 , 1.1536052222299027 ]

> Note:  Delta is extremely constistent in all three dimensions

------------------

AS-000055:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&CENTER=500%40399&START_TIME=JD2482629.293055556&STOP_TIME=JD2482631.293055556&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): -19.28525329614353,   21.76797703707829, 	-9.029374835211018E-05
Astronometria (Position): -19.285509145319633,  21.767745749170714, -8.1234355694032498E-05
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 0.0002558491761028847, 0.00023128790757453999, -0.000009059392658077679]

Last
HorizonsState (Position): -19.26710602526634,  	21.79013390454688,  0.00009769160334442119 
Astronometria (Position): -19.267362084692873, 	21.789901799080468,	0.00010654921976292923
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 0.00025605942653328384, 0.00023210546641294627, -0.000008857616418508048]

> Note: fairly large x and y deviations

Deltas:
[ 0.0002558491761028847,  0.00023128790757453999, -0.000009059392658077679]
[ 0.00025605942653328384, 0.00023210546641294627, -0.000008857616418508048]

Delta1/Delta2: is the change in the same magnitude or significant?
[ 0.9991788998622482, 0.9964776407421971, +1.022779970370812]

> Note: but Delta is extremely constistent in all three dimensions

------------------

AS-000145:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&CENTER=%4010&START_TIME=JD2460835.7125&STOP_TIME=JD2460837.7125&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): -1.651507806824707,	0.01280616329973561,	0.04076740121919679
Astronometria (Position): -1.6515078073471321,	0.01280566419916278,	0.04076757823112407
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 5.224249921553792E-10, 4.991005728306047E-7, -1.7701192728258874E-7]

Last
HorizonsState (Position): -1.650463442363736,	-0.01278855013476539, 	0.04020541380365419
Astronometria (Position): -1.6504634346527265,	-0.012789050733090524,	0.040205586560081955
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -7.71100960861304E-9, 5.005983251329948E-7,  -1.727564277673177E-7]

Deltas:
[ 5.224249921553792E-10, 4.991005728306047E-7, -1.7701192728258874E-7]
[ -7.71100960861304E-9 , 5.005983251329948E-7,  -1.727564277673177E-7]

Delta1/Delta2: is the change in the same magnitude or significant?
[ -0.06775053056241055,  0.9970080756822504,  1.0246329446045428 ]

> Note: quite big increase in x-dimension, however absolute number E-10...E-09 extremely small. Numerical effects are more significant.

------------------

AS-000158:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&CENTER=%4010&START_TIME=JD2341987.5&STOP_TIME=JD2378467.5&STEP_SIZE=30D&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
First
HorizonsState (Position): 1.407799641984646,	-4.987440160191103,		-0.01212242684361266
Astronometria (Position): 1.4077960950428399,	-4.9874424630042151,	-0.012122929907495706
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 0.0000035469418060429803, 0.000002302813111754176, 5.030638830470779E-7]


Last
HorizonsState (Position): 0.1920119051472330,  	5.118573741131689,	 	-0.02471401998681622
Astronometria (Position): 0.19201566206564932,	5.1185730525184576,		-0.024713036507234699
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.00000375691841633663, 6.886132313255189E-7, -9.834795815198694E-7]

Deltas:
[ 0.0000035469418060429803, 0.000002302813111754176,  5.030638830470779E-7]
[ -0.00000375691841633663 , 6.886132313255189E-7   , -9.834795815198694E-7]

Delta1/Delta2: is the change in the same magnitude or significant?
[ -0.9441093505303216, 3.344131374474851, -0.511514313565761 ]
------------------

AS-000208:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&CENTER=%4010&START_TIME=JD2561137.5&STOP_TIME=JD2597617.5&STEP_SIZE=30D&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): -16.95437137619433,	6.873208504133467,  	0.2441891102244828
Astronometria (Position): -16.954243743189892,	6.8732883852099071,		0.24418835669399913
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.0001276330044390761, -0.00007988107643974729, 7.535304836769896E-7]

Last
HorizonsState (Position): -11.53044466385520, 	-14.71358735679063,  	0.09535315418569087
Astronometria (Position): -11.53017478191552,	-14.713586506575782,	0.095348757642852319
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.0002698819396798058, -8.502148478584104E-7, 0.000004396542838555617 ]

> Note: comparably large x-deviation

Deltas:
[ -0.0001276330044390761, -0.00007988107643974729, 7.535304836769896E-7]
[ -0.0002698819396798058, -8.502148478584104E-7  , 0.000004396542838555617 ]

Delta1/Delta2: is the change in the same magnitude or significant?
[ -0.5032286144878393, 93.95398897226762,  0.17139159365601558]

> Note: but very consistent in x (50% deviation)
> Note: very high change in Delta in y
> Note: ~1000 years between first and last data set



------------------

AS-000360:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&CENTER=%4010&START_TIME=JD2816993.5&STOP_TIME=JD3182023.5&STEP_SIZE=211D&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): 0.3429318686104599, 	-0.9566522772701259,  	0.002047183540393147
Astronometria (Position): 0.34293316612133939,	-0.9566518600271966,	0.0020468680806211038
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.0000012975108795165724, -4.172429293181068E-7,  3.154597720432181E-7]

Last
HorizonsState (Positio): 0.4101738598806000,  	0.8993473963948926, 	-0.004293141232117196
Astronometria (Position): 0.41017379859679026,	0.89934722423958435,	-0.0042928180648827696
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 6.128380974912417E-8, 1.721553082312255E-7, -3.231672344259501E-7]

Deltas:
[ -0.0000012975108795165724, -4.172429293181068E-7,   3.154597720432181E-7] 
[      6.128380974912417E-8,   1.721553082312255E-7, -3.231672344259501E-7]

Delta1/Delta2: is the change in the same magnitude or significant?
[ -21.172164146259128, -2.423642544659145, -0.976150235662279 ]

> Note: change in Delta x  quite significant. However, 4000 years between first and last data set


------------------

AS-000362:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&CENTER=%4010&START_TIME=JD260089.5&STOP_TIME=JD1720334.5&STEP_SIZE=809D&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): -0.06209800970027116,		-0.7259165420001349,  	0.005716395560038235
Astronometria (Position): -0.062053157851247515,	-0.72592233058929645,	0.0057133378199465314
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.00004485184902364853, 0.000005788589161559443, 0.0000030577400917039532]

Last
HorizonsState (Position): -0.4340185951540235,  	0.5708433161190329,  	0.03014204810626103
Astronometria (Position): -0.43402249764932621,		0.5708402089546657,		0.030142303513482044
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 0.000003902495302687825, 0.0000031071643672442306, -2.554072210149416E-7]

Deltas:
[ -0.00004485184902364853, 0.000005788589161559443, 0.0000030577400917039532]
[ 0.000003902495302687825, 0.0000031071643672442306, -2.554072210149416E-7]

Delta1/Delta2: is the change in the same magnitude or significant?
[-11.493120566412221 / 1.8629813158849367 /  -11.972018956837058]

> Note: change in Delta x and Delta z quite significant. However, 4000 years between first and last data set

------------------

AS-000373:

HorizonsCall:  
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&CENTER=%4010&START_TIME=JD3182197.5&STOP_TIME=JD4642442.5&STEP_SIZE=809D&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D

First
HorizonsState (Position): 0.3387597133091629,  		0.6373457634001932, 	-0.006676875554348660
Astronometria (Position): 0.33875859211322451,		0.63734631889755367,	-0.0066768292433635155
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ 0.000001121195938369457, -5.554973604438018E-7, -4.631098514482762E-8]

Last
HorizonsState (Position): 0.1971369709558589, 		-0.6983947744162017,	-0.02961617524380873
Astronometria (Position): 0.19714043748096949,		-0.69839454435998938,	-0.029617168039592769
Delta: calculated online with https://uni-tuebingen.de/fileadmin/Uni_Tuebingen/Fakultaeten/MathePhysik/Institute/IAAT/AIT/Tools/taschenrechner.html:
[ -0.000003466525110601415, -2.3005621230254292E-7, 9.927957840376311E-7 ]


Deltas:
[ 0.000001121195938369457, -5.554973604438018E-7, -4.631098514482762E-8]
[ -0.000003466525110601415, -2.3005621230254292E-7, 9.927957840376311E-7 ]

Change in Drift:
[ 0.000004587721048970872, -3.254411481412589E-7, -0.0000010391067691824588]


Delta1/Delta2: is the change in the same magnitude or significant?
[-0.3234351122801872 / 2.4146157797002976 / -0.04664704049858479]


> **Result:**
> - model deviations appear reasonable
> - change in model deviation small in [1500, 2500]
> - change in model deviation larger in historical or far future time spans -4000, +8000 -> reasonable
> - VSOP model data appear to be accurate
>
> **Data quality is good. Ready to promote to Baseline**

