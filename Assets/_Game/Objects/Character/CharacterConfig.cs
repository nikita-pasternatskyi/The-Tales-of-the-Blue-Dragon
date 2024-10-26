using UnityEngine;

namespace MP.Game.Objects.Character
{
    [CreateAssetMenu(menuName = "MP/Character/Config")]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Parameters")]
        [field: SerializeField] public float Gravity { get; private set; }
        [field: SerializeField] public float GroundDrag { get; private set; }
        [field: SerializeField] public float AirDrag { get; private set; }
    }
}
