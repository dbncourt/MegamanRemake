using UnityEngine;

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
        public float HorizontalRaySpacing {get; set;}
        public float VerticalRaySpacing { get; set;}
        public RaycastOrigins RayOrigins { get; set; }

        private BoxCollider2D boxCollider;

        private float skinWidth;
        private int horizontalRayCount;
        private int verticalRayCount;

        public RaycastController(ref BoxCollider2D boxCollider, float skinWidth, ref int horizontalRayCount, ref int verticalRayCount)
        {
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

            HorizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            VerticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            RaycastOrigins rayOrigins = new RaycastOrigins()
            {
                bottomLeft = new Vector2(bounds.min.x, bounds.min.y),
                bottomRight = new Vector2(bounds.max.x, bounds.min.y),
                topLeft = new Vector2(bounds.min.x, bounds.max.y),
                topRight = new Vector2(bounds.max.x, bounds.max.y)
            };
            RayOrigins = rayOrigins;
        }
    }
}