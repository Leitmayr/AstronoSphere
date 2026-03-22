using Astronometria.Core.Bodies;
using Astronometria.Ephemerides.Vsop87D.Models;
using Astronometria.Ephemerides.Vsop87D.Terms;
using System;
using System.Diagnostics;

namespace Astronometria.Ephemerides.Vsop87D
{
    /// <summary>
    /// Berechnet die heliozentrisch-ekliptikalen VSOP87D-Koordinaten
    /// (L, B in Grad, R in AU) für einen Planeten zu einem gegebenen Zeitpunkt.
    ///
    /// Die Berechnung folgt exakt der VSOP87D-Theorie:
    ///   - L(t), B(t), R(t) werden jeweils als Polynome in t (Jahrtausende seit J2000.0)
    ///     berechnet
    ///   - die Koeffizienten dieser Polynome ergeben sich aus trigonometrischen Reihen
    ///     (Σ A*cos(B + C*t))
    ///   - die endgültige Auswertung erfolgt per Horner-Schema
    ///
    /// Diese Implementierung ist eine 1:1-Portierung der ursprünglichen C++-Logik,
    /// jedoch verallgemeinert auf variable Ordnung (z.B. R nur bis Ordnung 4 bei Neptune).
    /// </summary>
    public sealed class Vsop87DCalculator
    {
        /// <summary>
        /// Steuert die Genauigkeit der VSOP-Berechnung.
        /// High  = volle Termanzahl (Original VSOP87D)
        /// Low   = gekürzte Termanzahl (Performance)
        ///
        /// Entspricht dem C++-Switch C__vsopHighAccuracyAcvivationSwitch.
        /// </summary>
        public AccuracyMode Accuracy { get; set; } = AccuracyMode.High;

