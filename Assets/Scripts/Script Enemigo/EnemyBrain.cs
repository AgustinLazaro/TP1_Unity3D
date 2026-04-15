using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(AudioSource))]
public class EnemyBrain : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Transform[] patrolPoints;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Settings")]
    public float visionRange = 15f;
    public float attackRange = 8f;
    public float fireRate = 1.5f;
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float walkRadius = 15f;

    [Header("Audio")]
    public AudioClip alertSound;
    public AudioClip fireSound;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;

    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;

    private StateBase currentState;
    private AudioSource audioSource;
    private float nextFireTime;
    private bool hasAlerted = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
    }

    void Start()
    {
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackState = new AttackState();

        patrolState.Initialize(this, anim, target);
        chaseState.Initialize(this, anim, target);
        attackState.Initialize(this, anim, target);

        ChangeState(patrolState);
    }

    void Update() => currentState?.Update();

    public void ChangeState(StateBase newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();

        if (newState == chaseState || newState == attackState)
        {
            if (!hasAlerted && alertSound != null)
            {
                audioSource.PlayOneShot(alertSound);
                hasAlerted = true;
            }
        }
        else hasAlerted = false;

        currentState = newState;
        currentState.Enter();
    }

    public void FireWeapon()
    {
        if (Time.time >= nextFireTime && target != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

            if (fireSound != null) audioSource.PlayOneShot(fireSound);
            nextFireTime = Time.time + fireRate;
        }
    }

    public void RotateTowardsTarget(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}