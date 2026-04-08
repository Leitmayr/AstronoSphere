// ============================================================
// FILE: 99_Contracts.TestConsole/CoreModels.cs
// STATUS: NEW
// ============================================================

using System.Collections.Generic;

namespace Contracts.TestConsole
{
    public class Core
    {
        public Time Time { get; set; }
        public Observer Observer { get; set; }
        public ObservedObject ObservedObject { get; set; }
        public Frame Frame { get; set; }
    }

    public class Time
    {
        public double StartJD { get; set; }
        public double StopJD { get; set; }
        public string Step { get; set; }
        public string TimeScale { get; set; }
    }

    public class Observer
    {
        public string Type { get; set; }
        public string Body { get; set; }
    }

    public class ObservedObject
    {
        public string BodyClass { get; set; }
        public List<string> Targets { get; set; }
    }

    public class Frame
    {
        public string Type { get; set; }
        public string Epoch { get; set; }
    }
}