﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class freeClimbSystem : externalControllerBehavior
{
    [Header ("Main Settings")]
    [Space]

    public bool climbEnabled = true;
    public bool climbCheckCanBeUsed = true;

    public float minDistanceToSurfaceToGrabSurfaceOnGround = 0.8f;
    public float minDistanceToSurfaceToGrabSurfaceOnAir = 1;

    public bool activateClimbStateDirectlyOnPressInput;

    public bool canActivateClimbOnPlayerOnGround = true;

    public bool ignoreActivateClimbStateOnNextSurfaceDetectedActive;

    public float surfaceOffset = 0.05f;

    public float minWaitTimeSinceLastTimeActionActiveToClimb = 0.6f;

    [Space]
    [Header ("Climb Method Settings")]
    [Space]

    public bool useRaycastDetectionToClimb = true;

    public climbDetectionCollisionSystem mainClimbDetectionCollisionSystem;

    public Vector3 climbDetectionCollisionPositionOffset;

    public bool updateRigidbodyPositionSmoothlyOnDetectionCollision = true;

    public float minDelayToAdjustDetectionCollision = 1;

    public float adjustToFoundSurfaceSpeed = 4;

    public float adjustToFoundSurfaceOffset = 0.3f;

    [Space]
    [Header ("Reset Rotation Settings")]
    [Space]

    public float resetPlayerRotationSpeed = 5;

    public bool rotateCameraToPlayerRotationOnClimbActiveEnabled = true;

    [Space]

    public bool useCameraOffsetOnClimbActiveEnabled;
    public Vector3 cameraOffsetOnClimbActive;

    public float cameraOffsetOnClimbActiveLerpSpeed;

    [Space]
    [Header ("Stop Climb Settings")]
    [Space]

    public float offsetToCheckIfStopClimbStateOnRaycast = 1;

    public float minTimeToStopClimbIfNotSurfaceFound = 0.3f;

    public float maxRaycastDistanceToStopClimb = 0.75f;

    [Space]
    [Header ("Surface Detection Settings")]
    [Space]

    public LayerMask raycastLayermask;

    public float raycastDistance = 0.6f;

    public float raycastDistanceToCheckToActivateClimbOnGround = 0.9f;
    public float raycastDistanceToCheckToActivateClimbOnAir = 0.9f;

    public float verticalRaycastOffset = 1;

    [Space]

    public bool useClimbZoneDetectionRaycast;

    public float upOffsetToDetectSurfaceOnMovementInput = 0.5f;
    public float downOffsetToDetectSurfaceOnMovementInput = 0.2f;

    public float rightOffsetToDetectSurfaceOnMovementInput = 0.2f;
    public float leftOffsetToDetectSurfaceOnMovementInput = 0.2f;

    [Space]
    [Header ("Surface To Ignore Settings")]
    [Space]

    public bool useSurfacesToIgnoreTags;
    public List<string> surfacesToIgnoreTagsList = new List<string> ();

    [Space]

    public bool useSurfacesToCheckTagsOnClimbByInput;
    public List<string> surfacesToCheckTagsOnClimbByInputList = new List<string> ();

    public bool checkForClimbSurfaceZoneSystemOnClimbInput;

    [Space]

    public bool avoidMovementTowardSurfacesToIgnore;

    public bool stopClimbIfSurfaceToIgnoreDetected;

    public bool avoidMovementTowardNoClimbZones;

    public bool ignoreRigidbodies;

    [Space]
    [Header ("Surface In Front Detection Settings")]
    [Space]

    public bool checkSurfaceForLowerBodyEnabled = true;

    public bool useHangStateOnNoSurfaceOnLowerBody = true;

    public float lowerBodyRaycastOffset = 0.5f;

    public float upperBodyRaycastOffset = 1.5f;

    [Space]
    [Header ("Surface In Top And Bottom Detection Settings")]
    [Space]

    public bool checkSurfaceAbovePlayerEnabled = true;

    public float abovePlayerRaycastOffset = 0.5f;

    public bool checkSurfaceBelowPlayerEnabled = true;

    public float belowPlayerRaycastOffset = -0.2f;

    [Space]
    [Header ("Surface Sides Detection Settings")]
    [Space]

    public bool checkSidesEnabled = true;
    public float raycastDistanceToCheckSides = 0.8f;

    public bool checkCloseSidesEnabled = true;

    public float raycastDistanceToCheckCloseSides = 0.7f;

    [Space]
    [Header ("Slide Down Settings")]
    [Space]

    public bool slideDownEnabled = true;
    public float slideDownSpeedThirdPerson = 10;
    public float SlideDownSpeedFirstPerson = 10;

    [Space]
    [Header ("Other Settings")]
    [Space]

    public bool useMovementCurve = true;

    public AnimationCurve mainMovementCurve;

    public bool useVelocityCurve = true;

    public AnimationCurve mainVelocityCurve;

    public bool stopClimbAtMinAngleOnSurface = true;
    public float minAngleToStopClimb = -50;

    public bool stopClimbAtMaxAngleOnSurface = true;
    public float maxAngleToStopClimb = 50;

    public float minTimeAngleLimitReachedToStopClimb = 1;

    public bool disableClimbStateOnDamageReceivedEnabled;

    [Space]
    [Header ("Climb Settings")]
    [Space]

    public float climbMovementSpeedThirdPerson = 5;
    public float climbMovementSpeedFirstPerson = 5;

    public float climbVelocityThirdPerson = 5;
    public float climbVelocityFirstPerson = 5;

    public float climbRotationSpeedThirdPerson = 200;
    public float climbRotationSpeedFirstPerson = 100;

    [Space]
    [Header ("Speed Settings")]
    [Space]

    public float climbTurboSpeed = 2;

    public Vector3 impulseOnJump;

    public float maxVelocityChangeSlide;

    public float jumpRotationSpeedThirdPerson = 1;
    public float jumpRotationSpeedFirstPerson = 0.5f;

    [Space]
    [Header ("Weapons Settings")]
    [Space]

    public bool keepWeapons;
    public bool drawWeaponsIfCarriedPreviously;

    [Space]
    [Header ("Third Person Settings")]
    [Space]

    public int actionID = 5654231;

    public string freeClimbAnimatorName = "Free Climb";

    public bool checkIfClimbHangOnAirForAction = true;
    public int freeClimbHangFromAirActionID = 5654232;

    public bool checkIfClimbHangOnGroundForAction = true;
    public int freeClimbHangFromGroundActionID = 5654233;

    public float minDelayToUseMovementInputOnClimbStart = 0.5f;

    public string externalControlleBehaviorActiveAnimatorName = "External Behavior Active";
    public string actionIDAnimatorName = "Action ID";

    public string horizontalAnimatorName = "Horizontal Action";
    public string verticalAnimatorName = "Vertical Action";

    public float inputLerpSpeed = 3;

    [Space]
    [Header ("Third Person Camera State Settings")]
    [Space]

    public bool setNewCameraStateOnThirdPerson;

    public string newCameraStateOnThirdPerson;

    [Space]
    [Header ("Climb Top Part Settings")]
    [Space]

    public Transform climbSurfaceActionSystemTransform;

    public float maxAngleDifferenceOnSurfaceToClimb = 20;

    public float checkTopSurfaceRaycastOffset = 1.3f;

    public float climbSurfaceTopSpeed = 2;

    [Space]
    [Space]

    public eventParameters.eventToCallWithGameObject eventToActivateClimbSurfaceActionSystem;

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;

    public bool checkIfDetectClimbActive;

    public bool climbActive;

    public bool isFirstPersonActive;

    public bool carryingWeaponsPreviously;

    public bool climbPaused;

    public bool activateClimbStateOnNextSurfaceDetectedActive;

    public bool slidingDownActive;

    public bool activateAutoSlideDownOnSurface;

    public bool slidingDownResult;

    public bool playerParentAssigned;

    public bool adjustingPlayerToSurfaceCollisionDetection;

    [Space]
    [Header ("Surfaces Detected Debug")]
    [Space]

    public float surfaceAngle;

    public bool surfaceFoundOnSide;

    public bool surfaceFoundOnCloseSide;

    public bool surfaceFoundOnRegularRaycast;

    public bool upperBodySurfaceDetected;

    public bool lowerBodySurfaceDetected;

    public bool surfaceDetectedAbovePlayer;

    public bool surfafeDetectedBelowPlayer;

    public int climbContactCount;

    public Vector3 contactNormal;

    public Vector3 lastClimbNormal;

    public float angleDifferenceWithSurface;

    public bool surfaceDetectedOnMovementInputRaycast;

    [Space]
    [Header ("Movement Input Debug")]
    [Space]

    public float currentVerticalMovement;

    public float currentHorizontalMovement;

    public Vector2 rawAxisValues;

    public bool moving;

    public bool movementInputPressed;

    public bool climbStateActivatedByInput;

    public bool movementInputPausedActive;

    public bool movementInputDisabledOnSurface;

    public bool cancelMovementInputActive;

    public bool turboActive;

    public bool pauseTurboActive;

    [Space]
    [Header ("Gizmo Settings")]
    [Space]

    public bool showGizmo;

    [Space]
    [Header ("First Person Events Settings")]
    [Space]

    public bool useEventsOnFirstPerson;

    public UnityEvent eventOnStartFirstPerson;
    public UnityEvent eventOnEndFirstPerson;

    [Space]
    [Header ("Third Person Events Settings")]
    [Space]

    public bool useEventsOnThirdPerson;

    public UnityEvent eventOnStartThirdPerson;
    public UnityEvent eventOnEndThirdPerson;

    [Space]
    [Header ("Other Events")]
    [Space]

    public UnityEvent eventOnCheckForClimbStateActive;
    public UnityEvent eventOnStopCheckForClimbStateActive;

    [Space]
    [Space]

    public UnityEvent eventOnClimbStateActive;
    public UnityEvent eventOnStopClimbStateActive;

    [Space]

    public UnityEvent eventBeforeCheckingForClimbStateActive;

    [Space]
    [Header ("Turbo Events")]
    [Space]

    public bool useEventOnTurbo;
    public UnityEvent eventOnStartTurbo;
    public UnityEvent eventOnEndTurbo;
    public UnityEvent eventOnRegularClimbSpeed;

    [Space]
    [Header ("Components")]
    [Space]

    public playerController mainPlayerController;
    public Transform playerTransform;
    public Rigidbody mainRigidbody;
    public Transform playerCameraTransform;
    public playerCamera mainPlayerCamera;
    public playerWeaponsManager mainPlayerWeaponsManager;

    public Animator mainAnimator;

    public headTrack mainHeadTrack;


    Coroutine resetPlayerCoroutine;

    Coroutine resetCameraCoroutine;

    bool originalClimbEnabled;

    Vector3 playerTransformUp;
    Vector3 playerTransformForward;

    RaycastHit hit;

    Vector3 velocityChange;

    int externalControlleBehaviorActiveAnimatorID;

    int actionIDAnimatorID;

    int horizontalAnimatorID;

    int verticalAnimatorID;

    int freeClimbAnimatorID;

    bool jumpInputUsed;

    Coroutine jumpCoroutine;

    float currentClimbRotationSpeed;

    float currentClimbMovementSpeed;

    float currentClimbVelocity;

    float currentClimbTurboSpeed;

    string previousCameraState = "";

    bool cameraOnThirdPersonOnLastClimbActiveState;


    float lastTimeMinAngleOnSurfaceReached;
    float lastTimeMaxAngleOnSurfaceReached;

    Vector3 lastSideDirection;
    Vector3 lastSideNormal;


    bool resetAnimatorIDValue;

    float lastTimeClimbActive;

    RaycastHit currentSurfaceHit;

    GameObject currentSurfaceDetected;
    GameObject previousSurfaceDetected;

    bool checkTagsToIgnoreOnManualClimbInputActive;

    int currentIDValue = -1;

    bool firstPersonChecked;

    bool thirdPersonChecked;

    float lastTimeActivateClimbStateOnNextSurfaceDetectedActive;

    Coroutine checkIfSurfaceDetectedInFrontCoroutine;

    float lastTimeSurfaceNotFound = 0;

    bool climbingOnTopInProcess;

    float lastTimeCollisionDetectionWithSurfaceNotFound = 0;

    Vector3 positionToAdjustPlayerToSurfaceCollision = Vector3.zero;

    bool isPlayerMovingOn3dWorld;

    bool increaseClimbSpeedActive;


    void Start ()
    {
        originalClimbEnabled = climbEnabled;

        externalControlleBehaviorActiveAnimatorID = Animator.StringToHash (externalControlleBehaviorActiveAnimatorName);
        actionIDAnimatorID = Animator.StringToHash (actionIDAnimatorName);

        horizontalAnimatorID = Animator.StringToHash (horizontalAnimatorName);

        verticalAnimatorID = Animator.StringToHash (verticalAnimatorName);

        freeClimbAnimatorID = Animator.StringToHash (freeClimbAnimatorName);
    }

    public override void checkChangeCameraViewStateOnExternalControllerBehavior ()
    {
        if (climbActive) {
            isFirstPersonActive = mainPlayerController.isPlayerOnFirstPerson ();

            bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

            if (isFullBodyAwarenessActive) {
                mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (true);

                mainPlayerCamera.setPivotCameraTransformParentCurrentTransformToFollow ();
            } else {
                if (isFirstPersonActive) {

                } else {
                    if (mainPlayerCamera.isFullBodyAwarenessEnabled ()) {
                        mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (false);

                        mainPlayerCamera.setPivotCameraTransformOriginalParent ();

                        mainPlayerCamera.resetPivotCameraTransformLocalRotation ();

                        setCameraState (true);
                    }
                }
            }
        }
    }

    void setCameraState (bool state)
    {
        if (useCameraOffsetOnClimbActiveEnabled) {
            mainPlayerCamera.setCameraPositionOffsetActiveState (state, cameraOffsetOnClimbActive, cameraOffsetOnClimbActiveLerpSpeed);

            mainPlayerCamera.setUpdatePlayerCameraPositionOnFixedUpdateActiveState (state);
        }

        if (setNewCameraStateOnThirdPerson && !isFirstPersonActive) {
            if (state) {
                if (previousCameraState == "") {
                    previousCameraState = mainPlayerCamera.getCurrentStateName ();

                    cameraOnThirdPersonOnLastClimbActiveState = !mainPlayerCamera.isFullBodyAwarenessActive ();
                }

                mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnThirdPerson);
            } else {

                if (previousCameraState != "") {
                    if (previousCameraState != newCameraStateOnThirdPerson) {
                        if (cameraOnThirdPersonOnLastClimbActiveState) {
                            if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                                previousCameraState = "";
                            }
                        } else {
                            if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                                previousCameraState = mainPlayerCamera.getDefaultThirdPersonStateName ();
                            }
                        }

                        if (previousCameraState != "") {
                            mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);
                        }
                    }

                    previousCameraState = "";
                }
            }
        }
    }

    public override void updateControllerBehavior ()
    {
        if (climbActive) {
            if (resetAnimatorIDValue) {
                if (Time.time > lastTimeClimbActive + 0.3f) {
                    mainAnimator.SetInteger (actionIDAnimatorID, 0);

                    resetAnimatorIDValue = false;
                }
            }

            if (movementInputPausedActive) {
                if (Time.time > lastTimeClimbActive + minDelayToUseMovementInputOnClimbStart) {

                    movementInputPausedActive = false;
                }
            }

            float currentFixedUpdateDeltaTime = Time.fixedDeltaTime;

            playerTransformUp = playerTransform.up;

            playerTransformForward = playerTransform.forward;

            Vector3 currentRaycastPosition = playerTransform.position + playerTransformUp;
            Vector3 currentRaycastDirection = playerTransformForward;

            currentVerticalMovement = mainPlayerController.getVerticalInput ();
            currentHorizontalMovement = mainPlayerController.getHorizontalInput ();

            isPlayerMovingOn3dWorld = mainPlayerController.isPlayerMovingOn3dWorld ();

            if (movementInputPausedActive || movementInputDisabledOnSurface || adjustingPlayerToSurfaceCollisionDetection) {
                currentVerticalMovement = 0;
                currentHorizontalMovement = 0;
            }

            if (!isPlayerMovingOn3dWorld) {
                currentHorizontalMovement = 0;
            }

            rawAxisValues = mainPlayerController.getRawAxisValues ();

            if (rawAxisValues.x == 0) {
                if (Mathf.Abs (currentHorizontalMovement) > 0.001f) {

                    currentHorizontalMovement = 0;
                }
            }

            if (rawAxisValues.y == 0) {
                if (Mathf.Abs (currentVerticalMovement) > 0.001f) {

                    currentVerticalMovement = 0;
                }
            }

            if (useRaycastDetectionToClimb) {
                if (!updateRaycastDetectionState (currentRaycastPosition, currentRaycastDirection, currentFixedUpdateDeltaTime)) {
                    return;
                }
            }

            //##################################################################################################
            //Update climb state by collision detection

            else {
                if (!updateCollisionDetectionState (currentRaycastDirection, currentFixedUpdateDeltaTime)) {
                    return;
                }
            }

            checkSurfaceAngleToStopClimb ();

            if (!surfaceFoundOnSide && !adjustingPlayerToSurfaceCollisionDetection) {
                checkIfTopSurfaceReached ();
            }

            checkTopBottomAndFrontSurfaces ();
        }
    }

    //COLLISION DETECTION METHOD
    bool updateCollisionDetectionState (Vector3 currentRaycastDirection, float currentFixedUpdateDeltaTime)
    {
        updateCheckCameraViewState ();


        currentClimbMovementSpeed = climbMovementSpeedThirdPerson;

        if (isFirstPersonActive) {
            currentClimbMovementSpeed = climbMovementSpeedFirstPerson;
        }

        currentClimbRotationSpeed = climbRotationSpeedThirdPerson;

        if (isFirstPersonActive) {
            currentClimbRotationSpeed = climbRotationSpeedFirstPerson;
        }

        currentClimbVelocity = climbVelocityThirdPerson;

        if (isFirstPersonActive) {
            currentClimbVelocity = climbVelocityFirstPerson;
        }

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            currentClimbVelocity = slideDownSpeedThirdPerson;

            if (isFirstPersonActive) {
                currentClimbVelocity = SlideDownSpeedFirstPerson;
            }
        }

        if (movementInputPausedActive || movementInputDisabledOnSurface || adjustingPlayerToSurfaceCollisionDetection) {
            rawAxisValues = Vector2.zero;
        }

        checkCurrentSurfaceDetected ();

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (!movementInputPausedActive && !cancelMovementInputActive && surfaceDetectedOnMovementInputRaycast) {
                currentVerticalMovement = -1;

                rawAxisValues.y = currentVerticalMovement;

                if (currentHorizontalMovement != 0) {
                    currentHorizontalMovement /= 2;
                }
            }
        }

        if (cancelMovementInputActive) {
            currentVerticalMovement = 0;

            currentHorizontalMovement = 0;

            rawAxisValues = Vector2.zero;
        }

        if (surfaceDetectedAbovePlayer) {
            currentVerticalMovement = Mathf.Clamp (currentVerticalMovement, -1, 0);

            rawAxisValues.y = Mathf.Clamp (rawAxisValues.y, -1, 0);
        }

        if (surfafeDetectedBelowPlayer) {
            currentVerticalMovement = Mathf.Clamp (currentVerticalMovement, 0, 1);

            rawAxisValues.y = Mathf.Clamp (rawAxisValues.y, 0, 1);
        }

        slidingDownResult = false;

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (currentVerticalMovement != 0) {
                if (currentIDValue != 1) {
                    currentIDValue = 1;

                    mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                    mainPlayerController.updateIdleIDOnAnimator ();
                }

                slidingDownResult = true;
            } else {
                if (currentIDValue != 0) {
                    currentIDValue = 0;

                    mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                    mainPlayerController.updateIdleIDOnAnimator ();
                }
            }
        } else {
            if (currentIDValue != 0) {
                currentIDValue = 0;

                mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                mainPlayerController.updateIdleIDOnAnimator ();
            }
        }


        moving = (Mathf.Abs (currentVerticalMovement) > 0.01f || Mathf.Abs (currentHorizontalMovement) > 0.01f);

        movementInputPressed = rawAxisValues != Vector2.zero;

        currentClimbTurboSpeed = 1;

        pauseTurboActive = false;

        bool enougMovementInput = (Mathf.Abs (currentVerticalMovement) > 0.7f || Mathf.Abs (currentHorizontalMovement) > 0.7f);

        if (increaseClimbSpeedActive && movementInputPressed && enougMovementInput && !slidingDownActive && !activateAutoSlideDownOnSurface) {

            if (surfaceDetectedOnMovementInputRaycast && angleDifferenceWithSurface < 3) {
                currentClimbTurboSpeed = climbTurboSpeed;
            } else {
                pauseTurboActive = true;
            }

            if (!turboActive) {
                mainPlayerController.setCurrentAirSpeedValue (2);

                turboActive = true;

                checkEventsOnTurbo (true);
            }
        } else {
            if (turboActive) {
                mainPlayerController.setCurrentAirSpeedValue (1);

                turboActive = false;

                mainPlayerController.stopShakeCamera ();

                checkEventsOnTurbo (false);

                eventOnRegularClimbSpeed.Invoke ();
            }
        }




        if (adjustingPlayerToSurfaceCollisionDetection) {
            //adjust player to surface before moving
            if (positionToAdjustPlayerToSurfaceCollision == Vector3.zero) {
                Vector3 mainRaycastPosition = playerTransform.position + playerTransform.up - playerTransform.forward * 0.3f;

                if (Physics.Raycast (mainRaycastPosition, currentRaycastDirection, out hit, 2, raycastLayermask)) {

                    Vector3 targetPosition = hit.point + adjustToFoundSurfaceOffset * hit.normal;



                    float currentPlayerAngleWithSurface = Vector3.SignedAngle (playerTransform.forward, -hit.normal, playerTransform.right);

                    float currentPlayerAngleWithSurfaceABS = Mathf.Abs (currentPlayerAngleWithSurface);

                    //					print (currentPlayerAngleWithSurface);

                    if (currentPlayerAngleWithSurfaceABS > 8) {
                        Vector3 playerTransformDown = -playerTransform.up;

                        playerTransformDown = Quaternion.AngleAxis (currentPlayerAngleWithSurface, playerTransform.right) * playerTransformDown;

                        playerTransformDown.Normalize ();

                        //						print (playerTransformDown);

                        targetPosition += playerTransformDown;

                        Debug.DrawRay (hit.point + adjustToFoundSurfaceOffset * hit.normal, playerTransformDown * 10000, Color.black, 5);

                        //			print (currentPlayerAngleWithSurface);
                    } else {
                        targetPosition -= playerTransform.up;
                    }

                    positionToAdjustPlayerToSurfaceCollision = targetPosition;
                }
            } else {
                mainRigidbody.position =
                    Vector3.MoveTowards (mainRigidbody.position, positionToAdjustPlayerToSurfaceCollision,
                    adjustToFoundSurfaceSpeed * currentFixedUpdateDeltaTime);

                bool closeToSurfaceDetected = GKC_Utils.distance (mainRigidbody.position, positionToAdjustPlayerToSurfaceCollision) < 0.01f;

                if (Time.time > lastTimeClimbActive + minDelayToAdjustDetectionCollision && closeToSurfaceDetected) {
                    adjustingPlayerToSurfaceCollisionDetection = false;

                    positionToAdjustPlayerToSurfaceCollision = Vector3.zero;

                    Vector3 newPosition = mainRigidbody.position;

                    if (climbDetectionCollisionPositionOffset != Vector3.zero) {
                        newPosition += climbDetectionCollisionPositionOffset.x * (-playerTransform.right);
                        newPosition += climbDetectionCollisionPositionOffset.y * (-playerTransform.up);
                        newPosition += climbDetectionCollisionPositionOffset.z * (-playerTransform.forward);
                    }

                    mainClimbDetectionCollisionSystem.updateObjectPosition (newPosition);
                }
            }
        } else {
            Vector3 climbPosition = mainClimbDetectionCollisionSystem.getMainRigidbodyPosition ();

            if (climbDetectionCollisionPositionOffset != Vector3.zero) {
                climbPosition += climbDetectionCollisionPositionOffset.x * playerTransform.right;
                climbPosition += climbDetectionCollisionPositionOffset.y * playerTransform.up;
                climbPosition += climbDetectionCollisionPositionOffset.z * playerTransform.forward;
            }

            if (updateRigidbodyPositionSmoothlyOnDetectionCollision) {
                if (useMovementCurve) {
                    currentClimbMovementSpeed = mainMovementCurve.Evaluate (currentClimbMovementSpeed);
                }

                mainRigidbody.position = Vector3.Lerp (mainRigidbody.position, climbPosition, currentClimbTurboSpeed * currentClimbMovementSpeed * currentFixedUpdateDeltaTime);
            } else {
                mainRigidbody.position = climbPosition;
            }
        }

        //mainRigidbody.velocity = mainClimbDetectionCollisionSystem.getMainRigidbodyVelocity ();

        //mainAnimator.velocity could be used to move it with root motion used as velocity

        if (movementInputPressed) {
            mainPlayerController.setCurrentVelocityValue (mainRigidbody.linearVelocity);
        } else {
            mainPlayerController.setCurrentVelocityValue (Vector3.zero);
        }

        float currentHorizontalMovementAnimatorValue = currentHorizontalMovement;
        float currentVerticalMovementAnimatorValue = currentVerticalMovement;


        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (slidingDownResult) {
                currentVerticalMovementAnimatorValue = 0;
            }
        }

        //update player animation states
        mainAnimator.SetFloat (horizontalAnimatorID, currentHorizontalMovementAnimatorValue, inputLerpSpeed, currentFixedUpdateDeltaTime);
        mainAnimator.SetFloat (verticalAnimatorID, currentVerticalMovementAnimatorValue, inputLerpSpeed, currentFixedUpdateDeltaTime);


        RaycastHit currentClimbCollisionHit = new RaycastHit ();

        Vector3 collisionDetectionDirection = -mainClimbDetectionCollisionSystem.getLastClimbNormal ();

        if (lastClimbNormalValue != -Vector3.one) {
            if (!moving) {
                collisionDetectionDirection = lastClimbNormalValue;
            }
        }

        if (moving) {
            lastClimbNormalValue = -mainClimbDetectionCollisionSystem.getLastClimbNormal ();
        }

        if (Physics.Raycast (mainClimbDetectionCollisionSystem.getMainRigidbodyPosition (), collisionDetectionDirection, out currentClimbCollisionHit, 3, raycastLayermask)) {
            collisionDetectionDirection = -currentClimbCollisionHit.normal;
        } else {
            collisionDetectionDirection = playerTransform.forward;

            if (Physics.Raycast (mainClimbDetectionCollisionSystem.getMainRigidbodyPosition (), collisionDetectionDirection, out currentClimbCollisionHit, 3, raycastLayermask)) {
                collisionDetectionDirection = -currentClimbCollisionHit.normal;
            }
        }

        bool setRotationOnPlayerResult = true;

        angleDifferenceWithSurface = Vector3.Angle (playerTransform.forward, collisionDetectionDirection);

        if (angleDifferenceWithSurface < 3) {
            if (!moving) {
                currentClimbRotationSpeed *= 0.8f;
            }
        }

        if (angleDifferenceWithSurface < 1) {
            setRotationOnPlayerResult = false;
        }

        if (setRotationOnPlayerResult) {
            Quaternion targetRotation = Quaternion.LookRotation (collisionDetectionDirection);

            playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, targetRotation, currentClimbRotationSpeed * currentFixedUpdateDeltaTime);
        }

        if (rotateCameraToPlayerRotationOnClimbActiveEnabled) {
            Vector3 myForwardCamera = Vector3.Cross (playerCameraTransform.right, playerTransform.up);
            Quaternion dstRotCamera = Quaternion.LookRotation (myForwardCamera, playerTransform.up);

            playerCameraTransform.rotation = Quaternion.Lerp (playerCameraTransform.rotation, dstRotCamera, currentClimbRotationSpeed * currentFixedUpdateDeltaTime);
        }

        if (Time.time > lastTimeClimbActive + 0.5f && !adjustingPlayerToSurfaceCollisionDetection) {
            if (climbContactCount > 0) {
                lastTimeCollisionDetectionWithSurfaceNotFound = 0;

                //				print ("DETECTED " + climbContactCount);
            } else {
                //				print ("NO DETECTED " + climbContactCount);

                if (lastTimeCollisionDetectionWithSurfaceNotFound == 0) {
                    lastTimeCollisionDetectionWithSurfaceNotFound = Time.time;

                } else {
                    if (lastTimeCollisionDetectionWithSurfaceNotFound != 0 && Time.time > lastTimeCollisionDetectionWithSurfaceNotFound + 1.2f) {
                        Vector3 lastContactNormal = contactNormal;

                        if (lastContactNormal == Vector3.zero) {
                            lastContactNormal = mainClimbDetectionCollisionSystem.getLastClimbNormal ();
                        }

                        if (lastContactNormal == Vector3.zero) {
                            lastContactNormal = Vector3.up;
                        }

                        Vector3 currentNormal = getCurrentNormal ();

                        float topSurfaceAngle = Vector3.Angle (currentNormal, lastContactNormal);

                        float topSurfaceAngleABS = Mathf.Abs (topSurfaceAngle);

                        if (showDebugPrint) {
                            print ("stop climb due to collision dropped, checking surface on top" + topSurfaceAngleABS + " " + lastContactNormal);
                        }

                        bool groundLocated = false;

                        if (topSurfaceAngleABS < 20) {
                            if (topSurfaceAngleABS > 10) {
                                groundLocated = true;
                            }
                        }

                        if (!groundLocated) {
                            if (Physics.Raycast (mainClimbDetectionCollisionSystem.getMainRigidbodyPosition (), -getCurrentNormal (), out currentClimbCollisionHit, 1, raycastLayermask)) {
                                groundLocated = true;

                                if (showDebugPrint) {
                                    print ("ground detected");
                                }
                            }
                        }

                        if (groundLocated) {
                            stopClimbToWalkOnSurfaceActive = true;

                            mainAnimator.CrossFadeInFixedTime (stopClimbToWalkOnSurfaceActionName, 0.1f);
                        }

                        lastTimeSurfaceNotFound = 0;

                        stopClimbAndDetectState ();

                        return false;
                    }
                }
            }
        }

        return true;
    }

    Vector3 lastClimbNormalValue = -Vector3.one;

    public void updateClimbContactCount (int newValue)
    {
        climbContactCount = newValue;
    }

    public void updateContactNormal (Vector3 newValue)
    {
        contactNormal = newValue;
    }

    public void updateLastClimbNormal (Vector3 newValue)
    {
        lastClimbNormal = newValue;
    }

    void updateCheckCameraViewState ()
    {
        //check change of view state
        bool isPlayerOnFirstPerson = mainPlayerController.isPlayerOnFirstPerson ();

        if (isPlayerOnFirstPerson) {
            if (!firstPersonChecked) {
                if (showDebugPrint) {
                    print ("checking swim state on first person");
                }

                if (thirdPersonChecked) {
                    //						mainPlayerController.resetAnimator ();

                    if (showDebugPrint) {
                        print ("reset animator state for first person");
                    }
                }

                firstPersonChecked = true;

                thirdPersonChecked = false;

                mainPlayerController.setPausePlayerRotateToCameraDirectionOnFirstPersonActiveState (true);
            }
        } else {
            if (!thirdPersonChecked) {
                if (showDebugPrint) {
                    print ("checking swim state on third person");
                }

                if (firstPersonChecked) {
                    //						mainPlayerController.resetAnimator ();

                    if (showDebugPrint) {
                        print ("reset animator state for first person");
                    }

                    mainPlayerController.setOnGroundState (false);

                    mainPlayerController.setOnGroundAnimatorIDValueWithoutCheck (false);

                    mainPlayerController.setFootStepManagerState (true);

                    mainAnimator.Play (freeClimbAnimatorID);

                    //						mainAnimator.SetInteger (actionIDAnimatorID, actionID);

                    mainAnimator.SetBool (externalControlleBehaviorActiveAnimatorID, true);

                    lastTimeClimbActive = Time.time;

                    resetAnimatorIDValue = true;
                }

                thirdPersonChecked = true;

                firstPersonChecked = false;

                mainPlayerController.setPausePlayerRotateToCameraDirectionOnFirstPersonActiveState (false);
            }
        }
    }

    void checkSurfaceAngleToStopClimb ()
    {
        if (useRaycastDetectionToClimb) {
            //check surface angle to stop the climb
            surfaceAngle = 0;

            Vector3 currentRaycastPosition = playerTransform.position + playerTransform.up;
            Vector3 currentRaycastDirection = playerTransform.forward;

            if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, raycastDistance, raycastLayermask)) {
                surfaceAngle = Vector3.SignedAngle (playerTransform.up, getCurrentNormal (), playerTransform.right);
            }

            if (surfaceAngle != 0 && !surfaceFoundOnSide && !surfaceFoundOnCloseSide) {
                if (stopClimbAtMinAngleOnSurface) {
                    if (surfaceAngle < 0) {
                        if (surfaceAngle < minAngleToStopClimb) {
                            if (lastTimeMinAngleOnSurfaceReached == 0) {
                                lastTimeMinAngleOnSurfaceReached = Time.time;
                            }
                        } else {
                            lastTimeMinAngleOnSurfaceReached = 0;
                        }
                    }
                }

                if (stopClimbAtMaxAngleOnSurface) {
                    if (surfaceAngle > 0) {
                        if (surfaceAngle > maxAngleToStopClimb) {
                            if (lastTimeMaxAngleOnSurfaceReached == 0) {
                                lastTimeMaxAngleOnSurfaceReached = Time.time;
                            }
                        } else {
                            lastTimeMaxAngleOnSurfaceReached = 0;
                        }
                    }
                }

                if (lastTimeMinAngleOnSurfaceReached != 0) {
                    if (Time.time > lastTimeMinAngleOnSurfaceReached + minTimeAngleLimitReachedToStopClimb) {
                        stopClimbAndDetectState ();

                        return;
                    }
                }

                if (lastTimeMaxAngleOnSurfaceReached != 0) {
                    if (Time.time > lastTimeMaxAngleOnSurfaceReached + minTimeAngleLimitReachedToStopClimb) {
                        stopClimbAndDetectState ();

                        return;
                    }
                }
            }
        } else {

        }
    }

    public bool isSlidingDownActive ()
    {
        return slidingDownActive;
    }

    public bool isPlayerMoving ()
    {
        return moving;
    }

    public float getCurrentClimbTurboSpeed ()
    {
        return currentClimbTurboSpeed;
    }

    public bool isTurboActive ()
    {
        return turboActive;
    }

    public bool isPauseTurboActive ()
    {
        return pauseTurboActive;
    }

    public Vector3 getCurrentNormal ()
    {
        return mainPlayerController.getCurrentNormal ();
    }

    void checkTopBottomAndFrontSurfaces ()
    {
        if (useRaycastDetectionToClimb) {
            if (checkSurfaceForLowerBodyEnabled) {
                Vector3 raycastPosition = playerTransform.position + (lowerBodyRaycastOffset * playerTransform.up);
                Vector3 raycastDirection = playerTransform.forward;

                if (useHangStateOnNoSurfaceOnLowerBody) {
                    if (Physics.Raycast (raycastPosition, raycastDirection, 1.1f, raycastLayermask)) {
                        if (!lowerBodySurfaceDetected) {
                            mainPlayerController.setCurrentAirIDValue (-1);

                            lowerBodySurfaceDetected = true;
                        }

                        if (showGizmo) {
                            Debug.DrawRay (raycastPosition, 2 * raycastDirection, Color.green);
                        }
                    } else {
                        if (lowerBodySurfaceDetected) {
                            mainPlayerController.setCurrentAirIDValue (0);

                            lowerBodySurfaceDetected = false;
                        }

                        if (showGizmo) {
                            Debug.DrawRay (raycastPosition, 2 * raycastDirection, Color.black);
                        }
                    }
                } else {
                    if (!lowerBodySurfaceDetected) {
                        mainPlayerController.setCurrentAirIDValue (-1);

                        lowerBodySurfaceDetected = true;
                    }
                }

                raycastPosition = playerTransform.position + (upperBodyRaycastOffset * playerTransform.up);
                raycastDirection = playerTransform.forward;

                upperBodySurfaceDetected = false;

                if (Physics.Raycast (raycastPosition, raycastDirection, 1.1f, raycastLayermask)) {
                    upperBodySurfaceDetected = true;

                    if (showGizmo) {
                        Debug.DrawRay (raycastPosition, 2 * raycastDirection, Color.green);
                    }
                } else {
                    if (showGizmo) {
                        Debug.DrawRay (raycastPosition, 2 * raycastDirection, Color.black);
                    }
                }
            }

            if (checkSurfaceAbovePlayerEnabled) {
                Vector3 raycastPosition = playerTransform.position + (abovePlayerRaycastOffset * playerTransform.up);
                Vector3 raycastDirection = playerTransform.up;

                RaycastHit abovePlayerHit;

                if (Physics.Raycast (raycastPosition, raycastDirection, out abovePlayerHit, 0.9f, raycastLayermask)) {

                    float currentAngle = Vector3.SignedAngle (abovePlayerHit.normal, -playerTransform.up, playerTransform.right);

                    if (Mathf.Abs (currentAngle) < 25) {
                        surfaceDetectedAbovePlayer = true;
                    } else {
                        surfaceDetectedAbovePlayer = false;
                    }

                } else {
                    surfaceDetectedAbovePlayer = false;
                }
            }

            if (checkSurfaceBelowPlayerEnabled) {
                Vector3 raycastPosition = playerTransform.position + (belowPlayerRaycastOffset * playerTransform.up);
                Vector3 raycastDirection = -playerTransform.up;

                RaycastHit belowPlayerHit;

                if (Physics.Raycast (raycastPosition, raycastDirection, out belowPlayerHit, 0.9f, raycastLayermask)) {

                    float currentAngle = Vector3.SignedAngle (belowPlayerHit.normal, playerTransform.up, playerTransform.right);

                    if (Mathf.Abs (currentAngle) < 25) {
                        surfafeDetectedBelowPlayer = true;
                    } else {
                        surfafeDetectedBelowPlayer = false;
                    }

                } else {
                    surfafeDetectedBelowPlayer = false;
                }
            }
        }
    }

    void checkIfTopSurfaceReached ()
    {
        Vector3 currentNormal = getCurrentNormal ();

        Vector3 raycastPosition = playerTransform.position + (checkTopSurfaceRaycastOffset * playerTransform.up);

        Vector3 playerForwardDirection = playerTransform.forward;

        float currentPlayerAngleWithSurface = Vector3.SignedAngle (playerTransform.up, getCurrentNormal (), playerTransform.right);

        float currentPlayerAngleWithSurfaceABS = Mathf.Abs (currentPlayerAngleWithSurface);

        if (currentPlayerAngleWithSurfaceABS > 5) {
            playerForwardDirection = Quaternion.AngleAxis (currentPlayerAngleWithSurface, playerTransform.right) * playerForwardDirection;

            Debug.DrawRay (raycastPosition, playerForwardDirection * 1000, Color.red);

            //			print (currentPlayerAngleWithSurface);
        }


        Vector3 raycastDirection = playerForwardDirection;

        //		print (raycastDirection);

        Vector3 climbSurfaceTargetPosition = Vector3.zero;

        RaycastHit upperHit = new RaycastHit ();
        RaycastHit lowerHit = new RaycastHit ();

        bool surfaceToLedgeDetected = false;

        Debug.DrawRay (raycastPosition, raycastDirection, Color.green);

        if (!Physics.Raycast (raycastPosition, raycastDirection, out upperHit, 2, raycastLayermask) &&
            Physics.Raycast (playerTransform.position, raycastDirection, out lowerHit, 2, raycastLayermask)) {

            //if not surface is found, then
            if (showGizmo) {
                Debug.DrawRay (raycastPosition, raycastDirection, Color.green);
            }

            //search for the closest point surface of that ledge, by lowering the raycast position until a surface is found
            surfaceToLedgeDetected = false;

            RaycastHit newHit = new RaycastHit ();
            RaycastHit previousHit = new RaycastHit ();

            int numberOfLoops = 0;

            Vector3 newRaycastPosition = playerTransform.position + (0.5f * playerTransform.up) - 0.6f * playerTransform.forward;

            //			raycastDirection = Vector3.Cross (playerTransform.right, currentNormal);

            RaycastHit topSurfaceHit = new RaycastHit ();

            float topSurfaceAngle = -1;

            float currentRaycastDistance = 2.6f;

            if (currentPlayerAngleWithSurface < -8) {
                print ("looking down");

                currentRaycastDistance = 2;
            }

            raycastDirection = playerTransform.forward;

            while (!surfaceToLedgeDetected && numberOfLoops < 80) {
                if (Physics.Raycast (newRaycastPosition, raycastDirection, out newHit, currentRaycastDistance, raycastLayermask)) {
                    previousHit = newHit;

                    newRaycastPosition += 0.04f * playerTransform.up;

                    if (currentPlayerAngleWithSurface < -8) {
                        if (currentRaycastDistance > 1.5f) {
                            currentRaycastDistance -= 0.05f;
                        }
                    } else {
                        if (currentRaycastDistance > 2) {
                            if (newHit.distance > 2) {
                                currentRaycastDistance -= 0.06f;
                            }
                        }
                    }
                } else {
                    if (previousHit.point == Vector3.zero) {
                        print ("NO LOCATED");
                    } else {

                        climbSurfaceTargetPosition = previousHit.point + 0.04f * playerTransform.up;

                        surfaceToLedgeDetected = true;

                        if (Physics.Raycast (climbSurfaceTargetPosition + 0.2f * currentNormal, raycastDirection - playerTransform.up, out topSurfaceHit, 3.3f, raycastLayermask)) {
                            topSurfaceAngle = Vector3.Angle (currentNormal, topSurfaceHit.normal);
                        }
                    }
                }

                numberOfLoops++;
            }

            if (surfaceToLedgeDetected) {

                float angleWithSurface = Vector3.SignedAngle (currentNormal, previousHit.normal, playerTransform.right);

                bool surfaceAngleNotValid = false;

                float angleWithSurfaceAux = Mathf.Abs (Mathf.Abs (angleWithSurface) - 90);

                //				if (showDebugPrint) {
                //					print ("angle with surface " + angleWithSurface + " " + angleWithSurfaceAux);
                //				}

                if (angleWithSurfaceAux > maxAngleDifferenceOnSurfaceToClimb) {
                    surfaceAngleNotValid = true;
                }

                if (topSurfaceAngle != -1) {
                    //					print ("top surface angle " + topSurfaceAngle);

                    if (Mathf.Abs (topSurfaceAngle) > maxAngleDifferenceOnSurfaceToClimb) {
                        surfaceAngleNotValid = true;
                    }
                }

                if (!useRaycastDetectionToClimb) {

                    if (showDebugPrint) {
                        print ("surface valid to climb on top " + topSurfaceAngle + "  " + angleWithSurface + " " + previousHit.normal);
                    }

                    if (topSurfaceAngle != -1) {

                        float topSurfaceAngleABS = Mathf.Abs (topSurfaceAngle);

                        if (showDebugPrint) {
                            print ("Top surface angle is " + topSurfaceAngle);
                        }

                        bool stopClimbToWalkNormallyResult = false;

                        if (topSurfaceAngleABS > 30) {
                            surfaceAngleNotValid = true;
                        } else if (topSurfaceAngleABS < 20 && topSurfaceAngleABS > 10) {
                            stopClimbToWalkNormallyResult = true;
                        }

                        if (currentPlayerAngleWithSurfaceABS < 30) {
                            if (showDebugPrint) {
                                print ("checking if climb on top on low player rotation");
                            }

                            if (climbSurfaceTargetPosition != Vector3.zero) {
                                if (showDebugPrint) {
                                    print ("surface to climb target position by raycast located");
                                }

                                if (Mathf.Abs (topSurfaceAngle) <= maxAngleDifferenceOnSurfaceToClimb) {
                                    if (showDebugPrint) {
                                        print ("climb on top on low rotation checked properly, activating");
                                    }

                                    stopClimbToWalkNormallyResult = false;

                                    surfaceAngleNotValid = false;
                                }
                            }
                        }

                        if (stopClimbToWalkNormallyResult) {
                            if (showDebugPrint) {
                                print ("stop climb to walk normally");
                            }

                            stopClimbToWalkOnSurfaceActive = true;

                            mainAnimator.CrossFadeInFixedTime (stopClimbToWalkOnSurfaceActionName, 0.1f);

                            lastTimeSurfaceNotFound = 0;

                            stopClimbAndDetectState ();

                            surfaceAngleNotValid = true;
                        }
                    } else {
                        surfaceAngleNotValid = true;
                    }
                }

                if (climbSurfaceTargetPosition == Vector3.zero) {
                    surfaceAngleNotValid = true;
                }

                if (!surfaceAngleNotValid) {
                    if (showGizmo) {
                        Debug.DrawRay (newRaycastPosition, 4 * raycastDirection, Color.blue, 4);
                    }

                    isFirstPersonActive = mainPlayerController.isPlayerOnFirstPerson ();

                    Transform climbSurfaceActionSystemTransformParent = null;

                    if (mainPlayerController.isPlayerSetAsChildOfParent ()) {
                        climbSurfaceActionSystemTransformParent = mainPlayerController.getCurrentTemporalPlayerParent ();
                    }

                    climbSurfaceActionSystemTransform.SetParent (climbSurfaceActionSystemTransformParent);


                    actionSystem climbactionSystem = climbSurfaceActionSystemTransform.GetComponent<actionSystem> ();

                    if (climbactionSystem != null) {
                        climbactionSystem.setPlayerParentDuringActionActiveValues (climbSurfaceActionSystemTransformParent != null,
                            climbSurfaceActionSystemTransformParent);

                        if (isFirstPersonActive) {
                            climbactionSystem.setUseMovingPlayerToPositionTargetValues (true, climbSurfaceTopSpeed, 0);
                        } else {
                            //							if (climbSurfaceActionSystemTransformParent != null) {
                            //								climbactionSystem.setUseMovingPlayerToPositionTargetValues (true, climbSurfaceTopSpeed, 1);
                            //							} else {
                            climbactionSystem.setUseMovingPlayerToPositionTargetValues (false, 0, 0);
                            //							}
                        }
                    }

                    climbingOnTopInProcess = true;

                    stopClimbAndDetectState ();

                    climbSurfaceActionSystemTransform.position = climbSurfaceTargetPosition;

                    Quaternion targetRotation = Quaternion.LookRotation (-previousHit.normal);

                    climbSurfaceActionSystemTransform.rotation = targetRotation;

                    float extraAngleX = 0;

                    if (climbSurfaceActionSystemTransform.up != currentNormal) {
                        extraAngleX = Vector3.SignedAngle (climbSurfaceActionSystemTransform.up, currentNormal, climbSurfaceActionSystemTransform.right);
                    }

                    if (extraAngleX != 0) {
                        climbSurfaceActionSystemTransform.Rotate (new Vector3 (extraAngleX, 0, 0));
                    }

                    float extraAngleY = Vector3.SignedAngle (climbSurfaceActionSystemTransform.forward, playerForwardDirection, climbSurfaceActionSystemTransform.up);

                    if (Mathf.Abs (extraAngleY) > 5) {
                        climbSurfaceActionSystemTransform.Rotate (new Vector3 (0, extraAngleY, 0));
                    }

                    climbSurfaceActionSystemTransform.gameObject.SetActive (true);

                    eventToActivateClimbSurfaceActionSystem.Invoke (playerTransform.gameObject);

                    if (showDebugPrint) {
                        print ("surface to climb detected");
                    }

                }
            } else {
                if (showDebugPrint) {
                    print ("surface to climb not detected");
                }
            }
        } else {
            if (!useRaycastDetectionToClimb) {
                Vector3 lastContactNormal = contactNormal;

                if (climbContactCount > 0) {
                    lastContactNormal = mainClimbDetectionCollisionSystem.getLastClimbNormal ();
                }

                float topSurfaceAngle = Vector3.Angle (currentNormal, lastContactNormal);

                float topSurfaceAngleABS = Mathf.Abs (topSurfaceAngle);

                if (topSurfaceAngleABS < 20) {
                    if (topSurfaceAngleABS > 10) {
                        if (showDebugPrint) {
                            print ("surface valid to climb on top by stopping climb" + topSurfaceAngleABS + " " + lastContactNormal);
                        }

                        stopClimbToWalkOnSurfaceActive = true;

                        mainAnimator.CrossFadeInFixedTime (stopClimbToWalkOnSurfaceActionName, 0.1f);

                        lastTimeSurfaceNotFound = 0;

                        stopClimbAndDetectState ();

                        print ("stop climb to walk normally");
                    }
                }
            }
        }
    }

    public string stopClimbToWalkOnSurfaceActionName = "Stop Climb To Walk On Surface";

    bool stopClimbToWalkOnSurfaceActive;

    public override void setExtraImpulseForce (Vector3 forceAmount, bool useCameraDirection)
    {
        setClimbImpulseForce (forceAmount, useCameraDirection);
    }

    public void setClimbImpulseForce (Vector3 forceAmount, bool useCameraDirection)
    {
        Vector3 impulseForce = forceAmount;

        if (maxVelocityChangeSlide > 0) {
            velocityChange = impulseForce - mainRigidbody.linearVelocity;

            velocityChange = Vector3.ClampMagnitude (velocityChange, maxVelocityChangeSlide);

        } else {
            velocityChange = impulseForce;
        }

        mainPlayerController.setVelocityChangeValue (velocityChange);

        mainRigidbody.AddForce (velocityChange, ForceMode.VelocityChange);
    }

    void checkCurrentSurfaceDetected ()
    {
        if (!useClimbZoneDetectionRaycast) {
            return;
        }

        Vector3 currentRaycastPosition = playerTransform.position + playerTransform.up - (0.3f * playerTransform.forward);
        Vector3 currentRaycastDirection = playerTransform.forward;

        Vector2 movementInput = rawAxisValues;

        if (slidingDownResult) {
            movementInput.y = -1;
        }

        if (movementInput.x > 0) {
            currentRaycastPosition += movementInput.x * rightOffsetToDetectSurfaceOnMovementInput * playerTransform.right;
        } else if (movementInput.x < 0) {
            currentRaycastPosition += movementInput.x * leftOffsetToDetectSurfaceOnMovementInput * playerTransform.right;
        }

        if (movementInput.y > 0) {
            currentRaycastPosition += movementInput.y * upOffsetToDetectSurfaceOnMovementInput * playerTransform.up;
        } else if (movementInput.y < 0) {
            currentRaycastPosition += movementInput.y * downOffsetToDetectSurfaceOnMovementInput * playerTransform.up;
        }

        if (showGizmo) {
            Debug.DrawRay (currentRaycastPosition, 2 * currentRaycastDirection, Color.white);
        }

        if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out currentSurfaceHit, raycastDistance + 0.3f, raycastLayermask)) {
            currentSurfaceDetected = currentSurfaceHit.collider.gameObject;

            surfaceDetectedOnMovementInputRaycast = true;

            if (currentSurfaceDetected != previousSurfaceDetected) {
                previousSurfaceDetected = currentSurfaceDetected;

                if (avoidMovementTowardNoClimbZones) {
                    freeClimbZoneSystem currentFreeClimbZoneSystem = currentSurfaceDetected.GetComponent<freeClimbZoneSystem> ();

                    if (currentFreeClimbZoneSystem != null) {
                        cancelMovementInputActive = false;
                    } else {
                        cancelMovementInputActive = true;
                    }
                }

                if (avoidMovementTowardSurfacesToIgnore) {
                    if (surfacesToIgnoreTagsList.Contains (currentSurfaceDetected.tag)) {
                        if (stopClimbIfSurfaceToIgnoreDetected) {
                            stopClimbAndDetectState ();
                        } else {
                            cancelMovementInputActive = true;
                        }
                    } else {
                        cancelMovementInputActive = false;
                    }
                }
            }
        } else {
            if (currentSurfaceDetected != null) {
                currentSurfaceDetected = null;

                previousSurfaceDetected = null;

                cancelMovementInputActive = false;
            }

            surfaceDetectedOnMovementInputRaycast = false;
        }

        if (avoidMovementTowardSurfacesToIgnore) {
            if (!cancelMovementInputActive) {
                currentRaycastPosition = playerTransform.position + playerTransform.up - (0.3f * playerTransform.forward);

                movementInput = rawAxisValues;

                if (slidingDownResult) {
                    movementInput.y = -1;
                }

                if (movementInput.x > 0) {
                    currentRaycastDirection = movementInput.x * playerTransform.right;
                } else if (movementInput.x < 0) {
                    currentRaycastPosition = movementInput.x * playerTransform.right;
                }

                if (movementInput.y > 0) {
                    currentRaycastPosition = movementInput.y * playerTransform.up;
                } else if (movementInput.y < 0) {
                    currentRaycastPosition = movementInput.y * playerTransform.up;
                }

                if (showGizmo) {
                    Debug.DrawRay (currentRaycastPosition, 2 * currentRaycastDirection, Color.white);
                }

                if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out currentSurfaceHit, raycastDistance + 0.3f, raycastLayermask)) {
                    currentSurfaceDetected = currentSurfaceHit.collider.gameObject;

                    surfaceDetectedOnMovementInputRaycast = true;

                    if (currentSurfaceDetected != previousSurfaceDetected) {
                        previousSurfaceDetected = currentSurfaceDetected;

                        if (avoidMovementTowardNoClimbZones) {
                            freeClimbZoneSystem currentFreeClimbZoneSystem = currentSurfaceDetected.GetComponent<freeClimbZoneSystem> ();

                            if (currentFreeClimbZoneSystem == null) {
                                cancelMovementInputActive = true;
                            }
                        }

                        if (surfacesToIgnoreTagsList.Contains (currentSurfaceDetected.tag)) {
                            if (stopClimbIfSurfaceToIgnoreDetected) {
                                stopClimbAndDetectState ();
                            } else {
                                cancelMovementInputActive = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void stopClimbFromDamageReceived ()
    {
        if (!disableClimbStateOnDamageReceivedEnabled) {
            return;
        }

        if (!climbActive) {
            return;
        }

        stopClimbAndDetectState ();
    }

    void stopClimbAndDetectState ()
    {
        setClimbActiveState (false);

        if (climbStateActivatedByInput) {
            setCheckIfDetectClimbActiveState (false);
        }
    }

    //Traversal movement functions
    public override void setJumpActiveForExternalForce ()
    {
        setJumpActive (impulseOnJump);
    }

    public void setJumpActive (Vector3 newImpulseOnJumpAmount)
    {
        if (climbActive) {
            jumpInputUsed = true;

            setClimbActiveState (false);

            Vector3 totalForce = newImpulseOnJumpAmount.y * playerTransform.up + newImpulseOnJumpAmount.z * playerTransform.forward;

            mainPlayerController.useJumpPlatform (totalForce, ForceMode.Impulse);

            if (climbStateActivatedByInput) {
                setCheckIfDetectClimbActiveState (false);
            }

            rotateCharacterOnJump ();

            if (rotateCameraToPlayerRotationOnClimbActiveEnabled) {
                resetCameraRotation ();
            }
        }
    }

    public override void setExternalForceActiveState (bool state)
    {
        setClimbActiveState (state);
    }

    public void setMovementInputDisabledOnSurfaceState (bool state)
    {
        movementInputDisabledOnSurface = state;
    }

    public void setCheckIfDetectClimbActiveState (bool state)
    {
        if (!climbEnabled) {
            return;
        }

        if (!climbCheckCanBeUsed) {
            return;
        }

        if (checkIfDetectClimbActive == state) {
            return;
        }

        if (mainPlayerController.isUseExternalControllerBehaviorPaused ()) {
            return;
        }

        if (state) {
            externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();

            if (currentExternalControllerBehavior != null && currentExternalControllerBehavior != this) {
                if (canBeActivatedIfOthersBehaviorsActive && checkIfCanEnableBehavior (currentExternalControllerBehavior.behaviorName)) {
                    currentExternalControllerBehavior.disableExternalControllerState ();
                } else {
                    return;
                }
            }

            eventBeforeCheckingForClimbStateActive.Invoke ();
        }

        bool checkIfDetectClimbActivePrevioulsy = checkIfDetectClimbActive;

        checkIfDetectClimbActive = state;

        if (checkIfDetectClimbActive) {
            //			externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();
            //			
            //			if (currentExternalControllerBehavior != null && currentExternalControllerBehavior != this) {
            //				currentExternalControllerBehavior.disableExternalControllerState ();
            //			}
            //
            mainPlayerController.setExternalControllerBehavior (this);
        } else {
            if (checkIfDetectClimbActivePrevioulsy) {
                externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();

                if (currentExternalControllerBehavior == null || currentExternalControllerBehavior == this) {
                    mainPlayerController.setExternalControllerBehavior (null);
                }
            }
        }

        mainPlayerController.setFallDamageCheckPausedState (state);

        if (!checkIfDetectClimbActive) {
            setClimbActiveState (false);
        }

        if (checkIfDetectClimbActive) {
            eventOnCheckForClimbStateActive.Invoke ();
        } else {
            eventOnStopCheckForClimbStateActive.Invoke ();
        }

        if (!checkIfDetectClimbActive) {
            climbStateActivatedByInput = false;
        }

        //		if (checkIfDetectClimbActive) {
        //			if (activateClimbStateOnEnterTriggerClimbZone) {
        //				if (!ignoreActivateClimbStateOnEnterTriggerClimbZone) {
        //					setClimbActiveState (true);
        //
        //					climbStateActivatedByInput = true;
        //				}
        //
        //				activateClimbStateOnEnterTriggerClimbZone = false;
        //			}
        //		}
    }

    public void setClimbActiveState (bool state)
    {
        if (!climbEnabled) {
            return;
        }

        if (climbActive == state) {
            return;
        }


        if (state && climbPaused) {
            return;
        }

        climbActive = state;

        mainPlayerController.setAddExtraRotationPausedState (state);

        mainPlayerController.setExternalControlBehaviorForAirTypeActiveState (state);

        mainPlayerController.setFreeClimbActiveState (state);

        setBehaviorCurrentlyActiveState (state);

        setCurrentPlayerActionSystemCustomActionCategoryID ();

        bool playerOnGround = mainPlayerController.isPlayerOnGround ();

        if (state) {
            mainPlayerController.setCheckOnGroungPausedState (true);

            mainPlayerController.setPlayerOnGroundState (false);

            mainPlayerController.setPlayerOnGroundAnimatorStateOnOverrideOnGroundWithTime (false);

            mainPlayerController.overrideOnGroundAnimatorValue (0);

            mainPlayerController.setPlayerOnGroundAnimatorStateOnOverrideOnGround (false);

            mainPlayerController.setOnGroundAnimatorIDValue (false);

            mainPlayerController.setPlayerVelocityToZero ();

        } else {
            mainPlayerController.setCheckOnGroungPausedState (false);

            mainPlayerController.setPlayerOnGroundState (false);

            mainPlayerController.setPlayerOnGroundAnimatorStateOnOverrideOnGroundWithTime (true);

            mainPlayerController.disableOverrideOnGroundAnimatorValue ();

            mainPlayerController.setPauseResetAnimatorStateFOrGroundAnimatorState (true);

            if (jumpInputUsed) {
                mainPlayerController.setOnGroundAnimatorIDValue (false);
            } else {
                if (mainPlayerController.getCurrentSurfaceBelowPlayer () != null || stopClimbToWalkOnSurfaceActive) {

                    mainPlayerController.setPlayerOnGroundState (true);

                    mainPlayerController.setOnGroundAnimatorIDValue (true);
                }
            }

            stopClimbToWalkOnSurfaceActive = false;
        }

        mainPlayerController.setFootStepManagerState (state);

        if (showDebugPrint) {
            print ("Climb active state " + state);
        }

        isFirstPersonActive = mainPlayerController.isPlayerOnFirstPerson ();

        if (climbActive) {
            checkEventsOnStateChange (true);

            if (!isFirstPersonActive) {
                bool playMainAction = true;

                if (checkIfClimbHangOnAirForAction) {
                    if (!playerOnGround) {
                        mainAnimator.SetInteger (actionIDAnimatorID, freeClimbHangFromAirActionID);

                        playMainAction = false;
                    }
                }

                if (checkIfClimbHangOnGroundForAction) {
                    if (playerOnGround) {
                        mainAnimator.SetInteger (actionIDAnimatorID, freeClimbHangFromGroundActionID);

                        playMainAction = false;
                    }
                }

                if (playMainAction) {
                    mainAnimator.SetInteger (actionIDAnimatorID, actionID);
                }
            }

            mainAnimator.SetBool (externalControlleBehaviorActiveAnimatorID, state);

            mainPlayerController.setJumpsAmountValue (0);

            //			mainPlayerCamera.enableOrDisableChangeCameraView (false);

            if (!isFirstPersonActive) {
                if (keepWeapons) {
                    carryingWeaponsPreviously = mainPlayerWeaponsManager.isPlayerCarringWeapon ();

                    if (carryingWeaponsPreviously) {
                        mainPlayerWeaponsManager.checkIfDisableCurrentWeapon ();
                    }

                    mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (false);
                }
            }
        } else {
            checkEventsOnStateChange (false);

            if (!isFirstPersonActive) {
                mainAnimator.SetBool (externalControlleBehaviorActiveAnimatorID, state);

                mainAnimator.SetInteger (actionIDAnimatorID, 0);
            }

            //			mainPlayerCamera.setOriginalchangeCameraViewEnabledValue ();

            if (keepWeapons) {
                mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (true);
            }

            if (carryingWeaponsPreviously) {
                if (!drawWeaponsIfCarriedPreviously) {
                    carryingWeaponsPreviously = false;
                }
            }
        }

        if (mainHeadTrack != null) {
            mainHeadTrack.setHeadTrackSmoothPauseState (climbActive);
        }

        //		mainPlayerCamera.setPausePlayerCameraViewChangeState (climbActive);

        mainPlayerController.setLastTimeFalling ();

        mainPlayerCamera.stopShakeCamera ();

        mainPlayerController.setPauseCameraShakeFromGravityActiveState (state);

        if (!climbActive) {
            if (!jumpInputUsed) {
                if (playerTransform.up != getCurrentNormal ()) {
                    if (!climbingOnTopInProcess || isFirstPersonActive) {
                        resetPlayerRotation ();
                    }

                    if (rotateCameraToPlayerRotationOnClimbActiveEnabled) {
                        resetCameraRotation ();
                    }
                }
            }
        }

        climbingOnTopInProcess = false;

        setCameraState (state);

        jumpInputUsed = false;

        surfaceFoundOnSide = false;

        surfaceFoundOnCloseSide = false;

        lastClimbNormalValue = -Vector3.one;

        if (state) {
            eventOnClimbStateActive.Invoke ();
        } else {
            eventOnStopClimbStateActive.Invoke ();
        }

        if (state) {
            eventOnRegularClimbSpeed.Invoke ();

        } else {
            if (turboActive || increaseClimbSpeedActive) {
                mainPlayerController.setCurrentAirSpeedValue (1);

                turboActive = false;

                increaseClimbSpeedActive = false;
            }

            mainPlayerController.setOriginalAirIDValue ();
        }

        lowerBodySurfaceDetected = false;

        upperBodySurfaceDetected = false;

        surfaceDetectedAbovePlayer = false;

        surfafeDetectedBelowPlayer = false;

        lastTimeClimbActive = Time.time;

        resetAnimatorIDValue = state;

        if (activateClimbStateOnNextSurfaceDetectedActive) {
            setActivateClimbStateOnNextSurfaceDetectedActiveState (false);
        }

        if (state) {
            movementInputPausedActive = true;

            positionToAdjustPlayerToSurfaceCollision = Vector3.zero;

            if (!useRaycastDetectionToClimb) {
                adjustingPlayerToSurfaceCollisionDetection = true;
            }

            lastTimeCollisionDetectionWithSurfaceNotFound = 0;

            RaycastHit temporalHit;

            if (Physics.Raycast (playerTransform.position + playerTransform.up, playerTransform.forward, out temporalHit, raycastDistance, raycastLayermask)) {

                freeClimbZoneSystem currentFreeClimbZoneSystem = temporalHit.collider.GetComponent<freeClimbZoneSystem> ();

                if (currentFreeClimbZoneSystem != null) {
                    Transform newParent = currentFreeClimbZoneSystem.checkPlayerParentState ();

                    if (newParent != null) {
                        mainPlayerController.setPlayerAndCameraParent (newParent);

                        if (!useRaycastDetectionToClimb) {
                            mainClimbDetectionCollisionSystem.setNewParent (newParent);
                        }

                        playerParentAssigned = true;
                    }
                }
            }
        } else {
            if (playerParentAssigned) {
                mainPlayerController.setPlayerAndCameraParent (null);

                mainClimbDetectionCollisionSystem.setNewParent (null);
            }

            playerParentAssigned = false;

            movementInputDisabledOnSurface = false;
        }

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            slidingDownActive = false;

            activateAutoSlideDownOnSurface = false;

            currentIDValue = -1;

            mainPlayerController.setCurrentIdleIDValue (0);

            mainPlayerController.updateIdleIDOnAnimator ();
        }

        if (state) {
            firstPersonChecked = isFirstPersonActive;

            thirdPersonChecked = !isFirstPersonActive;
        } else {
            firstPersonChecked = false;

            thirdPersonChecked = false;
        }

        mainPlayerController.setPausePlayerRotateToCameraDirectionOnFirstPersonActiveState (firstPersonChecked);

        if (!useRaycastDetectionToClimb) {
            mainClimbDetectionCollisionSystem.enableOrDisableClimbDetection (state);
        }

        bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

        if (isFullBodyAwarenessActive) {
            mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (state);

            if (state) {
                mainPlayerCamera.setPivotCameraTransformParentCurrentTransformToFollow ();
            } else {
                mainPlayerCamera.setPivotCameraTransformOriginalParent ();

                mainPlayerCamera.resetPivotCameraTransformLocalRotation ();
            }
        }
    }

    public override void setExternalForceEnabledState (bool state)
    {
        setClimbEnabledState (state);
    }

    public void setClimbEnabledState (bool state)
    {
        if (!state) {
            stopClimbAndDetectState ();
        }

        climbEnabled = state;
    }

    public void setClimbCheckCanBeUsedState (bool state)
    {
        climbCheckCanBeUsed = state;
    }

    public void setClimbEnabledState ()
    {
        setClimbEnabledState (originalClimbEnabled);
    }

    public void setOriginalClimbEnabledState ()
    {
        if (climbActive) {
            setCheckIfDetectClimbActiveState (false);
        }

        setClimbEnabledState (originalClimbEnabled);
    }

    //	public void setClimbDownSpeedMultiplier (float newValue)
    //	{
    //		slideDownSpeedMultiplier = newValue;
    //	}

    //	public void setForceSlowDownOnSurfaceActiveState (bool state)
    //	{
    //		forceSlowDownOnSurfaceActive = state;
    //	}

    public void enableCheckIfDetectClimbActiveStateExternally ()
    {
        if (checkIfDetectClimbActive) {
            return;
        }

        setCheckIfDetectClimbActiveState (true);
    }

    public void disableCheckIfDetectClimbActiveStateExternally ()
    {
        if (!checkIfDetectClimbActive) {
            return;
        }

        setCheckIfDetectClimbActiveState (false);
    }


    public override void checkIfResumeExternalControllerState ()
    {
        if (checkIfDetectClimbActive) {
            if (showDebugPrint) {
                print ("resuming free climb state");
            }

            externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();

            if (currentExternalControllerBehavior != null && currentExternalControllerBehavior != this) {
                currentExternalControllerBehavior.disableExternalControllerState ();
            }

            checkIfDetectClimbActive = false;

            setCheckIfDetectClimbActiveState (true);
        }
    }

    public override void disableExternalControllerState ()
    {
        setCheckIfDetectClimbActiveState (false);
    }

    public void checkEventsOnStateChange (bool state)
    {
        if (isFirstPersonActive) {
            if (useEventsOnFirstPerson) {
                if (state) {
                    eventOnStartFirstPerson.Invoke ();
                } else {
                    eventOnEndFirstPerson.Invoke ();
                }
            }
        } else {
            if (useEventsOnThirdPerson) {
                if (state) {
                    eventOnStartThirdPerson.Invoke ();
                } else {
                    eventOnEndThirdPerson.Invoke ();
                }
            }
        }
    }

    public void setClimbPausedState (bool state)
    {
        if (state) {
            if (climbActive) {
                stopClimbAndDetectState ();
            }
        }

        climbPaused = state;
    }

    public void rotateCharacterOnJump ()
    {
        stopRotateCharacterOnJumpCoroutine ();

        jumpCoroutine = StartCoroutine (rotateCharacterOnJumpCoroutine ());
    }

    void stopRotateCharacterOnJumpCoroutine ()
    {
        if (jumpCoroutine != null) {
            StopCoroutine (jumpCoroutine);
        }
    }

    public IEnumerator rotateCharacterOnJumpCoroutine ()
    {
        bool targetReached = false;

        float movementTimer = 0;

        float t = 0;

        float duration = 0;

        if (isFirstPersonActive) {
            duration = 0.5f / jumpRotationSpeedFirstPerson;
        } else {
            duration = 0.5f / jumpRotationSpeedThirdPerson;
        }

        float angleDifference = 0;

        Transform objectToRotate = playerTransform;

        if (isFirstPersonActive) {
            objectToRotate = playerCameraTransform;
        }

        Quaternion targetRotation = Quaternion.identity;

        if (playerTransform.up != getCurrentNormal ()) {
            Vector3 currentNormal = getCurrentNormal ();

            Quaternion currentPlayerRotation = playerTransform.rotation;
            Vector3 currentPlayerForward = Vector3.Cross (-playerTransform.right, currentNormal);
            Quaternion playerTargetRotation = Quaternion.LookRotation (currentPlayerForward, currentNormal);

            targetRotation = playerTargetRotation;
        } else {
            targetRotation = Quaternion.LookRotation (-objectToRotate.forward, objectToRotate.up);
        }

        while (!targetReached) {
            t += Time.deltaTime / duration;

            objectToRotate.rotation = Quaternion.Lerp (objectToRotate.rotation, targetRotation, t);

            angleDifference = Quaternion.Angle (objectToRotate.rotation, targetRotation);

            movementTimer += Time.deltaTime;

            if (angleDifference < 0.2f || movementTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }
    }

    public override void setCurrentPlayerActionSystemCustomActionCategoryID ()
    {
        if (behaviorCurrentlyActive) {
            if (customActionCategoryID > -1) {
                mainPlayerController.setCurrentCustomActionCategoryID (customActionCategoryID);
            }
        } else {
            if (regularActionCategoryID > -1) {
                mainPlayerController.setCurrentCustomActionCategoryID (regularActionCategoryID);
            }
        }
    }

    public void resetPlayerRotation ()
    {
        stopResetPlayerRotationCoroutine ();

        resetPlayerCoroutine = StartCoroutine (resetPlayerRotationCoroutine ());
    }

    void stopResetPlayerRotationCoroutine ()
    {
        if (resetPlayerCoroutine != null) {
            StopCoroutine (resetPlayerCoroutine);
        }
    }

    IEnumerator resetPlayerRotationCoroutine ()
    {
        if (showDebugPrint) {
            print ("reset player rotation");
        }

        float movementTimer = 0;

        float t = 0;

        float duration = 1.5f;

        float angleDifference = 0;

        Vector3 currentNormal = getCurrentNormal ();

        Quaternion currentPlayerRotation = playerTransform.rotation;
        Vector3 currentPlayerForward = Vector3.Cross (playerTransform.right, currentNormal);
        Quaternion playerTargetRotation = Quaternion.LookRotation (currentPlayerForward, currentNormal);

        bool targetReached = false;

        while (!targetReached) {
            t += (Time.deltaTime / duration) * resetPlayerRotationSpeed;

            playerTransform.rotation = Quaternion.Slerp (playerTransform.rotation, playerTargetRotation, t);

            angleDifference = Quaternion.Angle (playerTransform.rotation, playerTargetRotation);

            movementTimer += Time.deltaTime;

            if (angleDifference < 0.01f || movementTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }
    }

    public void resetCameraRotation ()
    {
        stopResetCameraRotationCoroutine ();

        resetCameraCoroutine = StartCoroutine (resetCameraRotationCoroutine ());
    }

    void stopResetCameraRotationCoroutine ()
    {
        if (resetCameraCoroutine != null) {
            StopCoroutine (resetCameraCoroutine);
        }
    }

    IEnumerator resetCameraRotationCoroutine ()
    {
        if (showDebugPrint) {
            print ("reset camera rotation");
        }

        float movementTimer = 0;

        float t = 0;

        float duration = 1;

        float angleDifference = 0;

        Vector3 currentNormal = getCurrentNormal ();

        Quaternion currentCameraRotation = playerCameraTransform.rotation;
        Vector3 currentCameraForward = Vector3.Cross (playerCameraTransform.right, currentNormal);
        Quaternion cameraTargetRotation = Quaternion.LookRotation (currentCameraForward, currentNormal);

        bool targetReached = false;

        while (!targetReached) {
            t += (Time.deltaTime / duration) * resetPlayerRotationSpeed;

            playerCameraTransform.rotation = Quaternion.Slerp (playerCameraTransform.rotation, cameraTargetRotation, t);

            angleDifference = Quaternion.Angle (playerCameraTransform.rotation, cameraTargetRotation);

            movementTimer += Time.deltaTime;

            if (angleDifference < 0.01f || movementTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }
    }


    bool checkIfCanActivateClimbState ()
    {
        activateAutoSlideDownOnSurface = false;

        bool isPlayerOnGround = mainPlayerController.isPlayerOnGround ();

        if (!canActivateClimbOnPlayerOnGround) {
            if (isPlayerOnGround) {
                return false;
            }
        }

        float currentRaycastDistance = raycastDistanceToCheckToActivateClimbOnGround;

        if (!isPlayerOnGround) {
            currentRaycastDistance = raycastDistanceToCheckToActivateClimbOnAir;
        }

        Vector3 currentRaycastPosition = playerTransform.position + playerTransform.up;

        RaycastHit temporalHit;

        if (Physics.Raycast (currentRaycastPosition, playerTransform.forward, out temporalHit, currentRaycastDistance, raycastLayermask)) {
            bool surfaceDistanceTooFar = false;

            if (isPlayerOnGround) {
                if (temporalHit.distance > minDistanceToSurfaceToGrabSurfaceOnGround) {
                    surfaceDistanceTooFar = true;
                }
            } else {
                if (temporalHit.distance > minDistanceToSurfaceToGrabSurfaceOnAir) {
                    surfaceDistanceTooFar = true;
                }
            }

            if (surfaceDistanceTooFar) {

                if (showDebugPrint) {
                    print ("surface to climb too far");
                }

                return false;
            } else {
                if (useSurfacesToIgnoreTags) {
                    if (surfacesToIgnoreTagsList.Contains (temporalHit.collider.gameObject.tag)) {
                        if (showDebugPrint) {
                            print ("surface to ignore by tag found");
                        }

                        return false;
                    }
                }

                freeClimbZoneSystem currentFreeClimbZoneSystem = temporalHit.collider.GetComponent<freeClimbZoneSystem> ();

                if (checkTagsToIgnoreOnManualClimbInputActive) {
                    checkTagsToIgnoreOnManualClimbInputActive = false;

                    if (useSurfacesToCheckTagsOnClimbByInput) {
                        if (!surfacesToCheckTagsOnClimbByInputList.Contains (temporalHit.collider.gameObject.tag)) {
                            if (showDebugPrint) {
                                print ("surface to check by tag not found " + temporalHit.collider.gameObject.tag);
                            }

                            if (checkForClimbSurfaceZoneSystemOnClimbInput) {
                                if (currentFreeClimbZoneSystem != null) {
                                    if (!currentFreeClimbZoneSystem.isAllowClimbSurfaceOnInputEnabled ()) {
                                        return false;
                                    }
                                } else {
                                    return false;
                                }

                            } else {
                                return false;
                            }
                        }
                    }
                }

                if (ignoreRigidbodies) {
                    if (temporalHit.collider.attachedRigidbody != null) {
                        return false;
                    }
                }

                if (currentFreeClimbZoneSystem != null) {
                    if (currentFreeClimbZoneSystem.isIgnoreSurfaceToClimbEnabled ()) {
                        return false;
                    }

                    if (currentFreeClimbZoneSystem.activateAutoSlideDownOnSurface) {
                        activateAutoSlideDownOnSurface = true;
                    }
                }

                if (currentFreeClimbZoneSystem != null) {
                    movementInputDisabledOnSurface = currentFreeClimbZoneSystem.movementInputDisabledOnSurface;
                }
            }
        } else {
            if (showDebugPrint) {
                print ("surface to climb not found");
            }

            return false;
        }

        if (Physics.Raycast (currentRaycastPosition, playerTransform.forward, out temporalHit, currentRaycastDistance, raycastLayermask)) {
            surfaceAngle = Vector3.SignedAngle (temporalHit.normal, -playerTransform.forward, playerTransform.right);
        }

        if (showDebugPrint) {
            print ("surface angle to check before activate climb " + surfaceAngle);
        }

        if (surfaceAngle != 0) {
            if (stopClimbAtMinAngleOnSurface) {
                if (surfaceAngle < 0) {
                    if (surfaceAngle < minAngleToStopClimb) {
                        return false;
                    }
                }
            }

            if (stopClimbAtMaxAngleOnSurface) {
                if (surfaceAngle > 0) {
                    if (surfaceAngle > maxAngleToStopClimb) {
                        return false;
                    }
                }
            }
        }

        return true;
    }


    public void setActivateClimbStateOnNextSurfaceDetectedActiveState (bool state)
    {
        if (ignoreActivateClimbStateOnNextSurfaceDetectedActive) {
            return;
        }

        activateClimbStateOnNextSurfaceDetectedActive = state;

        if (showDebugPrint) {
            print ("setting activateClimbStateOnNextSurfaceDetectedActive " + activateClimbStateOnNextSurfaceDetectedActive);
        }

        stopCheckIfSurfaceDetectedInFrontCoroutine ();

        if (activateClimbStateOnNextSurfaceDetectedActive) {
            lastTimeActivateClimbStateOnNextSurfaceDetectedActive = Time.time;

            checkIfSurfaceDetectedInFrontCoroutine = StartCoroutine (updateCheckIfSurfaceDetectedInFrontCoroutine ());
        }
    }

    public void stopCheckIfSurfaceDetectedInFrontCoroutine ()
    {
        if (checkIfSurfaceDetectedInFrontCoroutine != null) {
            StopCoroutine (checkIfSurfaceDetectedInFrontCoroutine);
        }
    }

    IEnumerator updateCheckIfSurfaceDetectedInFrontCoroutine ()
    {
        var waitTime = new WaitForSeconds (0.00001f);

        while (true) {
            if (Time.time > lastTimeActivateClimbStateOnNextSurfaceDetectedActive + 0.5f) {
                if (mainPlayerController.isPlayerOnGround ()) {
                    stopCheckIfSurfaceDetectedInFrontCoroutine ();

                    activateClimbStateOnNextSurfaceDetectedActive = false;

                    if (showDebugPrint) {
                        print ("disabling activateClimbStateOnNextSurfaceDetectedActive");
                    }
                }

                if (climbActive) {
                    stopCheckIfSurfaceDetectedInFrontCoroutine ();

                    activateClimbStateOnNextSurfaceDetectedActive = false;

                    if (showDebugPrint) {
                        print ("disabling activateClimbStateOnNextSurfaceDetectedActive");
                    }
                }
            }

            if (Time.time > lastTimeActivateClimbStateOnNextSurfaceDetectedActive + 5) {
                if (showDebugPrint) {
                    print ("disabling activateClimbStateOnNextSurfaceDetectedActive from too much time on air");
                }

                stopCheckIfSurfaceDetectedInFrontCoroutine ();

                activateClimbStateOnNextSurfaceDetectedActive = false;
            }

            if (Physics.Raycast (playerTransform.position + (verticalRaycastOffset * playerTransformUp),
                    playerTransform.forward, minDistanceToSurfaceToGrabSurfaceOnAir, raycastLayermask)) {

                inputGrabSurface ();

                stopCheckIfSurfaceDetectedInFrontCoroutine ();

                activateClimbStateOnNextSurfaceDetectedActive = false;

                if (showDebugPrint) {
                    print ("activating climb from detecting next surface");
                }
            }

            yield return waitTime;
        }
    }


    public void activateGrabSurface ()
    {
        if (checkIfDetectClimbActive) {
            if (!climbActive) {
                if (!checkIfCanActivateClimbState ()) {
                    return;
                }
            }

            if (activateClimbStateOnNextSurfaceDetectedActive) {
                setActivateClimbStateOnNextSurfaceDetectedActiveState (false);
            }

            setClimbActiveState (!climbActive);
        }
    }

    void checkEventsOnTurbo (bool state)
    {
        if (useEventOnTurbo) {
            if (state) {
                eventOnStartTurbo.Invoke ();
            } else {
                eventOnEndTurbo.Invoke ();
            }
        }
    }

    #region UpdateRaycastDetection

    //REGULAR RAYCAST DETECTION METHOD
    bool updateRaycastDetectionState (Vector3 currentRaycastPosition, Vector3 currentRaycastDirection, float currentFixedUpdateDeltaTime)
    {
        surfaceFoundOnRegularRaycast = false;

        float currentRaycastDistance = raycastDistance;

        if (surfaceFoundOnSide || surfaceFoundOnCloseSide) {
            Vector3 heading = lastSideDirection - (playerTransform.position + playerTransformUp);

            float distance = heading.magnitude;

            currentRaycastDirection = heading / distance;

            currentRaycastDistance = raycastDistance * 2;
        }

        float regularRaycastDistance = currentRaycastDistance;

        if (movementInputPausedActive) {
            regularRaycastDistance += 1;
        }

        if (Physics.Raycast (playerTransform.position + (verticalRaycastOffset * playerTransformUp),
                currentRaycastDirection, regularRaycastDistance, raycastLayermask)) {

            surfaceFoundOnRegularRaycast = true;
        }

        Vector3 mainRaycastPosition = playerTransform.position + playerTransformUp;

        Physics.Raycast (mainRaycastPosition, currentRaycastDirection, out hit, regularRaycastDistance, raycastLayermask);


        //check sides with raycast
        if (checkSidesEnabled) {
            RaycastHit temporalHit;

            if (!surfaceFoundOnCloseSide && !upperBodySurfaceDetected) {
                if (!surfaceFoundOnSide) {
                    if (!Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out temporalHit,
                            raycastDistanceToCheckSides, raycastLayermask)) {

                        bool movingRight = currentHorizontalMovement > 0;

                        bool movingLeft = currentHorizontalMovement < 0;

                        Vector3 newRaycastDirection = playerTransform.forward;

                        if (movingRight || movingLeft) {
                            if (movingRight) {
                                newRaycastDirection = Quaternion.AngleAxis (-45, Vector3.up) * newRaycastDirection;

                                currentRaycastPosition = playerTransform.position + playerTransform.right + playerTransformUp;

                            } else {
                                newRaycastDirection = Quaternion.AngleAxis (45, Vector3.up) * newRaycastDirection;

                                currentRaycastPosition = playerTransform.position - playerTransform.right + playerTransformUp;
                            }
                        }

                        if (showGizmo) {
                            Debug.DrawRay (currentRaycastPosition, 4 * newRaycastDirection, Color.red, 5);
                        }

                        if (Physics.Raycast (currentRaycastPosition, newRaycastDirection, out hit, raycastDistance * 3, raycastLayermask)) {
                            surfaceFoundOnRegularRaycast = true;

                            surfaceFoundOnSide = true;

                            lastSideDirection = hit.point;

                            lastSideNormal = hit.normal;

                            if (showGizmo) {
                                Debug.DrawRay (currentRaycastPosition, 20 * newRaycastDirection, Color.black, 5);
                            }
                        }
                    }
                }
            }
        }


        //check close sides with raycast
        if (checkCloseSidesEnabled) {
            if (!surfaceFoundOnCloseSide) {
                bool movingRight = currentHorizontalMovement > 0;

                bool movingLeft = currentHorizontalMovement < 0;

                if (movingRight) {
                    currentRaycastDirection = playerTransform.right;
                } else {
                    currentRaycastDirection = -playerTransform.right;
                }

                if (movingLeft || movingRight) {
                    currentRaycastPosition = playerTransform.position + playerTransformUp;

                    RaycastHit temporalHit;

                    if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out temporalHit, raycastDistanceToCheckCloseSides, raycastLayermask)) {

                        surfaceFoundOnRegularRaycast = true;

                        surfaceFoundOnCloseSide = true;

                        lastSideDirection = temporalHit.point;

                        lastSideNormal = temporalHit.normal;

                        hit = temporalHit;

                        surfaceFoundOnSide = false;
                    }
                }
            }
        }

        //check change of view state
        updateCheckCameraViewState ();

        currentClimbMovementSpeed = climbMovementSpeedThirdPerson;

        if (isFirstPersonActive) {
            currentClimbMovementSpeed = climbMovementSpeedFirstPerson;
        }

        currentClimbRotationSpeed = climbRotationSpeedThirdPerson;

        if (isFirstPersonActive) {
            currentClimbRotationSpeed = climbRotationSpeedFirstPerson;
        }

        currentClimbVelocity = climbVelocityThirdPerson;

        if (isFirstPersonActive) {
            currentClimbVelocity = climbVelocityFirstPerson;
        }

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            currentClimbVelocity = slideDownSpeedThirdPerson;

            if (isFirstPersonActive) {
                currentClimbVelocity = SlideDownSpeedFirstPerson;
            }
        }

        if (movementInputPausedActive || movementInputDisabledOnSurface) {
            rawAxisValues = Vector2.zero;
        }

        checkCurrentSurfaceDetected ();

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (!movementInputPausedActive && !cancelMovementInputActive && lowerBodySurfaceDetected && surfaceDetectedOnMovementInputRaycast) {
                currentVerticalMovement = -1;

                rawAxisValues.y = currentVerticalMovement;

                if (currentHorizontalMovement != 0) {
                    currentHorizontalMovement /= 2;
                }
            }
        }

        if (cancelMovementInputActive) {
            currentVerticalMovement = 0;

            currentHorizontalMovement = 0;

            rawAxisValues = Vector2.zero;
        }

        if (!lowerBodySurfaceDetected) {
            if (!surfaceDetectedOnMovementInputRaycast) {
                currentVerticalMovement = 0;

                rawAxisValues.y = 0;
            }
        }

        if (surfaceDetectedAbovePlayer) {
            currentVerticalMovement = Mathf.Clamp (currentVerticalMovement, -1, 0);

            rawAxisValues.y = Mathf.Clamp (rawAxisValues.y, -1, 0);
        }

        if (surfafeDetectedBelowPlayer) {
            currentVerticalMovement = Mathf.Clamp (currentVerticalMovement, 0, 1);

            rawAxisValues.y = Mathf.Clamp (rawAxisValues.y, 0, 1);
        }

        slidingDownResult = false;

        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (currentVerticalMovement != 0) {
                if (currentIDValue != 1) {
                    currentIDValue = 1;

                    mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                    mainPlayerController.updateIdleIDOnAnimator ();
                }

                slidingDownResult = true;
            } else {
                if (currentIDValue != 0) {
                    currentIDValue = 0;

                    mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                    mainPlayerController.updateIdleIDOnAnimator ();
                }
            }
        } else {
            if (currentIDValue != 0) {
                currentIDValue = 0;

                mainPlayerController.setCurrentIdleIDValue (currentIDValue);

                mainPlayerController.updateIdleIDOnAnimator ();
            }
        }


        moving = (Mathf.Abs (currentVerticalMovement) > 0.01f || Mathf.Abs (currentHorizontalMovement) > 0.01f);

        movementInputPressed = rawAxisValues != Vector2.zero;

        currentClimbTurboSpeed = 1;

        bool enougMovementInput = (Mathf.Abs (currentVerticalMovement) > 0.7f || Mathf.Abs (currentHorizontalMovement) > 0.7f);

        if (increaseClimbSpeedActive && movementInputPressed && enougMovementInput && !slidingDownActive && !activateAutoSlideDownOnSurface) {
            currentClimbTurboSpeed = climbTurboSpeed;

            if (!turboActive) {
                mainPlayerController.setCurrentAirSpeedValue (2);

                turboActive = true;

                checkEventsOnTurbo (true);
            }
        } else {
            if (turboActive) {
                mainPlayerController.setCurrentAirSpeedValue (1);

                turboActive = false;

                mainPlayerController.stopShakeCamera ();

                checkEventsOnTurbo (false);

                eventOnRegularClimbSpeed.Invoke ();
            }
        }


        //update player velocity
        if (surfaceFoundOnRegularRaycast) {
            Vector3 targetPosition = hit.point + surfaceOffset * hit.normal;

            targetPosition -= playerTransformUp;

            if (useMovementCurve) {
                currentClimbMovementSpeed = mainMovementCurve.Evaluate (currentClimbMovementSpeed);
            }

            bool closeToSurfaceDetected = GKC_Utils.distance (mainRigidbody.position, targetPosition) > 0.01f;

            if (movementInputPressed || closeToSurfaceDetected || movementInputPausedActive) {
                mainRigidbody.position = Vector3.Lerp (mainRigidbody.position, targetPosition, currentClimbTurboSpeed * currentClimbMovementSpeed * currentFixedUpdateDeltaTime);
            }
        }

        if (surfaceFoundOnRegularRaycast || surfaceFoundOnSide || surfaceFoundOnCloseSide) {

            Vector2 axisValues = new Vector2 (currentHorizontalMovement, currentVerticalMovement);

            if (useVelocityCurve) {
                currentClimbVelocity = mainVelocityCurve.Evaluate (currentClimbVelocity);
            }

            if (movementInputPressed) {
                mainRigidbody.linearVelocity = currentClimbVelocity * currentClimbTurboSpeed * playerTransform.TransformDirection (axisValues);
            }
        }

        if (movementInputPressed || surfaceFoundOnSide || surfaceFoundOnCloseSide) {
            mainPlayerController.setCurrentVelocityValue (mainRigidbody.linearVelocity);
        } else {
            mainPlayerController.setCurrentVelocityValue (Vector3.zero);
        }


        if (slidingDownActive || activateAutoSlideDownOnSurface) {
            if (slidingDownResult) {
                currentVerticalMovement = 0;
            }
        }

        //update player animation states
        mainAnimator.SetFloat (horizontalAnimatorID, currentHorizontalMovement, inputLerpSpeed, currentFixedUpdateDeltaTime);
        mainAnimator.SetFloat (verticalAnimatorID, currentVerticalMovement, inputLerpSpeed, currentFixedUpdateDeltaTime);



        //update player rotation
        currentRaycastPosition = playerTransform.position + playerTransformUp;

        currentRaycastDirection = playerTransform.forward;

        currentRaycastDistance = raycastDistance;

        if (surfaceFoundOnSide || surfaceFoundOnCloseSide) {

            currentRaycastDirection = lastSideDirection - (playerTransform.position + playerTransformUp);

            if (currentRaycastDirection.magnitude > 1) {
                currentRaycastDirection.Normalize ();
            }

            float currentClimbAngle = Vector3.SignedAngle (playerTransform.up, getCurrentNormal (), playerTransform.right);

            if (Mathf.Abs (currentClimbAngle) > 3) {
                currentRaycastDistance += 4;
            } else {
                currentRaycastDistance += 2;
            }
        }

        if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, currentRaycastDistance, raycastLayermask)) {
            Quaternion targetRotation = Quaternion.LookRotation (-hit.normal);

            playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, targetRotation, currentClimbRotationSpeed * currentFixedUpdateDeltaTime);

            if (rotateCameraToPlayerRotationOnClimbActiveEnabled) {
                Vector3 myForwardCamera = Vector3.Cross (playerCameraTransform.right, playerTransform.up);
                Quaternion dstRotCamera = Quaternion.LookRotation (myForwardCamera, playerTransform.up);

                playerCameraTransform.rotation = Quaternion.Lerp (playerCameraTransform.rotation, dstRotCamera, currentClimbRotationSpeed * currentFixedUpdateDeltaTime);
            }

            if (surfaceFoundOnSide) {
                float temporalAngle = Vector3.SignedAngle (lastSideNormal, -playerTransform.forward, playerTransform.up);

                if (Mathf.Abs (temporalAngle) < 2) {
                    surfaceFoundOnSide = false;
                }
            }

            if (surfaceFoundOnCloseSide) {
                float temporalAngle = Vector3.SignedAngle (lastSideNormal, -playerTransform.forward, playerTransform.up);

                if (Mathf.Abs (temporalAngle) < 2) {
                    surfaceFoundOnCloseSide = false;
                }
            }

            if (showGizmo) {
                Debug.DrawRay (currentRaycastPosition, currentRaycastDistance * currentRaycastDirection, Color.gray, 5);
            }
        } else {
            if (showGizmo) {
                Debug.DrawRay (currentRaycastPosition, currentRaycastDistance * currentRaycastDirection, Color.magenta, 5);
            }
        }


        //check if surface not found, to stop the climb action
        if (!surfaceFoundOnSide && !surfaceFoundOnCloseSide && !movementInputPausedActive) {

            currentRaycastPosition = playerTransform.position + offsetToCheckIfStopClimbStateOnRaycast * playerTransformUp;

            if (!Physics.Raycast (currentRaycastPosition, playerTransform.forward, maxRaycastDistanceToStopClimb, raycastLayermask)) {
                if (lastTimeSurfaceNotFound == 0) {

                    lastTimeSurfaceNotFound = Time.time;
                }
            } else {
                if (showGizmo) {
                    Debug.DrawRay (currentRaycastPosition, 2 * playerTransform.forward, Color.yellow);
                }

                lastTimeSurfaceNotFound = 0;
            }

            if (lastTimeSurfaceNotFound > 0) {
                if (Time.time > lastTimeSurfaceNotFound + minTimeToStopClimbIfNotSurfaceFound) {
                    lastTimeSurfaceNotFound = 0;

                    stopClimbAndDetectState ();

                    return false;
                }
            }
        }

        return true;
    }

    #endregion

    //Input functions
    public void inputSetSlideDownState (bool state)
    {
        if (!climbActive) {
            return;
        }

        if (!slideDownEnabled) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (activateAutoSlideDownOnSurface) {
            return;
        }

        if (slidingDownActive == state) {
            return;
        }

        slidingDownActive = state;

        currentIDValue = -1;
    }

    public void inputGrabSurface ()
    {
        if (!canUseInput ()) {

            if (showDebugPrint) {
                print ("can't use input grab surface");
            }

            return;
        }

        if (climbPaused) {
            return;
        }

        if (!climbActive) {
            if (Time.time < mainPlayerController.getLastTimeActionActive () + minWaitTimeSinceLastTimeActionActiveToClimb) {
                return;
            }
        }

        if (activateClimbStateDirectlyOnPressInput) {
            if (checkIfDetectClimbActive) {
                setCheckIfDetectClimbActiveState (false);

                climbStateActivatedByInput = false;
            } else {
                checkTagsToIgnoreOnManualClimbInputActive = true;

                if (checkIfCanActivateClimbState ()) {
                    setCheckIfDetectClimbActiveState (true);

                    activateGrabSurface ();

                    climbStateActivatedByInput = true;
                }

                checkTagsToIgnoreOnManualClimbInputActive = false;
            }
        } else {
            activateGrabSurface ();
        }
    }

    public void inpuSetIncreaseClimbSpeedState (bool state)
    {
        if (!climbActive) {
            return;
        }

        if (!slideDownEnabled) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (activateAutoSlideDownOnSurface) {
            return;
        }

        if (increaseClimbSpeedActive == state) {
            return;
        }

        increaseClimbSpeedActive = state;
    }

    bool canUseInput ()
    {
        if (mainPlayerController.isGravityPowerActive ()) {
            return false;
        }

        if (mainPlayerController.isPlayerUsingPowers ()) {
            return false;
        }

        if (mainPlayerController.isUsingDevice ()) {
            return false;
        }

        if (mainPlayerController.isPlayerDead ()) {
            return false;
        }

        if (mainPlayerController.isPlayerMenuActive ()) {
            return false;
        }

        if (mainPlayerController.isUsingSubMenu ()) {
            return false;
        }

        if (mainPlayerController.isActionActive ()) {
            return false;
        }

        if (mainPlayerController.isPlayerDriving ()) {
            return false;
        }

        return true;
    }

    public void setClimbEnabledStateFromEditor (bool state)
    {
        setClimbEnabledState (state);

        updateComponent ();
    }

    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Update Free Climb System", gameObject);
    }
}