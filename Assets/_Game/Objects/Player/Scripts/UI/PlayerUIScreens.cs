using MP.Game.Objects.Health;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Player.Scripts.UI
{

    public class PlayerUIScreens : SerializedMonoBehaviour
    {
        [SerializeField] private UIScreen _background;
        [SerializeField] private Dictionary<InputActionReference, Screen> _screens;
        private Screen _currentScreen;

        [System.Serializable]
        private struct Screen
        {
            public UIScreen UIScreen;
            public bool PausesGame;
            public bool UseBackground;

            public static bool operator== (Screen obj1, Screen obj2)=>obj1.UIScreen == obj2.UIScreen;
            public static bool operator !=(Screen obj1, Screen obj2)=>obj1.UIScreen != obj2.UIScreen;
        }

        private void Awake()
        {
            LockMouse();
            foreach (var  screen in _screens.Values) 
            {
                screen.UIScreen.gameObject.SetActive(true);
            }
            _background.gameObject.SetActive(true);
        }

        private void Update()
        {
            foreach(var input in _screens.Keys)
            {
                if(input.action.WasPressedThisFrame())
                {
                    ChangeScreen(_screens[input]);
                    return;
                }
            }
        }

        private void ChangeScreen(Screen newScreen)
        {
            if (_currentScreen.UIScreen != null)
            {
                _currentScreen.UIScreen.Hide();
                if (newScreen == _currentScreen)
                {
                    if (_currentScreen.PausesGame)
                        Game.Instance.UnpauseGame();
                    if (_currentScreen.UseBackground)
                        HideBackground();
                    LockMouse();
                    _currentScreen = default;
                    return;
                }
            }

            UnlockMouse();
            _currentScreen = newScreen;
            _currentScreen.UIScreen.Show();
            if (_currentScreen.UseBackground)
                ShowBackground();
            if (newScreen.PausesGame)
                Game.Instance.PauseGame();
        }

        public void Exit()
        {
            ChangeScreen(_currentScreen);
        }

        private void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockMouse()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private void ShowBackground()
        {
            _background.Show();
        }
        private void HideBackground()
        {
            _background.Hide();
        }
    }
}
