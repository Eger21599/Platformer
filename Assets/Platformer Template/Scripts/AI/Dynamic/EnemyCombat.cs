using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyCombat : Combat, ICombat
    {
        [Header("Config")]
        public float damage;
        public EnemyConfig enemyConfig; //Config, drap and drop in inspector your config

        //Components
        private Rigidbody2D rigid2D;
        private AnimatorController animatorController;
        private EnemyController enemyController;

        [Header("Variables")]
        public float attackRateSpeed; //Rate of enemy attack
        private float attackRateTimer; //Rate local timer

        [HideInInspector] public bool isAttacking; //check status of attack

        private bool isAttackDetected;

        public override void Start()
        {
            base.Start(); //parent start method 

            //Get components
            rigid2D = GetComponentInParent<Rigidbody2D>();
            enemyController = GetComponentInParent<EnemyController>();
            animatorController = GetComponent<AnimatorController>();

            //Set enemy data from config
            //damageRange = new DoubleFloat(enemyConfig.damageRange.min, enemyConfig.damageRange.max);

            //add method hitInfo to enent, need for setup logic when enemy get hit
            hitInfo += HitDetected;
        }

        //Hit method
        public override void HitDetected()
        {
            if (ColliderDetected.gameObject.tag == "Player") //if is player
            {
                Stats playerStats = ColliderDetected.GetComponent<Stats>(); //Get data component from object
                PlayerController playerController = ColliderDetected.gameObject.GetComponent<PlayerController>();

                if(playerController != null && !playerController.inRoll)
                {
                    playerController.audioSource.PlayOneShot(playerController.enemyMeleeHitSound);
                }

                if(playerController != null && !playerController.inRoll)
                {
                    //Make visual hit effect
                    HitEffect hitEffect = ColliderDetected.GetComponentInChildren<HitEffect>();
                    hitEffect.PlayEffect();

                    float damage = damageRange.Random(); //get 1 random value damage of 2 (min,max)

                    MeleeAttack(playerStats, damage); //Damage

                    CameraManager.Instance.cameraShake.Shake(); //Camera shake
                }
            }
            else
            {
                if(enemyController != null)
                {
                    enemyController.audioSource.PlayOneShot(enemyController.attackSound);
                }
            }
        }

        //Method for animator, when enemy melee attack end
        public void OnMeleeAttackEnd()
        {
            isAttacking = false; //Attack status
            animatorController.SetBool("MeleeAttack", false); //Animator bool MeleeAttack = false
        }

        public void AttackForce(float forcePower)
        {
            // Here you can add any logic of the moment of impact, similar to that of a player.
        }

        public override void RangeAttack()
        {
            base.RangeAttack();
        }
        //Method for animator, when enemy range attack end
        public void OnRangeAttackEnd()
        {
            animatorController.SetBool("RangeAttack", false);
        }

        void Update()
        {
            if (isAttacking) 
            {
                if (attackRateTimer > 0) //attack rate timer
                {
                    attackRateTimer -= Time.deltaTime; //attack rate timer - 1 every frame(you can change Update to FixedUpdate for everysecond)
                }
                else
                {
                    attackRateTimer = attackRateSpeed; //set attack rate timer to attack rate time

                    animatorController.SetBool("MeleeAttack", true);
                }
            }
        }
    }
}