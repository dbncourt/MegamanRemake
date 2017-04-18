using UnityEngine;
using System.Collections;

namespace Project.Physics
{
    public class RaycastController
    {
        public struct RaycastOrigins
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 bottomLeft;
            public Vector2 bottomRight;
        }
        public float horizontalRaySpacing
        {
            get { return _horizontalRaySpacing; }
        }
        public float verticalRaySpacing
        {
            get { return _verticalRaySpacing; }
        }
        public RaycastOrigins rayOrigins
        {
            get { return _rayOrigins; }
        }

        private LayerMask collisionMask;
        private BoxCollider2D boxCollider;

        private float skinWidth;
        private int horizontalRayCount;
        private int verticalRayCount;
        private float _horizontalRaySpacing;
        private float _verticalRaySpacing;
        private RaycastOrigins _rayOrigins;

        public RaycastController(ref LayerMask collisionMask, ref BoxCollider2D boxCollider, float skinWidth, ref int horizontalRayCount, ref int verticalRayCount)
        {
            this.collisionMask = collisionMask;
            this.boxCollider = boxCollider;
            this.skinWidth = skinWidth;
            this.horizontalRayCount = horizontalRayCount;
            this.verticalRayCount = verticalRayCount;

            CalculateRaySpacing();
        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            _rayOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _rayOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _rayOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            _rayOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
    }
}
