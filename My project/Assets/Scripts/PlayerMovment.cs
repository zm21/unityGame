using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class PlayerMovment : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private float moveSpeed = 7f;
    
    [SerializeField]
    private float jumpForce = 14f;
    private float dirX;

    private Animator animator;
    private enum MovementState { idle, running, jumping, falling };


    [SerializeField]
    private LayerMask jumbleGround;

    [SerializeField]
    private Text timerText;
    private DateTime gameTime { get; set; }
    private Timer _timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        gameTime = new DateTime();
        _timer = new Timer(TimerCallback, this, 0, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX*moveSpeed, rb.velocity.y);
        if(Input.GetButtonDown("Jump") && isOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimatorState();
    }

    private void TimerCallback(object o)
    {
        gameTime = gameTime.AddSeconds(1);
        Debug.Log(gameTime.Second);
        timerText.text = $"{gameTime.Minute}:{gameTime.Second}";
    }

    void UpdateAnimatorState()
    {
        MovementState state;
        if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }


        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }

    bool isOnGround()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            Vector2.down, .1f, jumbleGround);
    }

    public void Move()
    {
        dirX = 1;
        UpdateAnimatorState();
    }
}
