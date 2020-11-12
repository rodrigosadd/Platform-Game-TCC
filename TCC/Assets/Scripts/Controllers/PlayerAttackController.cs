﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
     public Transform targetAttack;
     public LayerMask layerEnemy;
     public int maxCombo;
     public int currentAttack;
     public float delayNextAttack;
     public float maxDistanceAttack;
     public float attackImpulse;
     public bool attaking;

#if UNITY_EDITOR
     public bool seeAttackRange;
#endif

     private float _currentMaxSpeed;
     private float _lastAttackTime;

     void LateUpdate()
     {
          InputsAttack();
          CanAttack();
     }

     public void InputsAttack()
     {
          if (Input.GetButtonDown("Fire1") && PlayerController.instance.movement.slowed == false && (PlayerController.instance.movement.stateCharacter == CharacterState.RUNNNING || PlayerController.instance.movement.stateCharacter == CharacterState.IDLE) && !PlayerController.instance.death.dead)
          {
               if (!attaking)
               {
                    _currentMaxSpeed = PlayerController.instance.movement.fixedMaxSpeed;
               }
               FirstAttack();
          }
     }

     public void FirstAttack()
     {
          if (!PlayerController.instance.death.dead)
          {
               _lastAttackTime = Time.time;
               currentAttack++;

               if (currentAttack == 1)
               {
                    PlayerController.instance.animator.SetBool("First Attack", true);
               }
               currentAttack = Mathf.Clamp(currentAttack, 0, maxCombo);
          }
     }

     public void SecondAttack()
     {
          if (!PlayerController.instance.death.dead)
          {
               if (currentAttack >= 2)
               {
                    PlayerController.instance.animator.SetBool("Second Attack", true);
               }
               else
               {
                    PlayerController.instance.animator.SetBool("First Attack", false);
                    PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
                    attaking = false;
                    currentAttack = 0;
               }
          }
     }

     public void FinalAttack()
     {
          if (!PlayerController.instance.death.dead)
          {
               if (currentAttack >= 3)
               {
                    PlayerController.instance.animator.SetBool("Final Attack", true);
               }
               else
               {
                    PlayerController.instance.animator.SetBool("Second Attack", false);
                    currentAttack = 0;
               }
          }
     }

     public void ResetAttack()
     {
          PlayerController.instance.animator.SetBool("First Attack", false);
          PlayerController.instance.animator.SetBool("Second Attack", false);
          PlayerController.instance.animator.SetBool("Final Attack", false);
          PlayerController.instance.movement.maxSpeed = _currentMaxSpeed;
          attaking = false;
          currentAttack = 0;
     }

     public void Attaking()
     {
          if (!PlayerController.instance.death.dead)
          {
               attaking = true;
               if (PlayerController.instance.movement.slowed == false)
               {
                    PlayerController.instance.movement.maxSpeed = 0f;
                    PlayerController.instance.movement.rbody.AddForce(PlayerController.instance.characterGraphic.forward * attackImpulse, ForceMode.Impulse);
               }
          }
     }

     public void AttackDetection()
     {
          Collider[] _hitEnemy = Physics.OverlapSphere(targetAttack.position, maxDistanceAttack, layerEnemy);

          foreach (Collider _hit in _hitEnemy)
          {
               _hit.transform.GetComponent<MeleeEnemy>().TakeHit();
          }
     }

     public void CanAttack()
     {
          if (Time.time - _lastAttackTime > delayNextAttack)
          {
               currentAttack = 0;
          }
     }

#if UNITY_EDITOR
     void OnDrawGizmos()
     {
          if (seeAttackRange)
          {
               Gizmos.color = Color.blue;
               Gizmos.DrawWireSphere(targetAttack.position, maxDistanceAttack);
          }
     }
#endif
}
