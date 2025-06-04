using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;


namespace Oculus.Interaction
{
    public enum FurnitureType
    {
        Floor,
        Wall,
        Ceiling
    }

    /// <summary>
    /// An <see cref="ITransformer"/> that can translate, rotate and scale a transform using any number of grab points
    /// while also constraining the transformation if desired. This is the default Interaction SDK grab behavior
    /// and should be preferred over older implementations such as <see cref="OneGrabFreeTransformer"/> and
    /// <see cref="TwoGrabFreeTransformer"/>.
    /// </summary>
    public class FurnitureGrabTransform : MonoBehaviour, ITransformer
    {
        public FurnitureType type = FurnitureType.Floor;
        public float gridSize = 1.0f;
        public float rayStart = 1.0f;
        public RayInteractor righHandInteractor;
        public RayInteractor leftHandInteractor;
        public GameObject marker;

        private IGrabbable _grabbable;
        private Pose _grabDeltaInLocalSpace;
        private float _initialrightHandRotation;
        private Vector3? _initialRayHitPoint;
        private Vector3 _initialObjectPosition;

        private Quaternion _initialRotation = Quaternion.identity;

        private GrabPointDelta[] _deltas;

        // Controlling the time of the selection
        private float _selectionStartTime;
        public float requiredHoldTime = 0.4f; //minimum time to hold the object before moving it
        private bool _canMove = false;

        internal struct GrabPointDelta
        {
            private const float _epsilon = 0.000001f;

            public Vector3 PrevCentroidOffset { get; private set; }
            public Vector3 CentroidOffset { get; private set; }

            public Quaternion PrevRotation { get; private set; }
            public Quaternion Rotation { get; private set; }

            public GrabPointDelta(Vector3 centroidOffset, Quaternion rotation)
            {
                this.PrevCentroidOffset = this.CentroidOffset = centroidOffset;
                this.PrevRotation = this.Rotation = rotation;
            }

            public void UpdateData(Vector3 centroidOffset, Quaternion rotation)
            {
                this.PrevCentroidOffset = this.CentroidOffset;
                this.CentroidOffset = centroidOffset;

                this.PrevRotation = this.Rotation;

                //Quaternions have two ways of expressing the same rotation.
                //This code ensures that the result is the same rotation but expressed in the desired sign.
                if (Quaternion.Dot(rotation, this.Rotation) < 0)
                {
                    rotation.x = -rotation.x;
                    rotation.y = -rotation.y;
                    rotation.z = -rotation.z;
                    rotation.w = -rotation.w;
                }

                this.Rotation = rotation;
            }

            public bool IsValidAxis()
            {
                return CentroidOffset.sqrMagnitude > _epsilon;
            }
        }

        /// <summary>
        /// Implementation of <see cref="ITransformer.Initialize(IGrabbable)"/>; for details, please refer to the related documentation
        /// provided for that interface.
        /// </summary>
        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        /// <summary>
        /// Implementation of <see cref="ITransformer.BeginTransform"/>; for details, please refer to the related documentation
        /// provided for that interface.
        /// </summary>
        public void BeginTransform()
        {
            // Starts the time counts
            _selectionStartTime = Time.time;
            _canMove = false;

            int count = _grabbable.GrabPoints.Count;

            //rent space only while using
            _deltas = ArrayPool<GrabPointDelta>.Shared.Rent(count);

            InitializeDeltas(count, _grabbable.GrabPoints, ref _deltas);
            Vector3 centroid = GetCentroid(_grabbable.GrabPoints);

            Transform targetTransform = _grabbable.Transform;
            _grabDeltaInLocalSpace = new Pose(
                targetTransform.InverseTransformVector(centroid - targetTransform.position),
                targetTransform.rotation);
            _initialRotation = targetTransform.rotation;

            _initialObjectPosition = targetTransform.position;
            _initialrightHandRotation = righHandInteractor.Rotation.eulerAngles.z;


            var collider = targetTransform.GetComponentInParent<Collider>();
            _initialRayHitPoint = GetInteractorRayHit(righHandInteractor.Ray, collider)?.point;

            marker.GetComponent<Renderer>().enabled = true;
        }

        /// <summary>
        /// Implementation of <see cref="ITransformer.UpdateTransform"/>; for details, please refer to the related documentation
        /// provided for that interface.
        /// </summary>
        public void UpdateTransform()
        {
            if (!_canMove)
            {
                if (Time.time - _selectionStartTime >= requiredHoldTime)
                {
                    _canMove = true;
                }
                else
                {
                    return; // ainda n�o pode mover
                }
            }

            int count = _grabbable.GrabPoints.Count;
            Transform targetTransform = _grabbable.Transform;


            var targetCollider = targetTransform.GetComponentInParent<Collider>();

            var hit = SnapToRayIntersection(targetTransform);
            if (hit.HasValue)
            {
                var surfaceNormal = hit.Value.normal;

                if (this.type == FurnitureType.Floor && !(Vector3.Dot(surfaceNormal, Vector3.up) > 0.99))
                {
                    return;
                }
                Debug.Log("Hit point: " + hit.Value.point.ToString());
                SnapTransformToSurface(targetTransform, targetCollider, surfaceNormal);
                //SnapTransformToGrid(marker.transform);
                //ResolveWallPenetration(targetTransform, targetCollider);

                targetCollider.enabled = false;
                var markerCollider = marker.GetComponentInParent<Collider>();
                //SnapTransformToSurface(marker.transform, markerCollider, surfaceNormal);
                targetCollider.enabled = true;

            }

            //RotateTransformWithInteractor(targetTransform.GetChild(2));
        }

