using System;
using KBCore.Refs;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed=1f;
    public float swimForce=5f;
    public float maxVerticalSpeed = 5f;
    public float speedMultipplier = 2f;
    [SerializeField] private float velocitySmoothingFactor = .5f; // 0f for no movement, 1f for faster acceleration
    [SerializeField] GameObject forwardLight, backwardLight;
    [SerializeField, Self] Rigidbody2D body;
    [SerializeField, Child] SpriteRenderer playerSprite;
    [SerializeField, Child] Animator animator; 
    private float horizontalInput;
    private bool jumping = false;

    public float swimCooldown = 0.5f;
    public float lastSwimTime = -Mathf.Infinity;

    void OnValidate() {
        this.ValidateRefs();
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
            animator.Play("walk");
        } else if (!jumping) {
            animator.Play("Idle");
        }
        
        
        // prevent spamming that lauch the player into mar 
        if(Input.GetKeyDown(KeyCode.W) && Time.time - lastSwimTime >= swimCooldown) {
            jumping = true;
            SwimUp();   
            animator.Play("Jump");
            lastSwimTime = Time.time;
            Invoke(nameof(ResetJump), swimCooldown);
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
    }

    private void ResetJump() {
        jumping = false;
    }
    
}
