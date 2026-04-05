using System.Data.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    public Transform target;
    public ParticleSystem hurtEffect;
    private NavMeshAgent agent;
    private Animator monsterAnimator;
    public Collider monsterCollider;
    public LayerMask targetLayer;

    public MonsterData data;
    public AudioClip hurtClip;
    public AudioClip deathClip;

    public float attackDistance;
    public float attackInterval = 1f;
    private float lastAttackTime;
    public float spawnInterval;

    private bool isSinking;
    public float sinkSpeed;

    private float damage;
    public int score {get; private set;}    

    private Status currentStatus;
    private AudioSource monsterAudioSource;

    public Status CurrentStatus
    {
        get {return currentStatus;}
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.Idle:
                    monsterAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    monsterAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    monsterAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Die:
                    monsterAnimator.SetTrigger("Die");
                    agent.isStopped = true;
                    agent.enabled = false;
                    monsterCollider.enabled = false;
                    monsterAudioSource.PlayOneShot(deathClip);
                    break;
            }
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);

        startingHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
        spawnInterval = data.spawnInterval;
        attackDistance = data.attackDistance;
        score = data.score;

        gameObject.SetActive(true);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sinkSpeed = 0.5f;
        monsterAnimator = GetComponent<Animator>();
        monsterAudioSource = GetComponent<AudioSource>();
        monsterCollider = GetComponent<Collider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        agent.isStopped = false;
        isSinking = false;
        agent.ResetPath();
        
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        monsterCollider.enabled = true;

        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        if (isSinking)
        {
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
        }

        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (target != null)
        {
            CurrentStatus = Status.Trace;
            return;
        }
    }

    private void UpdateTrace()
    {
        var LookAt = target.position;
        LookAt.y = transform.position.y;
        transform.LookAt(LookAt);

        if (Vector3.Distance(target.position, transform.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateAttack()
    {
        if (Vector3.Distance(target.position, transform.position) >= attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var LookAt = target.position;
        LookAt.y = transform.position.y;
        transform.LookAt(LookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            var LivingEntity = target.GetComponent<LivingEntity>();
            if (LivingEntity != null)
            {
                if (!LivingEntity.IsDead)
                {
                    LivingEntity.OnDamage(damage, transform.position, -transform.forward);
                }
            }
        }

        agent.SetDestination(target.position);
    }

    private void UpdateDie()
    {
        
    }

    public override void OnDamage(float damage, Vector3 hitpoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitpoint, hitNormal);

        monsterAudioSource.PlayOneShot(hurtClip);
        hurtEffect.transform.position = hitpoint;
        hurtEffect.transform.forward = hitNormal;
        hurtEffect.Play();
    }

    public override void Die()
    {
        CurrentStatus = Status.Die;
        base.Die();
    }

    private void StartSinking()
    {
        isSinking = true;
        Destroy(gameObject, 3f);
    }
}