        private void ResolveWallPenetration(Transform targetTransform, Collider collider)
        {
            var hits = Physics.OverlapBox(targetTransform.position, collider.bounds.extents, targetTransform.rotation);

            foreach (var hit in hits)
            {
                if (hit.tag == "wall")
                {
                    var adjustedCollider = hit.gameObject.AddComponent<BoxCollider>();
                    adjustedCollider.center = new Vector3(0.0f, -2.5f, 0.0f);
                    adjustedCollider.size = new Vector3(100.0f, 5.0f, 100.0f);

                    Debug.Log("IS HITING WALL!!");
                    if (Physics.ComputePenetration(
                        adjustedCollider, hit.transform.position, hit.transform.rotation,
                        collider, targetTransform.position, targetTransform.rotation,
                        out Vector3 direction, out float distance))
                    {
                        Debug.Log("Penetration is: " + distance);
                        //direction.y = 0;
                        targetTransform.position -= direction * distance;
                    }

                    Destroy(adjustedCollider);
                }
            }

        }

        private void RotateTransformWithInteractor(Transform targetTransform)
        {
            var currentRotationX = targetTransform.localRotation.eulerAngles.x;
            var currentRotationZ = targetTransform.localRotation.eulerAngles.z;
            //var newRotationY = currentRotationY + 4 * (_initialrightHandRotation - righHandInteractor.Rotation.eulerAngles.z);
            var deltaRotationY = 2 * (_initialrightHandRotation - righHandInteractor.Rotation.eulerAngles.z);
            var initialRotationY = _initialRotation.eulerAngles.y;
            targetTransform.localRotation = Quaternion.Euler(currentRotationX, initialRotationY + deltaRotationY, currentRotationZ);
        }


        private RaycastHit? SnapToRayIntersection(Transform targetTransform)
        {
            var collider = targetTransform.GetComponentInParent<Collider>();

            var interactorRay = righHandInteractor.Ray;

            var hit = GetInteractorRayHit(interactorRay, collider);

            if (hit != null)
            {
                targetTransform.position = hit.Value.point;
            }

            return hit;
        }

        private RaycastHit? GetInteractorRayHit(Ray ray, Collider collider)
        {
            var originalColliderState = collider.enabled;

            collider.enabled = false;

            ray.origin = ray.origin + rayStart * ray.direction;

            RaycastHit? hitPoint = null;

            if (Physics.Raycast(ray, out var hitInfo))
            {
                hitPoint = hitInfo;
            }

            collider.enabled = originalColliderState;

            return hitPoint;
        }

        private void SnapTransformToGrid(Transform targetTransform)
        {
            targetTransform.position = new Vector3(snapToGrid(transform.position.x, gridSize), transform.position.y, snapToGrid(transform.position.z, gridSize));
        }

        private float snapToGrid(float v, float gridSize)
        {
            return (float)Math.Round(v / gridSize) * gridSize;
        }

        private static void SnapTransformToSurface(Transform targetTransform, Collider collider, Vector3 surfaceNormal)
        {
            var rayDistance = Math.Abs(Vector3.Dot(collider.bounds.extents, surfaceNormal)) + 0.5f;
            var rayOrigin = targetTransform.position + surfaceNormal * rayDistance;
            var rayDirection = -surfaceNormal;

            var extents = collider.bounds.extents;

            var isEnabled = collider.enabled;

            collider.enabled = false;
            var hits = Physics.BoxCastAll(rayOrigin, extents, rayDirection);

            var closestValidHit = GetClosestHit(hits);

            if (closestValidHit != null)
            {
                collider.enabled = isEnabled;

                //InstantiateSphere(rayOrigin, targetTransform, Color.blue);
                //InstantiateSphere(closestValidHit.Value.point, targetTransform, Color.red);
                var newPosition = ProjectPointOntoPlane(targetTransform.position, closestValidHit.Value.point, surfaceNormal);

                targetTransform.position = newPosition;
                targetTransform.position = newPosition;
                AlignWithVector(targetTransform, surfaceNormal);
            }

            collider.enabled = isEnabled;


            RaycastHit? GetClosestHit(RaycastHit[] hits)
            {
                RaycastHit? closest = null;

                float finalDistance = float.PositiveInfinity;

                foreach (var hit in hits)
                {
                    if (hit.transform.gameObject.tag == "floor" || hit.transform.gameObject.tag == "Furniture" || hit.transform.gameObject.tag == "wall")
                    {
                        float distance = (rayOrigin - hit.point).sqrMagnitude;
                        if (distance < finalDistance)
                        {
                            closest = hit;
                            finalDistance = distance;
                        }
                    }
                }

                return closest;
            }

            static Vector3 ProjectPointOntoPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
            {
                // Vector from planePoint to the point
                Vector3 toPoint = point - planePoint;

                // Distance from point to plane along the normal
                float distance = Vector3.Dot(toPoint, planeNormal.normalized);

                // Projected point is the original point minus the component in the normal direction
                return point - distance * planeNormal.normalized;
            }

            static void AlignWithVector(Transform transform, Vector3 surfaceNormal, Vector3? forwardHint = null)
            {
                if (Math.Abs(Vector3.Dot(transform.up, surfaceNormal)) < 0.01f)
                {
                    var right = Vector3.Cross(transform.up, surfaceNormal);
                    var forward = Vector3.Cross(surfaceNormal, right);
                    // Calculate a rotation where the Y-axis points along the surface normal
                    Quaternion rotation = Quaternion.LookRotation(forward, surfaceNormal);

                    transform.rotation = rotation;
                }
            }
        }

