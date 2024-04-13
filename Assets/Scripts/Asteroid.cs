using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] sprites;

    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxLifetime = 30f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Assigna propietats aleatòries perquè cada asteroide se senti únic
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Estableix l'escala i la massa de l'asteroide en funció de la mida assignada la física és més realista
        transform.localScale = Vector3.one * size;
        rb.mass = size;

        // Destrueix l'asteroide després que arribi a la seva vida útil màxima
        Destroy(gameObject, maxLifetime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        // L'asteroide només necessita una força per afegir una vegada, ja que no en tenen arrossegueu perquè deixin de moure's
        rb.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Comproveu si l'asteroide és prou gran com per dividir-se per la meitat (les dues parts han de ser més grans que la mida mínima)
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            GameManager.Instance.OnAsteroidDestroyed(this);

            // Destrueix l'asteroide actual, ja que és substituït per dos asteroides nous o prou petits per ser destruïts per la bala
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.deathPlayer();
        }

    }

    private Asteroid CreateSplit()
    {
        // Estableix la nova posició de l'asteroide perquè sigui la mateixa que l'asteroide actual però amb un lleuger desplaçament perquè no apareguin l'un dins l'altre
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Crea el nou asteroide a la meitat de la mida del corrent
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;

        // Estableix una trajectòria aleatòria
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }

}