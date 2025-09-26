
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem shotEffect;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float patrolRadius = 5f;
    private Vector3 spawnPosition;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private Animator animator;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        animator.SetBool("attack", false);

    }
    private void SearchWalkPoint()
    {
        // Generate a random point within patrolRadius from spawnPosition
        Vector3 randomPoint = spawnPosition + new Vector3(Random.Range(-patrolRadius, patrolRadius), 0f, Random.Range(-patrolRadius, patrolRadius));

        // Validate that the point is on the NavMesh
        float distanceFromSource = 1f;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, distanceFromSource, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 4f)
        {
            animator.SetBool("attack", true);
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath(); // prestaje pratit al nastavi ako se udaljin
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        //transform.LookAt(player);

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Apply a slight leftward offset
        Vector3 leftOffset = Quaternion.Euler(0, 20, 0) * directionToPlayer;

        // Rotate enemy to look slightly left of the player
        transform.rotation = Quaternion.LookRotation(leftOffset);

        if (!alreadyAttacked)
        {
            AudioManager.PlaySound(Sounds.Shot, 0.2f);
            shotEffect.Play();
            player.gameObject.GetComponent<PlayerHealth>().Damage(10);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Die()
    {
        if (agent)
            agent.enabled = false;

        animator.SetTrigger("death");

        float animTime = GetAnimationLength("newDeath");
        Destroy(gameObject, animTime);

        this.enabled = false;
    }

    private float GetAnimationLength(string clipName)
    {

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        return 0f;
    }
}
