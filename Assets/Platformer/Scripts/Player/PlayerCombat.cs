using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerCombat : Combat, ICombat
    {
        
        [SerializeField] private float force = 20f;

        //Components
        private Rigidbody2D rigid2D;
        private AnimatorController animatorController;
        private InputManager inputManager;
        private PlayerController playerController;

        private bool canCombo;

        public override void Start()
        {
            base.Start();

            playerController = GetComponentInParent<PlayerController>();
            rigid2D = GetComponentInParent<Rigidbody2D>();
            animatorController = GetComponent<AnimatorController>();

            inputManager = playerController.inputManager;

            //Add hit event
            hitInfo += HitDetected;
        }
        //Hit method
        public override void HitDetected()
        {
            if (ColliderDetected.gameObject.tag == "Enemy")//if is enemy
            {
                Stats enemyStats = ColliderDetected.GetComponent<Stats>(); //Get data component from object
                EnemyController enemyController = ColliderDetected.gameObject.GetComponent<EnemyController>();

                if(enemyController != null)
                {
                    enemyController.audioSource.PlayOneShot(enemyController.meleeHitSound);
                }
                                                                          
                //Make visual hit effect
                HitEffect hitEffect = ColliderDetected.GetComponentInChildren<HitEffect>();
                if(hitEffect != null) hitEffect.PlayEffect();

                float damage = damageRange.Random(); //get 1 random value damage of 2 (min,max)

                MeleeAttack(enemyStats, damage);

                CameraManager.Instance.cameraShake.Shake(); //Damage
            }
        }
        //Method for animator, when player melee attack begin
        public void OnMeleeAttackBegin(float timeToCombo)
        {
            playerController.staminaValue -= playerController.needStaminaForPunch;
            playerController.audioSource.PlayOneShot(playerController.meleeSound);
            StartCoroutine(ICombo(timeToCombo)); //Start combo system 
        }

        public void AttackForce(float forcePower)
        {
            rigid2D.AddForce(Vector2.right * transform.localScale.x * force); //Makes jerk when attacking
        }

        //Method for animator, when player melee attack end
        public void OnMeleeAttackEnd()
        {
            StopCoroutine(ICombo(0)); //Stop combo 

            //Animator update
            animatorController.ResetTrigger("AttackCombo");
            animatorController.SetBool("MeleeAttack", false);

            canCombo = false; //block combo
            playerController.isAttacking = false;
        }

        public void OnRangeAttackEnd()
        {
            animatorController.SetBool("RangeAttack", false);
            playerController.isAttacking = false;
        }

        //Combo system
        IEnumerator ICombo(float comboTimer)
        {
            canCombo = false;
            yield return new WaitForSeconds(comboTimer);
            canCombo = true;

            while (canCombo)
            {
                if (inputManager.MeleeAttack)
                {
                    canCombo = false;
                    animatorController.SetTrigger("AttackCombo");
                    StopCoroutine(ICombo(0));
                }
                yield return null;
            }
        }
    }
}
