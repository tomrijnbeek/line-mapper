using OpenTK.Mathematics;

namespace LineMapper.Rendering.Helpers;

readonly struct Ray2
{
    /// <summary>
    /// The ray starting in the origin pointing along the positive X axis.
    /// </summary>
    public static readonly Ray2 PositiveX = new(Vector2.Zero, Vector2.UnitX);

    /// <summary>
    /// The ray starting in the origin pointing along the positive Y axis.
    /// </summary>
    public static readonly Ray2 PositiveY = new(Vector2.Zero, Vector2.UnitY);

    /// <summary>
    /// Creates a new ray instance without normalizing the vector.
    /// </summary>
    public static Ray2 WithoutNormalizing(Vector2 origin, Vector2 vector) => new(origin, vector);

    /// <summary>
    /// The origin of this ray.
    /// </summary>
    public Vector2 Origin { get; }

    /// <summary>
    /// The direction vector of this ray.
    /// </summary>
    public Vector2 Vector { get; }

    private Ray2(Vector2 origin, Vector2 vector)
    {
        Origin = origin;
        Vector = vector;
    }
}
