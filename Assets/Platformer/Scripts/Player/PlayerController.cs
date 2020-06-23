using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
    public class PlayerController : MonoBehaviour, IController
    {
        //Components
        private Stats stats;
        private AnimatorController animatorController;
        private PlayerCombat playerCombat;
        private UIManager uiManager;
        [HideInInspector] public AudioSource audioSource;

        public InputManager inputManager = new InputManager();

        [Header("Sounds")]
        public AudioClip bowSound;
        public AudioClip meleeSound;
        public AudioClip arrowHitSound;
        public AudioClip enemyMeleeHitSound;

        [Header("Variables")]
        public float moveSpeed;
        public float jumpForce;
        public int arrows = 24;
        public float staminaValue = 100f;
        public float staminaMaxValue = 100f;
        public float needStaminaForRoll = 20f;
        public float needStaminaForJump = 20f;
        public float needStaminaForPunch = 10f;
        public float rollForce;
        public float rollImmortalityTime = 1f;
        public float raycastDistance = 0.15f;
        [Tooltip("On before game start")] public bool staminaOn;

        private Transform playerSprite;
        [HideInInspector] public Rigidbody2D rigid2D { get; set; }

        [SerializeField] private bool isGrounded;
        [HideInInspector] public bool isAttacking { get; set; }

        [HideInInspector] public bool isGameEnd;
        [HideInInspector] public bool onLadder;
        private bool isClimping;
        [HideInInspector] public bool inRoll;
        private bool rolling;
        [HideInInspector] public int score;

        private float rollTimer;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            stats = GetComponent<Stats>();
            rigid2D = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            playerSprite = transform.GetChild(0);

            animatorController = GetComponentInChildren<AnimatorController>();

            inputManager.inputConfig.UpdateDictionary(); //

            //add Death event
            stats.OnDeath += Death;
        }

        public void FixedUpdate()
        {
            if (LevelManager.Instance.isGame && !LevelManager.Instance.isPause) //check Game status
            {
                Move();
                LadderClimb();
                GroundCheck();
            }
        }

        private void Update()
        {
            if (LevelManager.Instance.isGame && !LevelManager.Instance.isPause)
            {
                Stamina();
                Rotation();
                Jump();
                Roll();
                Attack();
                Animation();
            }
        }

        private void Stamina()
        {
            if(staminaOn)
            {
                staminaValue = Mathf.Clamp(staminaValue, 0, staminaMaxValue);
                staminaValue += 10 * Time.deltaTime;
            }
            else
            {
                needStaminaForJump = 0f;
                needStaminaForPunch = 0f;
                needStaminaForRoll = 0f;
            }
        }

        //Move method
        public void Move()
        {
            if (!isAttacking)
                transform.Translate(new Vector2(inputManager.Horizontal * moveSpeed * Time.deltaTime, 0));
        }

        //Rotation method
        public void Rotation()
        {
            if (inputManager.Horizontal != 0) //if player any horizontal side move
            {
                if (inputManager.Horizontal < 0)
                    playerSprite.localScale = new Vector3(-1, 1, 1);
                else
                    playerSprite.localScale = new Vector3(1, 1, 1);
            }

        }

        //Roll method
        private void Roll()
        {
            if (isGrounded && inputManager.Roll && !isAttacking && !isClimping && staminaValue >= needStaminaForRoll) //Check for availability rollback
            {
                if (inputManager.Horizontal != 0)
                {
                    rolling = true;

                    staminaValue -= needStaminaForRoll;
                    animatorController.SetTrigger("Roll");
                    rigid2D.velocity = Vector2.right * inputManager.Horizontal * rollForce;
                }
            }

            if(rolling)
            {
                rollTimer += Time.deltaTime;

                inRoll = true;

                if(rollTimer >= rollImmortalityTime)
                {
                    inRoll = false;
                    rollTimer = 0.0f;
                    rolling = false;
                }
            }
        }

        //Jump method
        private void Jump()
        {
            if (inputManager.Jump && isGrounded && !isAttacking && !isClimping && staminaValue >= needStaminaForJump)
            {
                staminaValue -= needStaminaForJump;
                rigid2D.velocity = Vector2.up * jumpForce;
                animatorController.SetTrigger("Jump");
            }
        }

        //Jump method for button
        public void Button_Jump()
        {
            if (isGrounded && !isAttacking && !isClimping)
            {
                rigid2D.velocity = Vector2.up * jumpForce;
                animatorController.SetTrigger("Jump");
            }
        }

        //Attack method
        public void Attack()
        {
            if (isGrounded && (inputManager.MeleeAttack || inputManager.RangeAttack) && !isAttacking && staminaValue >= needStaminaForPunch/* && !isClimping*/) 
            {
                if (inputManager.MeleeAttack)
                {
                    isAttacking = true;
                    animatorController.SetBool("MeleeAttack", isAttacking); //Set animator bool
                }
                else if (inputManager.RangeAttack && arrows > 0)
                {
                    isAttacking = true;
                    arrows--;
                    animatorController.SetBool("RangeAttack", isAttacking);
                    audioSource.PlayOneShot(bowSound);
                }
            }
        }

        //Animator method
        public void Animation()
        {
            if (!isClimping) //Lader check
                if (inputManager.Horizontal != 0)
                    animatorController.SetBool("Move", true);
                else
                    animatorController.SetBool("Move", false);
            else
            {
                animatorController.SetBool("Move", false);
            }

            animatorController.SetBool("isGrounded", isGrounded);
        }

        void LadderClimb()
        {
            if (onLadder) //check lader status
            {
                if (inputManager.Vertical != 0)
                {
                    isClimping = true; 
                    rigid2D.velocity = Vector2.up * inputManager.Vertical * moveSpeed; //move up or down
                }
                else
                {
                    rigid2D.velocity = Vector2.zero; 
                }

            }
            else //if leave ladder
            {
                isClimping = false;
            }

            if (onLadder)
                rigid2D.gravityScale = 0;
            else
                rigid2D.gravityScale = 1;
        }

        //Death event
        public void Death()
        {
            animatorController.SetTrigger("Death");
            LevelManager.Instance.GameOver(); //set game state to game over
        }

        //Check ground 
        void GroundCheck()
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance);

            if (raycastHit2D.collider != null)
            {
                if (Vector2.Distance(transform.position, raycastHit2D.point) <= raycastHit2D.distance) //if raycast 
                {
                    isGrounded = true; //is ground 
                    Debug.DrawLine(transform.position, raycastHit2D.point); //draw line only in editor
                }
            }
            else
            {
                isGrounded = false; 
            }

        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if(collision2D.gameObject.tag == "Finish")
            {
                Time.timeScale = 0;
                uiManager.finishScreen.SetActive(true);
                isGameEnd = true;
                this.enabled = false;
            }
        }

    }
}
