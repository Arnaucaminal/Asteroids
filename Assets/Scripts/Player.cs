

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private Bullet bulletPrefab;

    public float thrustSpeed = 1f;
    private bool thrusting;
    public bool IsThrusting => thrusting;

    public float rotationSpeed = 0.1f;
    private float turnDirection;

    public float respawnDelay = 3f;
    public float respawnInvulnerability = 9f;

    public bool screenWrapping = true;
    private Bounds screenBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");

        // Desactiveu tots els límits si l'ajustament de pantalla est� habilitat
        for (int i = 0; i < boundaries.Length; i++)
        {
            boundaries[i].SetActive(!screenWrapping);
        }

        // Converteix els l�mits de l'espai de la pantalla en els l�mits de l'espai mundial
        screenBounds = new Bounds();
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));
    }

    private void CanviTag()
    {
        // Desactiveu les col�lisions durant uns segons despr�s de la generaci� per assegurar-vos que el jugador tingui prou temps per allunyar-se dels asteroides amb seguretat.
        TurnOffCollisions();
        Debug.Log("Respawn invulnerabilitat: " + respawnInvulnerability);
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up * thrustSpeed);
        }

        if (turnDirection != 0f)
        {
            rb.AddTorque(rotationSpeed * turnDirection);
        }

        if (screenWrapping)
        {
            ScreenWrap();
        }
    }

    private void ScreenWrap()
    {
        // Mou-te al costat oposat de la pantalla si el jugador supera els l�mits
        if (rb.position.x > screenBounds.max.x + 0.5f)
        {
            rb.position = new Vector2(screenBounds.min.x - 0.5f, rb.position.y);
        }
        else if (rb.position.x < screenBounds.min.x - 0.5f)
        {
            rb.position = new Vector2(screenBounds.max.x + 0.5f, rb.position.y);
        }
        else if (rb.position.y > screenBounds.max.y + 0.5f)
        {
            rb.position = new Vector2(rb.position.x, screenBounds.min.y - 0.5f);
        }
        else if (rb.position.y < screenBounds.min.y - 0.5f)
        {
            rb.position = new Vector2(rb.position.x, screenBounds.max.y + 0.5f);
        }
    }

    private void Shoot()
    {
        float distanceX = 0.5f;
        Vector3 posicionDesplazada = transform.position + transform.up * distanceX;
        Bullet bullet = Instantiate(bulletPrefab, posicionDesplazada, transform.rotation);
        bullet.Shoot(transform.up);
    }

    private void TurnOffCollisions()
    {
        gameObject.tag = "Ignore Collisions";
    }

    private void TurnOnCollisions()
    {
        gameObject.tag = "Player";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;

            CanviTag();
            //GameManager.Instance.OnPlayerDeath(this);
        }
    }

   

}