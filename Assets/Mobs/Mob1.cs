using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Mob1 : MonoBehaviour
{
    private NavMeshAgent Agent;
    private Vector3 StartPosition;
    private GameObject AggroTarget;
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

    public ushort Health = 100;
    public byte Energy = 40;
    public Material DocileMaterial;
    public Material EngagedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        StartPosition = this.transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        // If aggro is not on cooldown, attempt to chase the player
        if (!AggroCooldown)
            AggroHumans();

        MoveTimer += Time.deltaTime;

        // If we are engaged with a target
        if (AggroTarget != null)
        {
            Vector3 newPosition = Vector3.zero;

            // If we are still within max aggro range, continue to follow target
            if (Vector3.Distance(this.transform.position, StartPosition) <= MaxAggroRadius && 
                !AggroCooldown)
            {
                // Continue to follow player until stopping distance threshold has been reached                
                if (Vector3.Distance(this.transform.position, AggroTarget.transform.position) > StoppingDistance)
                {
                    newPosition = AggroTarget.transform.position;
                    this.GetComponent<MeshRenderer>().material = EngagedMaterial;
                }
                else
                {
                    // Stop mob from moving once it is engaged (within range) of target
                    Agent.SetDestination(this.transform.position);                    
                    return;
                }
            }
            else
            {
                newPosition = RandomNavSphere(StartPosition, WanderRadius, -1);
                AggroTarget = null;
                AggroCooldown = true;
                this.GetComponent<MeshRenderer>().material = DocileMaterial;
            }                

            if (newPosition != Vector3.zero)
                Agent.SetDestination(newPosition);
        }
        else if (MoveTimer > MoveTimerThreshold)
        {
            ResetWanderCounter++;

            Vector3 newPosition;
            if (ResetWanderCounter < ResetWanderThreshold)
            {
                newPosition = RandomNavSphere(StartPosition, WanderRadius, -1);
                MoveTimer = 0;
            }                
            else
            {
                newPosition = StartPosition;
                ResetWanderCounter = 0;
                MoveTimer = -5f; // Wait a little bit longer after returning to StartPosition to move again
            }

            Agent.SetDestination(newPosition);            
        }

        // Reset aggro
        if (Vector3.Distance(this.transform.position, StartPosition) <= AggroCooldownRadius)
            AggroCooldown = false;
    }

    public void DealDamage(ushort damage)
    {
        if (Health - damage <= 0)
        {
            this.gameObject.SetActive(false);
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
                AggroTarget = hitColliders[i].gameObject;
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
