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

        [Header("Level 2")]
        [SerializeField] private Text level_2_difficultText;

        [Header("Battery")]
        [SerializeField] private Slider batterySlider;
        [SerializeField] private GameObject computerImage;

        public bool currentLevelIsHard;

        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
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
            if(level_1_difficultText.text.Equals("Normal"))
            {
                level_1_difficultText.text = "Hard";
            }
        }

        public void Level_1_LessDifficult()
        {
           if(level_1_difficultText.text.Equals("Hard"))
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

            if(level_1_difficultText.text.Equals("Normal"))
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
            if(level_2_difficultText.text.Equals("Normal"))
            {
                level_2_difficultText.text = "Hard";
            }
        }

        public void Level_2_LessDifficult()
        {
           if(level_2_difficultText.text.Equals("Hard"))
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

            if(level_2_difficultText.text.Equals("Normal"))
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