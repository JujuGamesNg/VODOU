﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameKitController.Audio;
using UnityEngine.Events;

public class ragdollActivator : MonoBehaviour
{
    //Main variables
    public deathType typeOfDeath;
    public float timeToGetUp;
    public bool checkPlayerOnGroundToGetUp = true;

    //Ragdoll physics variables
    public float ragdollToMecanimBlendTime = 0.5f;
    public float maxRagdollVelocity;
    public float maxVelocityToGetUp;
    public float extraForceOnRagdoll;
    public LayerMask layer;

    //Ragdoll state variables
    public ragdollState currentState = ragdollState.animated;
    public healthState playerState;
    public bool onGround;
    public bool canMove = true;

    //Player variables
    public float timeToShowMenu;

    //AI variables
    public bool usedByAI;

    public UnityEvent eventOnEnterRagdoll;
    public UnityEvent eventOnExitRagdoll;

    public UnityEvent eventOnDeath;

    public string tagForColliders;

    public bool setLayerOnRagdollState;
    public string layerOnRagdollState;

    public bool useDeathSound = true;
    public AudioClip deathSound;
    public AudioSource mainAudioSource;
    public AudioElement deathSoundAudioElement;

    public float getUpDelay = 2;
    float lastTimeStateEnabled;

    public bool activateRagdollAfterDeath;
    public float delayToActivateRagdollAfterDeath;

    //Damage on impact variables
    public bool ragdollCanReceiveDamageOnImpact;
    public float minTimeToReceiveDamageOnImpact;
    public float minVelocityToReceiveDamageOnImpact;
    public float receiveDamageOnImpactMultiplier;
    public float minTimToReceiveImpactDamageAgain;
    float lastTimeDamageOnImpact;

    public enum healthState
    {
        alive,
        dead,
        fallen
    }

    public enum ragdollState
    {
        animated,
        ragdolled,
        blendToAnim
    }

    public enum deathType
    {
        ragdoll,
        mecanim
    }

    public List<bodyPart> bodyParts = new List<bodyPart> ();
    public List<Collider> colliderBodyParts = new List<Collider> ();
    public List<Rigidbody> rigidbodyBodyParts = new List<Rigidbody> ();

    public List<extraRagdollsInfo> extraRagdollsInfoList = new List<extraRagdollsInfo> ();

    public ragdollBuilder mainRagdollBuilder;

    public bool checkGetUpPaused;

    public bool ragdollAdded;

    public bool showComponents;

    public bool canMoveCharacterOnRagdollState;
    public float moveCharacterOnRagdollStateForceAmount;
    public ForceMode moveCharacterOnRagdollStateForceMode;

    public string getUpFromBellyAnimatorName = "Get Up From Belly";
    public string getUpFromBackAnimatorName = "Get Up From Back";
    public string deathAnimatorName = "Dead";
    public int deathAnimationID = 1534;
    public string actionIDAnimatorName = "Action ID";
    public bool useGetUpFromBellyAfterDeathActive;

    public bool enableAnimatorLayerOnDeath;
    public string animatorLayerToEnableName;

    public bool ignoreIfRagdollOnGroundToShowDeathMenu;

    public playerWeaponsManager weaponsManager;
    public Transform mainCameraTransform;
    public playerStatesManager statesManager;
    public health healthManager;
    public Collider mainCollider;
    public playerInputManager playerInput;

    public Animator mainAnimator;
    public playerController playerControllerManager;
    public playerCamera cameraManager;
    public otherPowers powersManager;
    public gravitySystem gravityManager;
    public IKSystem IKSystemManager;
    public Rigidbody mainRigidbody;

    public footStepManager stepsManager;
    public closeCombatSystem combatManager;

    public bool usingExtraRagdollActive;

    public int currentExtraRagdollActiveIndex = -1;

    public GameObject characterBody;

    public Transform playerCOM;

    public Transform rootMotion;

    public Transform headTransform;
    public Transform leftFootTransform;
    public Transform rightFootTransform;
    public GameObject skeleton;

    public Rigidbody hipsRigidbody;

    public Rigidbody currentHipsRigidbody;

    public Transform currentRootMotion;

    public Transform currentCharacterBody;

    public bool checkpointManagerStateEnabled = true;

    public bool forceQuickGetUpOnCharacter;


    public bool setRagdollStateOnStart;
    public bool pauseGetUpStateOnRagdollOnStart;
    public float ragdollOnStartForce = 10;

    public bool setSleepStateOnStart;
    public bool useDurationForSleepStateOnStart;
    public float durationForSleepStateOnStart;


    extraRagdollsInfo currentExtraRagdollsInfo;

    checkpointSystem checkpointManager;

    bool checkpointManagerLocated;

    bool checkpointManagerChecked;

    Rigidbody currentRigidbodyPart;

    Collider currentCollider;

    RaycastHit hit;
    bool dropCamera;
    bool enableBehaviour;

    float currentSedateDuration;
    bool sedateUntilReceiveDamageState;
    float lastTimeSedated;
    bool sedateActive;

    float lastTimeRagdolled;

    bool carryinWeaponsPreviously;

    Coroutine dropOrPickCameraMovement;
    Vector3 damageVelocity;

    bool previouslyRagdolled;

    int getUpFromBellyAnimatorID;
    int getUpFromBackAnimatorID;
    int deathAnimatorID;
    int actionIDAnimatorID;

    bool applyForceOnRagdollActive = true;

    bool usedCustomGetUpTimeActive;
    float customGetUpTime;

    Transform mainTransform;

    float lastTimeDeadActive;

    bool playerControllerLocated;

    bool playerCameraLocated;

    bool playerWeaponsManagerLocated;

    bool closeCombatSystemLocated;

    bool otherPowersLocated;

    bool playerStatesManagerLocated;

    bool footStepManagerLocated;

    bool gravitySystemLocated;

    string originalTag;

    Transform ragdollExternalParent;
    Coroutine sedateCoroutine;

    bool overrideNotUseGraviytOnRagdollActive;
    bool overrideUseGraviytOnRagdollActive;

    bool pushAllBodyParts;

    float mecanimToGetUpTransitionTime = 0.05f;
    float ragdollingEndTime = -1;
    float deadMenuTimer;
    Vector3 ragdolledHipPosition;
    Vector3 ragdolledHeadPosition;
    Vector3 ragdolledFeetPosition;
    Vector3 playerVelocity;
    Vector3 damagePos;
    Vector3 damageDirection;
    Vector3 originalFirtPersonPivotPosition;

    Rigidbody closestPart;

    bool pauseAnimatorStatesToGetUp;

    bool currentHipsRigidbodyLocated;

    bool activateQuickGetUpInsteadOfRegularGetUp;


    private void InitializeAudioElements ()
    {
        if (mainAudioSource != null) {
            deathSoundAudioElement.audioSource = mainAudioSource;
        }

        if (deathSound != null) {
            deathSoundAudioElement.clip = deathSound;
        }
    }

    void Start ()
    {
        InitializeAudioElements ();

        playerControllerLocated = playerControllerManager != null;

        playerCameraLocated = cameraManager != null;

        otherPowersLocated = powersManager != null;

        playerWeaponsManagerLocated = weaponsManager != null;

        closeCombatSystemLocated = combatManager != null;

        playerStatesManagerLocated = statesManager != null;

        footStepManagerLocated = stepsManager != null;

        gravitySystemLocated = gravityManager != null;

        mainTransform = transform;

        if (mainRagdollBuilder == null) {
            mainRagdollBuilder = GetComponentInParent<ragdollBuilder> ();
        }

        getUpFromBellyAnimatorID = Animator.StringToHash (getUpFromBellyAnimatorName);
        getUpFromBackAnimatorID = Animator.StringToHash (getUpFromBackAnimatorName);
        actionIDAnimatorID = Animator.StringToHash (actionIDAnimatorName);

        deathAnimatorID = Animator.StringToHash (deathAnimatorName);

        if (playerCOM == null) {
            playerCOM = IKSystemManager.getIKBodyCOM ();
        }

        if (colliderBodyParts.Count == 0) {
            setBodyColliderList ();
        }

        if (rigidbodyBodyParts.Count == 0) {
            setBodyRigidbodyList ();
        }

        setKinematic (true);

        //store all the part inside the model of the player, in this case, his bones
        if (bodyParts.Count == 0) {
            setBodyParts ();
        }

        if (rootMotion != null) {
            skeleton = rootMotion.parent.gameObject;

            if (hipsRigidbody == null) {
                hipsRigidbody = rootMotion.GetComponent<Rigidbody> ();
            }
        }

        currentHipsRigidbody = hipsRigidbody;

        currentHipsRigidbodyLocated = currentHipsRigidbody != null;

        currentRootMotion = rootMotion;

        currentCharacterBody = characterBody.transform;

        if (rigidbodyBodyParts.Count > 0) {
            ragdollAdded = true;
        }

        originalTag = tag;

        if (setRagdollStateOnStart) {
            StartCoroutine (ragdollStateOnStartCoroutine ());
        }

        if (setSleepStateOnStart) {
            StartCoroutine (sleepStateOnStartCoroutine ());
        }
    }

