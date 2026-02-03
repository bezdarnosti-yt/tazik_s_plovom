using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настройки врага")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float gravity = 20f;
    
    private Transform player;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        ChasePlayer();
    }
    
    private void ChasePlayer()
    {
        if (player == null) return;
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        
        moveDirection.x = direction.x * moveSpeed;
        moveDirection.z = direction.z * moveSpeed;
        
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        
        if (direction != Vector3.zero)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
    
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        FindFirstObjectByType<EnemySpawner>()?.OnEnemyDied();
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(25f);
            Destroy(other.gameObject);
        }
    }
}