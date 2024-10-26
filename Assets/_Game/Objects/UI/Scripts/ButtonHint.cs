using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

namespace MP.Game.Objects.UI
{

    public class ButtonHint : MonoBehaviour
    {
        public ButtonIcons Icons;
        public InputActionReference Button;
        public GameObject TextBasedView;
        public TextMeshProUGUI UIText;
        public GameObject ImageBasedView;
        public Image Image;

        private static string XBOX_Controller = "XInput";
        private static string Mouse = "Mouse";


        private void Update()
        {
            if (Button == null)
                return;
            var deviceLayout = default(string);
            var controlPath = default(string);

            InputAction action = Button.action;

            var currentControlScheme = InputUser.all[0].controlScheme;

            var bindingIndex = action.GetBindingIndex(currentControlScheme.Value.bindingGroup);
            var bindingString = action.GetBindingDisplayString(bindingIndex, out deviceLayout, out controlPath);

            if (action.bindings[bindingIndex].isPartOfComposite)
            {
                if (action.expectedControlType is "Vector2")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        bindingString += "/" + action.GetBindingDisplayString(bindingIndex + i, out deviceLayout, out controlPath);
                    }
                }

            }

            UIText.text = bindingString;
            if(deviceLayout.Contains(Mouse))
            {
                if (Icons.Dictionary.TryGetValue($"{bindingString}", out Sprite sprite))
                {
                    Image.sprite = sprite;
                }
                TextBasedView.SetActive(false);
                ImageBasedView.SetActive(true);
            }
            else if (deviceLayout.Contains(XBOX_Controller))
            {
                if (Icons.Dictionary.TryGetValue($"XBOX_{bindingString}", out Sprite sprite))
                {
                    Image.sprite = sprite;
                }
                TextBasedView.SetActive(false);
                ImageBasedView.SetActive(true);
            }
            else
            {
                TextBasedView.SetActive(true);
                ImageBasedView.SetActive(false);
            }

        }
    }
}