        /// <summary>
        /// Einstiegspunkt für die VSOP-Berechnung.
        /// Delegiert auf planetenspezifische Termtabellen.
        /// </summary>
        public Sspher CalcVsop(PlanetId planet, double jtSince2000)
        {
            return planet switch
            {
                PlanetId.Mercury => CalcPlanet(
                    MercuryTerms.L, MercuryTerms.B, MercuryTerms.R,
                    MercuryTerms.SizeLHigh, MercuryTerms.SizeBHigh, MercuryTerms.SizeRHigh,
                    MercuryTerms.SizeLLow, MercuryTerms.SizeBLow, MercuryTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Venus => CalcPlanet(
                    VenusTerms.L, VenusTerms.B, VenusTerms.R,
                    VenusTerms.SizeLHigh, VenusTerms.SizeBHigh, VenusTerms.SizeRHigh,
                    VenusTerms.SizeLLow, VenusTerms.SizeBLow, VenusTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Earth => CalcPlanet(
                    EarthTerms.L, EarthTerms.B, EarthTerms.R,
                    EarthTerms.SizeLHigh, EarthTerms.SizeBHigh, EarthTerms.SizeRHigh,
                    EarthTerms.SizeLLow, EarthTerms.SizeBLow, EarthTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Mars => CalcPlanet(
                    MarsTerms.L, MarsTerms.B, MarsTerms.R,
                    MarsTerms.SizeLHigh, MarsTerms.SizeBHigh, MarsTerms.SizeRHigh,
                    MarsTerms.SizeLLow, MarsTerms.SizeBLow, MarsTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Jupiter => CalcPlanet(
                    JupiterTerms.L, JupiterTerms.B, JupiterTerms.R,
                    JupiterTerms.SizeLHigh, JupiterTerms.SizeBHigh, JupiterTerms.SizeRHigh,
                    JupiterTerms.SizeLLow, JupiterTerms.SizeBLow, JupiterTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Saturn => CalcPlanet(
                    SaturnTerms.L, SaturnTerms.B, SaturnTerms.R,
                    SaturnTerms.SizeLHigh, SaturnTerms.SizeBHigh, SaturnTerms.SizeRHigh,
                    SaturnTerms.SizeLLow, SaturnTerms.SizeBLow, SaturnTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Uranus => CalcPlanet(
                    UranusTerms.L, UranusTerms.B, UranusTerms.R,
                    UranusTerms.SizeLHigh, UranusTerms.SizeBHigh, UranusTerms.SizeRHigh,
                    UranusTerms.SizeLLow, UranusTerms.SizeBLow, UranusTerms.SizeRLow,
                    jtSince2000),

                PlanetId.Neptune => CalcPlanet(
                    NeptuneTerms.L, NeptuneTerms.B, NeptuneTerms.R,
                    NeptuneTerms.SizeLHigh, NeptuneTerms.SizeBHigh, NeptuneTerms.SizeRHigh,
                    NeptuneTerms.SizeLLow, NeptuneTerms.SizeBLow, NeptuneTerms.SizeRLow,
                    jtSince2000),

                _ => throw new NotSupportedException($"Planet {planet} not implemented.")
            };
        }

        /// <summary>
        /// Berechnet L, B und R für einen Planeten.
        ///
        /// L, B, R bestehen jeweils aus mehreren Ordnungen (z.B. L0..L5),
        /// deren Anzahl planetenspezifisch ist.
        ///
        /// Diese Methode ist bewusst generisch gehalten, um:
        ///   - unterschiedliche maximale Ordnungen (z.B. R nur bis Ordnung 4)
        ///   - High-/Low-Accuracy ohne Code-Duplikation
        /// zu unterstützen.
        /// </summary>
        private Sspher CalcPlanet(
            double[][,] Lseries, double[][,] Bseries, double[][,] Rseries,
            int[] sizeLHigh, int[] sizeBHigh, int[] sizeRHigh,
            int[] sizeLLow, int[] sizeBLow, int[] sizeRLow,
            double t)
        {


            // Berechnung der Polynomkoeffizienten:
            // coeffs[i] = Σ A*cos(B + C*t)  (VSOP-Formel)
            bool full = (Accuracy == AccuracyMode.High);
            var Lcoeffs = SumCoefficients(Lseries, full ? sizeLHigh : sizeLLow, t, useFullSeries: full);
            var Bcoeffs = SumCoefficients(Bseries, full ? sizeBHigh : sizeBLow, t, useFullSeries: full);
            var Rcoeffs = SumCoefficients(Rseries, full ? sizeRHigh : sizeRLow, t, useFullSeries: full);

            // Endauswertung per Horner-Schema
            // L, B: Ergebnis in RAD → DEG → [0..360]
            // R: Ergebnis in AU
            double Ldeg = ProcessAngleL(Lcoeffs, t);
            double Bdeg = ProcessAngleB(Bcoeffs, t);
            double R_au = ProcessRadius(Rcoeffs, t);

            return new Sspher(Ldeg, Bdeg, R_au);
        }

        /// <summary>
        /// Berechnet für jede Ordnung (z.B. L0, L1, ...)
        /// den Summenwert der trigonometrischen VSOP-Terme:
        ///
        ///   Σ A_j * cos(B_j + C_j * t)
        ///
        /// Das Ergebnis ist ein Koeffizienten-Array, das anschließend
        /// im Horner-Schema ausgewertet wird.
        /// </summary>
        private static double[] SumCoefficients(double[][,] series, int[] sizes, double t, bool useFullSeries)
        {
            if (series == null) throw new ArgumentNullException(nameof(series));
            if (sizes == null) throw new ArgumentNullException(nameof(sizes));
            if (series.Length != sizes.Length)
                throw new ArgumentException($"Series length ({series.Length}) must match sizes length ({sizes.Length}).");

            var coeffs = new double[series.Length];

            for (int i = 0; i < series.Length; i++)
            {
                var terms = series[i];
                if (terms == null)
                    throw new ArgumentException($"VSOP terms table is null: seriesIndex={i}.");

                int rows = terms.GetLength(0);

                // HighAccuracy: immer die komplette Tabelle (robust gegen falsche SizeHigh Arrays)
                // LowAccuracy: die truncate Größe aus sizes[i], aber nie größer als rows.
                int count = useFullSeries ? rows : Math.Min(Math.Max(sizes[i], 0), rows);


                if (count == 0)
                {
                    coeffs[i] = 0.0;
                    continue;
                }

                coeffs[i] = SumSeries(terms, count, t);
            }

            return coeffs;
        }

        /// <summary>
        /// Innere Schleife der VSOP-Theorie:
        /// Summiert A*cos(B + C*t) für eine Ordnung.
        /// </summary>
        private static double SumSeries(double[,] terms, int count, double t)
        {
            double sum = 0.0;
            for (int j = 0; j < count; j++)
                sum += terms[j, 0] * Math.Cos(terms[j, 1] + terms[j, 2] * t);
            return sum;
        }

        /// <summary>
        /// Horner-Schema zur stabilen und effizienten Auswertung
        /// eines Polynoms:
        ///
        ///   a0 + a1*t + a2*t² + ...
        ///
        /// Wird bewusst generisch verwendet, da die maximale Ordnung
        /// planetenspezifisch ist.
        /// </summary>
        private static double Horner(double[] coeffs, double t)
        {

            int sizeC = coeffs.Length;

            

            double v = 0.0;
            if (sizeC == 6)
            { 
                v = coeffs[0] + coeffs[1]*Math.Pow(t, 1) 
                    + coeffs[2]* Math.Pow(t, 2) + coeffs[3] *Math.Pow(t, 3) + coeffs[4] * Math.Pow(t, 4) + coeffs[5] *Math.Pow(t, 5);
            }
            else if (sizeC == 5)
            {
                v = coeffs[0] + coeffs[1] * Math.Pow(t, 1)
                    + coeffs[2] * Math.Pow(t, 2) + coeffs[3] * Math.Pow(t, 3) + coeffs[4] * Math.Pow(t, 4);
            }
             else if (sizeC == 4)
            {
                v = coeffs[0] + coeffs[1] * Math.Pow(t, 1)
                    + coeffs[2] * Math.Pow(t, 2) + coeffs[3] * Math.Pow(t, 3);
            }
             else if (sizeC == 3)
            {
                v = coeffs[0] + coeffs[1] * Math.Pow(t, 1)
                    + coeffs[2] * Math.Pow(t, 2);
            }
             else if (sizeC == 2)
            {
                v = coeffs[0] + coeffs[1] * Math.Pow(t, 1);
            }
             else if (sizeC == 1)
            {
                v = coeffs[0];
            }
            /*

            if (coeffs.Length == 0) return 0.0;

            double v = coeffs[coeffs.Length - 1];
            for (int i = coeffs.Length - 2; i >= 0; i--)
                v = coeffs[i] + t * v;
        */
            return v;
        }

        /// <summary>
        /// Verarbeitet Winkelkoeffizienten:
        ///   - Ergebnis aus Horner ist in Radiant
        ///   - Umrechnung in Grad
        ///   - Normalisierung auf [0..360]
        ///
        /// Entspricht exakt process_LTerms / process_BTerms im C++-Code.
        /// </summary>
        private static double ProcessAngleL(double[] coeffs, double t)
        {
            double Lrad = Horner(coeffs, t);
            double Ldeg = Lrad * AngleUtil.DegPerRad;
            return AngleUtil.WrapDegrees360(Ldeg);
        }


        /// <summary>
        /// Verarbeitet Winkelkoeffizienten:
        ///   - Ergebnis aus Horner ist in Radiant
        ///   - Umrechnung in Grad
        ///   - Normalisierung auf [-90..90]
        ///
        /// Entspricht exakt process_LTerms / process_BTerms im C++-Code.
        /// </summary>
        private static double ProcessAngleB(double[] coeffs, double t)
        {
            double Brad = Horner(coeffs, t);
            double Bdeg = Brad * AngleUtil.DegPerRad;
            return AngleUtil.WrapDegrees90(Bdeg);
        }



        /// <summary>
        /// Verarbeitet Radiuskoeffizienten:
        ///   - Ergebnis aus Horner ist direkt in AU
        ///   - keine Winkel-Normalisierung
        ///
        /// Entspricht process_RTerms im C++-Code.
        /// </summary>
        private static double ProcessRadius(double[] coeffs, double t)
        {
            return Horner(coeffs, t);
        }
    }
}