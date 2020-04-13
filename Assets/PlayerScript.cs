using GameCreator.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private NavMeshAgent Agent;
    private PlayerCharacter PlayerCharacter;
    private CharacterController CharacterController;
    private GameObject TargetedObject;
    private float TargetableRadius = 50f;

    private ushort Health = 100;
    private byte Energy = 20;

    private bool Attacking = false;
    private float AttackSpeed = 3f;
    private float AttackSpeedTimer = 0f;
    private ushort AttackDamage = 30;

    public TargetingManager TargetingManager;
    public PlayerHealth PlayerHealth;
    public PlayerEnergy PlayerEnergy;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        PlayerCharacter = GetComponent<PlayerCharacter>();
        CharacterController = GetComponent<CharacterController>();

        PlayerHealth.Set(Health);
        PlayerEnergy.Set(Energy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Attacking)
            AttackSpeedTimer += Time.deltaTime;

        if (AttackSpeedTimer > AttackSpeed)
        {
            AttackSpeedTimer = 0f;
            TargetedObject.GetComponent<Mob1>().DealDamage(AttackDamage);
        }

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            TargetedObject = null;
            PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.PointAndClick);
            PlayerCharacter.SetTargetFromMouseClick(mousePosition);
        }
        else if (TargetedObject != null)
        {
            // Continue to walk closer to target if not in range
            if (Vector3.Distance(this.transform.position, TargetedObject.transform.position) > 1.3f)
            {
                PlayerCharacter.SetInputPointTarget(TargetedObject.transform.position);
            }
            else
            {
                // Stop walking closer to target
                PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.Directional);
                Attacking = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TargetingManager.Cancel();
        } 
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject target = TargetingManager.MoveTowards();            
            if (target != null)
            {
                PlayerCharacter.UpdateLocomotion(PlayerCharacter.INPUT_TYPE.PointAndClick);
                TargetedObject = TargetingManager.MoveTowards();
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, TargetableRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag.Contains("|Enemy|"))
                {
                    TargetingManager.Target(hitColliders[i].gameObject, true);
                }
            }
        }
    }
}
