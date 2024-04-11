using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 500f;
    public float maxLifetime = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction)
    {
        // La bala nom�s necessita una for�a per afegir una vegada, ja que no tenen arrossegament per fer que deixin de moure's
        rb.AddForce(direction * speed);

        // Destrueix la bala despres de recorre la distancia maxima
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid")){
            // Destrueix la bala tan bon punt xoqui amb qualsevol cosa
            Destroy(gameObject);
        }
    }

}