    void Update ()
    {
        //when the ragdoll is enabled
        if (!usedByAI) {
            if (playerState == healthState.dead) {
                //check if the player is on the ground, so he can get up
                Vector3 raycastPosition = currentRootMotion.position;

                if (Physics.Raycast (raycastPosition + Vector3.up, -Vector3.up, out hit, 2.7f, layer) &&
                    (!currentHipsRigidbodyLocated || currentHipsRigidbody.linearVelocity.magnitude < maxVelocityToGetUp)) {

                    float cameraDistance = GKC_Utils.distance (cameraManager.transform.position, raycastPosition);

                    if (cameraDistance < 1) {

                        onGround = true;

                        if (!dropCamera && cameraManager.isFirstPersonActive ()) {
                            originalFirtPersonPivotPosition = mainCameraTransform.localPosition;

                            dropOrPickCamera (true);

                            dropCamera = true;
                        }
                    } else {
                        onGround = false;
                    }
                } else {
                    onGround = false;
                }

                if (checkpointManagerStateEnabled) {
                    if (checkIfSpawnDirectlyOnCheckpointEnabled ()) {
                        onGround = true;
                    }
                }

                if (ignoreIfRagdollOnGroundToShowDeathMenu) {
                    onGround = true;
                }

                if (ignoreIfRagdollOnGroundToShowDeathMenuOnNextDeath) {
                    onGround = true;
                }

                //enable the die menu after a few seconds and he is on the ground
                if (deadMenuTimer > 0 && (onGround || !checkPlayerOnGroundToGetUp)) {
                    deadMenuTimer -= Time.deltaTime;

                    if (deadMenuTimer < 0) {
                        eventOnDeath.Invoke ();
                    }
                }
            }
        }

        if (activateRagdollAfterDeath) {
            if (playerState == healthState.dead) {
                if (currentState != ragdollState.ragdolled) {
                    if (Time.time > lastTimeDeadActive + delayToActivateRagdollAfterDeath) {
                        pushCharacterWithoutForceAfterDeath ();

                        if (usedByAI) {
                            enabled = false;
                        }
                    }
                }
            }
        }

        if (playerState == healthState.fallen) {
            if (sedateActive) {
                if (Time.time > lastTimeSedated + currentSedateDuration && !sedateUntilReceiveDamageState) {
                    stopSedateStateCoroutine ();
                }
            } else {
                //check if the player is on the ground, so he can get up
                if (!currentHipsRigidbodyLocated || currentHipsRigidbody.linearVelocity.magnitude < maxVelocityToGetUp) {
                    onGround = true;
                } else {
                    onGround = false;
                }

                if (onGround) {
                    if (currentHipsRigidbodyLocated) {
                        if (playerCameraLocated) {
                            float cameraDistance = GKC_Utils.distance (cameraManager.transform.position, currentHipsRigidbody.position);

                            if (cameraDistance > 1) {
                                onGround = false;
                            }
                        }
                    }
                }

                if (!checkGetUpPaused) {
                    //enable the die menu after a few seconds and he is on the ground
                    if (onGround) {
                        if (usedCustomGetUpTimeActive) {
                            if (customGetUpTime > 0) {
                                customGetUpTime -= Time.deltaTime;

                                if (customGetUpTime <= 0) {
                                    if (healthManager.isDead ()) {
                                        playerState = healthState.dead;

                                        //print ("character dead trying to get up, cancelling");
                                    } else {
                                        getUp ();
                                    }

                                    usedCustomGetUpTimeActive = false;
                                }
                            }
                        } else {
                            if (deadMenuTimer > 0) {
                                deadMenuTimer -= Time.deltaTime;

                                if (deadMenuTimer <= 0) {
                                    getUp ();
                                }
                            }
                        }
                    }
                }
            }
        }

        if (currentState == ragdollState.ragdolled) {
            if (currentRootMotion != null) {
                //set the empty player gameObject position with the hips of the character
                mainTransform.position = currentRootMotion.position;

                if (canMoveCharacterOnRagdollState) {

                    playerInput.setIgnorePlayerIsDeadOnCameraAxisState (true);

                    Vector2 movementDirection = playerInput.getRealMovementAxisInputAnyType ();

                    Vector3 targetDirection = movementDirection.y * mainCameraTransform.forward + movementDirection.x * mainCameraTransform.right;

                    currentHipsRigidbody.AddForce (moveCharacterOnRagdollStateForceAmount * targetDirection, moveCharacterOnRagdollStateForceMode);
                }

                //prevent the ragdoll reachs a high velocity
                if (currentHipsRigidbody.linearVelocity.y <= -maxRagdollVelocity) {
                    Vector3 newVelocity = new Vector3 (currentHipsRigidbody.linearVelocity.x, -maxRagdollVelocity, currentHipsRigidbody.linearVelocity.z);

                    currentHipsRigidbody.linearVelocity = newVelocity;
                }
            }
        }

        if (currentState == ragdollState.animated && enableBehaviour) {
            if (Time.time > lastTimeStateEnabled + 0.5f) {
                if (!pauseAnimatorStatesToGetUp) {
                    mainAnimator.SetBool (getUpFromBackAnimatorID, false);
                    mainAnimator.SetBool (getUpFromBellyAnimatorID, false);
                }
            }

            if (Time.time > lastTimeStateEnabled + getUpDelay) {
                if (typeOfDeath == deathType.ragdoll || previouslyRagdolled) {
                    //allow the scripts work again
                    if (playerCameraLocated) {
                        gravityManager.death (false);
                    }

                    if (otherPowersLocated) {
                        powersManager.death (false);
                    }

                    if (playerControllerLocated) {
                        playerControllerManager.changeScriptState (true);

                        playerControllerManager.setHeadTrackCanBeUsedState (true);

                        playerControllerManager.setPlayerDeadState (false);

                        playerControllerManager.setApplyRootMotionAlwaysActiveState (false);

                        playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);
                    }

                    enableBehaviour = false;

                    canMove = true;

                    if (playerControllerLocated) {
                        playerControllerManager.setCanRagdollMoveState (true);

                        playerControllerManager.setRagdollCurrentlyActiveState (false);
                    }

                    eventOnExitRagdoll.Invoke ();

                    if (playerControllerLocated) {
                        if (!playerControllerManager.isCharacterControlOverrideActive ()) {
                            healthManager.setSliderVisibleState (true);
                        }
                    }

                    if (playerCameraLocated) {
                        if (!cameraManager.isFirstPersonActive ()) {
                            checkDrawWeaponsWhenResurrect ();
                        }
                    }

                    previouslyRagdolled = false;
                } else {
                    enableComponentsFromGettingUp (false);

                    enableBehaviour = false;

                    canMove = true;

                    if (playerControllerLocated) {
                        playerControllerManager.setCanRagdollMoveState (true);

                        playerControllerManager.setRagdollCurrentlyActiveState (false);
                    }
                }

                if (!usedByAI) {
                    playerInput.setInputCurrentlyActiveState (true);
                }

                reactiveMainCollider ();

                if (playerCameraLocated) {
                    cameraManager.checkGetUpStateFromDeathOnFBA ();
                }
            }
        }
    }

    void LateUpdate ()
    {
        if (currentState == ragdollState.blendToAnim) {
            if (Time.time <= ragdollingEndTime + mecanimToGetUpTransitionTime) {
                //set the position of all the parts of the character to match them with the animation

                Vector3 animatedToRagdolled = ragdolledHipPosition - currentRootMotion.position;

                Vector3 newRootPosition = currentCharacterBody.position + animatedToRagdolled;

                //				Debug.DrawLine (newRootPosition, newRootPosition + 5 * Vector3.up, Color.yellow, 2);

                //use a raycast downwards and find the highest hit that does not belong to the character 
                RaycastHit [] hits = Physics.RaycastAll (new Ray (newRootPosition + mainTransform.up, Vector3.down), 1.7f, layer);

                //				Debug.DrawLine (newRootPosition + mainTransform.up, newRootPosition + mainTransform.up + 100 * Vector3.up, Color.green, 2);

                float distance = Mathf.Infinity;

                int hitsLength = hits.Length;

                for (int i = 0; i < hitsLength; i++) {

                    RaycastHit hit = hits [i];

                    if (!hit.transform.IsChildOf (currentCharacterBody)) {
                        if (distance < Mathf.Max (newRootPosition.y, hit.point.y)) {
                            distance = Mathf.Max (newRootPosition.y, hit.point.y);
                        }
                    }
                }

                if (distance != Mathf.Infinity) {
                    newRootPosition.y = distance;
                }

                //				Debug.DrawLine (newRootPosition, newRootPosition +  100*Vector3.up, Color.blue, 2);

                //				Debug.DrawLine (newRootPosition, newRootPosition +  5*Vector3.up, Color.blue, 2);

                currentCharacterBody.position = newRootPosition;
                //set the rotation of all the parts of the character to match them with the animation

                Vector3 ragdolledDirection = ragdolledHeadPosition - ragdolledFeetPosition;
                ragdolledDirection.y = 0;

                Vector3 meanFeetPosition = 0.5f * (leftFootTransform.position + rightFootTransform.position);
                Vector3 animatedDirection = headTransform.position - meanFeetPosition;
                animatedDirection.y = 0;

                if (usingExtraRagdollActive && currentExtraRagdollsInfo.rotationYOffset != 0) {
                    ragdolledDirection = currentCharacterBody.forward;

                    Vector3 targetDirection = new Vector3 (0, ragdolledDirection.y, 0);

                    currentCharacterBody.rotation = Quaternion.FromToRotation (ragdolledDirection.normalized, targetDirection.normalized);
                } else {
                    currentCharacterBody.rotation *= Quaternion.FromToRotation (animatedDirection.normalized, ragdolledDirection.normalized);
                }

                setPlayerRotationOnGetUp ();
            }

            //compute the ragdoll blend amount in the range 0 to 1
            float ragdollBlendAmount = 1.0f - (Time.time - ragdollingEndTime - mecanimToGetUpTransitionTime) / ragdollToMecanimBlendTime;
            ragdollBlendAmount = Mathf.Clamp01 (ragdollBlendAmount);

            //to get a smooth transition from a ragdoll to animation, lerp the position of the hips 
            //and slerp all the rotations towards the ones stored when ending the ragdolling

            List<bodyPart> currentBodyParts = bodyParts;

            if (usingExtraRagdollActive) {
                currentBodyParts = currentExtraRagdollsInfo.bodyParts;
            }

            int currentBodyPartsCount = currentBodyParts.Count;

            for (int i = 0; i < currentBodyPartsCount; i++) {
                bodyPart b = currentBodyParts [i];

                //this if is to avoid change the root of the character, only the actual body parts
                if (b.transform != currentCharacterBody) {
                    //position is only interpolated for the hips
                    if (b.transform == currentRootMotion) {
                        b.transform.position = Vector3.Lerp (b.transform.position, b.storedPosition, ragdollBlendAmount);
                    }

                    //rotation is interpolated for all body parts
                    b.transform.rotation = Quaternion.Slerp (b.transform.rotation, b.storedRotation, ragdollBlendAmount);
                }
            }

            //if the ragdoll blend amount has decreased to zero, change to animated state
            if (ragdollBlendAmount == 0) {
                setPlayerRotationOnGetUp ();

                setPlayerToRegularState ();

                currentState = ragdollState.animated;

                return;
            }
        }
    }

    IEnumerator ragdollStateOnStartCoroutine ()
    {
        yield return new WaitForSeconds (0.3f);

        if (pauseGetUpStateOnRagdollOnStart) {
            setCheckGetUpPausedState (true);
        }

        if (ragdollOnStartForce > 0) {
            float randomXForce = Random.Range (-1.01f, 1.01f);
            float randomZForce = Random.Range (-1.01f, 1.01f);

            if (Mathf.Abs (randomXForce) < 0.5f && Mathf.Abs (randomZForce) < 0.5f) {
                randomZForce *= 2;
            }

            Vector3 randomForceDirection = randomXForce * transform.right + randomZForce * transform.forward;

            pushCharacter (ragdollOnStartForce * randomForceDirection);
        } else {
            pushCharacterWithoutForce ();
        }
    }

    IEnumerator sleepStateOnStartCoroutine ()
    {
        yield return new WaitForSeconds (0.3f);

        if (useDurationForSleepStateOnStart) {
            sedateCharacter (0, false, durationForSleepStateOnStart);
        } else {
            sedateCharacter (0, true, 0);
        }
    }

    //get the direction of the projectile that killed the player
    public void deathDirection (Vector3 dir)
    {
        damageDirection = dir;
    }

    //the player has dead, get the last damage position, and the rigidbody velocity of the player
    public void die (Vector3 pos)
    {
        if (playerStatesManagerLocated) {
            statesManager.disableVehicleDrivenRemotely ();
        }

        canMove = false;

        if (playerControllerLocated) {
            playerControllerManager.setRagdollCurrentlyActiveState (true);
        }

        eventOnEnterRagdoll.Invoke ();

        playerState = healthState.dead;

        if (playerWeaponsManagerLocated) {
            //check if the player was using weapons before dying
            carryinWeaponsPreviously = weaponsManager.isPlayerCarringWeapon ();

            //set dead state in weapon to drop the current or currents weapons that the player has if he is in weapon mode and is carrying one of them
            weaponsManager.setDeadState (true);
        }

        if (playerStatesManagerLocated) {
            statesManager.checkPlayerStates ();
        }

        damagePos = pos;

        playerVelocity = mainRigidbody.linearVelocity;
        //check if the player has a ragdoll, if he hasn't it, then use the mecanim instead, to avoid issues

        bool canUseRagdoll = false;

        if (!ragdollAdded) {
            typeOfDeath = deathType.mecanim;
        } else {
            if (!Physics.Raycast (mainTransform.position + Vector3.up, -Vector3.up, out hit, 2, layer)) {
                canUseRagdoll = true;
            }
        }

        if (typeOfDeath == deathType.ragdoll) {
            if (playerControllerLocated) {
                if (playerControllerManager.isUsingGenericModelActive ()) {
                    if (usingExtraRagdollActive) {
                        if (currentExtraRagdollsInfo.hipsRigidbody == null) {
                            canUseRagdoll = false;

                            typeOfDeath = deathType.mecanim;
                        }
                    }
                }
            }
        }

        if (closeCombatSystemLocated) {
            combatManager.enableOrDisableTriggers (false);
        }

        if (footStepManagerLocated) {
            stepsManager.enableOrDisableFootSteps (false);
        }

        bool isFirstPersonActive = false;

        if (playerCameraLocated) {
            isFirstPersonActive = cameraManager.isFirstPersonActive ();
        }

        if (enableAnimatorLayerOnDeath) {
            if (!playerControllerManager.isUsingGenericModelActive ()) {
                playerControllerManager.enableAnimatorLayerWeight (animatorLayerToEnableName);
            }
        }

        //check if the player use mecanim for the death, and if the first person mode is enabled, to use animations instead ragdoll
        if ((typeOfDeath == deathType.mecanim || isFirstPersonActive) && !canUseRagdoll) {
            //disable the player and enable the gravity in the player's ridigdboby
            if (playerControllerLocated) {
                playerControllerManager.changeScriptState (false);

                playerControllerManager.setHeadTrackCanBeUsedState (false);

                playerControllerManager.setPlayerDeadState (true);

                playerControllerManager.setApplyRootMotionAlwaysActiveState (true);

                playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);
            }

            if (gravitySystemLocated) {
                gravityManager.death (true);
            }

            if (playerCameraLocated) {
                cameraManager.death (true, typeOfDeath == deathType.ragdoll);
            }

            if (otherPowersLocated) {
                powersManager.death (true);
            }

            if (!isFirstPersonActive) {
                //set the dead state in the mecanim
                mainAnimator.SetBool (deathAnimatorID, true);
                mainAnimator.SetInteger (actionIDAnimatorID, deathAnimationID);
            }
        }

        //else enable the ragdoll
        else {
            enableOrDisableRagdoll (true);
        }

        if (usedByAI) {
            tag = "Untagged";
        } else {
            deadMenuTimer = timeToShowMenu;

            playerInput.setInputCurrentlyActiveState (false);
        }

        if (useDeathSound) {
            if (deathSoundAudioElement != null) {
                AudioPlayer.PlayOneShot (deathSoundAudioElement, gameObject);
            }
        }

        if (playerControllerLocated) {
            playerControllerManager.setCanRagdollMoveState (false);
        }

        stopSedateStateCoroutine ();

        lastTimeDeadActive = Time.time;
    }

    public void setUseDeathSoundState (bool state)
    {
        useDeathSound = state;
    }

    public void checkLastCheckpoint ()
    {
        setcheckToGetUpState ();
    }

    bool checkIfSpawnDirectlyOnCheckpointEnabled ()
    {
        if (checkpointManagerStateEnabled) {
            if (!checkpointManagerChecked) {
                if (checkpointManager == null) {
                    checkpointManager = FindObjectOfType<checkpointSystem> ();
                }

                checkpointManagerLocated = checkpointManager != null;

                checkpointManagerChecked = true;
            }

            if (checkpointManagerLocated) {
                return checkpointManager.isDeathLoadCheckpointRespawnActivee ();
            }
        }

        return false;
    }

    public void setForceQuickGetUpOnCharacterState (bool state)
    {
        forceQuickGetUpOnCharacter = state;
    }

    public void setActivateQuickGetUpInsteadOfRegularGetUpState (bool state)
    {
        activateQuickGetUpInsteadOfRegularGetUp = state;
    }

    public void setcheckToGetUpState ()
    {
        bool usingCheckPoint = false;

        if (usedByAI) {
            getUp ();
        } else {
            if (checkpointManagerStateEnabled) {
                if (!checkpointManagerChecked) {
                    if (checkpointManager == null) {
                        checkpointManager = FindObjectOfType<checkpointSystem> ();
                    }

                    checkpointManagerLocated = checkpointManager != null;

                    checkpointManagerChecked = true;
                }

                if (checkpointManagerLocated && checkpointManager.isCheckpointSystemEnabled ()) {
                    usingCheckPoint = checkpointManager.checkCheckpointStateOnCharacterGetUp (this, playerControllerManager.gameObject);
                }
            }
        }

        if (!usingCheckPoint) {
            if (forceQuickGetUpOnCharacter) {
                quickGetUp ();
            } else {
                playerState = healthState.fallen;

                deadMenuTimer = 0.3f;

                healthManager.resurrect ();
            }
        }
    }


    //play the game again
    public void getUp ()
    {
        if (activateQuickGetUpInsteadOfRegularGetUp) {
            quickGetUp ();

            activateQuickGetUpInsteadOfRegularGetUp = false;

            return;
        }

        if (playerState == healthState.dead) {
            healthManager.resurrect ();
        }

        if (usedByAI) {
            tag = originalTag;
        }

        playerState = healthState.alive;

        onGround = false;

        mainCollider.isTrigger = false;

        if (closeCombatSystemLocated) {
            combatManager.enableTriggersIfCombatModeActive ();
        }

        if (footStepManagerLocated) {
            stepsManager.enableOrDisableFootSteps (true);
        }

        bool isFirstPersonActive = false;

        if (playerCameraLocated) {
            isFirstPersonActive = cameraManager.isFirstPersonActive ();
        }

        if (ragdollExternalParent != null) {
            currentRootMotion.SetParent (skeleton.transform);

            if (currentHipsRigidbody.isKinematic) {
                currentHipsRigidbody.isKinematic = false;
            }

            ragdollExternalParent = null;
        }

        //check if the player use mecanim for the death, and if the first person mode is enabled, to use animations instead ragdoll
        if ((typeOfDeath == deathType.mecanim || (playerCameraLocated && isFirstPersonActive)) &&
            currentState == ragdollState.animated && currentState != ragdollState.ragdolled) {
            if (isFirstPersonActive) {
                enableComponentsFromGettingUp (isFirstPersonActive);
            } else {
                //set the get up animation in the mecanim
                mainAnimator.SetBool (deathAnimatorID, false);

                if (useGetUpFromBellyAfterDeathActive) {
                    mainAnimator.SetBool (getUpFromBellyAnimatorID, true);
                } else {
                    mainAnimator.SetBool (getUpFromBackAnimatorID, true);
                }

                enableBehaviour = true;
                lastTimeStateEnabled = Time.time;
            }
        } else {
            if (typeOfDeath == deathType.mecanim) {
                mainAnimator.SetBool (deathAnimatorID, false);
            }

            //else disable the ragdoll
            enableOrDisableRagdoll (false);
        }

        damageDirection = Vector3.zero;

        resetLastTimeMoved ();

        if (playerWeaponsManagerLocated) {
            weaponsManager.setDeadState (false);
        }

        if (isFirstPersonActive) {
            playerInput.setInputCurrentlyActiveState (true);
        }

        ignoreIfRagdollOnGroundToShowDeathMenuOnNextDeath = false;
    }

    public void quickGetUp ()
    {
        if (playerState == healthState.dead) {
            healthManager.resurrect ();
        }

        if (usedByAI) {
            tag = originalTag;
        }

        setPlayerRotationOnGetUp ();

        setPlayerToRegularState ();

        playerState = healthState.alive;

        currentState = ragdollState.animated;

        onGround = false;

        combatManager.enableTriggersIfCombatModeActive ();

        stepsManager.enableOrDisableFootSteps (true);

        //enable again the player
        if (playerControllerLocated) {
            playerControllerManager.enabled = true;
        }

        gravityManager.death (false);

        cameraManager.death (false, typeOfDeath == deathType.ragdoll);

        cameraManager.checkGetUpStateFromDeathOnFBA ();

        powersManager.death (false);

        bool isFirstPersonActive = false;

        if (playerCameraLocated) {
            isFirstPersonActive = cameraManager.isFirstPersonActive ();
        }

        //reset the rotation of the player
        if (isFirstPersonActive) {
            if (dropCamera) {
                mainCameraTransform.SetParent (cameraManager.pivotCameraTransform.transform);
                mainCameraTransform.localPosition = originalFirtPersonPivotPosition;
                mainCameraTransform.localRotation = Quaternion.identity;

                dropCamera = false;
            }
        }

        setKinematic (true);

        resumeAnimator ();

        currentRootMotion.localPosition = Vector3.zero;

        playerControllerManager.changeScriptState (true);

        playerControllerManager.setHeadTrackCanBeUsedState (true);

        playerControllerManager.setPlayerDeadState (false);

        playerControllerManager.setApplyRootMotionAlwaysActiveState (false);

        canMove = true;

        damageDirection = Vector3.zero;

        resetLastTimeMoved ();

        weaponsManager.setDeadState (false);

        carryinWeaponsPreviously = false;

        if (playerControllerLocated) {
            playerControllerManager.setCanRagdollMoveState (true);

            playerControllerManager.setRagdollCurrentlyActiveState (false);
        }

        playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);

        ignoreIfRagdollOnGroundToShowDeathMenuOnNextDeath = false;

        if (typeOfDeath == deathType.mecanim) {
            playerControllerManager.resetAnimatorEvenIfFullBodyAwarenessActive ();

            stopAnimator ();

            resumeAnimator ();

            if (enableAnimatorLayerOnDeath) {
                if (!playerControllerManager.isUsingGenericModelActive ()) {
                    playerControllerManager.disableAnimatorLayerWeight (animatorLayerToEnableName);
                }
            }

            playerControllerManager.resetAnimatorEvenIfFullBodyAwarenessActive ();
        }
    }

    public void enableComponentsFromGettingUp (bool isFirstPersonActive)
    {
        //enable again the player
        if (playerControllerLocated) {
            playerControllerManager.enabled = true;
        }

        if (gravitySystemLocated) {
            gravityManager.death (false);
        }

        if (playerCameraLocated) {
            cameraManager.death (false, typeOfDeath == deathType.ragdoll);
        }

        if (otherPowersLocated) {
            powersManager.death (false);
        }

        //reset the rotation of the player
        if (playerCameraLocated && isFirstPersonActive) {
            if (dropCamera) {
                dropOrPickCamera (false);

                dropCamera = false;
            }
        }

        if (playerControllerLocated) {
            playerControllerManager.changeScriptState (true);

            playerControllerManager.setHeadTrackCanBeUsedState (true);

            playerControllerManager.setPlayerDeadState (false);

            playerControllerManager.setApplyRootMotionAlwaysActiveState (false);

            playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);
        }

        if (playerCameraLocated && isFirstPersonActive) {
            reactiveMainCollider ();

            canMove = true;

            if (playerControllerLocated) {
                playerControllerManager.setCanRagdollMoveState (true);

                playerControllerManager.setRagdollCurrentlyActiveState (false);
            }
        }
    }

    public void reactiveMainCollider ()
    {
        if (mainCollider != null) {
            mainCollider.enabled = false;
            mainCollider.enabled = true;
        }
    }

    public void damageToFall ()
    {
        if (playerState == healthState.fallen || playerState == healthState.dead) {
            return;
        }

        bool isFirstPersonActive = false;

        if (playerCameraLocated) {
            isFirstPersonActive = cameraManager.isFirstPersonActive ();
        }

        if (!isFirstPersonActive && ragdollAdded) {
            if (playerWeaponsManagerLocated) {
                carryinWeaponsPreviously = weaponsManager.isUsingWeapons ();
            }

            if (playerControllerLocated) {
                playerControllerManager.setRagdollCurrentlyActiveState (true);
            }

            eventOnEnterRagdoll.Invoke ();

            canMove = false;

            if (healthManager.isDead ()) {
                playerState = healthState.dead;
            } else {
                playerState = healthState.fallen;
            }

            if (playerStatesManagerLocated) {
                statesManager.checkPlayerStates ();
            }

            damagePos = currentHipsRigidbody.position;

            playerVelocity = mainRigidbody.linearVelocity;
            //damageDirection = Vector3.zero;

            if (closeCombatSystemLocated) {
                combatManager.enableOrDisableTriggers (false);
            }

            if (footStepManagerLocated) {
                stepsManager.enableOrDisableFootSteps (false);
            }

            enableOrDisableRagdoll (true);

            deadMenuTimer = timeToGetUp;

            if (!usedByAI) {
                playerInput.setInputCurrentlyActiveState (false);
            }

            if (playerControllerLocated) {
                playerControllerManager.setCanRagdollMoveState (false);
            }
        }
    }

    public void dropOrPickCamera (bool state)
    {
        if (dropOrPickCameraMovement != null) {
            StopCoroutine (dropOrPickCameraMovement);
        }

        dropOrPickCameraMovement = StartCoroutine (dropOrPickCameraCoroutine (state));
    }

    IEnumerator dropOrPickCameraCoroutine (bool state)
    {
        Vector3 targetPosition = Vector3.zero;
        Vector3 currentPosition = Vector3.zero;

        if (state) {
            mainCameraTransform.SetParent (null);

            if (Physics.Raycast (mainCameraTransform.position, -Vector3.up, out hit, Mathf.Infinity, layer)) {
                targetPosition = hit.point + 0.3f * mainTransform.up;
            }

            currentPosition = mainCameraTransform.position;
        } else {
            mainCameraTransform.SetParent (cameraManager.pivotCameraTransform.transform);

            targetPosition = originalFirtPersonPivotPosition;
            currentPosition = mainCameraTransform.localPosition;
        }

        float i = 0.0f;

        while (i < 1.0f) {
            i += Time.deltaTime * 3;

            if (state) {
                mainCameraTransform.position = Vector3.Lerp (currentPosition, targetPosition, i);
            } else {
                mainCameraTransform.localPosition = Vector3.Lerp (currentPosition, targetPosition, i);
                mainCameraTransform.localRotation = Quaternion.Slerp (mainCameraTransform.localRotation, Quaternion.identity, i);
            }

            yield return null;
        }

        //		print (state + " " + carryinWeaponsPreviously);
        if (!state) {
            checkDrawWeaponsWhenResurrect ();
        }
    }

    //public property that can be set to toggle between ragdolled and animated character
    public void enableOrDisableRagdoll (bool value)
    {
        if (value) {
            if (enableAnimatorLayerOnDeath) {
                if (!playerControllerManager.isUsingGenericModelActive ()) {
                    playerControllerManager.enableAnimatorLayerWeight (animatorLayerToEnableName);
                }
            }

            if (currentState == ragdollState.animated || currentState == ragdollState.blendToAnim) {
                setTransitionToRagdoll ();
            } else if (sedateActive || playerState == healthState.dead) {
                List<Collider> currentColliderBodyParts = colliderBodyParts;

                if (usingExtraRagdollActive) {
                    currentColliderBodyParts = currentExtraRagdollsInfo.colliderBodyParts;
                }

                int currentColliderBodyPartsCount = currentColliderBodyParts.Count;

                for (int i = 0; i < currentColliderBodyPartsCount; i++) {
                    if (currentColliderBodyParts [i] != null) {
                        currentColliderBodyParts [i].tag = tagForColliders;
                    }
                }
            }
        } else {
            if (currentState == ragdollState.ragdolled) {
                setTransitionToAnimation ();
            }
        }
    }

    void resumeAnimator ()
    {
        mainAnimator.enabled = true;
    }
    void stopAnimator ()
    {
        mainAnimator.Update (0.1f);

        mainAnimator.enabled = false;
    }

    public void setTransitionToRagdoll ()
    {
        bool disableAnimatorAfterResult = true;

        if (playerControllerLocated) {
            if (playerControllerManager.isUsingGenericModelActive ()) {
                stopAnimator ();

                disableAnimatorAfterResult = false;
            }
        }

        currentCharacterBody.SetParent (null);

        currentRootMotion.SetParent (null);

        currentCharacterBody.rotation = new Quaternion (0, currentCharacterBody.rotation.y, 0, currentCharacterBody.rotation.w);

        currentRootMotion.SetParent (skeleton.transform);

        setKinematic (false);

        //		mainAnimator.Rebind ();

        if (disableAnimatorAfterResult) {
            stopAnimator ();
        }

        currentState = ragdollState.ragdolled;

        previouslyRagdolled = true;

        //pause the scripts to stop any action of the player 
        if (playerControllerLocated) {
            playerControllerManager.changeScriptState (false);

            playerControllerManager.setHeadTrackCanBeUsedState (false);

            playerControllerManager.setPlayerDeadState (true);

            playerControllerManager.activateOrDeactivateStrafeMode (false);

            playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);

            playerInput.setIgnorePlayerIsDeadOnCameraAxisState (false);
        }

        if (gravitySystemLocated) {
            gravityManager.death (true);
        }

        if (playerCameraLocated) {
            cameraManager.death (true, true);
        }

        if (otherPowersLocated) {
            powersManager.death (true);
        }

        mainCollider.enabled = false;

        healthManager.setSliderVisibleState (false);

        lastTimeRagdolled = Time.time;
    }

    public void setTransitionToAnimation ()
    {
        //transition from ragdolled to animated through the blendToAnim state
        setKinematic (true);

        //store the ragdolled position for blending

        List<bodyPart> currentBodyParts = bodyParts;

        if (usingExtraRagdollActive) {
            currentBodyParts = currentExtraRagdollsInfo.bodyParts;
        }

        int currentBodyPartsCount = currentBodyParts.Count;

        for (int i = 0; i < currentBodyPartsCount; i++) {
            bodyPart b = currentBodyParts [i];

            b.storedRotation = b.transform.rotation;
            b.storedPosition = b.transform.position;
        }

        //store the state change time
        ragdollingEndTime = Time.time;

        resumeAnimator ();

        setPlayerRotationOnGetUp ();

        currentState = ragdollState.blendToAnim;

        //save some key positions
        ragdolledFeetPosition = 0.5f * (leftFootTransform.position + rightFootTransform.position);
        ragdolledHeadPosition = headTransform.position;
        ragdolledHipPosition = currentRootMotion.position;

        //start the get up animation checking if the character is on his back or face down, to play the correct animation
        if (!pauseAnimatorStatesToGetUp) {
            bool characterLookingUp = true;

            Transform chestTransform = GKC_Utils.getMountPointTransformByName ("Chest", transform);

            if (chestTransform != null) {
                float angle = Vector3.Angle (chestTransform.up, playerControllerManager.getCurrentNormal ());

                //				print (angle);

                if (Mathf.Abs (angle) > 90) {
                    characterLookingUp = false;
                }
            } else {
                if (currentRootMotion.up.y > 0) {
                    characterLookingUp = false;
                }
            }

            //			print (characterLookingUp + " " + currentRootMotion.up.y + " " + (currentRootMotion.up.y > 0));


            if (characterLookingUp) {
                mainAnimator.SetBool (getUpFromBackAnimatorID, true);
            } else {
                mainAnimator.SetBool (getUpFromBellyAnimatorID, true);
            }
        }
    }

    public void setOverrideNotUseGraviytOnRagdollActiveState (bool state)
    {
        overrideNotUseGraviytOnRagdollActive = state;
    }

    public void setOverrideUseGraviytOnRagdollActiveState (bool state)
    {
        overrideUseGraviytOnRagdollActive = state;
    }

    //set the state of all the rigidbodies inside the character
    //kinematic is enabled or disabled according to the state
    void setKinematic (bool state)
    {
        if (ragdollTriggerActive) {
            disableRagdollTriggerStateIfActive ();
        }

        //if state== false, it means the player has dead, so get the position of the projectile that kills him,
        //and them the closest rigidbody of the character, to add velocity in the opposite direction to that part of the player
        if (!state) {
            closestPart = searchClosestBodyPart ();
        }

        bool zeroGravityActive = false;

        if (playerControllerLocated) {
            zeroGravityActive = playerControllerManager.isPlayerOnZeroGravityMode ();
        }

        if (zeroGravityActive) {
            if (Vector3.Angle (-mainTransform.forward, damageDirection) < 0.1f) {
                damageDirection = mainCameraTransform.forward;
            }
        }

        if (!state) {
            damageVelocity = extraForceOnRagdoll * damageDirection;

            if (damageVelocity == Vector3.zero) {
                damageVelocity = playerVelocity / 1.75f;
            }

            if (float.IsNaN (damageVelocity.x) || float.IsNaN (damageVelocity.y) || float.IsNaN (damageVelocity.z)) {
                damageVelocity = mainCameraTransform.forward;
            }
        }

        int ignoreRaycastLayerIndex = LayerMask.NameToLayer ("Ignore Raycast");
        int defaultLayerIndex = LayerMask.NameToLayer ("Default");

        string untaggedTagName = "Untagged";

        int customLayerOnRagdollStateIndex = -1;

        if (setLayerOnRagdollState) {
            customLayerOnRagdollStateIndex = LayerMask.NameToLayer (layerOnRagdollState);
        }

        List<Collider> currentColliderBodyParts = colliderBodyParts;

        List<Rigidbody> currentRigidbodyBodyParts = rigidbodyBodyParts;

        if (usingExtraRagdollActive) {
            currentColliderBodyParts = currentExtraRagdollsInfo.colliderBodyParts;

            currentRigidbodyBodyParts = currentExtraRagdollsInfo.rigidbodyBodyParts;
        }

        int currentColliderBodyPartsCount = currentColliderBodyParts.Count;

        for (int i = 0; i < currentColliderBodyPartsCount; i++) {
            //set the state of the colliders and rigidbodies inside the character to enable or disable them
            currentRigidbodyPart = currentRigidbodyBodyParts [i];

            currentCollider = currentColliderBodyParts [i];

            if (currentRigidbodyPart != null) {
                currentRigidbodyPart.isKinematic = state;
                currentCollider.enabled = !state;

                if (zeroGravityActive && !state) {
                    currentRigidbodyPart.useGravity = false;

                    gravityManager.addObjectToCurrentZeroGravityRoomSystem (currentRigidbodyPart.gameObject);
                } else {
                    if (overrideNotUseGraviytOnRagdollActive) {
                        currentRigidbodyPart.useGravity = false;

                    } else if (overrideUseGraviytOnRagdollActive) {
                        currentRigidbodyPart.useGravity = true;
                    }
                }

                //change the layer of the colliders in the ragdoll, so the camera has not problems with it
                if (!usedByAI) {
                    if (forceRegularLayerState) {
                        currentCollider.gameObject.layer = defaultLayerIndex;
                    } else {
                        if (!state) {
                            currentCollider.gameObject.layer = ignoreRaycastLayerIndex;
                        } else {
                            currentCollider.gameObject.layer = defaultLayerIndex;
                        }
                    }
                } else {
                    if (playerState != healthState.fallen) {
                        if (!state) {
                            currentCollider.tag = tagForColliders;
                        } else {
                            currentCollider.tag = untaggedTagName;
                        }
                    }

                    if (setLayerOnRagdollState && customLayerOnRagdollStateIndex != -1) {
                        currentCollider.gameObject.layer = customLayerOnRagdollStateIndex;
                    }
                }

                //if state== false, it means the player has dead, so get the position of the projectile that kills him,
                //and them the closest rigidbody of the character, to add velocity in the opposite direction to that part of the player
                if (applyForceOnRagdollActive) {
                    if (!state) {
                        if (playerState == healthState.fallen) {
                            currentRigidbodyPart.linearVelocity = playerVelocity / 1.75f;
                        }

                        if (pushAllBodyParts) {
                            currentRigidbodyPart.linearVelocity = (extraForceOnRagdoll / 2) * damageDirection;
                        } else {
                            if (currentRigidbodyPart == closestPart) {
                                currentRigidbodyPart.linearVelocity = damageVelocity;
                            } else {
                                if (playerState == healthState.dead) {
                                    currentRigidbodyPart.linearVelocity = playerVelocity / 1.75f;
                                }
                            }
                        }
                    }
                }
            } else {
                if (currentCollider != null && mainCollider != null) {
                    Physics.IgnoreCollision (mainCollider, currentCollider);
                }
            }
        }

        pushAllBodyParts = false;

        forceRegularLayerState = false;
    }

    bool ragdollTriggerActive;

    public void enableOrDisableRagdollTriggerState (bool state, string newLayer)
    {
        if (state) {
            if (playerState == healthState.dead) {
                return;
            }
        }

        List<Collider> currentColliderBodyParts = colliderBodyParts;

        if (usingExtraRagdollActive) {
            currentColliderBodyParts = currentExtraRagdollsInfo.colliderBodyParts;
        }

        int currentColliderBodyPartsCount = currentColliderBodyParts.Count;

        //int ignoreRaycastLayerIndex = LayerMask.NameToLayer ("Ignore Raycast");

        int defaultLayerIndex = LayerMask.NameToLayer ("Default");

        int newLayerIndex = -1;

        if (newLayer != "") {
            newLayerIndex = LayerMask.NameToLayer (newLayer);
        }

        for (int i = 0; i < currentColliderBodyPartsCount; i++) {

            currentCollider = currentColliderBodyParts [i];

            if (currentCollider != null) {
                currentCollider.enabled = state;
                currentCollider.isTrigger = state;

                if (state) {
                    if (newLayerIndex != -1) {
                        currentCollider.gameObject.layer = newLayerIndex;
                    }
                } else {
                    //currentCollider.gameObject.layer = ignoreRaycastLayerIndex;
                    currentCollider.gameObject.layer = defaultLayerIndex;
                }
            }
        }

        ragdollTriggerActive = state;
    }
    public void enableRagdollTriggerState (string newLayer)
    {
        enableOrDisableRagdollTriggerState (true, newLayer);
    }
    public void disableRagdollTriggerStateIfActive ()
    {
        if (ragdollTriggerActive) {
            enableOrDisableRagdollTriggerState (false, "");
        }
    }

    public void enableOrDisableRagdollPhysics (bool state)
    {
        setKinematic (!state);
    }

    public void enableOrDisableRagdollGravityState (bool state)
    {
        List<Rigidbody> currentRigidbodyBodyParts = rigidbodyBodyParts;

        if (usingExtraRagdollActive) {
            currentRigidbodyBodyParts = currentExtraRagdollsInfo.rigidbodyBodyParts;
        }

        int currentRigidbodyBodyPartsCount = currentRigidbodyBodyParts.Count;

        for (int i = 0; i < currentRigidbodyBodyPartsCount; i++) {
            //set the state of the colliders and rigidbodies inside the character to enable or disable them
            currentRigidbodyPart = currentRigidbodyBodyParts [i];

            if (currentRigidbodyPart != null) {

                currentRigidbodyPart.useGravity = state;
            }
        }
    }

    public void setVelocityToRagdollInDirection (Vector3 forceDirection)
    {
        List<Rigidbody> currentRigidbodyBodyParts = rigidbodyBodyParts;

        if (usingExtraRagdollActive) {
            currentRigidbodyBodyParts = currentExtraRagdollsInfo.rigidbodyBodyParts;
        }

        int currentRigidbodyBodyPartsCount = currentRigidbodyBodyParts.Count;

        for (int i = 0; i < currentRigidbodyBodyPartsCount; i++) {
            //set the state of the colliders and rigidbodies inside the character to enable or disable them
            currentRigidbodyPart = currentRigidbodyBodyParts [i];

            if (currentRigidbodyPart != null) {

                currentRigidbodyPart.linearVelocity = forceDirection;
            }
        }
    }

    public void setApplyForceOnRagdollActiveState (bool state)
    {
        applyForceOnRagdollActive = state;
    }

    public List<Collider> getBodyColliderList ()
    {
        if (usingExtraRagdollActive) {
            return currentExtraRagdollsInfo.colliderBodyParts;
        } else {
            return colliderBodyParts;
        }
    }

    public void ignoreCollisionWithBodyColliderList (List<Collider> colliderList)
    {
        List<Collider> currentColliderBodyParts = colliderBodyParts;

        if (usingExtraRagdollActive) {
            currentColliderBodyParts = currentExtraRagdollsInfo.colliderBodyParts;
        }

        int currentColliderBodyPartsCount = currentColliderBodyParts.Count;

        for (int i = 0; i < currentColliderBodyPartsCount; i++) {
            for (int j = 0; j < colliderList.Count; j++) {
                Physics.IgnoreCollision (currentColliderBodyParts [i], colliderList [j]);
            }
        }
    }

    public void setBodyColliderList ()
    {
        colliderBodyParts.Clear ();

        for (int i = 0; i < mainRagdollBuilder.bones.Count; i++) {
            colliderBodyParts.Add (mainRagdollBuilder.bones [i].boneCollider);
        }
    }

    public void setBodyRigidbodyList ()
    {
        rigidbodyBodyParts.Clear ();

        for (int i = 0; i < mainRagdollBuilder.bones.Count; i++) {
            rigidbodyBodyParts.Add (mainRagdollBuilder.bones [i].boneRigidbody);
        }
    }

    public void setBodyParts ()
    {
        bodyParts.Clear ();

        for (int i = 0; i < mainRagdollBuilder.bonesTransforms.Count; i++) {
            bodyPart bodyPart = new bodyPart ();

            bodyPart.name = mainRagdollBuilder.bonesTransforms [i].name;

            bodyPart.transform = mainRagdollBuilder.bonesTransforms [i];

            bodyParts.Add (bodyPart);
        }
    }

    public List<Rigidbody> getBodyRigidbodyList ()
    {
        if (usingExtraRagdollActive) {
            return currentExtraRagdollsInfo.rigidbodyBodyParts;
        } else {
            return rigidbodyBodyParts;
        }
    }

    //get the closest rigidbody to the projectile that killed the player, to add velocity with an opposite direction of the bullet
    Rigidbody searchClosestBodyPart ()
    {
        float distance = 100;

        Rigidbody part = new Rigidbody ();

        List<Rigidbody> currentRigidbodyBodyParts = rigidbodyBodyParts;

        if (usingExtraRagdollActive) {
            currentRigidbodyBodyParts = currentExtraRagdollsInfo.rigidbodyBodyParts;
        }

        int currentRigidbodyBodyPartsCount = currentRigidbodyBodyParts.Count;

        for (int i = 0; i < currentRigidbodyBodyPartsCount; i++) {
            float currentDistance = GKC_Utils.distance (currentRigidbodyBodyParts [i].transform.position, damagePos);

            if (currentDistance < distance) {
                distance = currentDistance;
                part = currentRigidbodyBodyParts [i];
            }
        }

        return part;
    }

    void setPlayerRotationOnGetUp ()
    {
        if (usingExtraRagdollActive) {
            float targetYRotation = currentCharacterBody.eulerAngles.y;

            targetYRotation = targetYRotation + currentExtraRagdollsInfo.rotationYOffset;

            Vector3 targetRotation = new Vector3 (0, targetYRotation, 0);

            //			Quaternion targetQuaterion = Quaternion.Euler (targetRotation);

            mainTransform.eulerAngles = targetRotation;
        } else {
            mainTransform.rotation = new Quaternion (0, currentCharacterBody.rotation.y, 0, currentCharacterBody.rotation.w);
        }
    }

    void setPlayerToRegularState ()
    {
        //set the parent of every object to back everything to the situation before the player died
        mainTransform.position = currentCharacterBody.position;

        currentCharacterBody.SetParent (playerCOM);

        reactiveMainCollider ();

        enableBehaviour = true;

        if (playerCameraLocated) {
            cameraManager.death (false, typeOfDeath == deathType.ragdoll);
        }

        lastTimeStateEnabled = Time.time;
    }

    public void setModelAndSkeletonParentInsideCharacter ()
    {
        if (playerState == healthState.dead || playerState == healthState.fallen) {
            if (currentCharacterBody.parent != playerCOM) {
                currentCharacterBody.SetParent (playerCOM);

                currentRootMotion.SetParent (skeleton.transform);
            }
        }
    }

    public void setCharacterBodyParentOut ()
    {
        currentCharacterBody.SetParent (null);
    }

    public void setCharacterBody (GameObject newCharacterBody, Animator currentAnimator)
    {
        characterBody = newCharacterBody;

        rootMotion = currentAnimator.GetBoneTransform (HumanBodyBones.Hips);

        headTransform = currentAnimator.GetBoneTransform (HumanBodyBones.Head);

        leftFootTransform = currentAnimator.GetBoneTransform (HumanBodyBones.LeftFoot);

        rightFootTransform = currentAnimator.GetBoneTransform (HumanBodyBones.RightFoot);

        if (rootMotion != null) {
            hipsRigidbody = rootMotion.GetComponent<Rigidbody> ();

            currentHipsRigidbody = hipsRigidbody;
        }

        currentRootMotion = rootMotion;

        if (characterBody != null) {
            currentCharacterBody = characterBody.transform;
        }

        updateComponent ();
    }

    public void setCharacterBodyActiveState (bool state)
    {
        if (characterBody != null) {
            if (characterBody.activeSelf != state) {
                characterBody.SetActive (state);
            }
        }
    }

    public void resetLastTimeMoved ()
    {
        if (playerControllerLocated) {
            playerControllerManager.setLastTimeMoved ();
        }

        if (playerCameraLocated) {
            cameraManager.setLastTimeMoved ();
        }

        if (otherPowersLocated) {
            powersManager.setLastTimeFired ();
        }

        if (playerWeaponsManagerLocated) {
            weaponsManager.setLastTimeFired ();
        }
    }

    public List<bodyPart> getBodyPartsList ()
    {
        if (usingExtraRagdollActive) {
            return currentExtraRagdollsInfo.bodyParts;
        } else {
            return bodyParts;
        }
    }

    public List<Transform> getBodyPartsTransformList ()
    {
        List<Transform> bonesList = new List<Transform> ();

        int bodyPartsCount = bodyParts.Count;

        for (int i = 0; i < bodyPartsCount; i++) {
            bonesList.Add (bodyParts [i].transform);
        }

        return bonesList;
    }

    public void pushCharacter (Vector3 direction)
    {
        if (currentHipsRigidbodyLocated) {
            damagePos = currentHipsRigidbody.position;
            damageDirection = direction;

            damageToFall ();
        }
    }

    public void setForceRegularLayerState (bool state)
    {
        forceRegularLayerState = state;
    }

    bool forceRegularLayerState;

    public void pushCharacterWithoutForce ()
    {
        applyForceOnRagdollActive = false;

        damageToFall ();

        applyForceOnRagdollActive = true;
    }

    public void pushCharacterWithoutForceAfterDeath ()
    {
        applyForceOnRagdollActive = false;

        enableOrDisableRagdoll (true);

        applyForceOnRagdollActive = true;
    }

    public void pushCharacterWithoutForceXAmountOfTime (float newValue)
    {
        usedCustomGetUpTimeActive = true;
        customGetUpTime = newValue;

        pushCharacterWithoutForce ();
    }

    public void pushFullCharacterCheckingIfCharaterBusy ()
    {
        if (!playerControllerManager.isPlayerDriving () && !playerControllerManager.playerIsBusy ()) {
            if (isRagdollActive ()) {
                pushHipsRigidobdy (80 * transform.up);
            } else {
                pushFullCharacter ((transform.forward + transform.up) / 2);
            }
        }
    }

    public void pushFullCharacter (Vector3 direction)
    {
        pushAllBodyParts = true;

        pushCharacter (direction);
    }

    public void pushFullCharacterXAmountOfTime (float newValue, Vector3 direction)
    {
        usedCustomGetUpTimeActive = true;
        customGetUpTime = newValue;

        pushFullCharacter (direction);
    }

    public void pushHipsRigidobdy (Vector3 direction)
    {
        if (currentState == ragdollState.ragdolled) {
            currentHipsRigidbody.AddForce (currentHipsRigidbody.mass * direction, ForceMode.Impulse);
        }
    }

    public void pushCharacterOnLastVelocity (float speedMultiplier)
    {
        pushFullCharacter (speedMultiplier * mainRigidbody.linearVelocity);
    }

    public Transform getRootMotion ()
    {
        return currentRootMotion;
    }

    public Rigidbody getHipsRigidbody ()
    {
        return currentHipsRigidbody;
    }

    public void sedateCharacter (float sedateDelay, bool sedateUntilReceiveDamage, float sedateDuration)
    {
        if (sedateCoroutine != null) {
            StopCoroutine (sedateCoroutine);
        }

        sedateCoroutine = StartCoroutine (setSedateStateCoroutine (sedateDelay, sedateUntilReceiveDamage, sedateDuration));
    }

    IEnumerator setSedateStateCoroutine (float sedateDelay, bool sedateUntilReceiveDamage, float sedateDuration)
    {
        if (!sedateActive) {
            yield return new WaitForSeconds (sedateDelay);
        }

        sedateUntilReceiveDamageState = sedateUntilReceiveDamage;
        currentSedateDuration = sedateDuration;
        lastTimeSedated = Time.time;
        sedateActive = true;

        damageToFall ();

        healthManager.setSedateState (true);
    }

    public void stopSedateStateCoroutine ()
    {
        if (sedateCoroutine != null) {
            StopCoroutine (sedateCoroutine);
        }

        sedateActive = false;
        sedateUntilReceiveDamageState = false;
        healthManager.setSedateState (false);
    }

    public void setImpactReceivedInfo (Vector3 impactVelocity, Collider impactCollider)
    {
        if (ragdollCanReceiveDamageOnImpact) {
            if (currentState == ragdollState.ragdolled) {
                List<Collider> currentColliderBodyParts = colliderBodyParts;

                if (usingExtraRagdollActive) {
                    currentColliderBodyParts = currentExtraRagdollsInfo.colliderBodyParts;
                }

                if (!currentColliderBodyParts.Contains (impactCollider)) {
                    if (Time.time > lastTimeRagdolled + minTimeToReceiveDamageOnImpact) {
                        float currentImpactVelocity = impactVelocity.magnitude;

                        float currentImpactVelocityABS = Mathf.Abs (impactVelocity.magnitude);
                        //print (currentImpactVelocity);

                        if (currentImpactVelocityABS > minVelocityToReceiveDamageOnImpact) {
                            if (Time.time > lastTimeDamageOnImpact + minTimToReceiveImpactDamageAgain) {
                                healthManager.takeConstantDamage (currentImpactVelocity * receiveDamageOnImpactMultiplier);
                                //	print ("take " + currentImpactVelocity * receiveDamageOnImpactMultiplier);
                                lastTimeDamageOnImpact = Time.time;
                            }
                        }
                    }
                }
            }
        }
    }

    public void checkDrawWeaponsWhenResurrect ()
    {
        if (carryinWeaponsPreviously && weaponsManager.isDrawWeaponWhenResurrectActive () && weaponsManager.checkIfWeaponsAvailable ()) {
            weaponsManager.setWeaponsModeActive (true);
            carryinWeaponsPreviously = false;
        }
    }

    public void setCheckGetUpPausedState (bool state)
    {
        checkGetUpPaused = state;

        if (!checkGetUpPaused) {
            deadMenuTimer = timeToGetUp;
        }
    }

    public void disabledCheckGetUpPaused ()
    {
        checkGetUpPaused = false;
    }

    public void setIgnoreIfRagdollOnGroundToShowDeathMenuState (bool state)
    {
        ignoreIfRagdollOnGroundToShowDeathMenu = state;
    }

    bool ignoreIfRagdollOnGroundToShowDeathMenuOnNextDeath;

    public void setIgnoreIfRagdollOnGroundToShowDeathMenuOnNextDeathState (bool state)
    {
        if (playerState != healthState.dead) {
            ignoreIfRagdollOnGroundToShowDeathMenuOnNextDeath = state;
        }
    }


    public void setUsedByAIState (bool state)
    {
        usedByAI = state;
    }

    public void setDeathWithRagdollActiveState (bool state)
    {
        if (state) {
            typeOfDeath = deathType.ragdoll;
        } else {
            typeOfDeath = deathType.mecanim;
        }
    }

    public void setDeathAnimationID (int newValue)
    {
        deathAnimationID = newValue;
    }

    public bool isRagdollActive ()
    {
        return currentState == ragdollState.ragdolled;
    }

    public void checkIfDisableRagdollActivatorComponent ()
    {
        if (!activateRagdollAfterDeath) {
            enabled = false;
        }
    }

    public void setRagdollOnExternalParent (Transform newParent)
    {
        ragdollExternalParent = newParent;
    }

    public void setPauseAnimatorStatesToGetUpState (bool state)
    {
        pauseAnimatorStatesToGetUp = state;
    }

    public void setExtraForceOnRagdollValue (float newValue)
    {
        extraForceOnRagdoll = newValue;
    }

    public void addNewExtraRagdollInfo (GameObject newCharacterBody, string ragdollName, List<Transform> bonesList,
                                        List<Rigidbody> rigidbodyList, List<Collider> colliderList, Transform hipsTransform,
                                        float rotationYOffsetValue, bool addingRagdollOnEditorTime)
    {
        extraRagdollsInfo newExtraRagdollsInfo = new extraRagdollsInfo ();

        bool ragdollFound = false;

        int ragdollIndex = extraRagdollsInfoList.FindIndex (s => s.Name == ragdollName);

        if (ragdollIndex > -1) {
            newExtraRagdollsInfo = extraRagdollsInfoList [ragdollIndex];

            ragdollFound = true;
        }

        newExtraRagdollsInfo.Name = ragdollName;

        if (hipsTransform != null) {
            newExtraRagdollsInfo.hipsRigidbody = hipsTransform.GetComponent<Rigidbody> ();
        }

        newExtraRagdollsInfo.rootMotion = hipsTransform;

        newExtraRagdollsInfo.characterBody = newCharacterBody;

        if (ragdollFound) {
            newExtraRagdollsInfo.rigidbodyBodyParts.Clear ();
            newExtraRagdollsInfo.colliderBodyParts.Clear ();
            newExtraRagdollsInfo.bodyParts.Clear ();
        }

        newExtraRagdollsInfo.rigidbodyBodyParts = rigidbodyList;

        newExtraRagdollsInfo.colliderBodyParts = colliderList;

        newExtraRagdollsInfo.rotationYOffset = rotationYOffsetValue;

        for (int i = 0; i < bonesList.Count; i++) {
            bodyPart bodyPart = new bodyPart ();

            bodyPart.name = bonesList [i].name;

            bodyPart.transform = bonesList [i];

            newExtraRagdollsInfo.bodyParts.Add (bodyPart);
        }

        if (!ragdollFound) {
            extraRagdollsInfoList.Add (newExtraRagdollsInfo);
        }

        if (addingRagdollOnEditorTime) {
            print ("Extra ragdoll added " + ragdollName);
        }

        if (!addingRagdollOnEditorTime) {

            updateComponent ();
        }
    }

    public void setCurrentRagdollInfo (string ragdollName)
    {
        usingExtraRagdollActive = false;

        currentExtraRagdollActiveIndex = -1;

        currentHipsRigidbody = hipsRigidbody;

        currentRootMotion = rootMotion;

        currentCharacterBody = characterBody.transform;

        currentExtraRagdollsInfo = null;

        skeleton = rootMotion.parent.gameObject;

        if (ragdollName != "") {
            int ragdollIndex = extraRagdollsInfoList.FindIndex (s => s.Name == ragdollName);

            if (ragdollIndex > -1) {
                for (int i = 0; i < extraRagdollsInfoList.Count; i++) {
                    extraRagdollsInfoList [i].isCurrentRagdoll = (ragdollIndex == i);
                }

                currentExtraRagdollActiveIndex = ragdollIndex;

                usingExtraRagdollActive = true;

                currentExtraRagdollsInfo = extraRagdollsInfoList [ragdollIndex];

                currentHipsRigidbody = currentExtraRagdollsInfo.hipsRigidbody;

                currentRootMotion = currentExtraRagdollsInfo.rootMotion;

                if (currentExtraRagdollsInfo.characterBody != null) {
                    currentCharacterBody = currentExtraRagdollsInfo.characterBody.transform;
                }

                if (currentRootMotion != null) {
                    skeleton = currentRootMotion.parent.gameObject;
                }
            }
        }

        if (currentHipsRigidbodyLocated) {
            if (currentHipsRigidbody != null) {
                cameraManager.updateHipsTransformInGame (currentHipsRigidbody.transform);
            }
        }
    }

    public void setUseDeathSoundStateFromEditor (bool state)
    {
        useDeathSound = state;

        updateComponent ();
    }

    public void setDeathWithRagdollActiveStateFromEditor (bool state)
    {
        setDeathWithRagdollActiveState (state);

        updateComponent ();
    }

    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Update Ragdoll Activator " + gameObject.name, gameObject);
    }

    [System.Serializable]
    public class bodyPart
    {
        public string name;
        public Transform transform;
        public Vector3 storedPosition;
        public Quaternion storedRotation;
    }

    [System.Serializable]
    public class extraRagdollsInfo
    {
        public string Name;

        public bool isCurrentRagdoll;

        public GameObject characterBody;

        public Transform rootMotion;

        public Rigidbody hipsRigidbody;

        public float rotationYOffset;

        public List<bodyPart> bodyParts = new List<bodyPart> ();
        public List<Collider> colliderBodyParts = new List<Collider> ();
        public List<Rigidbody> rigidbodyBodyParts = new List<Rigidbody> ();
    }
}