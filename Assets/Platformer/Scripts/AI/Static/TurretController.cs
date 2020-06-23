using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(RangeWeapon), typeof(AudioSource), typeof(Stats))]
    public class TurretController : MonoBehaviour
    {

        [SerializeField] private float damage = 10f;
        [SerializeField] private int scorePoints = 15;
        [SerializeField] private AudioClip deathSound;

        public GameObject weaponPrefab; 
        public Transform shootStartPos; //weapon spawn position

        [SerializeField] private float shootRate;
        [SerializeField] private float arrowDestroyTime = 1f;
        private float shootRateTimer; //local rate timer

        private AudioSource soundManagerAudioSource;
        private AudioSource audioSource;
        private Stats stats;
        private PlayerController playerController;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            stats = GetComponent<Stats>();

            soundManagerAudioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
            stats.OnDeath += Death;
        }

        private void FixedUpdate()
        {
            Shoot();
        }

        //Shoot method
        private void Shoot()
        {
            if (shootRateTimer > 0)
            {
                shootRateTimer -= Time.deltaTime;
            }
            else
            {
                GameObject weapon = Instantiate(weaponPrefab, shootStartPos.position, shootStartPos.rotation); //Spawn weapon

                RangeWeapon rangeWeapon = weapon.GetComponent<RangeWeapon>(); //Get component
                rangeWeapon.destroyTime = arrowDestroyTime;
                rangeWeapon.fromPlayer = false; 
                rangeWeapon.enemyDamage = damage; //Get damage value

                shootRateTimer = shootRate; //set timer
            }
        }

        private void Death()
        {
            if(deathSound != null && soundManagerAudioSource != null) soundManagerAudioSource.PlayOneShot(deathSound);
            playerController.score += scorePoints;
            Destroy(gameObject);
        }
    }
}