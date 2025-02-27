using System;
using System.ComponentModel.Design.Serialization;
using KBCore.Refs;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance;
    public float speed=1f;
    public float swimForce=5f;
    public float maxVerticalSpeed = 5f;
    public float speedMultipplier = 2f;
    [SerializeField] private float velocitySmoothingFactor = .5f; // 0f for no movement, 1f for faster acceleration
    [SerializeField] GameObject forwardLight, backwardLight, bubbleEffect;
    [SerializeField, Self] Rigidbody2D body;
    [SerializeField, Child] SpriteRenderer playerSprite;
    [SerializeField] Animator spriteAnimator, lightAnimator; 
    private float horizontalInput;
    private bool jumping = false;
    

    public float swimCooldown = 0.5f;
    public float lastSwimTime = -Mathf.Infinity;

    public int maxJump = 3;
    public float jumpRestartTimer = 0f;
    public bool inBubble = false;
    public int jumpcounter = 0;
    private bool jumpCooldown = false;

    public GameObject Bubble;

    //sound
    [SerializeField] private AudioClip swimsound;
    private AudioSource audioSource; 
    void OnValidate() {
        this.ValidateRefs();
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        // horizontal movement 
        horizontalInput = Input.GetAxis("Horizontal");
        

        
        
        // Flip sprite depending on way player is moving
        if (horizontalInput != 0) {
            playerSprite.flipX = horizontalInput < 0;
            backwardLight.SetActive(playerSprite.flipX);
            forwardLight.SetActive(!backwardLight.activeSelf);
            spriteAnimator.Play("walk");
        } else if (!jumping) {
            spriteAnimator.Play("Idle");
        }
        
        
        // prevent spamming that lauch the player into mar 
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(inBubble || (Time.time - lastSwimTime >= swimCooldown && jumpcounter <= maxJump && !jumpCooldown))
            {
                lightAnimator.Play("blinkonce");
                jumping = true;
                jumpcounter++;
                SwimUp();
                spriteAnimator.Play("Jump");
                lastSwimTime = Time.time;
                Invoke(nameof(ResetJump), swimCooldown);
                CancelInvoke(nameof(ResetJumpCounter));
                Invoke(nameof(ResetJumpCounter), 3f);
                
                if(jumpcounter>=maxJump)
                {
                    jumpCooldown = true;
                    
                }
            }
            
            
        } 
        if(inBubble == true)
        {
            maxJump = 999;
        }
        else
        {
            maxJump = 3;
        }
    }
    
    
   
    
    
    void FixedUpdate()
    {
        // swim faster and check if it being hold or not 
        float newSpeed = speed;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            newSpeed *= speedMultipplier;
        }
        float targetSpeed = horizontalInput * newSpeed;
        float smoothedSpeed = Mathf.Lerp(body.linearVelocity.x, targetSpeed, velocitySmoothingFactor);
        body.linearVelocity = new Vector2(smoothedSpeed, body.linearVelocity.y);
    }
    private void SwimUp()
    {
        //reseting
        float newVerticalSpeed = swimForce;
        //cap the vertical movement
        newVerticalSpeed = Mathf.Clamp(newVerticalSpeed, -Mathf.Infinity, maxVerticalSpeed);
        // apply force
        body.linearVelocity = new Vector2(body.linearVelocity.x, newVerticalSpeed);
        audioSource.pitch = Random.Range(0.5f, 0.64f);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/swimstroke"));
    }

    private void ResetJump() {
        jumping = false;
    }

    private void ResetJumpCounter()
    {
        lightAnimator.Play("blinktwice");
        jumpcounter = 0;
        jumpCooldown = false;
    }

    public void ActivateBubble() {
        body.gravityScale = 0f;
        bubbleEffect.SetActive(true); 
        Invoke(nameof(PopBubble), 10f);
        
    }

    public void PopBubble() {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/bubblepop"));
        body.gravityScale = .3f;
        inBubble = false;
        CancelInvoke(nameof(PopBubble));
        audioSource.pitch = Random.Range(0.7f, 0.8f);
        bubbleEffect.GetComponent<Animator>().Play("bubblepop");
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 3) { // terrain layer
            if (bubbleEffect.activeSelf) {
                PopBubble();
            }
        }
    }




}
