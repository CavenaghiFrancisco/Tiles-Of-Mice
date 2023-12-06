using UnityEngine;
using System;

namespace TOM
{
    public class GameManager : MonoBehaviour
    {
        private static bool gamePaused = true;
        public static bool IsPaused => gamePaused;
        public static void PauseGame() => gamePaused = true;
        public static void ResumeGame() => gamePaused = false;
        public static void ReloadGame() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        public static Action OnPause;
        public static Action OnResume;

        private Controls controls;

        private void Awake()
        {
            controls = new Controls();

            controls.General.Pause.performed += context =>
            {
                Debug.Log("Apreto la tecla pa pausar");
                gamePaused = !gamePaused;
                if (gamePaused)
                {
                    OnPause?.Invoke();
                }
                else
                {
                    OnResume?.Invoke();

                }
            };
        }

        private void OnEnable()
        {
            controls.General.Enable();
        }

        private void OnDisable()
        {
            controls.General.Disable();
        }
    }
}