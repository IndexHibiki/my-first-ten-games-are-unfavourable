using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Basic Status")]
    [SerializeField] float forwardSpeed = 5f;
    [SerializeField] float backSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int canJumpTimes = 2;
    [SerializeField] List<Transform> groundCheckPoints;

    int jumpTime;

    Vector2 moveInput = Vector2.zero;
    Rigidbody2D rigid;
    NinjaGirlAnimeChange ninjaGirlAnimeChange;
    Shooter shooter;

    Vector2 minBound;
    Vector2 maxBound;
    [SerializeField] Vector2 paddingLeftBottom;

    [Header("Kawarimi no Jutsu")]
    [SerializeField] float pushDistance = 10f;
    [SerializeField] GameObject kawarimi;
    [SerializeField] public float skillCD = 5f;
    [HideInInspector] public float currentSkillCD = 0f;
    bool isSkillCD = false;
    [SerializeField] float objExistTime = 3f;
    [SerializeField] ParticleSystem psSuddenSmoke;
    [SerializeField] Transform transParent;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ninjaGirlAnimeChange = GetComponent<NinjaGirlAnimeChange>();
        shooter = GetComponent<Shooter>();

        jumpTime = canJumpTimes;

        initWorldBounds();

        ninjaGirlAnimeChange.AnimateRun();
    }

    private void FixedUpdate()
    {
        Move();
        UpdateAnimationState();
        CheckInScreen();
    }

    void Move()
    {
        transform.position += (Vector3)moveInput * (moveInput.x >= 0 ? forwardSpeed : backSpeed) * Time.deltaTime;
    }

    void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    void OnJump(InputValue input)
    {
        if (input.isPressed && jumpTime > 0)
        {
            jumpTime--;
            rigid.velocity = new Vector2(0f, jumpPower);
        }
    }

    void OnShoot(InputValue input)
    {
        shooter.isShooting = input.isPressed;
    }

    void OnSkill1(InputValue input)
    {
        if(input.isPressed && isSkillCD == false)
        {
            isSkillCD = true;
            StartCoroutine(SkillCDTimer());

            GameObject obj = Instantiate(kawarimi, transform.position, Quaternion.identity, transParent);
            ParticleSystem ps = Instantiate(psSuddenSmoke, transform.position, Quaternion.identity, transParent);

            if (moveInput.x >= 0f)
            {
                transform.position += Vector3.right * pushDistance;
            }
            else
            {
                transform.position += Vector3.left * pushDistance;
            }

            Destroy(obj, objExistTime);
            Destroy(ps, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }

    IEnumerator SkillCDTimer()
    {
        currentSkillCD = skillCD;

        while(currentSkillCD > float.Epsilon)
        {
            currentSkillCD -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        isSkillCD = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsOnTheGround() == true)
        {
            jumpTime = canJumpTimes;
        }
    }

    bool IsOnTheGround()
    {
        bool grounded = false;
        Color rayColor;

        for(int i = 0; i < groundCheckPoints.Count; i++)
        {
            RaycastHit2D result = Physics2D.Linecast(
                transform.position, 
                groundCheckPoints[i].position, 
                1 << LayerMask.NameToLayer("Ground"));

            if (result)
            {
                grounded = true;
                rayColor = Color.green;
            }
            else
            {
                grounded = false;
                rayColor = Color.red;
            }

            Debug.DrawLine(transform.position, groundCheckPoints[i].position, rayColor);
        }

        return grounded;
    }

    void UpdateAnimationState()
    {
        if(IsOnTheGround() == false)
        {
            ninjaGirlAnimeChange.AnimateJump();
        }
        else if(shooter.isShooting == true)
        {
            ninjaGirlAnimeChange.AnimateShoot();
        }
        else
        {
            ninjaGirlAnimeChange.AnimateRun();
        }
    }

    void initWorldBounds()
    {
        Camera camera = Camera.main;

        minBound = camera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBound = camera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void CheckInScreen()
    {
        if(transform.position.x < minBound.x - paddingLeftBottom.x || transform.position.y < minBound.y - paddingLeftBottom.y)
        {
            Debug.Log("Out of Bound");
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);

        GamingController.Instance.OnLose();
    }
}
