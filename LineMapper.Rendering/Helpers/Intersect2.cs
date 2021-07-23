using System.Diagnostics.CodeAnalysis;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Helpers
{
    /// <summary>
    /// Calculates intersections in 2-dimensional space.
    /// </summary>
    static class Intersect2
    {
        /// <summary>
        /// Calculates the intersection between rays.
        /// </summary>
        /// <returns>True if an intersection is found, false if the rays are parallel or collinear.</returns>
        public static bool Intersect(Ray2 r1, Ray2 r2, [NotNullWhen(true)] out Vector2 point)
        {
            var denominator = r1.Vector.Y * r2.Vector.X - r1.Vector.X * r2.Vector.Y;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (denominator == 0)
            {
                // Lines are parallel or collinear.
                point = Vector2.Zero;
                return false;
            }

            var xDif = r1.Origin.X - r2.Origin.X;
            var yDif = r2.Origin.Y - r1.Origin.Y;

            var r1Nominator = r2.Vector.Y * xDif + r2.Vector.X * yDif;
            var r1F = r1Nominator / denominator;

            point = r1.Origin + r1.Vector * r1F;
            return true;
        }
    }
}
