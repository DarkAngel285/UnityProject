using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 100;
    public float speed = 5f;
    public float jumpPower = 13f;

    private int HP;
    private bool dead;
    private bool locked;
    private bool isGround;
    private bool doubleJump;

    public Slider progressbar;

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        HP = maxHP;
        progressbar.maxValue = maxHP;
        progressbar.value = HP;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Flip()
    {
        if (Input.GetKeyDown(left))
            sr.flipX = true;
        else if (Input.GetKeyDown(right))
            sr.flipX = false;
    }

    private void Move()
    {
        if (Input.GetKey(left))
            rb.velocity = new Vector2(-speed * (isGround ? 1f : 0.9f), rb.velocity.y);
        else if (Input.GetKey(right))
            rb.velocity = new Vector2(speed * (isGround ? 1f : 0.9f), rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jump))
            if (isGround || doubleJump)
            {
                doubleJump = isGround;
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
    }

    private void Die()
    {
        if (HP == 0 && !dead)
        {
            Lock();
            dead = true;
            anim.Play("Dead");
        }
    }

    public void Hit(int damage)
    {
        if (!dead)
        {
            Lock();
            HP = Mathf.Clamp(HP - damage, 0, maxHP);
            progressbar.value = HP;
            anim.Play("Hit");
        }
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    private void Animations()
    {
        if (!locked)
        {
            if (isGround)
            {
                if (rb.velocity.x != 0)
                    anim.Play("Run");
                else
                    anim.Play("Idle");
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    if (doubleJump)
                        anim.Play("Jump");
                    else
                        anim.Play("Double Jump");
                }
                else
                    anim.Play("Fall");
            }
        }
    }

    private void Update()
    {
        if (!dead)
        {
            Flip();
            Move();
            Jump();
            Die();
            Animations();
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            if (!locked && dead)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            isGround = true;
            doubleJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
            isGround = false;
    }
}
