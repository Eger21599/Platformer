using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class RangeWeapon : MonoBehaviour
    {
        [Header("Variables")]
        public float moveSpeed;
        [HideInInspector] public float playerDamage;
        [HideInInspector] public float enemyDamage;
        [HideInInspector] public float destroyTime = 1f;
        public bool fromPlayer;

        private void Start()
        {
            Destroy(gameObject, destroyTime); //Destroyed through Destroy(gameObject, TIME TO DESTROY)
        }

        public void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Player": //if player
                    if (!fromPlayer)
                    {
                        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

                        if(playerController != null && !playerController.inRoll) Damage(collision);

                        if(playerController != null && !playerController.inRoll)
                        {
                            playerController.audioSource.PlayOneShot(playerController.arrowHitSound);
                        }
                    }
                    break;
                case "Enemy":
                    if (fromPlayer)
                    {
                        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

                        Damage(collision);

                        if(enemyController != null)
                        {
                            enemyController.audioSource.PlayOneShot(enemyController.bowHitSound);
                        }

                        CameraManager.Instance.cameraShake.Shake(); //Camera shake effect
                    }
                    break;
            }
        }

        void Damage(Collider2D collision)
        {
            Stats collInfo = collision.GetComponent<Stats>();
            if(collision.gameObject.tag == "Enemy") collInfo.GetDamage(playerDamage);
            if(collision.gameObject.tag == "Player") collInfo.GetDamage(enemyDamage);

            //Hit visual effect
            HitEffect hitEffect = collision.GetComponentInChildren<HitEffect>();
            if(hitEffect != null) hitEffect.PlayEffect();

            Destroy(gameObject);
        }

        public void Move()
        {
            //Simple move, you can use Physics(rigidbody2d) but i and rigidbody2d are not best friends :)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

    }
}