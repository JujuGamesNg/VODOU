﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class multipleInteractionSystem : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public bool interactionEnabled = true;

    public string playerTag = "Player";

    [Space]
    [Header ("Interaction Info List Settings")]
    [Space]

    public List<interactionInfo> interactionInfoList = new List<interactionInfo> ();

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;

    public bool interactionPauseActive;

    public List<GameObject> playerList = new List<GameObject> ();

    [Space]
    [Header ("Events Settings")]
    [Space]

    public bool useEventOnTriggerEnter;
    public eventParameters.eventToCallWithGameObject eventOnTriggerEnter;

    public bool useEventOnTriggerExit;
    public eventParameters.eventToCallWithGameObject eventOnTriggerExit;

    [Space]
    [Header ("Components")]
    [Space]

    public GameObject mainInteractionObject;
    public Collider mainCollider;


    Coroutine delayCoroutine;


    public void activateInteraction (List<string> typeNameList, GameObject character)
    {
        if (!interactionEnabled) {
            return;
        }

        if (interactionPauseActive) {
            return;
        }

        if (showDebugPrint) {
            print ("Sending interaction signal from " + character.name);

            for (int i = 0; i < typeNameList.Count; i++) {
                print (typeNameList [i]);
            }
        }

        for (int i = 0; i < interactionInfoList.Count; i++) {
            interactionInfo currentInteractionInfo = interactionInfoList [i];

            if (currentInteractionInfo.interactionEnabled) {

                if (typeNameList.Contains (currentInteractionInfo.Name)) {
                    if (currentInteractionInfo.useDelayOnInteraction) {
                        stopActivateInteractionCoroutine ();

                        delayCoroutine = StartCoroutine (activateInteractionCoroutine (currentInteractionInfo, character));
                    } else {
                        confirmActivateInteraction (currentInteractionInfo, character);
                    }

                    return;
                }
            }
        }
    }

    void confirmActivateInteraction (interactionInfo currentInteractionInfo, GameObject character)
    {
        currentInteractionInfo.eventToSendCharacter.Invoke (character);

        currentInteractionInfo.eventOnInteraction.Invoke ();

        if (showDebugPrint) {
            print ("Sending interaction signal for " + currentInteractionInfo.Name);
        }

        usingDevicesSystem currentUsingDevicesSystem = character.GetComponent<usingDevicesSystem> ();

        bool removeDeviceFromListResult = false;

        if (currentInteractionInfo.removeDeviceFromUsingDeviceSystemOnUse) {
            removeDeviceFromListResult = true;
        }

        if (currentInteractionInfo.disableDeviceTrigger) {
            mainCollider.enabled = false;

            mainInteractionObject.tag = "Untagged";

            removeDeviceFromListResult = true;
        }

        if (removeDeviceFromListResult && currentUsingDevicesSystem != null) {
            currentUsingDevicesSystem.removeDeviceFromList (mainInteractionObject);
        }

        if (currentInteractionInfo.useRemoteEventsOnCharacter) {
            remoteEventSystem currentRemoteEventSystem = character.GetComponent<remoteEventSystem> ();

            if (currentRemoteEventSystem != null) {
                for (int j = 0; j < currentInteractionInfo.remoteEventNameList.Count; j++) {

                    currentRemoteEventSystem.callRemoteEvent (currentInteractionInfo.remoteEventNameList [j]);
                }
            }
        }
    }

    public void checkEventOnEnterExit (bool state, GameObject newPlayer)
    {
        if (state) {
            if (useEventOnTriggerEnter) {
                eventOnTriggerEnter.Invoke (newPlayer);
            }

        } else {
            if (useEventOnTriggerExit) {
                eventOnTriggerExit.Invoke (newPlayer);
            }
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag (playerTag)) {
            if (!playerList.Contains (col.gameObject)) {
                playerList.Add (col.gameObject);

                checkEventOnEnterExit (true, col.gameObject);
            }
        }
    }

    void OnTriggerExit (Collider col)
    {
        if (col.CompareTag (playerTag)) {
            if (playerList.Contains (col.gameObject)) {
                playerList.Remove (col.gameObject);

                checkEventOnEnterExit (false, col.gameObject);
            }
        }
    }

    public void setInteractionPauseActiveState (bool state)
    {
        interactionPauseActive = state;
    }

    public void removeDeviceFromPlayerList ()
    {
        if (playerList.Count > 0) {
            for (int i = 0; i < playerList.Count; i++) {
                usingDevicesSystem currentUsingDevicesSystem = playerList [i].GetComponent<usingDevicesSystem> ();

                if (currentUsingDevicesSystem != null) {
                    currentUsingDevicesSystem.removeDeviceFromList (mainInteractionObject);
                }
            }
        }
    }

    public void setMainColliderEnabledState (bool state)
    {
        if (mainCollider.enabled != state) {
            mainCollider.enabled = state;
        }
    }

    public void disableInteractionIfActive ()
    {
        if (interactionPauseActive) {
            return;
        }

        enableOrDisableInteraction (false);
    }

    public void enableOrDisableInteraction (bool state)
    {
        if (!interactionEnabled) {
            return;
        }

        setInteractionPauseActiveState (!state);

        if (!state) {
            removeDeviceFromPlayerList ();
        }

        setMainColliderEnabledState (state);

        if (showDebugPrint) {
            print ("enableOrDisableInteraction " + state);
        }
    }

    public void removeDeviceFromListAndDisableInteraction ()
    {
        removeDeviceFromPlayerList ();

        gameObject.tag = "Untagged";
    }

    public void disableEntireMultipleInteraction ()
    {
        if (!interactionEnabled) {
            return;
        }

        enableOrDisableInteraction (false);

        interactionEnabled = false;

        removeDeviceFromListAndDisableInteraction ();
    }

    public void stopActivateInteractionCoroutine ()
    {
        if (delayCoroutine != null) {
            StopCoroutine (delayCoroutine);
        }
    }

    IEnumerator activateInteractionCoroutine (interactionInfo currentInteractionInfo, GameObject character)
    {
        WaitForSeconds delay = new WaitForSeconds (currentInteractionInfo.delayToActivateInteraction);

        yield return delay;

        confirmActivateInteraction (currentInteractionInfo, character);
    }

    public void disableEntireMultipleInteractionFromEditor ()
    {
        disableEntireMultipleInteraction ();

        updateComponent ();
    }

    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Update Multiple Interaction System" + gameObject.name, gameObject);
    }

    [System.Serializable]
    public class interactionInfo
    {
        [Space]
        [Header ("Main Settings")]
        [Space]

        public string Name;

        public bool interactionEnabled = true;

        public GameObject interactionObject;

        public bool removeDeviceFromUsingDeviceSystemOnUse;

        public bool disableDeviceTrigger;

        [Space]

        public bool useDelayOnInteraction;
        public float delayToActivateInteraction;

        [Space]
        [Header ("Events Settings")]
        [Space]

        public UnityEvent eventOnInteraction;

        [Space]

        public eventParameters.eventToCallWithGameObject eventToSendCharacter;

        [Space]
        [Header ("Remote Events Settings")]
        [Space]

        public bool useRemoteEventsOnCharacter;
        public List<string> remoteEventNameList = new List<string> ();
    }
}
