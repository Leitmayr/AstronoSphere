using Astronometria.Adapters;
using Astronometria.Data.Parsers.BrightStarCatalog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Astronometria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            RB_4mag.IsChecked = true;

            DateAndTimeOfNow();
            GeograficLocation();

        }



        #region CheckBoxEvaluation

        bool isCBStarsChecked = false;
        bool isCBConstellationsChecked = false;
        bool isCBPlanetsChecked = false;
        bool isCBCirclesChecked = false;
        bool isCBStarColorsChecked = false;

        bool isDaylightSavingsTimeChecked = false;


        void DaylightSavingsChecked(object sender, RoutedEventArgs e)
        {
            isDaylightSavingsTimeChecked = true;
        }
        void DaylightSavingsUnchecked(object sender, RoutedEventArgs e)
        {
            isDaylightSavingsTimeChecked = false;
        }

        void CBDayUpdateDaylightSavingsTime(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            double loc_JD = locDate.JD;

            czeit locDisplayTime;

            if (isDaylightSavingsTimeChecked)
            {
                loc_JD = loc_JD + (double)1 / (double)24;
                locDisplayTime = new czeit(loc_JD);

                // get new date and time back
                int correctedYear = locDisplayTime.Date.year;
                int correctedMonth = locDisplayTime.Date.month;
                int correctedDay = locDisplayTime.Date.day;

                int correctedHour = locDisplayTime.UT.hour;
                int correctedMinute = locDisplayTime.UT.minute;
                int correctedSecond = locDisplayTime.UT.second;

                if (correctedSecond > 50)
                {
                    correctedMinute += 1;
                    if (correctedMinute >= 60)
                    {
                        correctedMinute -= 60;
                        correctedHour += 1;
                    }
                }

                // write new date and time to Windows
                tbDay.Text = "" + correctedDay;
                tbMonth.Text = "" + correctedMonth;
                tbYear.Text = "" + correctedYear;

                tbHour.Text = "" + correctedHour;
                tbMinute.Text = "" + correctedMinute;

            }
            else if (!isDaylightSavingsTimeChecked)
            {
                loc_JD = loc_JD - (double)1 / (double)24;
                locDisplayTime = new czeit(loc_JD);


                // get new date and time back
                int correctedYear = locDisplayTime.Date.year;
                int correctedMonth = locDisplayTime.Date.month;
                int correctedDay = locDisplayTime.Date.day;

                int correctedHour = locDisplayTime.UT.hour;
                int correctedMinute = locDisplayTime.UT.minute;
                int correctedSecond = locDisplayTime.UT.second;

                if (correctedSecond > 50)
                {
                    correctedMinute += 1;
                    if (correctedMinute >= 60)
                    {
                        correctedMinute -= 60;
                        correctedHour += 1;
                    }
                }

                // write new date and time to Windows
                tbDay.Text = "" + correctedDay;
                tbMonth.Text = "" + correctedMonth;
                tbYear.Text = "" + correctedYear;

                tbHour.Text = "" + correctedHour;
                tbMinute.Text = "" + correctedMinute;


            }
            else
            {
                // do nothing
            }

   
        }


        // Stars Checkbock: Checked, Unchecked, Click
        void CBStarsChecked(object sender, RoutedEventArgs e)
        {
            isCBStarsChecked = true;
        }
        void CBStarsUnchecked(object sender, RoutedEventArgs e)
        {
            isCBStarsChecked = false;
        }
        void CBUpdateStarsInMap(object sender, RoutedEventArgs e)
        {
            //Call Start Updater Routine
        }

        // Constellations Checkbock: Checked, Unchecked, Click
        void CBConstellationssChecked(object sender, RoutedEventArgs e)
        {
            isCBConstellationsChecked = true;
        }
        void CBConstellationssUnchecked(object sender, RoutedEventArgs e)
        {
            isCBConstellationsChecked = false;
        }
        void CBUpdateConstellationsInMap(object sender, RoutedEventArgs e)
        {
            //Call Constellation Updater Routine
        }

        // Planets Checkbock: Checked, Unchecked, Click
        void CBPlanetsChecked(object sender, RoutedEventArgs e)
        {
            isCBPlanetsChecked = true;
        }
        void CBPlanetsUnchecked(object sender, RoutedEventArgs e)
        {
            isCBPlanetsChecked = false;
        }
        void CBUpdatePlanetsInMap(object sender, RoutedEventArgs e)
        {
            //Call Planet Updater Routine
        }

        // Circles Checkbock: Checked, Unchecked, Click
        void CBCirclesChecked(object sender, RoutedEventArgs e)
        {
            isCBCirclesChecked = true;
        }
        void CBCirclesUnchecked(object sender, RoutedEventArgs e)
        {
            isCBCirclesChecked = false;
        }
        void CBUpdateCirclesInMap(object sender, RoutedEventArgs e)
        {
            //Call Circles Updater Routine
        }

        // Star Colors Checkbock: Checked, Unchecked, Click
        void CBStarColorsChecked(object sender, RoutedEventArgs e)
        {
            isCBStarColorsChecked = true;
        }
        void CBStarColorsUnchecked(object sender, RoutedEventArgs e)
        {
            isCBStarColorsChecked = false;
        }
        void CBUpdateStarColorsInMap(object sender, RoutedEventArgs e)
        {
            //Call Star Colors Updater Routine
        }

        #endregion



        #region RadioButtonEvaluation

        //bool is1MagSelected = false;
        bool is2MagSelected = false;
        bool is3MagSelected = false;
        bool is4MagSelected = false;
        bool is55MagSelected = false;
        //bool is6MagSelected = false;
        bool is65MagSelected = false;

        void RBUpdateStarsInMap(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);

            updateDisplay(locDate);
        }

        // 2 mag selected
        void mag2Checked(object sender, RoutedEventArgs e)
        {
            is2MagSelected = true;
        }
        void mag2Unchecked(object sender, RoutedEventArgs e)
        {
            is2MagSelected = false;
        }


        // 3 mag selected
        void mag3Checked(object sender, RoutedEventArgs e)
        {
            is3MagSelected = true;
        }
        void mag3Unchecked(object sender, RoutedEventArgs e)
        {
            is3MagSelected = false;
        }


        // 4 mag selected
        void mag4Checked(object sender, RoutedEventArgs e)
        {
            is4MagSelected = true;
        }
        void mag4Unchecked(object sender, RoutedEventArgs e)
        {
            is4MagSelected = false;
        }


        // 5.5 mag selected
        void mag55Checked(object sender, RoutedEventArgs e)
        {
            is55MagSelected = true;
        }
        void mag55Unchecked(object sender, RoutedEventArgs e)
        {
            is55MagSelected = false;
        }

        // 6.5 mag selected
        void mag65Checked(object sender, RoutedEventArgs e)
        {
            is65MagSelected = true;
        }
        void mag65Unchecked(object sender, RoutedEventArgs e)
        {
            is65MagSelected = false;
        }


        #endregion RadioButtonEvaluation


        #region DateAndTimeClicks

        void PlusHour(object sender, RoutedEventArgs e)
        {

            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedTime = correctShiftHour(locDate, 1);
            updateDisplay(correctedTime);
        }


        void MinusHour(object sender, RoutedEventArgs e)
        {

            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedTime = correctShiftHour(locDate, -1);
            updateDisplay(correctedTime);
        }

        void PlusMinute(object sender, RoutedEventArgs e)
        {

            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedTime = correctShiftMinute(locDate, 1);
            updateDisplay(correctedTime);

        }

        void MinusMinute(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;

            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedTime = correctShiftMinute(locDate, -1);
            updateDisplay(correctedTime);

        }



        void PlusDay(object sender, RoutedEventArgs e)
        {

            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;



            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, 1);
            updateDisplay(correctedDate);
        }

        void MinusDay(object sender, RoutedEventArgs e)
        {


            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, -1);


            updateDisplay(correctedDate);

        }

        void PlusMonth(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, 30);


            updateDisplay(correctedDate);

        }

        void MinusMonth(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, -30);
            updateDisplay(correctedDate);

        }

        void PlusYear(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, 365);
            updateDisplay(correctedDate);
        }

        void MinusYear(object sender, RoutedEventArgs e)
        {
            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            // process the new date and time
            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            czeit correctedDate = correctShiftDate(locDate, -365);
            updateDisplay(correctedDate);
        }



        #endregion DateAndTimeClicks


        #region ButtonsRightBottom
        public void DateAndTimeOfNow()
        {
            DateTime thisDate = DateTime.Now;
            // format see here: https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
            string DateString = thisDate.ToString("dd-MM-yyyy HH:mm");


            string thisDay = DateString.Substring(0, 2);
            string thisMonth = DateString.Substring(3, 2);
            string thisYear = DateString.Substring(6, 4);
            string thisHour = DateString.Substring(11, 2);
            string thisMinute = DateString.Substring(14, 2);


            tbDay.Text = thisDay;
            tbMonth.Text = thisMonth;
            tbYear.Text = thisYear;

            tbHour.Text = thisHour;
            tbMinute.Text = thisMinute;


        }

        public void GeograficLocation()
        {
            double geograficLongitude = 10.0;
            double geograficLatitude = 50.0;
            double timeZone = 1.0;
            tbGeoLong.Text = "" + geograficLongitude;
            tbGeoLat.Text = "" + geograficLatitude;
            tbTimeZone.Text = "" + timeZone;


        }


        void Now(object sender, RoutedEventArgs e)
        {

            DateAndTimeOfNow();
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;


            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);
            updateDisplay(locDate);
        }

        public void Refresh(object sender, RoutedEventArgs e)
        {

            InitializeComponent();      // To Do: cleared Canvas needs to be re-loadable


            // ----------------------------------------------------------------------------------
            // read date and time
            int locDay = int.Parse(tbDay.Text);
            int locMonth = int.Parse(tbMonth.Text);
            int locYear = int.Parse(tbYear.Text);
            //
            int locHour = int.Parse(tbHour.Text);
            int locMinute = int.Parse(tbMinute.Text);
            int locSecond = 0;



            czeit locDate = new czeit(locDay, locMonth, locYear, locHour, locMinute, locSecond);

            updateDisplay(locDate);

        }

        #endregion


        czeit NormalizeTimeToUT(czeit argDate, double arg_timeShift)
        {

            double loc_JD = argDate.JD;
            // subtract the time shift from initial Date (argDate)
            loc_JD = loc_JD - arg_timeShift / 24.0;

            czeit retNormalizedTime = new czeit(loc_JD);
            return (retNormalizedTime);
        }

        observationPoint myHomeTown(czeit arg_localTime)
        {
            string loc_myHomeTown = "NN";
            double loc_geoLong = double.Parse(tbGeoLong.Text);
            double loc_geoLat = double.Parse(tbGeoLat.Text);

            double loc_timeShift = calcTimeShift();


            observationPoint ret_ObsPoint = new observationPoint(loc_myHomeTown, loc_geoLong, loc_geoLat, arg_localTime, loc_timeShift);

            return (ret_ObsPoint);
        }


        double calcTimeShift()
        {

            double ret_TimeShift = 0.0;

            double tbTimeZone = double.Parse(tbHour.Text);
            int loc_DaylightSavingsTime = 0;
            if (isDaylightSavingsTimeChecked)
            {
                loc_DaylightSavingsTime++;
            }
            ret_TimeShift = tbTimeZone + loc_DaylightSavingsTime;

            return (ret_TimeShift);
        }



        void updateDisplay(czeit arg_Date)
        {
            InitializeComponent();
            var parser = new BrightStarCatalogParser();

            var records = StarCatalogLoader.LoadToList("StarCat_6-5mag.txt");

            double maxMagnitude = 0.0;

            if (is2MagSelected)
            {
                maxMagnitude = 2.0;
            }
            else if (is3MagSelected)
            {

                maxMagnitude = 3.0;
                
            }
            else if (is4MagSelected)
            {
                maxMagnitude = 4.0;
            }
            else if (is55MagSelected)
            {
                maxMagnitude = 5.5;
            }
            else if (is65MagSelected)
            {
                maxMagnitude = 6.5;
            }
            else
            {
                maxMagnitude = 6.5;
            }




            double locTimeShift = calcTimeShift();
            czeit locDisplayTime = arg_Date;



            // get new date and time back
            int correctedYear = locDisplayTime.Date.year;
            int correctedMonth = locDisplayTime.Date.month;
            int correctedDay = locDisplayTime.Date.day;

            int correctedHour = locDisplayTime.UT.hour;
            int correctedMinute = locDisplayTime.UT.minute;
            int correctedSecond = locDisplayTime.UT.second;

            if (correctedSecond > 50)
            {
                correctedMinute += 1;
                if (correctedMinute >= 60)
                {
                    correctedMinute -= 60;
                    correctedHour += 1;

                }
            }


            //tbJulianDate.Text = "" + displayJD;

            // write new date and time to Windows
            tbDay.Text = "" + correctedDay;
            tbMonth.Text = "" + correctedMonth;
            tbYear.Text = "" + correctedYear;

            tbHour.Text = "" + correctedHour;
            tbMinute.Text = "" + correctedMinute;




            // normalized locUTTime (=transformed from MEZ, MESZ, EST, ... to GMT=UT):
            // from now on used for all calculations of celestial objects
            czeit locUTTime = NormalizeTimeToUT(locDisplayTime, locTimeShift);


            observationPoint myObsPoint = myHomeTown(locDisplayTime);

            StarMapControl.Viewport.Scale = 1.0;
            StarMapControl.Viewport.PanXPx = 0;
            StarMapControl.Viewport.PanYPx = 0;
            StarMapControl.InvalidateVisual();


            StarMapControl.ConstellationLines = ConstellationLineLoader.Load("ConstellationLines_with_RADEC.csv");
            StarMapControl.InvalidateVisual();

            //var stars = StarRecordAdapter.ToStars(records, myObsPoint, locDisplayTime);
            var stars = StarRecordAdapter.ToStars(
                records.Where( r => r.VisualMagnitude <= maxMagnitude ), 
                myObsPoint, 
                locDisplayTime);

            StarMapControl.InitMap(myObsPoint, locDisplayTime, stars);
            StarMapControl.SetStars(stars);



            // normalized locUTTime (=transformed from MEZ, MESZ, EST, ... to GMT=UT):
            // from now on used for all calculations of celestial objects
            //czeit locUTTime = NormalizeTimeToUT(locDisplayTime, locTimeShift);



        }
    



        czeit correctShiftDate(czeit argDate, int argDays)
        {

            int locYear = argDate.Date.year;
            int locMonth = argDate.Date.month;
            int locDay = argDate.Date.day;

            int locHour = argDate.UT.hour;
            int locMinute = argDate.UT.minute;
            int locSecond = argDate.UT.second;

            double JD = argDate.JD;
            JD = JD + argDays;

            // return the result
            czeit returnDate = new czeit(JD);
            return returnDate;
        }

        czeit correctShiftHour(czeit argDate, int argHours)
        {

            int locYear = argDate.Date.year;
            int locMonth = argDate.Date.month;
            int locDay = argDate.Date.day;

            int locHour = argDate.UT.hour;
            int locMinute = argDate.UT.minute;
            int locSecond = argDate.UT.second;

            int locArg = argHours;
            double locDelta = (double) locArg / 24.0;

            double JD = argDate.JD;
            double JD1 = JD + locDelta; ;

            // return the result
            czeit returnDate = new czeit(JD1);
            return returnDate;
        }

        czeit correctShiftMinute(czeit argDate, int argMinutes)
        {

            int locYear = argDate.Date.year;
            int locMonth = argDate.Date.month;
            int locDay = argDate.Date.day;

            int locHour = argDate.UT.hour;
            int locMinute = argDate.UT.minute;
            int locSecond = argDate.UT.second;

            int locArg = argMinutes;
            double locDelta = (double)locArg / (24.0 * 60.0);

            double JD = argDate.JD;
            JD = JD + (double) argMinutes/(24*60) ;

            // return the result
            czeit returnDate = new czeit(JD);
            return returnDate;
        }



    }


}
