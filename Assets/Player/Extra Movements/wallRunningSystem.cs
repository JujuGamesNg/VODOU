﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class wallRunningSystem : externalControllerBehavior
{
	[Header ("Main Settings")]
	[Space]

	public bool wallRunningEnabled = true;

	public LayerMask raycastLayermask;

	public float wallRunningSpeed;
	public float wallSprintSpeed;
	public float wallRunningRotationSpeed;
	public bool wallRunningCanBeUsed = true;

	public Vector3 wallRunningImpulseOnJump;
	public Vector3 wallRunningStopMovemmentImpulse;
	public Vector3 wallRunningEndOfSurfaceImpulse;
	public Vector3 wallRunningImpulseOnJumpCameraDirection;

	public float raycastDistance = 0.6f;

	[Space]
	[Header ("2.5d Settings")]
	[Space]

	public float raycastDistance2_5d = 2;

	public float minWaitTimeToAdhereToWallAfterLastTime2_5d = 1;

	public Vector3 wallRunningImpulseOnJump2_5d = new Vector3 (5, 8, 10);

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool checkForObstaclesInFront = true;
	public float raycastDistanceObstaclesInFront = 1;
	public float minAngleForObstaclesInFront = 5;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool useDownMovementOnWallRunning;
	public float downMovementOnWallRunningAmount;
	public float delayDownMovementOnWallRunning;

	public bool useUpAndDownMovementOnWallRunning;
	public float upAndDownMovementOnWallRunningDelay;
	public float upAndDownMovementOnWallRunningAmount;

	public bool useStopWallRunnigAfterDelay;
	public float stopWallRunningDelay;

	public bool useSurfaceAngleDiffToJumpInCameraDirection;
	public Vector2 surfaceAngleDiffRangeToWallRunning;

	public float maxVelocityChangeWallRunning;

	public bool keepWeaponsOnWallRunning;
	public bool drawWeaponsIfCarriedPreviously;

	[Space]
	[Header ("Third Person Settings")]
	[Space]

	public string wallRunningRightActionName = "Wall Running Right";
	public string wallRunnigLeftActionName = "Wall Running Left";

	public string actionActiveAnimatorName = "Action Active";

	public bool useCharacterCOMOffset = true;
	public float rotationOffsetOnCharacterCOM = 10;
	public float positionOffsetOnCharacterCOM = 0.15f;
	public float offsetAdjustmentSpeed = 5;

	public float playerRotationToSurfaceSpeedThirdPerson = 14;

	public float playerRotationToSurfaceSpeedFirstPerson = 14;

	public float playerRotationToSurfaceSpeedFBA = 3;

	[Space]
	[Header ("Third Person Camera State Settings")]
	[Space]

	public bool setNewCameraStateOnThirdPerson;

	public string newCameraStateOnThirdPersonRightSide;
	public string newCameraStateOnThirdPersonLeftSide;

	[Space]
	[Header ("Ignore Surface Settings")]
	[Space]

	public bool useTagsToIgnore;
	public List<string> tagsToIgnore = new List<string> ();

	public bool ignoreRigidbodies;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool wallRunningActive;

	public bool isFirstPersonActive;

	public bool wallRunningOnRightSide;
	public bool wallRunningOnLeftSide;

	public bool carryingWeaponsPreviously;

	public Vector3 currentMovementDirection;

	public float extraYPosition;

	public float playerYPositionOnWallRunningStart;

	public Vector3 playerVelocity;

	public bool checkSurfaceDetected = true;

	public bool isPlayerMovingOn3dWorld;

	[Space]
	[Header ("First Person Events Settings")]
	[Space]

	public UnityEvent eventOnWallRunningStartFirstPerson;
	public UnityEvent eventOnWallRunningToRightFirstPerson;
	public UnityEvent eventOnWallRunningToLeftFirstPerson;
	public UnityEvent eventOnWallRunningEndFirstPerson;

	[Space]
	[Header ("Third Person Events Settings")]
	[Space]

	public UnityEvent eventOnWallRunningStartThirdPerson;
	public UnityEvent eventOnWallRunningToRightThirdPerson;
	public UnityEvent eventOnWallRunningToLeftThirdPerson;
	public UnityEvent eventOnWallRunningEndThirdPerson;

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

	public Transform COM;

	float lastTimeWallRunningActive;

	float lastTimeWallRunningDisabled;

	bool previouslyWallRunningOnRightSide;
	bool previouslyWallRunningOnLeftSide;
	float lastTimeWallRunningOnRightSide;
	RaycastHit wallRunningHit;

	bool autoUseDownMovementOnWallRunningActive;
	bool autoUseStopWallRunningAfterDelay;
	bool originalWallRunningEnabled;

	Vector3 playerTransformUp;
	Vector3 playerTransformForward;
	Vector3 playerTransformRight;

	RaycastHit hit;

	float half = 0.5f;

	Vector3 velocityChange;

	int actionActiveAnimatorID;

	float originalCOMYPosition;

	Coroutine resetCOMCoroutine;

	float currentRotationTarget;
	float currentPositionTarget;

	string previousCameraState;

	float rotationAmount;

	bool ignoreHorizontalCameraRotationInputState;

	GameObject currentSurfaceDetected;
	GameObject previousSurfaceDetected;


	void Start ()
	{
		originalWallRunningEnabled = wallRunningEnabled;

		actionActiveAnimatorID = Animator.StringToHash (actionActiveAnimatorName);

		originalCOMYPosition = COM.localPosition.y;
	}

	public override void updateControllerBehavior ()
	{
		if (!wallRunningActive) {
			return;
		}

		playerTransformUp = playerTransform.up;

		playerTransformForward = playerTransform.forward;

		playerTransformRight = playerTransform.right;

		Vector3 currentRaycastPosition = playerTransform.position + playerTransformUp;
		Vector3 currentRaycastDirection = playerTransformRight;

		if (!wallRunningOnRightSide) {
			currentRaycastDirection = -playerTransformRight;
		}

		bool movementInputActive = false;

		if (isPlayerMovingOn3dWorld) {
			movementInputActive = mainPlayerController.isPlayerMovingVertical (0.7f);
		} else {
			movementInputActive = mainPlayerController.isPlayerMovingHorizontal (0.7f);
		}

		if (movementInputActive) {
			Debug.DrawRay (currentRaycastPosition, currentRaycastDirection, Color.red, 5);

			float currentRaycastDistance = 1;

			if (!isPlayerMovingOn3dWorld) {
				currentRaycastDistance = raycastDistance2_5d;
			}

			if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, currentRaycastDistance, raycastLayermask)) {
				wallRunningHit = hit;

				float currentWallRunningSpeed = wallRunningSpeed;

				if (mainPlayerController.isPlayerRunning ()) {
					currentWallRunningSpeed = wallSprintSpeed;
				}

				currentMovementDirection = wallRunningHit.point + playerTransformForward;

				currentMovementDirection = currentMovementDirection - playerTransform.InverseTransformDirection (currentMovementDirection).y * playerTransformUp;

				currentMovementDirection += (playerYPositionOnWallRunningStart + extraYPosition) * playerTransformUp;

				if (useDownMovementOnWallRunning || autoUseDownMovementOnWallRunningActive) {
					if (Time.time > delayDownMovementOnWallRunning + lastTimeWallRunningActive) {
						extraYPosition -= downMovementOnWallRunningAmount;
					}
				} else {
					if (useUpAndDownMovementOnWallRunning && Time.time > upAndDownMovementOnWallRunningDelay + lastTimeWallRunningActive) {
						float wallRunningInput = 0;

						float horizontalInput = 0;

						if (isPlayerMovingOn3dWorld) {
							horizontalInput = mainPlayerController.getHorizontalInput ();
						} else {
							horizontalInput = mainPlayerController.getVerticalInput ();
						}

						if (horizontalInput > half) {
							wallRunningInput = upAndDownMovementOnWallRunningAmount * horizontalInput;
						} else if (horizontalInput < -half) {
							wallRunningInput = upAndDownMovementOnWallRunningAmount * horizontalInput;
						}

						if (isPlayerMovingOn3dWorld) {
							if (!wallRunningOnRightSide) {
								wallRunningInput *= (-1);
							}
						}

						if (showDebugPrint) {
							print (wallRunningInput);
						}

						extraYPosition += wallRunningInput;
					}
				}

				if (useStopWallRunnigAfterDelay || autoUseStopWallRunningAfterDelay) {
					if (Time.time > stopWallRunningDelay + lastTimeWallRunningActive) {
						setWallRunningActiveState (false);

						setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);

						return;
					}
				}

				float currentFixedUpdateDeltaTime = mainPlayerController.getCurrentDeltaTime ();

				//probar moveposition m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
				mainRigidbody.position = Vector3.MoveTowards (mainRigidbody.position, currentMovementDirection, currentFixedUpdateDeltaTime * currentWallRunningSpeed);
			} else {
				setWallRunningActiveState (false);

				setWallRunningImpulseForce (wallRunningEndOfSurfaceImpulse, false);

				return;
			}

			if (checkForObstaclesInFront) {

				currentRaycastDirection = playerTransformForward;

				if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, raycastDistanceObstaclesInFront, raycastLayermask)) {

					float obstacleAngle = Vector3.SignedAngle (playerTransformForward, -hit.normal, playerTransformUp);

					if (minAngleForObstaclesInFront == 0 || Mathf.Abs (obstacleAngle) < minAngleForObstaclesInFront) {

						setWallRunningActiveState (false);

						setWallRunningImpulseForce (wallRunningEndOfSurfaceImpulse, false);

						return;
					}
				}
			}
		} else {
			setWallRunningActiveState (false);

			setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);

			return;
		}

		mainPlayerController.setCurrentVelocityValue (mainRigidbody.linearVelocity);
	
		//Manage player rotation to adjust to wall normal

		Vector3 wallNormal = -wallRunningHit.normal;

		float angle = 0;

		bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

		if (isFullBodyAwarenessActive) {
			Vector3 playerCameraRight = mainPlayerCamera.transform.right;

			if (wallRunningOnRightSide) {
				angle = Vector3.SignedAngle (playerCameraRight, wallNormal, playerTransformUp);
			} else {
				angle = Vector3.SignedAngle (-playerCameraRight, wallNormal, playerTransformUp);
			}
		} else {
			if (wallRunningOnRightSide) {
				angle = Vector3.SignedAngle (playerTransformRight, wallNormal, playerTransformUp);
			} else {
				angle = Vector3.SignedAngle (-playerTransformRight, wallNormal, playerTransformUp);
			}
		}

		if (showDebugPrint) {
			print ("running on right side value " + wallRunningOnRightSide);
		}

		float currentRotationSpeed = playerRotationToSurfaceSpeedThirdPerson;

		if (isFirstPersonActive) {
			currentRotationSpeed = playerRotationToSurfaceSpeedFirstPerson;
		}

		if (isFullBodyAwarenessActive) {
			currentRotationSpeed = playerRotationToSurfaceSpeedFBA;
		}

		if (isFullBodyAwarenessActive) {
			rotationAmount = Mathf.MoveTowards (rotationAmount, angle, currentRotationSpeed * Time.fixedDeltaTime);
		} else {
			rotationAmount = Mathf.Lerp (rotationAmount, angle, currentRotationSpeed * Time.fixedDeltaTime);
		}


		if (isFullBodyAwarenessActive) {
			if (!ignoreHorizontalCameraRotationInputState) {
				mainPlayerCamera.setIgnoreHorizontalCameraRotationOnFBAState (true);

				ignoreHorizontalCameraRotationInputState = true;
			}
		
			Quaternion targetRotation = mainPlayerCamera.transform.rotation * Quaternion.Euler (mainPlayerCamera.transform.up * angle);

			mainPlayerCamera.transform.rotation = 
					Quaternion.Lerp (mainPlayerCamera.transform.rotation, targetRotation, currentRotationSpeed * Time.fixedDeltaTime);
		} else {
			if (Mathf.Abs (rotationAmount) > 0.001f) {
				playerTransform.Rotate (0, rotationAmount, 0);

				if (ignoreHorizontalCameraRotationInputState) {
					mainPlayerCamera.setIgnoreHorizontalCameraRotationOnFBAState (false);

					ignoreHorizontalCameraRotationInputState = false;
				}
			}
		}

		//Set foot step state and head bob
		mainPlayerController.setCurrentFootStepsState ();

		if (mainPlayerController.updateHeadbobState) {
			mainPlayerController.setCurrentHeadBobState ();
		}

		if (useCharacterCOMOffset) {
			if (wallRunningOnRightSide) {
				currentRotationTarget = -rotationOffsetOnCharacterCOM;
				currentPositionTarget = -positionOffsetOnCharacterCOM;
			} else {
				currentRotationTarget = rotationOffsetOnCharacterCOM;
				currentPositionTarget = positionOffsetOnCharacterCOM;
			}

			Quaternion COMTargetRotation = Quaternion.Euler (currentRotationTarget * Vector3.forward);
		
			COM.localRotation = Quaternion.Lerp (COM.localRotation, COMTargetRotation,	offsetAdjustmentSpeed * Time.fixedDeltaTime);

			Vector3 COMTargetPosition = currentPositionTarget * Vector3.right + originalCOMYPosition * Vector3.up;

			COM.localPosition = Vector3.Lerp (COM.localPosition, COMTargetPosition, offsetAdjustmentSpeed * Time.fixedDeltaTime);
		}

		playerVelocity = mainRigidbody.linearVelocity;
	}

	public override void checkIfActivateExternalForce ()
	{
		if (wallRunningEnabled && wallRunningCanBeUsed && !mainPlayerController.useFirstPersonPhysicsInThirdPersonActive) {
			if (!wallRunningActive && !mainPlayerController.pauseAllPlayerDownForces && !mainPlayerController.ignoreExternalActionsActiveState) {

				isPlayerMovingOn3dWorld = mainPlayerController.isPlayerMovingOn3dWorld ();

				bool wallRunningSurfaceFound = false;

				bool movementInputActive = false;

				if (isPlayerMovingOn3dWorld) {
					movementInputActive = (Mathf.Abs (mainPlayerController.getVerticalInput ()) > 0);
				} else {
					movementInputActive = (Mathf.Abs (mainPlayerController.getHorizontalInput ()) > 0);
				}

				bool waitToNextWallRunningActiveResult = false;

				if (isPlayerMovingOn3dWorld) {
					waitToNextWallRunningActiveResult = (Time.time > lastTimeWallRunningActive + 0.05f);
				} else {
					waitToNextWallRunningActiveResult = (lastTimeWallRunningActive == 0 ||
					(Time.time > lastTimeWallRunningActive + minWaitTimeToAdhereToWallAfterLastTime2_5d));
				}

				if (movementInputActive && !mainPlayerController.isCrouching () && waitToNextWallRunningActiveResult) {
					playerTransformUp = playerTransform.up;

					playerTransformRight = playerTransform.right;

					Vector3 currentRaycastPosition = playerTransform.position + playerTransformUp;
					Vector3 currentRaycastDirection = playerTransformRight;

					float currentRaycastDistance = raycastDistance;

					if (!isPlayerMovingOn3dWorld) {
						currentRaycastDistance = raycastDistance2_5d;
					}

					if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, currentRaycastDistance, raycastLayermask)) {
						currentSurfaceDetected = hit.collider.gameObject;

						if (currentSurfaceDetected != previousSurfaceDetected) {
							previousSurfaceDetected = currentSurfaceDetected;

							checkSurfaceDetected = true;

							if (ignoreRigidbodies) {
								if (hit.collider.attachedRigidbody != null) {

									checkSurfaceDetected = false;
								}
							}

							if (checkSurfaceDetected && useTagsToIgnore) {
								if (tagsToIgnore.Contains (previousSurfaceDetected.tag)) {
									checkSurfaceDetected = false;
								}
							}
						}

						if (!isPlayerMovingOn3dWorld && checkSurfaceDetected) {
							Transform currentLockedCameraTransform = mainPlayerCamera.getLockedCameraTransform ();
						
							float raycastDirectionAngle = Vector3.SignedAngle (currentLockedCameraTransform.forward, currentRaycastDirection, playerTransformUp);

							float raycastDirectionAngleABS = Mathf.Abs (raycastDirectionAngle);

							if (raycastDirectionAngleABS > 160) {
								if (raycastDirectionAngleABS > 165) {
									checkSurfaceDetected = false;
								}
							} else {
								if (raycastDirectionAngleABS > 15) {
									checkSurfaceDetected = false;
								}
							}

							if (!checkSurfaceDetected) {
								previousSurfaceDetected = null;

								previouslyWallRunningOnRightSide = false;

								return;
							}
						}

						if (previouslyWallRunningOnRightSide != wallRunningOnRightSide) {
							previouslyWallRunningOnRightSide = wallRunningOnRightSide;

							lastTimeWallRunningOnRightSide = Time.time;

							if (showDebugPrint) {
								print ("Running Right Side");
							}
						}

						if (checkForObstaclesInFront && checkSurfaceDetected) {
							currentRaycastDirection = playerTransformForward;

							currentRaycastPosition -= playerTransformRight;

							if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, raycastDistanceObstaclesInFront, raycastLayermask)) {

								float obstacleAngle = Vector3.SignedAngle (playerTransformForward, -hit.normal, playerTransformUp);

								if (minAngleForObstaclesInFront == 0 || Mathf.Abs (obstacleAngle) < minAngleForObstaclesInFront) {

									checkSurfaceDetected = false;

									previouslyWallRunningOnLeftSide = false;
									previouslyWallRunningOnRightSide = false;
									wallRunningOnRightSide = false;
									wallRunningOnLeftSide = false;

									currentSurfaceDetected = null;
									previousSurfaceDetected = null;
								}
							}
						}

						if (checkSurfaceDetected) {
							wallRunningOnRightSide = true;
							wallRunningOnLeftSide = false;
							previouslyWallRunningOnLeftSide = false;

							if (!previouslyWallRunningOnRightSide || Time.time > lastTimeWallRunningOnRightSide + 0.3f) {
								setWallRunningActiveState (true);

								previouslyWallRunningOnRightSide = false;
							}

							wallRunningSurfaceFound = true;
						}
					} else {
						currentRaycastDirection = -playerTransformRight;

						if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, currentRaycastDistance, raycastLayermask)) {
							currentSurfaceDetected = hit.collider.gameObject;

							if (currentSurfaceDetected != previousSurfaceDetected) {
								previousSurfaceDetected = currentSurfaceDetected;

								checkSurfaceDetected = true;

								if (ignoreRigidbodies) {
									if (hit.collider.attachedRigidbody != null) {
										checkSurfaceDetected = false;
									}
								}

								if (checkSurfaceDetected && useTagsToIgnore) {
									if (tagsToIgnore.Contains (previousSurfaceDetected.tag)) {
										checkSurfaceDetected = false;
									}
								}
							}


							if (!isPlayerMovingOn3dWorld && checkSurfaceDetected) {
								Transform currentLockedCameraTransform = mainPlayerCamera.getLockedCameraTransform ();

								float raycastDirectionAngle = Vector3.SignedAngle (currentLockedCameraTransform.forward, currentRaycastDirection, playerTransformUp);

								float raycastDirectionAngleABS = Mathf.Abs (raycastDirectionAngle);

								if (raycastDirectionAngleABS > 160) {
									if (raycastDirectionAngleABS > 165) {
										checkSurfaceDetected = false;
									}
								} else {
									if (raycastDirectionAngleABS > 15) {
										checkSurfaceDetected = false;
									}
								}

								if (!checkSurfaceDetected) {
									previousSurfaceDetected = null;

									previouslyWallRunningOnLeftSide = false;

									return;
								}
							}

							if (previouslyWallRunningOnLeftSide != wallRunningOnLeftSide) {
								previouslyWallRunningOnLeftSide = wallRunningOnLeftSide;

								lastTimeWallRunningOnRightSide = Time.time;

								if (showDebugPrint) {
									print ("Running Left Side");
								}
							}

							if (checkForObstaclesInFront && checkSurfaceDetected) {
								currentRaycastDirection = playerTransformForward;

								currentRaycastPosition += playerTransformRight;

								if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, raycastDistanceObstaclesInFront, raycastLayermask)) {

									float obstacleAngle = Vector3.SignedAngle (playerTransformForward, -hit.normal, playerTransformUp);

									if (minAngleForObstaclesInFront == 0 || Mathf.Abs (obstacleAngle) < minAngleForObstaclesInFront) {

										checkSurfaceDetected = false;

										previouslyWallRunningOnLeftSide = false;
										previouslyWallRunningOnRightSide = false;
										wallRunningOnRightSide = false;
										wallRunningOnLeftSide = false;

										currentSurfaceDetected = null;
										previousSurfaceDetected = null;
									}
								}
							}

							if (checkSurfaceDetected) {
								wallRunningOnRightSide = false;
								wallRunningOnLeftSide = true;
								previouslyWallRunningOnRightSide = false;

								if (!previouslyWallRunningOnLeftSide || Time.time > lastTimeWallRunningOnRightSide + 0.3f) {
									setWallRunningActiveState (true);

									previouslyWallRunningOnLeftSide = false;
								}

								wallRunningSurfaceFound = true;
							}
						}
					}
				}

				if (!wallRunningSurfaceFound && Time.time > lastTimeWallRunningDisabled + half) {
					previouslyWallRunningOnLeftSide = false;
					previouslyWallRunningOnRightSide = false;
					wallRunningOnRightSide = false;
					wallRunningOnLeftSide = false;
				}
			}
		}
	}

	public override void setExtraImpulseForce (Vector3 forceAmount, bool useCameraDirection)
	{
		setWallRunningImpulseForce (forceAmount, useCameraDirection);
	}

	public void setWallRunningImpulseForce (Vector3 forceAmount, bool useCameraDirection)
	{
		Transform mainImpulseTransform = playerTransform;

		isPlayerMovingOn3dWorld = mainPlayerController.isPlayerMovingOn3dWorld ();

		if (useCameraDirection && isPlayerMovingOn3dWorld) {
			mainImpulseTransform = playerCameraTransform; 
		}

		Vector3 impulseForce = forceAmount.z * mainImpulseTransform.forward + forceAmount.y * mainImpulseTransform.up;

		if (wallRunningOnRightSide) {
			impulseForce -= forceAmount.x * mainImpulseTransform.right;
		} else {
			impulseForce += forceAmount.x * mainImpulseTransform.right;
		}

		if (maxVelocityChangeWallRunning > 0) {
			velocityChange = impulseForce - mainRigidbody.linearVelocity;

			velocityChange = Vector3.ClampMagnitude (velocityChange, maxVelocityChangeWallRunning);

		} else {
			velocityChange = impulseForce;
		}

		mainPlayerController.setVelocityChangeValue (velocityChange);

		mainRigidbody.AddForce (velocityChange, ForceMode.VelocityChange);
	}

	public override void setJumpActiveForExternalForce ()
	{
		setJumpActive ();
	}

	public void setJumpActive ()
	{
		if (wallRunningActive) {
			isPlayerMovingOn3dWorld = mainPlayerController.isPlayerMovingOn3dWorld ();

			if (useSurfaceAngleDiffToJumpInCameraDirection && isPlayerMovingOn3dWorld) {
				float angleSurface = Vector3.SignedAngle (playerCameraTransform.forward, playerTransform.forward, playerTransform.up);

				if (Mathf.Abs (angleSurface) > surfaceAngleDiffRangeToWallRunning.x && Mathf.Abs (angleSurface) < surfaceAngleDiffRangeToWallRunning.y) {
					setWallRunningImpulseForce (wallRunningImpulseOnJumpCameraDirection, true);
				} else {
					setWallRunningImpulseForce (wallRunningImpulseOnJump, false);
				}
			} else {
				if (isPlayerMovingOn3dWorld) {
					setWallRunningImpulseForce (wallRunningImpulseOnJump, false);
				} else {
					setWallRunningImpulseForce (wallRunningImpulseOnJump2_5d, false);
				}
			}

			mainPlayerController.setCurrentVelocityValue (mainRigidbody.linearVelocity);

			lastTimeWallRunningActive = Time.time;

			mainPlayerController.setWallRunningActiveValue (false);

			setWallRunningActiveState (false);
		}
	}

	public override void setExternalForceActiveState (bool state)
	{
		setWallRunningActiveState (state);
	}

	public void setWallRunningActiveState (bool state)
	{
		if (!wallRunningEnabled) {
			return;
		}

		if (wallRunningActive == state) {
			return;
		}

		if (!state) {
			lastTimeWallRunningDisabled = Time.time;
		}

		if (state) {
			if (mainPlayerController.isFreeClimbActive ()) {
				return;
			}
		}

		wallRunningActive = state;

		setBehaviorCurrentlyActiveState (state);

		setCurrentPlayerActionSystemCustomActionCategoryID ();

		mainPlayerController.setWallRunningActiveValue (state);

		mainPlayerController.setUpdate2_5dClampedPositionPausedState (state);

		isPlayerMovingOn3dWorld = mainPlayerController.isPlayerMovingOn3dWorld ();

		if (showDebugPrint) {
			print ("Wall running active state " + state);
		}

		isFirstPersonActive = mainPlayerController.isPlayerOnFirstPerson ();

		if (wallRunningActive) {
			if (isFirstPersonActive) {
				eventOnWallRunningStartFirstPerson.Invoke ();
				if (wallRunningOnRightSide) {
					eventOnWallRunningToRightFirstPerson.Invoke ();
				} else {
					eventOnWallRunningToLeftFirstPerson.Invoke ();
				}

			} else {
				eventOnWallRunningStartThirdPerson.Invoke ();

				if (wallRunningOnRightSide) {
					eventOnWallRunningToRightThirdPerson.Invoke ();
				} else {
					eventOnWallRunningToLeftThirdPerson.Invoke ();
				}

				if (wallRunningOnRightSide) {
					mainAnimator.CrossFadeInFixedTime (wallRunningRightActionName, 0.1f);
				} else {
					mainAnimator.CrossFadeInFixedTime (wallRunnigLeftActionName, 0.1f);
				}

				mainAnimator.SetBool (actionActiveAnimatorID, state);
			}

			extraYPosition = 0;

			playerYPositionOnWallRunningStart = playerTransform.position.y;

			mainPlayerController.setJumpsAmountValue (0);

			lastTimeWallRunningActive = Time.time;

			mainPlayerCamera.enableOrDisableChangeCameraView (false);

			if (useCharacterCOMOffset) {
				stopResetCOMRotationCoroutine ();
			}

			if (!isFirstPersonActive) {
				if (keepWeaponsOnWallRunning) {
					carryingWeaponsPreviously = mainPlayerWeaponsManager.isPlayerCarringWeapon ();

					if (carryingWeaponsPreviously) {
						mainPlayerWeaponsManager.checkIfDisableCurrentWeapon ();
					}
					
					mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (false);
				}
			}
		} else {
			if (isFirstPersonActive) {
				eventOnWallRunningEndFirstPerson.Invoke ();
			} else {
				eventOnWallRunningEndThirdPerson.Invoke ();

				mainAnimator.SetBool (actionActiveAnimatorID, state);
			}

			mainPlayerCamera.setOriginalchangeCameraViewEnabledValue ();

			if (useCharacterCOMOffset) {
				resetCOMRotation ();
			}

			if (keepWeaponsOnWallRunning) {
				mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (true);
			}

			if (carryingWeaponsPreviously) {
				if (!drawWeaponsIfCarriedPreviously) {
					carryingWeaponsPreviously = false;
				}
			}

			if (ignoreHorizontalCameraRotationInputState) {
				mainPlayerCamera.setIgnoreHorizontalCameraRotationOnFBAState (false);

				ignoreHorizontalCameraRotationInputState = false;
			}
		}

		mainPlayerCamera.setCameraPositionMouseWheelEnabledState (!wallRunningActive);

		mainPlayerController.stepManager.setWallRunningState (wallRunningActive, wallRunningOnRightSide);

		mainPlayerController.setLastTimeFalling ();

		if (setNewCameraStateOnThirdPerson && !isFirstPersonActive) {
			if (state) {
				previousCameraState = mainPlayerCamera.getCurrentStateName ();

				if (wallRunningOnRightSide) {
					mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnThirdPersonRightSide);
				} else {
					mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnThirdPersonLeftSide);
				}
			} else {
				
				if (previousCameraState != "") {
					if (wallRunningOnRightSide) {
						if (previousCameraState != newCameraStateOnThirdPersonRightSide) {
							mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);
						}
					} else {
						if (previousCameraState != newCameraStateOnThirdPersonLeftSide) {
							mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);
						}
					}

					previousCameraState = "";
				}
			}
		}

		rotationAmount = 0;

		currentSurfaceDetected = null;
		previousSurfaceDetected = null;

		checkSurfaceDetected = true;
	}

	public override void setExternalForceEnabledState (bool state)
	{
		setWallRunningEnabledState (state);
	}

	public void setWallRunningEnabledState (bool state)
	{
		if (!state) {
			if (wallRunningActive) {
				setWallRunningActiveState (false);

				setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);

				previouslyWallRunningOnLeftSide = false;
				previouslyWallRunningOnRightSide = false;
				wallRunningOnRightSide = false;
				wallRunningOnLeftSide = false;
			}
		}

		wallRunningEnabled = state;
	}

	public void setWallRunningCanBeUsedState (bool state)
	{
		wallRunningCanBeUsed = state;
	}

	public void setOriginalWallRunningEnabledState ()
	{
		setWallRunningEnabledState (originalWallRunningEnabled);
	}

	public void setAutoUseDownMovementOnWallRunningActiveState (bool state)
	{
		autoUseDownMovementOnWallRunningActive = state;
	}

	public void setAutoUseStopWallRunningAfterDelayState (bool state)
	{
		autoUseStopWallRunningAfterDelay = state;
	}

	public void resetCOMRotation ()
	{
		stopResetCOMRotationCoroutine ();

		resetCOMCoroutine = StartCoroutine (resetCOMRotationCoroutine ());
	}

	void stopResetCOMRotationCoroutine ()
	{
		if (resetCOMCoroutine != null) {
			StopCoroutine (resetCOMCoroutine);
		}
	}

	public IEnumerator resetCOMRotationCoroutine ()
	{
		bool targetReached = false;

		float movementTimer = 0;

		float t = 0;

		float duration = 0.5f;

		float angleDifference = 0;

		float positionDifference = 0;

		Vector3 targetPosition = originalCOMYPosition * Vector3.up;
		Quaternion targetRotation = Quaternion.identity;

		while (!targetReached) {
			t += Time.deltaTime / duration; 
			COM.localPosition = Vector3.Slerp (COM.localPosition, targetPosition, t);
			COM.localRotation = Quaternion.Slerp (COM.localRotation, targetRotation, t);

			angleDifference = Quaternion.Angle (COM.localRotation, targetRotation);

			positionDifference = GKC_Utils.distance (COM.localPosition, targetPosition);

			movementTimer += Time.deltaTime;

			if ((positionDifference < 0.01f && angleDifference < 0.2f) || movementTimer > (duration + 1)) {
				targetReached = true;
			}
			yield return null;
		}

		if (drawWeaponsIfCarriedPreviously) {
			WaitForSeconds delay = new WaitForSeconds (1);

			yield return delay;

			if (carryingWeaponsPreviously) {
				if (!mainPlayerController.isPlayerOnFirstPerson () && !wallRunningActive && mainPlayerController.canPlayerMove ()) {
					mainPlayerWeaponsManager.checkIfDrawSingleOrDualWeapon ();
				}

				carryingWeaponsPreviously = false;
			}
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

	public override void disableExternalControllerState ()
	{
		if (showDebugPrint) {
			print ("check to stop traversal movement " + wallRunningActive);
		}

		if (wallRunningActive) {
			setWallRunningActiveState (false);
		}
	}

	public void setWallRunningEnabledStateFromEditor (bool state)
	{
		setWallRunningEnabledState (state);

		updateComponent ();
	}

	void updateComponent ()
	{
		GKC_Utils.updateComponent (this);

		GKC_Utils.updateDirtyScene ("Update Wall Running System", gameObject);
	}
}
