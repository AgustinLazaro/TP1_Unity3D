using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AudioSource))]
public class CivilianBrain : MonoBehaviour
{
    public Transform target;
    public float scareDistance = 8f;
    public float walkRadius = 10f;

    [Header("Audio")]
    public AudioClip screamSound;
    public float screamCooldown = 5f;

    [HideInInspector] public Animator anim;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private ICivilianState currentState;
    private float nextScreamTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
    }

    void Start() => ChangeState(new CivilianIdleState());

    void Update() => currentState?.Update(this);

    public void ChangeState(ICivilianState newState)
    {
        currentState = newState;
        currentState.Enter(this);
    }

    public void PanicScream()
    {
        if (Time.time >= nextScreamTime && screamSound != null)
        {
            audioSource.PlayOneShot(screamSound);
            nextScreamTime = Time.time + screamCooldown;
        }
    }

    public void GoToRandomPoint()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * walkRadius;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, walkRadius, 1))
            agent.SetDestination(hit.position);
    }

    public bool HasReachedDestination() => !agent.pathPending && agent.remainingDistance < 0.5f;
    public bool IsPlayerNear() => Vector3.Distance(transform.position, target.position) < scareDistance;
    public void StopMovement() => agent.SetDestination(transform.position);

    public void LookAt(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<DroneController>() != null)
        {
            GameManager.instance?.CivilianRescued();
            Destroy(gameObject);
        }
    }
}