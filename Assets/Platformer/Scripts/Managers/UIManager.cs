using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class UIManager : MonoBehaviour
    {
        public enum ScreenState { Game, Pause, Lose }

        public static UIManager Instance;

        [Header("Components")]
        public GameObject gameScreen, pauseScreen, loseScreen, finishScreen;

        public ScreenState screenState;

        public Image hpBar;
        public Slider staminaSlider;
        public Text scoreText;
        public Text finishScore;
        public Text arrowsText;
        public Image[] stars;
        public Sprite emptyStar;
        public Sprite fullStar;
        public int oneStarLevelComplete;
        public int twoStarLevelComplete;
        public int threeStarLevelComplete;

        public GameObject mobileUI;

        private int currentSceneIndex;

        private PlayerController playerController;
        private GameManager gameManager;

        private void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Awake()
        {
            SingletonInit();

            playerController = FindObjectOfType<PlayerController>();
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            mobileUI.SetActive(true); //if mobile platform, mobile UI'll turn on
#endif

            UpdateHP(LevelManager.Instance.playerStats.statsData.currentHP, LevelManager.Instance.playerStats.statsData.maxHP); //clear UI
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (LevelManager.Instance.isPause)
                    ChangeScreen(ScreenState.Game);
                else
                    ChangeScreen(ScreenState.Pause);
            }

            if(playerController.isGameEnd) StarsController();
            UpdateScore();
            UpdateStamina();
            UpdateArrows();
        }

        //Player hp update method
        public void UpdateHP(float hpValue, float maxHpValue)
        {
            hpBar.fillAmount = hpValue / maxHpValue;
        }

        private void UpdateArrows()
        {
            arrowsText.text = "x" + playerController.arrows.ToString();
        }

        private void StarsController()
        {
            if(playerController.score >= oneStarLevelComplete && playerController.score < twoStarLevelComplete)
            {
                stars[0].sprite = fullStar;
                stars[1].sprite = emptyStar;
                stars[2].sprite = emptyStar;
            }
            else if(playerController.score >= twoStarLevelComplete && playerController.score < threeStarLevelComplete)
            {
                stars[0].sprite = fullStar;
                stars[1].sprite = fullStar;
                stars[2].sprite = emptyStar;
            }
            else if(playerController.score >= threeStarLevelComplete)
            {
                stars[0].sprite = fullStar;
                stars[1].sprite = fullStar;
                stars[2].sprite = fullStar;
            }
            else
            {
                stars[0].sprite = emptyStar;
                stars[1].sprite = emptyStar;
                stars[2].sprite = emptyStar;
            }
        }

        private void UpdateStamina()
        {
            staminaSlider.value = playerController.staminaValue / playerController.staminaMaxValue;
            if(!playerController.staminaOn) staminaSlider.gameObject.SetActive(false);
            else staminaSlider.gameObject.SetActive(true);
        }

        private void UpdateScore()
        {
            scoreText.text = $"Score: {playerController.score}";
            finishScore.text = $"Score: {playerController.score}";
        }

        public void ToMainMenu()
        {
            Time.timeScale = 1;
            if(gameManager != null) Destroy(gameManager.gameObject);
            SceneManager.LoadScene("MainMenu");
        }

        //Method to start new game from menu
        public void NewGame()
        {
            Time.timeScale = 1; //return time
            SceneManager.LoadScene(currentSceneIndex); //Reload scene
        }

        //Method calls pause from UI
        public void Pause()
        {
            ChangeScreen(ScreenState.Pause);
        }
        public void UnPause()
        {
            ChangeScreen(ScreenState.Game);
        }
        //Method for change screen
        public void ChangeScreen(ScreenState screenState)
        {
            switch (screenState)
            {
                case ScreenState.Game: //if game
                    //turn off other and turn on game
                    gameScreen.SetActive(true);
                    pauseScreen.SetActive(false);

                    //Disable pause
                    LevelManager.Instance.isPause = false;
                    //return normal time
                    Time.timeScale = 1;
                    break;
                case ScreenState.Pause:
                    gameScreen.SetActive(false);
                    pauseScreen.SetActive(true);

                    LevelManager.Instance.isPause = true;
                    Time.timeScale = 0;
                    break;
                case ScreenState.Lose:
                    loseScreen.SetActive(true);
                    Time.timeScale = 0;
                    break;
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}