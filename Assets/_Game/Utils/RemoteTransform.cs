using System;
using UnityEngine;

namespace MP.Game.Utils
{
    [ExecuteAlways]
    public class RemoteTransform : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private Item _copyPosition;
        [SerializeField] private Item _copyRotation;
        [SerializeField] private Item _copyScale;
        [SerializeField] private WhenToCopy _whenToCopy;
        public float PositionCopyMultiplier = 1;
        public float RotationCopyMultiplier = 1;
        public float ScaleCopyMultiplier = 1;

        [Serializable]
        private struct Item
        {
            public Vector3B AxisToCopy;
            public Vector3 Offset;
            public float Smoothing;

            public bool Copy => AxisToCopy.X == true || AxisToCopy.Y == true || AxisToCopy.Z == true;
            public static implicit operator bool(Item me) { return me.Copy; }
        }

        private enum WhenToCopy
        {
            Update, FixedUpdate, LateUpdate
        }

        private void Copy()
        {
            if (_target == null)
                return;
            if (_copyPosition)
            {
                var targetPosition = transform.position;

                if (_copyPosition.AxisToCopy.X)
                    targetPosition.x = _target.position.x + _copyPosition.Offset.x;
                if (_copyPosition.AxisToCopy.Y)
                    targetPosition.y = _target.position.y + _copyPosition.Offset.y;
                if (_copyPosition.AxisToCopy.Z)
                    targetPosition.z = _target.position.z + _copyPosition.Offset.z;

                var t = Time.deltaTime * _copyPosition.Smoothing;
                if (t == 0)
                    transform.position = targetPosition;
                else
                    transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            }

            if (_copyRotation)
            {
                var targetRotation = transform.rotation;
                var additionalRotation = Quaternion.Euler(_target.rotation.eulerAngles + _copyRotation.Offset);

                if (_copyRotation.Offset == Vector3.zero)
                    additionalRotation = _target.rotation;

                if (_copyRotation.AxisToCopy.X)
                    targetRotation.x = _target.rotation.x;//_target.rotation.x;
                if (_copyRotation.AxisToCopy.Y)
                    targetRotation.y = _target.rotation.y;// _target.rotation.y;
                if (_copyRotation.AxisToCopy.Z)
                    targetRotation.z = _target.rotation.z;// _target.rotation.z;

                var t = Time.deltaTime * _copyRotation.Smoothing;
                if (t == 0)
                    transform.rotation = _target.rotation;
                else
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            }

            if (_copyScale)
            {
                var targetScale = transform.lossyScale;

                if (_copyScale.AxisToCopy.X)
                    targetScale.x = _target.lossyScale.x + _copyScale.Offset.x;
                if (_copyScale.AxisToCopy.Y)
                    targetScale.y = _target.lossyScale.y + _copyScale.Offset.y;
                if (_copyScale.AxisToCopy.Z)
                    targetScale.z = _target.lossyScale.z + _copyScale.Offset.z;

                var t = Time.deltaTime * _copyScale.Smoothing;
                if (t == 0)
                    transform.localScale = targetScale;
                else
                    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
            }
        }

        private void FixedUpdate()
        {
            if (_whenToCopy == WhenToCopy.FixedUpdate) Copy();
        }

        private void LateUpdate()
        {
            if (_whenToCopy == WhenToCopy.LateUpdate) Copy();
        }

        private void Update()
        {
            if (_whenToCopy == WhenToCopy.Update) Copy();
        }
    }
}
