using UnityEngine;

public class BallBounceController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float initialSpeed = 6f;
    [SerializeField] private float speedIncreasePerHit = 0.5f;
    [SerializeField] private float maxSpeed = 12f;

    [Header("Bounce Settings")]
    [SerializeField] private float maxBounceAngle = 75f;
    [SerializeField] private float minVerticalInfluence = 0.2f;

    private float currentSpeed;
    private Vector2 direction;
    public Vector2 Direction => direction;

    private Rigidbody2D rb;
    private float topBound;
    private float bottomBound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        topBound    = GameObject.FindWithTag("TopWall").GetComponent<Collider2D>().bounds.min.y;
        bottomBound = GameObject.FindWithTag("BottomWall").GetComponent<Collider2D>().bounds.max.y;
        ResetBall();
    }

    void FixedUpdate()
    {
        Vector2 newPos = rb.position + direction * currentSpeed * Time.fixedDeltaTime;

        if (newPos.y >= topBound)
        {
            newPos.y = 2f * topBound - newPos.y;
            direction.y = -Mathf.Abs(direction.y);
            direction = EnforceMinimumVertical(direction);
        }
        else if (newPos.y <= bottomBound)
        {
            newPos.y = 2f * bottomBound - newPos.y;
            direction.y = Mathf.Abs(direction.y);
            direction = EnforceMinimumVertical(direction);
        }

        rb.MovePosition(newPos);
    }

    public void ResetBall()
    {
        currentSpeed = initialSpeed;

        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.Range(-0.5f, 0.5f);

        direction = new Vector2(dirX, dirY).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IncreaseSpeed();
            BounceFromPaddle(collision.transform);
        }
    }

    void IncreaseSpeed()
    {
        currentSpeed += speedIncreasePerHit;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }

    void BounceFromPaddle(Transform paddle)
    {
        float hitOffset = transform.position.y - paddle.position.y;
        float paddleHeight = paddle.GetComponent<Collider2D>().bounds.size.y;

        float normalizedOffset = Mathf.Clamp(hitOffset / (paddleHeight / 2f), -1f, 1f);

        float bounceAngle = normalizedOffset * maxBounceAngle * Mathf.Deg2Rad;

        float dirX = transform.position.x < paddle.position.x ? -1f : 1f;

        direction = new Vector2(
            dirX * Mathf.Cos(bounceAngle),
            Mathf.Sin(bounceAngle)
        );

        direction = EnforceMinimumVertical(direction);
    }

    Vector2 EnforceMinimumVertical(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) < minVerticalInfluence)
        {
            dir.y = minVerticalInfluence * Mathf.Sign(dir.y == 0 ? 1 : dir.y);
        }

        return dir.normalized;
    }
}