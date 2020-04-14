using GameCreator.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerActor : BaseActor
{
    private PlayerCharacter PlayerCharacter;
    private CharacterController CharacterController;
    private float TargetableRadius = 50f;

    private bool Engaged;
    private ushort XP = 0;
    
    public PlayerHealth PlayerHealth;
    public PlayerEnergy PlayerEnergy;

    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        PlayerCharacter = GetComponent<PlayerCharacter>();
        CharacterController = GetComponent<CharacterController>();

        PlayerHealth.Set(Health);
        PlayerEnergy.Set(Energy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
            Engaged = false;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            _target = null;
            Engaged = false;
            PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.PointAndClick);
            PlayerCharacter.SetTargetFromMouseClick(mousePosition);
        }
        else if (_target != null)
        {
            // Continue to walk closer to target if not in range
            if (Vector3.Distance(this.transform.position, _target.transform.position) > 1.3f)
            {
                PlayerCharacter.SetInputPointTarget(_target.transform.position);
            }
            else
            {
                // Stop walking closer to target
                PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
                if (HoldingItem != null && Engaged)
                    HoldingItem.CanAttack = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _targetingManager.Cancel();
            Engaged = false;
        } 
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            BaseActor target = _targetingManager.MoveTowards();            
            if (target != null)
            {
                PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.PointAndClick);
                _target = _targetingManager.MoveTowards();
                Engaged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Engaged = false;
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, TargetableRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag.Contains("|Enemy|"))
                {
                    _targetingManager.Target(hitColliders[i].gameObject.GetComponent<BaseActor>(), true);
                }
            }
        }

        // Stop attacking if we dis-engaged
        if (!Engaged && HoldingItem != null)
            HoldingItem.CanAttack = false;
    }
}
