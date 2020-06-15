using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {

        private MainMenuManager mainMenuManager;

        public static bool isHard;

        private GameManager[] objects;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                mainMenuManager = GameObject.FindObjectOfType<MainMenuManager>().GetComponent<MainMenuManager>();
            }
        }

        private void Update()
        {
            isHard = mainMenuManager.currentLevelIsHard;
        }
    }
}