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
        // La bala només necessita una força per afegir una vegada, ja que no tenen arrossegament per fer que deixin de moure's
        rb.AddForce(direction * speed);

        // Destroy the bullet after it reaches it max lifetime
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destrueix la bala tan bon punt xoqui amb qualsevol cosa
        Destroy(gameObject);
    }

}
