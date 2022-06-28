using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    [SerializeField] private float speed;
    private Vector2 direction;
    private LayerMask canCollideWith;

    private bool instantiated = false;
    private Rigidbody2D rigidBody;

    private void Update()
    {
        if (!instantiated) return;

        rigidBody.velocity = transform.up * Time.deltaTime * speed;
    }

    public void SpawnProjectile(Vector2 direction, float damage, float speed, LayerMask canCollideWith)
    {
        this.direction = direction;
        this.damage = damage;
        this.canCollideWith = canCollideWith;
        this.speed = speed;

        rigidBody = GetComponent<Rigidbody2D>();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        instantiated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCollideWith == (canCollideWith | (1 << collision.gameObject.layer)))
        {
            PlayerController receiver = collision.GetComponent<PlayerController>();
            if (receiver != null)
            {
                collision.GetComponent<PlayerController>().ReceiveDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}