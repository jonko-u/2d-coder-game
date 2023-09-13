using UnityEngine;
public class PlayerController : MonoBehaviour
{ 

    private Rigidbody2D rb2D;
    private Vector3 velocity = Vector3.zero;
    private bool lookingAtRight = true;
    // Declare isOnFloor as a class-level variable.[
    [Header("Initial Position")]
    [SerializeField] private float initialX;
    [SerializeField] private float initialY;

    [Header("Limit")]
    [SerializeField] private float minY;

    [Header("Movement")]
    [SerializeField] private float speedMovement;
    [SerializeField] private float smoothMovement;
    [SerializeField] private bool isOnFloor = false;
    [Header("Jump")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private Vector2 sizeBox;

    [Header("Animation")]
    private Animator animator;
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal") * speedMovement;
        animator.SetFloat("Horizontal", Mathf.Abs(xMovement));
        // Check if the character is on the floor.
        isOnFloor = Physics2D.OverlapBox(floorCheck.position, sizeBox, 0f, floorLayer);

        // Handle movement.
        Vector3 speedTarget = new Vector2(xMovement, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, speedTarget, ref velocity, smoothMovement);

        // Flip the character when moving left or right.
        if (xMovement > 0 && !lookingAtRight)
        {
            TurnAround();
        }
        else if (xMovement < 0 && lookingAtRight)
        {
            TurnAround();
        }

        // Handle jumping.
        if (Input.GetButtonDown("Jump") && isOnFloor)
        {
            Jump();
        }
        // Check if the character's Y position is below minY.
        if (transform.position.y < minY)
        {
            ResetCharacterPosition();
        }
    }
   
    private void ResetCharacterPosition()
    {
        transform.position = new Vector3(initialX, initialY, transform.position.z);
        rb2D.velocity = Vector2.zero; // Reset the character's velocity.
    }

    private void Jump()
    {
        // Apply an upward force for jumping.
        rb2D.AddForce(new Vector2(0f, jumpStrength), ForceMode2D.Impulse);

        // Change the sprite to the jump sprite if assigned.
    
    }

    private void TurnAround()
    {
        lookingAtRight = !lookingAtRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(floorCheck.position, sizeBox);
    }
}
