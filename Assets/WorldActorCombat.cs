using GameCreator.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldActorCombat : MonoBehaviour
{       
    private GameObject _target;
    private TargetingManager _targetingManager;
    private WorldActorMovement _worldActorMovement;
    private SphereCollider _adjacentCollider;
    private SphereCollider _nearbyCollider;
    private SphereCollider _areaCollider;
    private float _visionRadius = 50f;
    private bool _engaged;

    // Start is called before the first frame update
    void Start()
    {
        _targetingManager = GameObject.FindGameObjectWithTag("|TargetingManager|").GetComponent<TargetingManager>();

        _worldActorMovement = GetComponent<WorldActorMovement>();

        CreateColliders();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null && _engaged)
        {

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _targetingManager.Cancel();
            _engaged = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject target = null;// _targetingManager.GetCurrentlyTargeted();
            if (target != null)
            {
                _worldActorMovement.SetInput(PlayerCharacter.INPUT_TYPE.PointAndClick);
                _target = target;
                _engaged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _engaged = false;
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _visionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag.Contains("|Enemy|"))
                {
                    _targetingManager.Target2(hitColliders[i].gameObject);
                }
            }
        }
    }

    #region PrivatePropertyWrappers

    #region Colliders
    public float GetAdjacentColliderRadius()
    {
        return _adjacentCollider.radius;
    }
    #endregion

    #region Misc
    public GameObject GetTarget()
    {
        return _target;
    }

    public void SetTarget(GameObject gameObject)
    {
        _target = gameObject;
    }

    public bool GetEngaged()
    {
        return _engaged;
    }

    public void SetEngaged(bool isEngaged)
    {
        _engaged = isEngaged;
    }
    #endregion
    #endregion

    private void CreateColliders()
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
}
