using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveXDistance = 30f;
    [SerializeField] Vector2 moveRange;
    [SerializeField] float moveTime = 3f;
    float currentMoveTimer = 0f;

    [SerializeField] float jumpPower = 5f;
    [SerializeField] float slashDistance = 2f;

    Transform playerTransform;
    Animator animator;
    Coroutine currentCorourine;
    Rigidbody2D rigid;
    PathFinder pathFinder;
    GroundTileGenerator gtg;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        currentCorourine = null;
        rigid = GetComponent<Rigidbody2D>();
        pathFinder = FindObjectOfType<PathFinder>();
        gtg = FindObjectOfType<GroundTileGenerator>();
    }

    private void FixedUpdate()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            SwitchToNextState();
            HandleCurrentState();
        }
    }

    public enum EnemyStateType
    {
        Idle,
        Move,
        Attack,
        Jump
    }

    [System.Serializable]
    public class StateData
    {
        public EnemyStateType stateType;
        public float duration;
    }

    public StateData[] statesData;

    private EnemyStateType currentStateType;
    private float stateTimer;

    private void HandleCurrentState()
    {
        if(currentCorourine != null)
        {
            StopCoroutine(currentCorourine);
            currentCorourine = null;

            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isSlashing", false);
            animator.SetBool("isShooting", false);
        }

        switch (currentStateType)
        {
            case EnemyStateType.Idle:
                // Handle idle logic
                // Do nothing
                break;
            case EnemyStateType.Move:
                // Handle move logic
                currentCorourine = StartCoroutine(Move());
                break;
            case EnemyStateType.Attack:
                // Handle attack logic
                if(Vector3.Distance(transform.position, playerTransform.position) < slashDistance)
                {
                    currentCorourine = StartCoroutine(Slash());
                }
                else
                {
                    currentCorourine = StartCoroutine(Shoot());
                }
                break;
            case EnemyStateType.Jump:
                // Handle jump logic
                currentCorourine = StartCoroutine(Jump());
                break;
        }
    }

    private void SwitchToNextState()
    {
        // Implement a way to transition from one state to the next.
        // For simplicity, this just cycles through them in order.

/*        int nextStateIndex = ((int)currentStateType + 1) % System.Enum.GetValues(typeof(EnemyStateType)).Length;
        ChangeState((EnemyStateType)nextStateIndex);*/

        ChangeState((EnemyStateType)1);
    }

    private void ChangeState(EnemyStateType newState)
    {
        currentStateType = newState;
        stateTimer = GetStateDuration(newState);
    }

    private float GetStateDuration(EnemyStateType state)
    {
        foreach (var data in statesData)
        {
            if (data.stateType == state)
            {
                return data.duration;
            }
        }
        return 0f;
    }

    IEnumerator Move()
    {
        animator.SetBool("isRunning", true);

        //float targetX = transform.position.x + moveXDistance;
        //if(targetX > gtg.GetMovedOffsetX())
        List<Vector2Int> points = pathFinder.FindPath(transform.position, transform.position.x + 30);
        while(points == null)
        {
            yield return null;

            points = pathFinder.FindPath(transform.position, transform.position.x + 10);
        }

/*        Task<List<Vector2Int>> findPathTask = pathFinder.FindPathAsync(transform.position, transform.position.x + 10);

        while(findPathTask.IsCompleted == false)
        {
            yield return null;
        }

        List<Vector2Int> points = findPathTask.Result;*/

        int pointCount = 0;

        while(pointCount < points.Count)
        {
            Vector3 targetPosition = (Vector2)points[pointCount];
            float delta = moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);

            if(transform.position == targetPosition)
            {
                pointCount++;
            }
            yield return new WaitForFixedUpdate();
        }

        /*Vector2 moveDirection = new Vector2(
            Random.Range(-moveRange.x, moveRange.x),
            0)
            .normalized;

        while (true)
        {
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }*/
    }

    IEnumerator Slash()
    {
        animator.SetBool("isSlashing", true);

        yield return new WaitForFixedUpdate();
    }

    IEnumerator Shoot()
    {
        animator.SetBool("isShooting", true);

        yield return null;
    }

    IEnumerator Jump()
    {
        animator.SetBool("isJumping", true);

        Vector2 moveDirection = new Vector2(
            Random.Range(-moveRange.x, moveRange.x),
            Random.Range(0, moveRange.y))
            .normalized;

        yield return new WaitForFixedUpdate();

        while (true)
        {
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    /*    IEnumerator FiniteStateMachine()
        {
            while(true)
            {
                int stateIndex = (int)Random.Range(0f, 2.5f);

                switch (stateIndex)
                {
                    case 0:
                        Idle();
                        break;
                    case 1:
                        StartCoroutine(Run());
                        break;
                    case 2:
                        Attack();
                        break;
                    default:
                        break;
                }

                float waitTime = Random.Range(3, 6);

                yield return new WaitForSeconds(waitTime);
            }
        }

        void Idle()
        {

        }

        IEnumerator Run()
        {
            animator.SetBool("isRunning", true);

            Vector2 moveDirection = new Vector2(
                Random.Range(-moveRange.x, moveRange.x), 
                Random.Range(-moveRange.y, moveRange.y))
                .normalized;

            currentMoveTimer = 0f;

            while(currentMoveTimer < moveTime)
            {
                transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

                currentMoveTimer += Time.deltaTime;

                yield return new WaitForFixedUpdate();
            }

            animator.SetBool("isRunning", false);
        }

        IEnumerator Attack()
        {
            yield return null;
        }*/
}
