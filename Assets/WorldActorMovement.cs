using GameCreator.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerCharacter))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class WorldActorMovement : MonoBehaviour
{
    public bool IsPlayerControlled;

    private WorldActorCombat _worldActorCombat;
    private PlayerCharacter _playerCharacter;
    private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _worldActorCombat = GetComponent<WorldActorCombat>();
        _playerCharacter = GetComponent<PlayerCharacter>();
        _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PrivatePropertyWrappers

    #region PlayerCharacter
    public void SetInput(PlayerCharacter.INPUT_TYPE newInputType)
    {
        _playerCharacter.UpdateLocomotion(newInputType);
    }
    #endregion    
    #endregion


    private void ControlByPlayer()
    {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            _playerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
            _worldActorCombat.SetEngaged(false);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 terrainPosition = _playerCharacter.GetTerrainPositionFromMouseClick(mousePosition);

            // Don't move if we can't navigate there through the NavMeshAgent
            if (terrainPosition != Vector3.zero)
            {
                NavMeshPath newPath = new NavMeshPath();
                _navMeshAgent.CalculatePath(terrainPosition, newPath);
                if (newPath.status == NavMeshPathStatus.PathComplete)
                {
                    _worldActorCombat.SetTarget(null);
                    _worldActorCombat.SetEngaged(false);
                    _playerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.PointAndClick);
                    _playerCharacter.SetInputPointTarget(terrainPosition);
                }
            }
        }
        else if (_worldActorCombat.GetTarget() != null)
        {
            // Continue to walk closer to target if not in range
            if (Vector3.Distance(this.transform.position, _worldActorCombat.GetTarget().transform.position) > _worldActorCombat.GetAdjacentColliderRadius())
            {
                _playerCharacter.SetInputPointTarget(_worldActorCombat.GetTarget().transform.position);
            }
            else
            {
                // Stop walking closer to target
                _playerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
                //if (HoldingItem != null && Engaged)
                //{
                //    HoldingItem.CanAttack = true;

                    //// Look towards targeted enemy
                    //Vector3 enemyDirection = (_target.transform.position - this.gameObject.transform.position).normalized;
                    //Quaternion lookRotation = Quaternion.LookRotation(enemyDirection);

                    //this.gameObject.transform.rotation = lookRotation;//Quaternion.Slerp(this.gameObject.transform.rotation, lookRotation, Time.deltaTime * 10f);
                //}
            }
        }

        

        // Stop attacking if we dis-engaged
        //if (HoldingItem != null && !Engaged)
        //    HoldingItem.CanAttack = false;
    }

    private void ControlByAI()
    {

    }
}
