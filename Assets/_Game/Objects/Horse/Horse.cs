using UnityEngine;

namespace MP.Game
{
    public class Horse : MonoBehaviour
    {
        [field: SerializeField] public Transform MountPosition { get; private set; }
        public float Acceleration;
        public float Speed;
        public float RotationSpeed;

        private void OnDrawGizmosSelected()
        {
            if (MountPosition == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(MountPosition.position, Vector3.one * 0.25f);
        }
    }
}
