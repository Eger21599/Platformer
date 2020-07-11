using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField] private AudioClip clickSound;
        [SerializeField] private GameObject settingsButton;
        [SerializeField] private Sprite fullStar;

        [Header("Resources")]
        [SerializeField] private Image coins;   
        [SerializeField] private Text coinsText;
        [SerializeField] private float maxCoinsValue;
        [SerializeField] private Image iron;
        [SerializeField] private Text ironText;
        [SerializeField] private float maxIronValue;

        [Header("Panels")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject levelsPanel;
        [SerializeField] private GameObject shopPanel;

        [Header("Level panels")]
        [SerializeField] private GameObject mainLevelPanel;
        [SerializeField] private GameObject level_1_Panel;
        [SerializeField] private GameObject level_2_Panel;

        [Header("Level 1")]
        [SerializeField] private Text level_1_difficultText;
        [SerializeField] private Image Level_1_Normal_One_Star_Level_Complete;
        [SerializeField] private Image Level_1_Normal_Two_Star_Level_Complete;
        [SerializeField] private Image Level_1_Normal_Three_Star_Level_Complete;
        [SerializeField] private Image Level_1_Hard_One_Star_Level_Complete;
        [SerializeField] private Image Level_1_Hard_Two_Star_Level_Complete;
        [SerializeField] private Image Level_1_Hard_Three_Star_Level_Complete;

        [Header("Level 2")]
        [SerializeField] private Text level_2_difficultText;
        [SerializeField] private Button Level_2_Button;
        [SerializeField] private GameObject Level_2_Close_Panel;
        [SerializeField] private Image Level_2_Normal_One_Star_Level_Complete;
        [SerializeField] private Image Level_2_Normal_Two_Star_Level_Complete;
        [SerializeField] private Image Level_2_Normal_Three_Star_Level_Complete;
        [SerializeField] private Image Level_2_Hard_One_Star_Level_Complete;
        [SerializeField] private Image Level_2_Hard_Two_Star_Level_Complete;
        [SerializeField] private Image Level_2_Hard_Three_Star_Level_Complete;

        [Header("Battery")]
        [SerializeField] private Slider batterySlider;
        [SerializeField] private GameObject computerImage;

        public bool currentLevelIsHard;

        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (PlayerPrefs.HasKey("isLevelOneFinished"))
            {
                Level_2_Button.enabled = true;
                Level_2_Close_Panel.SetActive(false);
            }
            else
            {
                Level_2_Button.enabled = false;
                Level_2_Close_Panel.SetActive(true);
            }

            Level_1_Stars_Manager();
            Level_2_Stars_Manager();
        }

        private void Update()
        {
            BatteryManager();

            coins.fillAmount = float.Parse(coinsText.text) / maxCoinsValue;
            iron.fillAmount = float.Parse(ironText.text) / maxIronValue;
        }

        private void BatteryManager()
        {
            if (SystemInfo.batteryLevel != -1) batterySlider.value = SystemInfo.batteryLevel;
            else
            {
                batterySlider.gameObject.SetActive(false);
                computerImage.SetActive(true);
            }
        }

        #region Level 1
        public void Level_1()
        {
            audioSource.PlayOneShot(clickSound);
            mainLevelPanel.SetActive(false);
            settingsButton.SetActive(false);
            level_1_Panel.SetActive(true);
        }

        public void Level_1_MoreDifficult()
        {
            if (level_1_difficultText.text.Equals("Normal"))
            {
                level_1_difficultText.text = "Hard";
            }
        }

        public void Level_1_LessDifficult()
        {
            if (level_1_difficultText.text.Equals("Hard"))
            {
                level_1_difficultText.text = "Normal";
            }
        }

        public void Back_Level_1()
        {
            audioSource.PlayOneShot(clickSound);
            mainLevelPanel.SetActive(true);
            settingsButton.SetActive(true);
            level_1_Panel.SetActive(false);
        }

        public void Load_Level_1()
        {
            audioSource.PlayOneShot(clickSound);

            if (level_1_difficultText.text.Equals("Normal"))
            {
                currentLevelIsHard = false;
                SceneManager.LoadScene("Level_1_Normal");
            }
            else
            {
                currentLevelIsHard = true;
                SceneManager.LoadScene("Level_1_Hard");
            }
        }

        private void Level_1_Stars_Manager()
        {
            if (PlayerPrefs.HasKey("Level_1_Normal_Score"))
            {
                if (PlayerPrefs.GetInt("Level_1_Normal_Score") >= 100 && PlayerPrefs.GetInt("Level_1_Normal_Score") < 140)
                {
                    Level_1_Normal_One_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_1_Normal_Score") >= 140 && PlayerPrefs.GetInt("Level_1_Normal_Score") < 190)
                {
                    Level_1_Normal_One_Star_Level_Complete.sprite = fullStar;
                    Level_1_Normal_Two_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_1_Normal_Score") >= 190)
                {
                    Level_1_Normal_One_Star_Level_Complete.sprite = fullStar;
                    Level_1_Normal_Two_Star_Level_Complete.sprite = fullStar;
                    Level_1_Normal_Three_Star_Level_Complete.sprite = fullStar;
                }
            }
            if (PlayerPrefs.HasKey("Level_1_Hard_Score"))
            {
                if (PlayerPrefs.GetInt("Level_1_Hard_Score") >= 190 && PlayerPrefs.GetInt("Level_1_Hard_Score") < 250)
                {
                    Level_1_Hard_One_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_1_Hard_Score") >= 250 && PlayerPrefs.GetInt("Level_1_Hard_Score") < 310)
                {
                    Level_1_Hard_One_Star_Level_Complete.sprite = fullStar;
                    Level_1_Hard_Two_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_1_Hard_Score") >= 310)
                {
                    Level_1_Hard_One_Star_Level_Complete.sprite = fullStar;
                    Level_1_Hard_Two_Star_Level_Complete.sprite = fullStar;
                    Level_1_Hard_Three_Star_Level_Complete.sprite = fullStar;
                }
            }
        }
        #endregion

        #region Level 2
        public void Level_2()
        {
            audioSource.PlayOneShot(clickSound);
            mainLevelPanel.SetActive(false);
            settingsButton.SetActive(false);
            level_2_Panel.SetActive(true);
        }

        public void Level_2_MoreDifficult()
        {
            if (level_2_difficultText.text.Equals("Normal"))
            {
                level_2_difficultText.text = "Hard";
            }
        }

        public void Level_2_LessDifficult()
        {
            if (level_2_difficultText.text.Equals("Hard"))
            {
                level_2_difficultText.text = "Normal";
            }
        }

        public void Back_Level_2()
        {
            audioSource.PlayOneShot(clickSound);
            mainLevelPanel.SetActive(true);
            settingsButton.SetActive(true);
            level_2_Panel.SetActive(false);
        }

        public void Load_Level_2()
        {
            audioSource.PlayOneShot(clickSound);

            if (level_2_difficultText.text.Equals("Normal"))
            {
                currentLevelIsHard = false;
                SceneManager.LoadScene("Level_2_Normal");
            }
            else
            {
                currentLevelIsHard = true;
                SceneManager.LoadScene("Level_2_Hard");
            }
        }

        private void Level_2_Stars_Manager()
        {
            if (PlayerPrefs.HasKey("Level_2_Normal_Score"))
            {
                if (PlayerPrefs.GetInt("Level_2_Normal_Score") >= 365 && PlayerPrefs.GetInt("Level_2_Normal_Score") < 410)
                {
                    Level_2_Normal_One_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_2_Normal_Score") >= 410 && PlayerPrefs.GetInt("Level_2_Normal_Score") < 490)
                {
                    Level_2_Normal_One_Star_Level_Complete.sprite = fullStar;
                    Level_2_Normal_Two_Star_Level_Complete.sprite = fullStar;
                }
                if (PlayerPrefs.GetInt("Level_2_Normal_Score") >= 490)
                {
                    Level_2_Normal_One_Star_Level_Complete.sprite = fullStar;
                    Level_2_Normal_Two_Star_Level_Complete.sprite = fullStar;
                    Level_2_Normal_Three_Star_Level_Complete.sprite = fullStar;
                }
            }
            /*if(PlayerPrefs.HasKey("Level_2_Hard_Score"))
            {
                if(PlayerPrefs.GetInt("Level_2_Hard_Score") >= 190 && PlayerPrefs.GetInt("Level_2_Hard_Score") < 250)
                {
                    Level_2_Hard_One_Star_Level_Complete.sprite = fullStar;
                }
                if(PlayerPrefs.GetInt("Level_2_Hard_Score") >= 250 && PlayerPrefs.GetInt("Level_2_Hard_Score") < 310)
                {
                    Level_2_Hard_One_Star_Level_Complete.sprite = fullStar;
                    Level_2_Hard_Two_Star_Level_Complete.sprite = fullStar;
                }
                if(PlayerPrefs.GetInt("Level_2_Hard_Score") >= 310)
                {
                    Level_2_Hard_One_Star_Level_Complete.sprite = fullStar;
                    Level_2_Hard_Two_Star_Level_Complete.sprite = fullStar;
                    Level_2_Hard_Three_Star_Level_Complete.sprite = fullStar;
                }
            }*/
        }
        #endregion

        public void Settings_On()
        {
            audioSource.PlayOneShot(clickSound);
            settingsPanel.SetActive(true);
            settingsButton.SetActive(false);
            levelsPanel.SetActive(false);
        }

        public void Settings_Off()
        {
            audioSource.PlayOneShot(clickSound);
            settingsPanel.SetActive(false);
            settingsButton.SetActive(true);
            levelsPanel.SetActive(true);
        }

        public void Shop_On()
        {
            audioSource.PlayOneShot(clickSound);
            shopPanel.SetActive(true);
        }

        public void Shop_Off()
        {
            shopPanel.SetActive(false);
        }
    }
}