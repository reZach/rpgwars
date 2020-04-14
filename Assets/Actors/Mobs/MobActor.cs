using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MobActor : BaseActor
{
    private float AggroRadius = 10f;
    private float MaxAggroRadius = 15f; // From StartPosition
    private bool AggroCooldown = false;
    private float AggroCooldownRadius = 3f;
    private float StoppingDistance = 1.2f;
    private float WanderRadius = 11f;
    private float MoveTimer = 0f;
    private float MoveTimerThreshold = 5f;
    private byte ResetWanderCounter = 0;
    private byte ResetWanderThreshold = 5;

    public Material DocileMaterial;
    public Material EngagedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActiveAndEnabled)
            return;

        // If aggro is not on cooldown, attempt to chase the player
        if (!AggroCooldown)
            AggroHumans();

        MoveTimer += Time.deltaTime;

        // If we are engaged with a target
        if (_target != null)
        {
            Vector3 newPosition = Vector3.zero;

            // If we are still within max aggro range, continue to follow target
            if (Vector3.Distance(this.transform.position, _startPosition) <= MaxAggroRadius &&
                !AggroCooldown)
            {
                Debug.Log(Vector3.Distance(this.transform.position, _target.transform.position) > _adjacentCollider.radius + _radiusFix);
                // Continue to follow player until stopping distance threshold has been reached                
                if (Vector3.Distance(this.transform.position, _target.transform.position) > _adjacentCollider.radius + _radiusFix)
                {
                    newPosition = _target.transform.position;
                    this.GetComponent<MeshRenderer>().material = EngagedMaterial;
                }
                else
                {
                    // Stop mob from moving once it is engaged (within range) of target
                    _navMeshAgent.SetDestination(this.transform.position); 
                    AggroCooldown = false;
                    return;
                }
            }
            else
            {
                newPosition = RandomNavSphere(_startPosition, WanderRadius, -1);
                _target = null;
                AggroCooldown = true;
                this.GetComponent<MeshRenderer>().material = DocileMaterial;
            }

            if (newPosition != Vector3.zero)
                _navMeshAgent.SetDestination(newPosition);
        }
        else if (MoveTimer > MoveTimerThreshold)
        {
            ResetWanderCounter++;

            Vector3 newPosition;
            if (ResetWanderCounter < ResetWanderThreshold)
            {
                newPosition = RandomNavSphere(_startPosition, WanderRadius, -1);
                MoveTimer = 0;
            }
            else
            {
                newPosition = _startPosition;
                ResetWanderCounter = 0;
                MoveTimer = -5f; // Wait a little bit longer after returning to StartPosition to move again
            }

            _navMeshAgent.SetDestination(newPosition);
        }

        // Reset aggro
        if (Vector3.Distance(this.transform.position, _startPosition) <= AggroCooldownRadius)
            AggroCooldown = false;
    }

    public void DealDamage(ushort damage)
    {
        if (Health - damage <= 0)
        {
            Die();
            return;
        }

        Health -= damage;
    }

    /// <summary>
    /// Target any humans who get within our aggro range
    /// </summary>
    private void AggroHumans()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, AggroRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.tag.Contains("|Human|"))
            {
                _target = hitColliders[i].gameObject.GetComponent<BaseActor>();
            }
        }
    }

    /// <summary>
    /// Gets a random point on a given NavMesh
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="distance"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}
