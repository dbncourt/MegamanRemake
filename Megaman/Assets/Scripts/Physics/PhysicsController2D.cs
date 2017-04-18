using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Physics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PhysicsController2D : MonoBehaviour
    {
        private RaycastController raycastController;

        [SerializeField]
        private LayerMask collisionMask;
        private BoxCollider2D boxCollider;

        [SerializeField]
        private float skinWidth;
        [SerializeField]
        private int horizontalRayCount;
        [SerializeField]
        private int verticalRayCount;
        [SerializeField]
        private float maxClimbAngle;
        [SerializeField]
        private float maxDescendAngle;

        public struct CollisionInfo
        {
            public bool above;
            public bool below;
            public bool left;
            public bool right;
            public bool climbingSlope;
            public bool descendingSlope;
            public float slopeAngle;
            public float slopeAngleOld;
            public bool isFacingRight;
            public Vector3 velocityOld;

            public void Reset()
            {
                above = below = false;
                left = right = false;
                climbingSlope = false;
                descendingSlope = false;
                slopeAngleOld = slopeAngle;
                slopeAngle = 0;
            }
        }
        protected CollisionInfo _collisionInfo;

        public PhysicsController2D()
        {
            maxClimbAngle = 80.0f;
            maxDescendAngle = 80.0f;
        }

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            raycastController = new RaycastController(ref collisionMask, ref boxCollider, skinWidth, ref horizontalRayCount, ref verticalRayCount);
        }

        public void Move(Vector3 velocity)
        {
            raycastController.UpdateRaycastOrigins();
            _collisionInfo.Reset();
            _collisionInfo.velocityOld = velocity;

            if (velocity.x != 0)
            {
                _collisionInfo.isFacingRight = (Mathf.Sign(velocity.x) == 1);
            }

            if (velocity.y < 0)
            {
                DescendSlope(ref velocity);
            }

            EvaluateHorizontalCollisions(ref velocity);
            if (velocity.y != 0)
            {
                EvaluateVerticalCollisions(ref velocity);
            }

            transform.Translate(velocity);
        }

        private void EvaluateHorizontalCollisions(ref Vector3 velocity)
        {
            float directionX = _collisionInfo.isFacingRight ? 1.0f : -1.0f;
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            if (Mathf.Abs(velocity.x) < skinWidth)
            {
                rayLength = 2 * skinWidth;
            }

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (_collisionInfo.isFacingRight) ? raycastController.rayOrigins.bottomRight : raycastController.rayOrigins.bottomLeft;
                rayOrigin += Vector2.up * (raycastController.horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {

                    if (hit.distance == 0)
                    {
                        continue;
                    }

                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (i == 0 && slopeAngle <= maxClimbAngle)
                    {
                        if (_collisionInfo.descendingSlope)
                        {
                            _collisionInfo.descendingSlope = false;
                            velocity = _collisionInfo.velocityOld;
                        }
                        float distanceToSlopeStart = 0;
                        if (slopeAngle != _collisionInfo.slopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - skinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }
                        ClimbSlope(ref velocity, slopeAngle);
                        velocity.x += distanceToSlopeStart * directionX;
                    }

                    if (!_collisionInfo.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        rayLength = hit.distance;

                        if (_collisionInfo.climbingSlope)
                        {
                            velocity.y = Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }

                        _collisionInfo.left = directionX == -1;
                        _collisionInfo.right = directionX == 1;
                    }
                }
            }
        }

        private void EvaluateVerticalCollisions(ref Vector3 velocity)
        {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {

                Vector2 rayOrigin = (directionY == -1) ? raycastController.rayOrigins.bottomLeft : raycastController.rayOrigins.topLeft;
                rayOrigin += Vector2.right * (raycastController.verticalRaySpacing * i + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {

                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    if (_collisionInfo.climbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    _collisionInfo.below = directionY == -1;
                    _collisionInfo.above = directionY == 1;
                }
            }

            if (_collisionInfo.climbingSlope)
            {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + skinWidth;
                Vector2 rayOrigin = ((directionX == -1) ? raycastController.rayOrigins.bottomLeft : raycastController.rayOrigins.bottomRight) + Vector2.up * velocity.y;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != _collisionInfo.slopeAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        _collisionInfo.slopeAngle = slopeAngle;
                    }
                }


            }
        }

        private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if (velocity.y <= climbVelocityY)
            {
                velocity.y = climbVelocityY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                _collisionInfo.below = true;
                _collisionInfo.climbingSlope = true;
                _collisionInfo.slopeAngle = slopeAngle;
            }
        }

        private void DescendSlope(ref Vector3 velocity)
        {
            float directionX = _collisionInfo.isFacingRight ? 1.0f : -1.0f;
            Vector2 rayOrigin = _collisionInfo.isFacingRight ? raycastController.rayOrigins.bottomLeft : raycastController.rayOrigins.bottomRight;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if ((slopeAngle != 0 && slopeAngle <= maxDescendAngle) &&
                    (Mathf.Sign(hit.normal.x) == directionX) &&
                    (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)))
                {
                    float moveDistance = Mathf.Abs(velocity.x);
                    float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                    velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * directionX;
                    velocity.y -= descendVelocityY;

                    _collisionInfo.slopeAngle = slopeAngle;
                    _collisionInfo.descendingSlope = true;
                    _collisionInfo.below = true;
                }
            }
        }
    }
}