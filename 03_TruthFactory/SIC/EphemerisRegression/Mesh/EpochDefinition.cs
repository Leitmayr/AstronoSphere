using System;

namespace EphemerisRegression.Mesh
{
    public enum MeshEpochType
    {
        Core,
        Extended,
        Extreme
    }

    public sealed class EpochDefinition
    {
        public MeshEpochType EpochType { get; }
        public MeshUtc StartUtc { get; }
        public MeshUtc StopUtc { get; }
        public TimeSpan InnerStep { get; }
        public TimeSpan OuterStep { get; }

        public EpochDefinition(
            MeshEpochType epochType,
            MeshUtc startUtc,
            MeshUtc stopUtc,
            TimeSpan innerStep,
            TimeSpan outerStep)
        {
            EpochType = epochType;
            StartUtc = startUtc;
            StopUtc = stopUtc;
            InnerStep = innerStep;
            OuterStep = outerStep;
        }

        public static EpochDefinition CreateCore()
        {
            return new EpochDefinition(
                MeshEpochType.Core,
                new MeshUtc(1600, 1, 1),
                new MeshUtc(2400, 12, 31),
                TimeSpan.FromDays(30),
                TimeSpan.FromDays(60));
        }

        public static EpochDefinition CreateExtended()
        {
            return new EpochDefinition(
                MeshEpochType.Extended,
                new MeshUtc(0, 1, 1),
                new MeshUtc(4000, 12, 31),
                TimeSpan.FromDays(180),
                TimeSpan.FromDays(180));
        }

        public static EpochDefinition CreateExtreme()
        {
            return new EpochDefinition(
                MeshEpochType.Extreme,
                new MeshUtc(-4000, 1, 1),
                new MeshUtc(8000, 12, 31),
                TimeSpan.FromDays(365 * 2),
                TimeSpan.FromDays(365 * 2));
        }
    }
}