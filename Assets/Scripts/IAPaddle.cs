using UnityEngine;

public class IAPaddle : MonoBehaviour
{
    [Header("References")]
    private Transform ballTransform;
    private BallBounceController ballBounce;

    [Header("Difficulty")]
    public float speed = 5f;
    public float maxError = 0.5f;
    public float deadZone = 0.05f; // menor para suavizar movimiento

    [Header("Bounds")]
    [SerializeField] private float topLimit = 4f;
    [SerializeField] private float bottomLimit = -2.05f;

    [Header("Error Timing")]
    [SerializeField] private float errorUpdateTime = 0.5f;

    private float currentError;
    private float errorTimer;

    void OnEnable()
    {
        BallGeneration.OnBallSpawned += SetBall;
    }

    void OnDisable()
    {
        BallGeneration.OnBallSpawned -= SetBall;
    }

    public void SetBall(GameObject ballInstance)
    {
        ballTransform = ballInstance.transform;
        ballBounce = ballInstance.GetComponent<BallBounceController>();
    }

    void Update()
    {
        if (ballTransform == null || ballBounce == null) return;

        // Actualizar error de la IA periódicamente
        errorTimer -= Time.deltaTime;
        if (errorTimer <= 0f)
        {
            currentError = Random.Range(-maxError, maxError);
            errorTimer = errorUpdateTime;
        }

        // Solo moverse si la bola se está acercando a este paddle
        float ballDirX = ballBounce.Direction.x;
        bool ballApproaching = (transform.position.x - ballTransform.position.x) * ballDirX >= 0f;

        if (ballApproaching)
        {
            float perceivedBallY = Mathf.Clamp(ballTransform.position.y + currentError, bottomLimit, topLimit);

            if (Mathf.Abs(transform.position.y - perceivedBallY) > deadZone)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, perceivedBallY, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }

        // Clamp siempre aplicado, sin importar si la bola se acerca o no
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}