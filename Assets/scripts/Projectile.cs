using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    //private float currentLifeTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
