using System;
using UnityEngine;

public class BallGeneration : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;

    // Evento que avisa cuando la bola aparece
    public static Action<GameObject> OnBallSpawned;

    void Start()
    {
        GenerateBall();
    }

    public void GenerateBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        OnBallSpawned?.Invoke(ball);
    }
}
