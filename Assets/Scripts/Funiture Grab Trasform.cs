using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;


namespace Oculus.Interaction
{
    /// <summary>
    /// An <see cref="ITransformer"/> that can translate, rotate and scale a transform using any number of grab points
    /// while also constraining the transformation if desired. This is the default Interaction SDK grab behavior
    /// and should be preferred over older implementations such as <see cref="OneGrabFreeTransformer"/> and
    /// <see cref="TwoGrabFreeTransformer"/>.
    /// </summary>
    public class GrabFreeTransformer : MonoBehaviour, ITransformer
    {
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

        private Quaternion _lastRotation = Quaternion.identity;

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
            _lastRotation = Quaternion.identity;

            _initialObjectPosition = targetTransform.position;
            _initialrightHandRotation = righHandInteractor.Rotation.eulerAngles.z;


            var collider = targetTransform.GetComponentInParent<Collider>();
            _initialRayHitPoint = GetInteractorRayHitPosition(righHandInteractor.Ray, collider);

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

            RotateTransformWithInteractor(targetTransform);

            var targetCollider = targetTransform.GetComponentInParent<Collider>();

            SnapToRayIntersection(targetTransform);
            SnapTransformToGround(targetTransform, targetCollider);
            SnapTransformToGrid(marker.transform);

            targetCollider.enabled = false;
            var markerCollider = marker.GetComponentInParent<Collider>();
            SnapTransformToGround(marker.transform, markerCollider);
            targetCollider.enabled = true;
        }


        private void RotateTransformWithInteractor(Transform targetTransform)
        {
            var currentRotationX = targetTransform.localRotation.eulerAngles.x;
            var currentRotationZ = targetTransform.localRotation.eulerAngles.z;
            //var newRotationY = currentRotationY + 4 * (_initialrightHandRotation - righHandInteractor.Rotation.eulerAngles.z);
            var newRotationY = 2 * (_initialrightHandRotation - righHandInteractor.Rotation.eulerAngles.z);
            targetTransform.localRotation = Quaternion.Euler(currentRotationX, newRotationY, currentRotationZ);
        }


        private void SnapToRayIntersection(Transform targetTransform)
        {
            var collider = targetTransform.GetComponentInParent<Collider>();

            var interactorRay = righHandInteractor.Ray;

            var hitPoint = GetInteractorRayHitPosition(interactorRay, collider);

            if (hitPoint != null)
            {
                targetTransform.position = hitPoint.Value;
            }
        }

        private Vector3? GetInteractorRayHitPosition(Ray ray, Collider collider)
        {
            var originalColliderState = collider.enabled;

            collider.enabled = false;

            ray.origin = ray.origin + rayStart * ray.direction;

            Vector3? hitPoint = null;

            if (Physics.Raycast(ray, out var hitInfo))
            {
                hitPoint = hitInfo.point;
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

        private static void SnapTransformToGround(Transform targetTransform, Collider collider)
        {
            var rayOrigin = targetTransform.position + Vector3.up * 100.0f;
            var rayDirection = Vector3.down;

            var extents = collider.bounds.extents;

            var isEnabled = collider.enabled;
            var boundMinY = collider.bounds.min.y;
            collider.enabled = false;

            if (Physics.BoxCast(rayOrigin, extents, rayDirection, out var hitInfo))
            {

                collider.enabled = isEnabled;

                float halfHeight = 0.0f;
                targetTransform.position = new Vector3(
                    targetTransform.position.x,
                    hitInfo.point.y + halfHeight,
                    targetTransform.position.z);

            }
            else
            {
                collider.enabled = isEnabled;
            }
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
            SnapTransformToGround(_grabbable.Transform, collider);
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