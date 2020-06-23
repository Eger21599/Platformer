using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    //This script for creating configs of enemy
    //You can create new config and drap to your enemy

    [CreateAssetMenu(fileName = "Enemy Config", menuName = "Enemy/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        //Your settings
        public bool isRedSkeleton;
        public float normalHP;
        public float hardHP;

        //[HideInInspector] public float HP;
        [HideInInspector] public DoubleFloat damageRange;

        private void Awake()
        {
            /*if(GameManager.isHard)
            {
                HP = hardHP;
            }
            else
            {
                HP = normalHP;
            }*/
        }
    }
}
