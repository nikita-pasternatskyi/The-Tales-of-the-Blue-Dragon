using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    public class TeleportPlayerToPosition : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private bool _copyRotation;

        public void Teleport(Player player)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = _targetTransform.position;
            if (_copyRotation)
                player.transform.rotation = _targetTransform.rotation;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
