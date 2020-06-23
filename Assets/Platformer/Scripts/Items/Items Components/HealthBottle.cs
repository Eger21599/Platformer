using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HealthBottle : PickUpItem
    {
        [Header("Variables")]
        public float HealthNumber;

        private void Start()
        {
            //Add event
            OnPickedUP += OnPickedEvent;
        }

        //Method for event when player enter in trigger
        void OnPickedEvent()
        {
            /////////////////////////////////////////////////Here your logic
            if (LevelManager.Instance.playerStats.statsData.currentHP < LevelManager.Instance.playerStats.statsData.maxHP)
            {
                LevelManager.Instance.playerStats.statsData.currentHP += HealthNumber;

                if (LevelManager.Instance.playerStats.statsData.currentHP > LevelManager.Instance.playerStats.statsData.maxHP)
                    LevelManager.Instance.playerStats.statsData.currentHP = LevelManager.Instance.playerStats.statsData.maxHP;

                UIManager.Instance.UpdateHP(LevelManager.Instance.playerStats.statsData.currentHP, LevelManager.Instance.playerStats.statsData.maxHP);

                Destroy(gameObject);
            }
            /////////////////////////////////////////////////
        }
    }
}