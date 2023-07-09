using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 20.0f;
    [SerializeField] private float JumpForce = 700;
    [SerializeField] private GameObject Silhouette;
    [SerializeField] private float CoyoteTime = 0.1f;
    public float CoyoteCounter = 0;
    [SerializeField] private bool Jumped = false;
    private float distToGround;
    [SerializeField] private float distToGroundOffset = 0.1f;
    private new Collider2D collider;
    private Rigidbody2D rbody;
    private bool Grounded = true;
    private float ColExt;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rbody = GetComponent<Rigidbody2D>();
        distToGround = collider.bounds.extents.y + distToGroundOffset;
        ColExt = collider.bounds.extents.x;
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast((Vector2)transform.position + Vector2.left * (ColExt + 0.1f), Vector2.down, distToGround)
            || Physics2D.Raycast((Vector2)transform.position + Vector2.right * (ColExt + 0.1f), Vector2.down, distToGround);
    }
    void Update()
    {
        //Debug.DrawRay((Vector2)transform.position + Vector2.right * (ColExt + 0.1f), Vector2.down * distToGround, Color.red);
        //Debug.DrawRay((Vector2)transform.position + Vector2.left * (ColExt + 0.1f), Vector2.down * distToGround, Color.red);
        var dir = Input.GetAxis("Horizontal");
        if (dir != 0)
        {
            transform.position += (Vector3)Vector2.right * dir * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (!Jumped)
        {
            if (Grounded)
            {
                Jumped = true;
                rbody.velocity *= Vector2.right;
                rbody.AddForce(Vector2.up * JumpForce);
            }
            //test            
            Silhouette.transform.position = transform.position;
            //end test
        }
    }
    private void FixedUpdate()
    {
        Grounded = IsGrounded();
        if (Grounded)
        {
            CoyoteCounter = 0;
            Jumped = false;
        }
        else
        {
            if (CoyoteCounter < CoyoteTime && !Jumped)
            {
                CoyoteCounter += Time.deltaTime;
                Grounded = true;
            }
        }
    }
    private void Start()
    {
        //test
        //StartCoroutine(TestMove());
    }
    private IEnumerator TestMove()
    {
        bool CanJump = true;
        while (true)
        {
            transform.position += (Vector3)Vector2.right * Speed * Time.deltaTime;
            if (transform.position.x >= -2.5 && CanJump)
            {
                CanJump = false;
                Jump();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}