        public static GameObject InstantiateSphere(Vector3 position, Transform parentTransform = null, Color? color = null)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;

            if (parentTransform != null)
            {
                sphere.transform.SetParent(parentTransform);
            }

            // Remove collider
            Collider collider = sphere.GetComponent<Collider>();
            if (collider != null)
            {
                GameObject.Destroy(collider);
            }

            // Set color
            Renderer renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = color ?? Color.white; // Default to white if no color provided
            }

            // Destroy after 3 seconds
            GameObject.Destroy(sphere, 3f);

            return sphere;
        }


        /// <summary>
        /// Implementation of <see cref="ITransformer.EndTransform"/>; for details, please refer to the related documentation
        /// provided for that interface.
        /// </summary>
        public void EndTransform()
        {
            //return the uneeded space
            ArrayPool<GrabPointDelta>.Shared.Return(_deltas);
            _deltas = null;

            var collider = _grabbable.Transform.GetComponentInParent<Collider>();
            marker.GetComponent<Renderer>().enabled = false;
            SnapTransformToGrid(_grabbable.Transform);
            //SnapTransformToSurface(_grabbable.Transform, collider);
            ResolveWallPenetration(_grabbable.Transform, collider);
        }

        internal static void InitializeDeltas(int count, List<Pose> poses, ref GrabPointDelta[] deltas)
        {
            Vector3 centroid = GetCentroid(poses);
            for (int i = 0; i < count; i++)
            {
                Vector3 centroidOffset = GetCentroidOffset(poses[i], centroid);
                deltas[i] = new GrabPointDelta(centroidOffset, poses[i].rotation);
            }
        }

        internal static Vector3 UpdateTransformerPointData(List<Pose> poses, ref GrabPointDelta[] deltas)
        {
            Vector3 centroid = GetCentroid(poses);
            for (int i = 0; i < poses.Count; i++)
            {
                Vector3 centroidOffset = GetCentroidOffset(poses[i], centroid);
                deltas[i].UpdateData(centroidOffset, poses[i].rotation);
            }
            return centroid;
        }

        internal static Vector3 GetCentroid(List<Pose> poses)
        {
            int count = poses.Count;
            Vector3 sumPosition = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                Pose pose = poses[i];
                sumPosition += pose.position;
            }

            return sumPosition / count;
        }

        internal static Vector3 GetCentroidOffset(Pose pose, Vector3 centre)
        {
            Vector3 centroidOffset = centre - pose.position;
            return centroidOffset;
        }

        internal static Quaternion UpdateRotation(int count, GrabPointDelta[] deltas)
        {
            Quaternion combinedRotation = Quaternion.identity;

            //each point can only affect a fraction of the rotation
            float fraction = 1f / count;
            for (int i = 0; i < count; i++)
            {
                GrabPointDelta data = deltas[i];

                //overall delta rotation since last update
                Quaternion rotDelta = data.Rotation * Quaternion.Inverse(data.PrevRotation);

                if (data.IsValidAxis())
                {
                    Vector3 aimingAxis = data.CentroidOffset.normalized;
                    //rotation along aiming axis
                    Quaternion dirDelta = Quaternion.FromToRotation(data.PrevCentroidOffset.normalized, aimingAxis);
                    combinedRotation = Quaternion.Slerp(Quaternion.identity, dirDelta, fraction) * combinedRotation;

                    //twist along the aiming axis
                    rotDelta.ToAngleAxis(out float angle, out Vector3 axis);
                    float projectionFactor = Vector3.Dot(axis, aimingAxis);
                    rotDelta = Quaternion.AngleAxis(angle * projectionFactor, aimingAxis);
                }

                combinedRotation = Quaternion.Slerp(Quaternion.identity, rotDelta, fraction) * combinedRotation;
            }

            return combinedRotation;
        }
    }
}