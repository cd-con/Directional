using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rb;
    public Vector2 dir;
    public bool isStunned = false;
    private float continueTime;
    public ParticleSystem stunnParticles;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = PlayerController.Instance.playerTransform;
    }

    public void Stunn()
    {
        continueTime = Time.time + 5f;
        stunnParticles.Play();
        isStunned = true;
    }

    void FixedUpdate()
    {
        Debug.Log($"{this.name}, {target.position}, {transform.position}");
        dir = Vector3.Normalize(target.position - transform.position);
        if (!isStunned)
        {
            rb.MovePosition(rb.position + dir * 2f * Time.fixedDeltaTime);
        }
        if (Time.time > continueTime)
        {
            isStunned = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Impulse")
            if (PlayerController.Instance.fatalImpulse)
                Destroy(gameObject);
            else
                Stunn();
    }
}
    /*
    [SerializeField]
    private Rigidbody2D enemyRigidbody;
    [SerializeField]
    Vector2 movement;
    Vector2 movementTarget;
    [SerializeField]
    private Transform target;
    private int RaysToShoot = 36;

    private void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        movement = transform.up;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        float angle = 0;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector3 dir = new Vector3(transform.position.x + x, transform.position.y + y, 0);
            
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit))
            {
                if (hit.collider.tag != "Player")
                {
                    Debug.DrawLine(transform.position, dir, Color.red);
                    movementTarget = Vector2.MoveTowards(enemyRigidbody.position, hit.point - transform.position, 1f);
                }
                else
                {
                    Debug.DrawLine(transform.position, dir, Color.green);
                }
            }
            else
            {
                movementTarget = Vector2.MoveTowards(enemyRigidbody.position, target.position, 1f);
            }
        }
        enemyRigidbody.MovePosition(enemyRigidbody.position + movement * 3.5f * Time.fixedDeltaTime);
    }
    */
