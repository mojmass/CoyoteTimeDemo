using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 20.0f;
    public float JumpForce = 700;
    public GameObject Silhouette;
    public float CoyoteTime = 0.1f;
    private float CoyoteCounter = 0;
    private bool Jumped = false;
    private float distToGround;
    public float distToGroundOffset = 0.1f;
    private new Collider2D collider;
    private Rigidbody2D rbody;
    private bool Wait = false;
    private int WaitCounter = 0;
    [SerializeField] private int WaitTimeFrames = 5;
    private bool isFalling = false;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rbody = GetComponent<Rigidbody2D>();
        distToGround = collider.bounds.extents.y + distToGroundOffset;
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast((Vector2)transform.position , Vector2.down, distToGround);
    }

    //private void Start()
    //{
    //    StartCoroutine(TestMove());
    //}
    // Update is called once per frame
    void Update()
    {
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
            if (IsGrounded())
            {
                CoyoteCounter = CoyoteTime + 1.0f;
                Wait = true;
                Jumped = true;
                rbody.AddForce(Vector2.up * JumpForce);
            }
            else if (!IsGrounded() && CoyoteCounter < CoyoteTime)
            {
                CoyoteCounter = CoyoteTime + 1.0f;
                Jumped = true;
                rbody.AddForce(Vector2.up * JumpForce);
            }
            //test            
            Silhouette.transform.position = transform.position;
            //end test
        }
    }
    private void FixedUpdate()
    {
        if (Wait)
        {
            WaitCounter++;
            if (WaitCounter == WaitTimeFrames)
            {
                Wait = false;
                WaitCounter = 0;
            }
        }
        else
        {
            if (!IsGrounded() && !Jumped)
            {
                if (CoyoteCounter < CoyoteTime)
                {
                    CoyoteCounter += Time.deltaTime;
                }
                isFalling = true;
            }
            if (isFalling || Jumped)
            {
                if (IsGrounded())
                {
                    Jumped = false;
                    isFalling = false;
                    CoyoteCounter = 0;
                }
            }
        }
    }
    //private IEnumerator TestMove()
    //{
    //    bool CanJump = true;
    //    while (true)
    //    {
    //        transform.position += (Vector3)Vector2.right * Speed * Time.deltaTime;
    //        if (transform.position.x >= -3 && CanJump)
    //        {
    //            CanJump = false;
    //            Jump();
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }        
    //}
}
