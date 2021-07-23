using System;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Vertices;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Drawers
{
    sealed class ArcDrawer<TVertex, TVertexParameters> : IArcDrawer<TVertexParameters>
        where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateShapeVertex(Vector3 xyz, TVertexParameters parameters);

        private readonly IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder;
        private readonly CreateShapeVertex createVertex;

        public ArcDrawer(
            IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, CreateShapeVertex createVertex)
        {
            this.meshBuilder = meshBuilder;
            this.createVertex = createVertex;
        }

        public void DrawArc(
            float centerX, float centerY, float centerZ,
            float angleStart, float angleEnd,
            float radius,
            float lineWidth,
            TVertexParameters parameters,
            int edges)
        {
            if (edges < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least one edge.");
            }
            if (Math.Abs(angleEnd - angleStart) > MathHelper.TwoPi)
            {
                throw new ArgumentException(
                    $"{nameof(angleStart)} and {nameof(angleEnd)} must be at most two pi apart");
            }

            meshBuilder.Add((edges + 1) * 2, edges * 6, out var vertices, out var indices, out var indexOffset);

            var innerR = radius - lineWidth;

            var rotation = Matrix2.CreateRotation((angleEnd - angleStart) / edges);
            var xy = Vector2.UnitX * Matrix2.CreateRotation(angleStart);

            vertices[0] =
                createVertex(new Vector3(centerX + xy.X * radius, centerY + xy.Y * radius, centerZ), parameters);
            vertices[1] =
                createVertex(new Vector3(centerX + xy.X * innerR, centerY + xy.Y * innerR, centerZ), parameters);

            for (var i = 1; i <= edges; i++)
            {
                xy *= rotation;
                vertices[2 * i] =
                    createVertex(new Vector3(centerX + xy.X * radius, centerY + xy.Y * radius, centerZ), parameters);
                vertices[2 * i + 1] =
                    createVertex(new Vector3(centerX + xy.X * innerR, centerY + xy.Y * innerR, centerZ), parameters);
            }

            for (var i = 0; i < edges; i++)
            {
                var j = i * 6;
                var o = i * 2;

                indices[j] = (ushort)(indexOffset + o);
                indices[j + 1] = (ushort)(indexOffset + o + 2);
                indices[j + 2] = (ushort)(indexOffset + o + 1);

                indices[j + 3] = (ushort)(indexOffset + o + 1);
                indices[j + 4] = (ushort)(indexOffset + o + 2);
                indices[j + 5] = (ushort)(indexOffset + o + 3);
            }
        }
    }
}
