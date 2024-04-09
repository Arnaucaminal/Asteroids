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
        // Assigna propietats aleat�ries perqu� cada asteroide se senti �nic
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Estableix l'escala i la massa de l'asteroide en funci� de la mida assignada la f�sica �s m�s realista
        transform.localScale = Vector3.one * size;
        rb.mass = size;

        // Destrueix l'asteroide despr�s que arribi a la seva vida �til m�xima
        Destroy(gameObject, maxLifetime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        // L'asteroide nom�s necessita una for�a per afegir una vegada, ja que no en tenen arrossegueu perqu� deixin de moure's
        rb.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Comproveu si l'asteroide �s prou gran com per dividir-se per la meitat (les dues parts han de ser m�s grans que la mida m�nima)
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            GameManager.Instance.OnAsteroidDestroyed(this);

            // Destrueix l'asteroide actual, ja que �s substitu�t per dos asteroides nous o prou petits per ser destru�ts per la bala
            Destroy(gameObject);
        }
    }

    private Asteroid CreateSplit()
    {
        // Estableix la nova posici� de l'asteroide perqu� sigui la mateixa que l'asteroide actual per� amb un lleuger despla�ament perqu� no apareguin l'un dins l'altre
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Crea el nou asteroide a la meitat de la mida del corrent
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;

        // Estableix una traject�ria aleat�ria
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }

}
