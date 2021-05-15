﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObjectsDrop : MonoBehaviour
{
     public Camera3rdPerson camera3RdPerson;
     public Transform targetCam;
     public BreakableObject[] breakableObjects;
     public BreakableObject[] breakableObjectsDrop;
     public float timeToReturnPlayerTarget = 2f;
     public bool seeObject;
     private float countdownToReturnPlayerTarget;
     private bool canChangeTargetCam; 

     void Update()
     {
          CheckObjectsBroken();
          CountdownToReturnPlayerTarget();
     }

     public void CheckObjectsBroken()
     {
          bool _isComplete = true;

          for (int i = 0; i < breakableObjects.Length; i++)
          {
               if (!breakableObjects[i].triggerBroken)
               {
                    _isComplete = false;
                    break;
               }
          }

          if (_isComplete)
          {
             ActiveGravity();
             SeeObjectDrop();
          }
     }

     void ActiveGravity()
     {
         for(int i = 0; i < breakableObjectsDrop.Length; i++)
         {
                breakableObjectsDrop[i].rbody.isKinematic = false;
                breakableObjectsDrop[i].rbody.useGravity = true;
         }
     }

     public void SeeObjectDrop()
     {
          if(seeObject)
          {
               canChangeTargetCam = true;
               camera3RdPerson.targetCamera = targetCam;
               PlayerController.instance.movement.canMove = false;
          }
     }

     public void CountdownToReturnPlayerTarget()
     {
          if(canChangeTargetCam)
          {
               if(countdownToReturnPlayerTarget < 1)
               {
                    countdownToReturnPlayerTarget += Time.deltaTime / timeToReturnPlayerTarget;
               }
               else
               {
                    canChangeTargetCam = false;
                    camera3RdPerson.targetCamera = PlayerController.instance.movement.targetCam;
                    PlayerController.instance.movement.canMove = true;
                    seeObject = false;
               }
          }
     }
}
