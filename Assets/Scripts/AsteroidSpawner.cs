using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float spawnDistance = 12f;
    public float spawnRate = 1f;
    public int amountPerSpawn = 1;
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            // Trieu una direcció aleatòria des del centre del generador i genera l'asteroide a una distància
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);

            // Calcula una variància aleatòria en la rotació de l'asteroide que serà fa que canviï la seva trajectòria
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            // Creeu el nou asteroide clonant el prefabricat i establiu un aleatori mida dins del rang
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Estableix la trajectòria per moure's en la direcció del generador
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

}
