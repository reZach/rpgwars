using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseActor : MonoBehaviour
{
    protected NavMeshAgent _navMeshAgent;
    protected Vector3 _startPosition;
    protected BaseActor _target;
    private SphereCollider _adjacentCollider;
    private SphereCollider _nearbyCollider;
    private SphereCollider _areaCollider;
    protected TargetingManager _targetingManager;

    private ushort _health = 1;    
    private ushort _maxHealth = 1;    
    private byte _energy = 1;    
    private byte _maxEnergy = 1;

    public ushort StartingHealth = 1;
    public byte StartingEnergy = 1;
    public BaseHoldable HoldingItem;

    /// <summary>
    /// To be called in a child class's Start() method.
    /// </summary>
    protected void Initialize()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startPosition = this.transform.position;
        _targetingManager = GameObject.FindGameObjectWithTag("|TargetingManager|").GetComponent<TargetingManager>();
        HoldingItem = GetComponentInChildren<BaseHoldable>();

        MaxHealth = Health = StartingHealth;
        MaxEnergy = Energy = StartingEnergy;

        CreateColliders();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Stats

    public ushort Health
    {
        get
        {
            return _health;
        }

        protected set
        {
            _health = value;
        }
    }

    public ushort MaxHealth
    {
        get
        {
            return _maxHealth;
        }

        protected set
        {
            _maxHealth = value;
        }
    }

    public byte Energy
    {
        get
        {
            return _energy;
        }

        protected set
        {
            _energy = value;
        }
    }

    public byte MaxEnergy
    {
        get
        {
            return _maxEnergy;
        }

        protected set
        {
            _maxEnergy = value;
        }
    }
    #endregion

    #region Lifecycle

    /// <summary>
    /// A method that should be called when this actor is brought to life.
    /// </summary>
    protected virtual void Live()
    {
        CreateColliders();
    }

    /// <summary>
    /// A method that should be called when this actor dies.
    /// </summary>
    protected virtual void Die()
    {
        DeleteColliders();

        this.gameObject.SetActive(false);
        _targetingManager.Cancel(this.gameObject.GetInstanceID());
    }
    #endregion

    #region Colliders

    /// <summary>
    /// Creates colliders necessary for hit detection.
    /// </summary>
    protected virtual void CreateColliders()
    {
        _adjacentCollider = this.gameObject.AddComponent<SphereCollider>();
        _adjacentCollider.center = Vector3.zero;
        _adjacentCollider.radius = 1f;
        _adjacentCollider.isTrigger = true;

        _nearbyCollider = this.gameObject.AddComponent<SphereCollider>();
        _nearbyCollider.center = Vector3.zero;
        _nearbyCollider.radius = 2f;
        _nearbyCollider.isTrigger = true;

        _areaCollider = this.gameObject.AddComponent<SphereCollider>();
        _areaCollider.center = Vector3.zero;
        _areaCollider.radius = 4f;
        _areaCollider.isTrigger = true;
    }

    /// <summary>
    /// Destroys all colliders for hit detection.
    /// </summary>
    protected virtual void DeleteColliders()
    {
        Destroy(_adjacentCollider);
        Destroy(_nearbyCollider);
        Destroy(_areaCollider);
    }
    #endregion

    #region Engagement

    public void DealDamageToSingleTarget(ushort damage)
    {
        _target.ReceiveDamage(damage);
    }

    public void ReceiveDamage(ushort damage)
    {
        if (Health - damage <= 0)
        {
            Die();
            return;
        }

        Health -= damage;
    }
    #endregion    
}
