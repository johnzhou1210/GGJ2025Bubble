using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed=1f;
    public float swimForce=5f;
    public float maxVerticalSpeed = 5f;
    public float speedMultipplier = 2f;

    public Rigidbody2D body;
    private float horizontalInput;

    public float swimCooldown = 0.5f;
    public float lastSwimTime = -Mathf.Infinity; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // prevent spamming that lauch the player into mar 
        if(Input.GetKeyDown(KeyCode.W) && Time.time - lastSwimTime >= swimCooldown)
        {
            swimup();
            lastSwimTime = Time.time;
        }

        // horizontal movement 
        horizontalInput = Input.GetAxis("Horizontal");
    }
    void FixedUpdate()
    {
        // swim faster and check if it being hold or not 
        float newSpeed = speed;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            newSpeed *= speedMultipplier;
            Debug.Log("RuN");
        }
        float targetSpeed = horizontalInput * newSpeed;
        float smoothedSpeed = Mathf.Lerp(body.linearVelocity.x, targetSpeed, 0.1f);
        body.linearVelocity = new Vector2(smoothedSpeed, body.linearVelocity.y);
    }
    private void swimup()
    {
        //reseting
        float newVerticalSpeed = swimForce;
        //cap the vertical movement
        newVerticalSpeed = Mathf.Clamp(newVerticalSpeed, -Mathf.Infinity, maxVerticalSpeed);
        // apply force
        body.linearVelocity = new Vector2(body.linearVelocity.x, newVerticalSpeed);

    }
}
