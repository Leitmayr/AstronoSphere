using System.Collections.Generic;

namespace EphemerisRegression.Domain
{
    public sealed class ReferenceStateVector
    {
        public string Planet { get; }
        public string TestSuite { get; }
        public string EventName { get; }
        public string CorrectionLevel { get; }

        public ReferenceMetadata Metadata { get; }

        public IReadOnlyList<StateVector> Vectors { get; }

        public ReferenceStateVector(
            string planet,
            string testSuite,
            string eventName,
            string correctionLevel,
            IReadOnlyList<StateVector> vectors,
            ReferenceMetadata metadata)
        {
            Planet = planet;
            TestSuite = testSuite;
            EventName = eventName;
            CorrectionLevel = correctionLevel;
            Vectors = vectors;
            Metadata = metadata;
        }
    }
}

