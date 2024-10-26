using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Objects.UI
{
    [CreateAssetMenu(menuName = "MP/Button Icons")]
    public class ButtonIcons : SerializedScriptableObject
    {
        public Dictionary<string, Sprite> Dictionary;
    }
}
