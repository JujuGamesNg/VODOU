﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEngine.Events;

public class playerCharactersManager : MonoBehaviour
{
    public bool searchPlayersAtStart;

    public int currentNumberOfPlayers = 1;

    public GameObject mainCharacter;

    public List<playerInfo> playerList = new List<playerInfo> ();

    public string cameraStateNameToUse = "1 Player";

    public List<cameraStates> cameraStatesList = new List<cameraStates> ();

    public List<GameObject> extraCharacterList = new List<GameObject> ();

    public Vector3 newPlayerPositionOffset;

    public Vector2 regularReferenceResolution = new Vector2 (1280, 720);
    public Vector2 splitReferenceResolution = new Vector2 (2560, 1440);

    public bool ignoreCheckEmptyCharactersOnStartEnabled;


    Vector3 currentCharacterPosition;

    public List<GameObject> auxPlayerList = new List<GameObject> ();

    public playerInfo currentPlayeraInfoActive;

    public bool setCurrentCharacterToControlAndAIAtStart;

    public string currentCharacterToControlName = "Player 1";

    public float delayTimeToChangeBetweenCharacters = 1;

    public bool keepActiveOnlyCurrentPlayerSelected;

    public bool changeCameraPositionAtOnceOnCharacterChange;

    public bool setCurrentPlayerPositionToPreviousCharacter;

    int currentCharacterToControlIndex;

    public string [] cameraStatesListString;
    public int currentCameraStateIndex;

    bool characterToControlAtStartConfigured;

    float lastTimeCharacterChange;

    playerInfo previousCharacterInfoActive;

    public bool configureLocalMultiplayerCharactersOnAwake;
    public int numberOfPlayersOnAwake;
    public string cameraStateNameOnAwake;

    public dynamicSplitScreenSystem mainDynamicSplitScreenSystem;

    public GameObject extraPlayerPrefab;

    public Transform initialMultiplayerSpawnPosition;


    //Online elements

    public GameObject onlinePlayerPrefab;

    public bool useTransformPositionListToSpawnOnlinePlayer;

    public List<Transform> transformPositionListToSpawnOnlinePlayer = new List<Transform> ();


    public bool spawnOnlinePlayersOnStart;
    public int numberOfOnlinePlayersToSpawnOnStart;



    static List<playerInfo> playerListValue = new List<playerInfo> ();
    static int playerListCount;


    private static playerCharactersManager _playerCharactersManagerInstance;

    public static playerCharactersManager Instance { get { return _playerCharactersManagerInstance; } }

    bool instanceInitialized;


    public void getComponentInstance ()
    {
        if (instanceInitialized) {
            return;
        }

        if (_playerCharactersManagerInstance != null && _playerCharactersManagerInstance != this) {
            Destroy (this.gameObject);

            return;
        }

        _playerCharactersManagerInstance = this;

        instanceInitialized = true;
    }

    public void getComponentInstanceOnApplicationPlaying ()
    {
        if (Application.isPlaying) {
            getComponentInstance ();
        }
    }

    void Awake ()
    {
        getComponentInstance ();

        if (configureLocalMultiplayerCharactersOnAwake) {
            currentNumberOfPlayers = numberOfPlayersOnAwake;

            cameraStateNameToUse = cameraStateNameOnAwake;
        }

        if (!ignoreCheckEmptyCharactersOnStartEnabled) {
            checkPlayerElementsOnScene ();
        }

        if (searchPlayersAtStart) {
            searchPlayersOnTheLevel (false, false);
        }

        if (configureLocalMultiplayerCharactersOnAwake) {
            if (mainCharacter != null) {
                setNumberOfPlayers (false);
            } else {
                addExtraPlayerOnSceneForMultiplayer (currentNumberOfPlayers);
            }

            setCameraConfiguration (false);

            updateHUD (false);

            setPlayerID (false);

            assignMapSystem (false);
        }

        if (playerList.Count >= 1) {
            currentPlayeraInfoActive = playerList [0];
        }

        playerListValue = playerList;

        playerListCount = playerList.Count;
    }

    void Start ()
    {
        if (setCurrentCharacterToControlAndAIAtStart) {
            StartCoroutine (checkInitialStateOnCharacters ());
        }

        if (spawnOnlinePlayersOnStart) {
            StartCoroutine (checkspawnOnlinePlayersOnStartCoroutine ());
        }
    }

    IEnumerator checkInitialStateOnCharacters ()
    {
        yield return new WaitForSeconds (0.1f);

        if (setCurrentCharacterToControlAndAIAtStart && !characterToControlAtStartConfigured) {
            if (playerList.Count > 1) {
                setAsCurrentCharacterToControlByName (currentCharacterToControlName);

                characterToControlAtStartConfigured = true;
            }
        }
    }

    IEnumerator checkspawnOnlinePlayersOnStartCoroutine ()
    {
        yield return new WaitForSeconds (0.2f);

        for (int i = 0; i < numberOfOnlinePlayersToSpawnOnStart; i++) {
            spawnPlayerOnSceneForMultiplayer ();
        }

    }

    public void checkPlayerElementsOnScene ()
    {
        bool checkingEmptyPlayerElements = checkNotNullPlayerElements ();

        if (checkingEmptyPlayerElements) {
            searchPlayersOnTheLevel (false, false);
        }
    }

    public void spawnPlayerOnSceneForMultiplayer ()
    {
        if (onlinePlayerPrefab != null) {
            GameObject newCharacter = (GameObject)Instantiate (onlinePlayerPrefab, Vector3.zero, Quaternion.identity);

            addSpawnedPlayerOnSceneForMultiplayer (newCharacter);
        }
    }

    public void addSpawnedPlayerOnSceneForMultiplayer (GameObject newCharacter)
    {
        if (mainCharacter == null) {
            mainCharacter = newCharacter;
        }

        print ("SPAWN PLAYER");

        newCharacter.transform.SetParent (transform);

        playerComponentsManager mainplayerComponentsManager = newCharacter.GetComponentInChildren<playerComponentsManager> ();

        if (mainplayerComponentsManager != null) {
            GameObject newPlayerGameObject = mainplayerComponentsManager.gameObject;

            if (useTransformPositionListToSpawnOnlinePlayer) {
                GKC_PlayerPrefabSpawner currentGKC_PlayerPrefabSpawner = newCharacter.GetComponent<GKC_PlayerPrefabSpawner> ();

                if (currentGKC_PlayerPrefabSpawner != null) {
                    int currentTransformPositionIndex = playerListCount;

                    if (currentTransformPositionIndex >= transformPositionListToSpawnOnlinePlayer.Count) {
                        currentTransformPositionIndex = transformPositionListToSpawnOnlinePlayer.Count - 1;
                    }

                    if (currentTransformPositionIndex < transformPositionListToSpawnOnlinePlayer.Count) {
                        currentGKC_PlayerPrefabSpawner.setPositionToSpawn (transformPositionListToSpawnOnlinePlayer [currentTransformPositionIndex].position);

                        currentGKC_PlayerPrefabSpawner.setRotationToSpawn (transformPositionListToSpawnOnlinePlayer [currentTransformPositionIndex].eulerAngles);
                    }
                }
            }


            addPlayerToList (newPlayerGameObject, playerList.Count, false);

            playerListValue = playerList;

            playerListCount = playerList.Count;

            currentPlayeraInfoActive = playerList [0];
        }
    }

    void addPlayerToList (GameObject newPlayerGameObject, int playerIndex, bool creatingNewPlayers)
    {
        playerInfo newPlayerInfo = new playerInfo ();

        newPlayerInfo.Name = "Player " + (playerIndex + 1);
        newPlayerInfo.playerControllerGameObject = newPlayerGameObject;

        newPlayerInfo.mainPlayerComponentsManager = newPlayerInfo.playerControllerGameObject.GetComponent<playerComponentsManager> ();

        newPlayerInfo.playerControllerManager = newPlayerInfo.mainPlayerComponentsManager.getPlayerController ();

        newPlayerInfo.playerControllerManager.setPlayerID (playerIndex + 1);

        newPlayerInfo.playerCameraGameObject = newPlayerInfo.playerControllerManager.getPlayerCameraGameObject ();

        newPlayerInfo.playerCameraManager = newPlayerInfo.mainPlayerComponentsManager.getPlayerCamera ();

        newPlayerInfo.playerInput = newPlayerInfo.mainPlayerComponentsManager.getPlayerInputManager ();

        newPlayerInfo.pauseManager = newPlayerInfo.mainPlayerComponentsManager.getPauseManager ();

        newPlayerInfo.mainFriendListManager = newPlayerInfo.mainPlayerComponentsManager.getFriendListManager ();

        newPlayerInfo.mainSwitchCompanionSystem = newPlayerInfo.mainPlayerComponentsManager.getSwitchCompanionSystem ();

        newPlayerInfo.mainSaveGameSystem = newPlayerInfo.mainPlayerComponentsManager.getSaveGameSystem ();

        newPlayerInfo.playerParentGameObject = newPlayerInfo.pauseManager.transform.parent.gameObject;
        newPlayerInfo.playerParentGameObject.name = newPlayerInfo.Name;

        if (playerIndex == 0) {
            currentCharacterPosition = newPlayerInfo.playerControllerGameObject.transform.position;
        }

        if (creatingNewPlayers) {
            if (newPlayerInfo.playerParentGameObject != mainCharacter) {
                currentCharacterPosition += newPlayerPositionOffset;
                newPlayerInfo.playerControllerGameObject.transform.position = currentCharacterPosition;
                newPlayerInfo.playerCameraGameObject.transform.position = currentCharacterPosition;
            }
        }

        playerList.Add (newPlayerInfo);
    }

    public void addExtraPlayerOnSceneForMultiplayer (int numberOfExtraPlayers)
    {
        if (extraPlayerPrefab != null) {
            int newNumberOfPlayers = playerList.Count;

            for (int i = 0; i < numberOfExtraPlayers; i++) {
                GameObject newCharacter = (GameObject)Instantiate (extraPlayerPrefab, Vector3.zero, Quaternion.identity);

                if (mainCharacter == null) {
                    mainCharacter = newCharacter;
                }

                int numberOfPlayers = playerList.Count;

                newNumberOfPlayers++;

                newCharacter.name = "Player " + newNumberOfPlayers;
                newCharacter.transform.SetParent (transform);
            }

            updatePlayersInfoOnTheLevel ();

            Vector3 newPosition = Vector3.zero;
            Quaternion newRotation = Quaternion.identity;

            if (initialMultiplayerSpawnPosition != null) {
                newPosition = initialMultiplayerSpawnPosition.position;
                newRotation = initialMultiplayerSpawnPosition.rotation;
            }

            for (int i = 0; i < playerList.Count; i++) {
                playerInfo newPlayerInfo = playerList [i];

                newPlayerInfo.playerControllerGameObject.transform.position = newPosition;
                newPlayerInfo.playerCameraGameObject.transform.position = newPosition;

                newPlayerInfo.playerControllerGameObject.transform.rotation = newRotation;
                newPlayerInfo.playerCameraGameObject.transform.rotation = newRotation;

                newPosition += newPlayerPositionOffset;
            }
        }
    }

    public void addNewPlayerSpawned (GameObject newPlayer)
    {
        if (mainCharacter == null) {
            mainCharacter = newPlayer;
        }

        currentNumberOfPlayers++;

        newPlayer.name = "Player " + currentNumberOfPlayers;
        newPlayer.transform.SetParent (transform);

        updatePlayersInfoOnTheLevel ();

        setCameraConfiguration (false);

        updateHUD (false);

        setPlayerID (false);

        assignMapSystem (false);

        playerListValue = playerList;

        playerListCount = playerList.Count;

        currentPlayeraInfoActive = playerList [0];
    }

    public void setNewPositionToPlayersOnScene (List<Transform> positionList)
    {
        int numberOfPlayers = playerList.Count;

        for (int i = 0; i < numberOfPlayers; i++) {
            if (playerList [i].playerControllerGameObject != null) {
                Transform currentCharacter = playerList [i].playerControllerGameObject.transform;

                if (currentCharacter != null && positionList.Count <= i) {
                    currentCharacter.position = positionList [i].position;

                    currentCharacter.rotation = positionList [i].rotation;
                }
            }
        }
    }

    public void updatePlayersInfoOnTheLevel ()
    {
        searchPlayersOnTheLevel (false, false);
    }

    public bool checkNotNullPlayerElements ()
    {
        if (playerList.Count == 0) {
            return true;
        }

        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].playerControllerGameObject == null) {
                return true;
            }
        }

        return false;
    }

    public void searchPlayersOnTheLevel (bool creatingNewPlayers, bool creatingCharactersOnEditor)
    {
        playerController mainPlayerController = null;

        if ((creatingNewPlayers && creatingCharactersOnEditor) ||
            (!creatingNewPlayers && creatingCharactersOnEditor)) {
            if (playerList.Count > 0) {
                if (playerList [0].playerControllerManager != null) {
                    mainPlayerController = playerList [0].playerControllerManager;
                }
            }
        }

        playerList.Clear ();

        auxPlayerList.Clear ();

        currentCharacterPosition = Vector3.zero;

        playerController [] childrens = FindObjectsOfType<playerController> ();

        foreach (playerController c in childrens) {
            if (!c.isCharacterUsedByAI ()) {
                auxPlayerList.Add (c.gameObject);
            }
        }

        if ((creatingNewPlayers && creatingCharactersOnEditor) ||
            (!creatingNewPlayers && creatingCharactersOnEditor)) {
            if (mainPlayerController != null) {
                auxPlayerList.Remove (mainPlayerController.gameObject);

                auxPlayerList.Insert (0, mainPlayerController.gameObject);

                print ("original player asssigned at start");

                for (int i = 0; i < auxPlayerList.Count; i++) {
                    print (auxPlayerList [i].name);
                }
            }
        }

        for (int i = 0; i < auxPlayerList.Count; i++) {
            addPlayerToList (auxPlayerList [i], i, creatingNewPlayers);
        }

        if (creatingCharactersOnEditor) {
            updateComponent ();
        }
    }

    public void setExtraCharacterList ()
    {
        extraCharacterList.Clear ();

        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].playerParentGameObject != mainCharacter) {

                extraCharacterList.Add (playerList [i].playerParentGameObject);
            }
        }

        updateComponent ();
    }

    public void setPlayerID (bool creatingCharactersOnEditor)
    {
        int numberOfPlayerID = 0;

        for (int i = 0; i < playerList.Count; i++) {
            playerInfo currentPlayerInfo = playerList [i];

            if (currentPlayerInfo.playerInput.useOnlyKeyboard) {
                numberOfPlayerID--;
            }

            currentPlayerInfo.playerControllerManager.setPlayerID (i + 1 + numberOfPlayerID);
            currentPlayerInfo.playerInput.setPlayerID (i + 1 + numberOfPlayerID);

            if (creatingCharactersOnEditor) {
                GKC_Utils.updateComponent (currentPlayerInfo.playerControllerManager);
            }
        }

        print ("Player IDs configured");

        if (creatingCharactersOnEditor) {
            updateComponent ();
        }
    }

    public void setNumberOfPlayers (bool creatingCharactersOnEditor)
    {
        int newNumberOfPlayers = currentNumberOfPlayers - 1 - extraCharacterList.Count;

        if (newNumberOfPlayers > 0) {
            for (int i = 0; i < newNumberOfPlayers; i++) {
                GameObject newCharacter = (GameObject)Instantiate (mainCharacter, mainCharacter.transform.position, mainCharacter.transform.rotation);
                newCharacter.name = "Player " + (i + 2);
                newCharacter.transform.SetParent (transform);

                extraCharacterList.Add (newCharacter);
            }
        } else {
            newNumberOfPlayers = Mathf.Abs (newNumberOfPlayers);

            print (newNumberOfPlayers);

            newNumberOfPlayers = extraCharacterList.Count - newNumberOfPlayers;

            for (int i = extraCharacterList.Count - 1; i >= newNumberOfPlayers; i--) {
                DestroyImmediate (extraCharacterList [i]);
            }

            for (int i = 0; i < extraCharacterList.Count; i++) {
                if (extraCharacterList [i] == null) {
                    extraCharacterList.RemoveAt (i);
                    i = 0;
                }
            }

            if (currentNumberOfPlayers == 1) {
                extraCharacterList.Clear ();
            }
        }

        searchPlayersOnTheLevel (true, creatingCharactersOnEditor);

        print ("Players configured: " + playerList.Count);

        if (creatingCharactersOnEditor) {
            updateComponent ();
        }
    }

    public void setCameraConfiguration (bool creatingCharactersOnEditor)
    {
        for (int i = 0; i < playerList.Count; i++) {

            List<Camera> currentCameraList = playerList [i].playerCameraManager.getCameraList ();

            cameraStates currentCameraStates = getCameraStateByName (cameraStateNameToUse);

            if (currentCameraStates != null) {

                int cameraInfoListCount = currentCameraStates.cameraInfoList.Count;

                for (int j = 0; j < currentCameraList.Count; j++) {
                    int currentIndex = -1;

                    if (cameraInfoListCount >= currentNumberOfPlayers) {
                        currentIndex = i;
                    } else {
                        if (cameraInfoListCount == 1) {
                            currentIndex = 0;
                        }
                    }

                    if (currentIndex != -1) {
                        currentCameraList [j].rect =
                            new Rect (currentCameraStates.cameraInfoList [currentIndex].newX,
                            currentCameraStates.cameraInfoList [currentIndex].newY,
                                currentCameraStates.cameraInfoList [currentIndex].newW,
                                currentCameraStates.cameraInfoList [currentIndex].newH);
                    }

                    if (creatingCharactersOnEditor) {
                        GKC_Utils.updateComponent (currentCameraList [j]);
                    }
                }

                menuPause currentPauseManager = playerList [i].pauseManager;


                Camera canvasCamera = currentPauseManager.getMainCanvasCamera ();

                bool activatingMultiplayer = currentNumberOfPlayers > 1;

                if (currentCameraStateIndex >= 0 && currentCameraStateIndex < cameraStatesList.Count) {
                    if (currentCameraStates.numberfOfPlayers == 1) {
                        activatingMultiplayer = false;
                    }
                }

                canvasCamera.gameObject.SetActive (activatingMultiplayer);

                List<menuPause.canvasInfo> currentCanvasInfoList = currentPauseManager.getCanvasInfoList ();

                if (currentCanvasInfoList.Count == 0) {
                    currentPauseManager.addNewCanvasToList (currentPauseManager.getMainCanvas ());

                    currentCanvasInfoList = currentPauseManager.getCanvasInfoList ();
                }

                for (int k = 0; k < currentCanvasInfoList.Count; k++) {
                    if (currentCanvasInfoList [k].mainCanvas != null) {
                        currentCanvasInfoList [k].mainCanvas.worldCamera = canvasCamera;

                        if (activatingMultiplayer) {
                            currentCanvasInfoList [k].mainCanvasScaler.referenceResolution = splitReferenceResolution;
                            currentCanvasInfoList [k].mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                        } else {
                            currentCanvasInfoList [k].mainCanvasScaler.referenceResolution = regularReferenceResolution;
                            currentCanvasInfoList [k].mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        }

                        if (creatingCharactersOnEditor) {
                            GKC_Utils.updateComponent (currentCanvasInfoList [k].mainCanvasScaler);

                            GKC_Utils.updateComponent (currentCanvasInfoList [k].mainCanvas);
                        }
                    }
                }
            } else {
                print ("WARNING: Camera state called " + cameraStateNameToUse + " is not assigned");
            }

            if (i > 0) {
                playerList [i].playerCameraManager.enableOrDisableCameraAudioListener (false);
            }
        }

        print ("Player cameras configured");
    }

    public void updateCanvasValuesByPlayer (GameObject playerControllerGameObject, GameObject pauseManagerObject, GameObject newCanvasPanel)
    {
        if (pauseManagerObject == null && playerControllerGameObject == null) {
            return;
        }

        for (int i = 0; i < playerList.Count; i++) {
            playerInfo currentPlayerInfo = playerList [i];

            menuPause currentPauseManager = currentPlayerInfo.pauseManager;

            bool playerLocated = false;

            if (pauseManagerObject != null) {
                if (currentPauseManager.gameObject == pauseManagerObject) {
                    playerLocated = true;
                }
            }

            if (!playerLocated) {
                if (playerControllerGameObject != null) {
                    if (currentPlayerInfo.playerControllerGameObject == playerControllerGameObject) {
                        playerLocated = true;
                    }
                }
            }

            if (playerLocated) {
                Canvas currentCanvas = newCanvasPanel.GetComponent<Canvas> ();

                if (currentCanvas != null) {
                    currentPauseManager.addNewCanvasToList (currentCanvas);

                    bool activatingMultiplayer = currentNumberOfPlayers > 1;

                    if (currentCameraStateIndex >= 0 && currentCameraStateIndex < cameraStatesList.Count) {
                        cameraStates currentCameraStates = cameraStatesList [currentCameraStateIndex];

                        if (currentCameraStates.numberfOfPlayers == 1) {
                            activatingMultiplayer = false;
                        }
                    }

                    if (activatingMultiplayer) {
                        menuPause.canvasInfo currentCanvasInfo = currentPauseManager.getCanvasInfoListByCanvasObject (currentCanvas);

                        if (currentCanvasInfo != null) {
                            if (currentCanvasInfo.mainCanvas != null) {
                                Camera canvasCamera = currentPauseManager.getMainCanvasCamera ();

                                if (canvasCamera != null) {
                                    currentCanvasInfo.mainCanvas.worldCamera = canvasCamera;

                                    currentCanvasInfo.mainCanvasScaler.referenceResolution = splitReferenceResolution;
                                    currentCanvasInfo.mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                                }
                            }
                        }
                    }
                }

                return;
            }
        }
    }

    public void updateHUD (bool creatingCharactersOnEditor)
    {
        for (int i = 0; i < playerList.Count; i++) {

            playerList [i].pauseManager.setCanvasInfo (creatingCharactersOnEditor);
        }

        print ("Player HUD Updated");
    }

    public void assignMapSystem (bool creatingCharactersOnEditor)
    {
        mapCreator mapCreatorManager = mapCreator.Instance;

        bool mapCreatorManagerAssigned = mapCreatorManager != null;

        if (!mapCreatorManagerAssigned) {
            GKC_Utils.instantiateMainManagerOnSceneWithTypeOnApplicationPlaying (mapCreator.getMainManagerName (), typeof (mapCreator), true);

            mapCreatorManager = mapCreator.Instance;

            mapCreatorManagerAssigned = (mapCreatorManager != null);
        }

        if (!mapCreatorManagerAssigned) {
            mapCreatorManager = FindObjectOfType<mapCreator> ();

            mapCreatorManagerAssigned = mapCreatorManager != null;
        }

        if (mapCreatorManagerAssigned) {
            for (int i = 0; i < playerList.Count; i++) {
                mapCreatorManager.addNewPlayer (playerList [i].pauseManager.gameObject.GetComponent<mapSystem> (),
                    playerList [i].playerControllerGameObject, creatingCharactersOnEditor);
            }
        } else {
            print ("WARNING: There is no map creator in the scene, make sure to drag and drop the prefab and configure as you need if you want to use the map system");
        }

        print ("Map systems configured");
    }

    public void enableOrDisableSingleCameraState (bool state, bool creatingCharactersOnEditor)
    {
        List<Transform> multipleTargetsToFollowList = new List<Transform> ();

        for (int i = 0; i < playerList.Count; i++) {
            multipleTargetsToFollowList.Add (playerList [i].playerControllerGameObject.transform);
        }

        for (int i = 0; i < playerList.Count; i++) {
            playerList [i].playerCameraManager.setFollowingMultipleTargetsState (state, creatingCharactersOnEditor);

            if (state) {
                playerList [i].playerCameraManager.setMultipleTargetsToFollowList (multipleTargetsToFollowList, creatingCharactersOnEditor);
            } else {
                playerList [i].playerCameraManager.setMultipleTargetsToFollowList (null, creatingCharactersOnEditor);
            }
        }

        print ("Single Camera State configured as " + state);

        updateComponent ();

        GKC_Utils.updateDirtyScene ("Set Single Camera State", gameObject);
    }

    public void enableOrDisableDynamicSplitCameraState (bool state, bool creatingCharactersOnEditor)
    {
        if (playerList.Count > 0) {
            if (playerList.Count < 2) {
                mainDynamicSplitScreenSystem.setDynamicSplitScreenActiveState (false);

                if (creatingCharactersOnEditor) {
                    playerList [0].pauseManager.enableOrDisableDynamicElementsOnScreenFromEditor (!state);

                    GKC_Utils.updateComponent (playerList [0].pauseManager);
                } else {
                    playerList [0].pauseManager.enableOrDisableDynamicElementsOnScreen (!state);
                }
            } else {

                mainDynamicSplitScreenSystem.setDynamicSplitScreenActiveState (state, playerList [0].playerControllerGameObject,
                    playerList [1].playerControllerGameObject, creatingCharactersOnEditor);

                if (creatingCharactersOnEditor) {
                    playerList [0].pauseManager.enableOrDisableDynamicElementsOnScreenFromEditor (!state);

                    GKC_Utils.updateComponent (playerList [0].pauseManager);

                    playerList [1].pauseManager.enableOrDisableDynamicElementsOnScreenFromEditor (!state);

                    GKC_Utils.updateComponent (playerList [1].pauseManager);
                } else {
                    playerList [0].pauseManager.enableOrDisableDynamicElementsOnScreen (!state);

                    playerList [1].pauseManager.enableOrDisableDynamicElementsOnScreen (!state);
                }
            }

            if (creatingCharactersOnEditor) {
                updateComponent ();

                GKC_Utils.updateDirtyScene ("Enabling or disabling dynamic split screen " + state, gameObject);
            }
        }
    }

    public cameraStates getCameraStateByName (string cameraStateName)
    {
        for (int j = 0; j < cameraStatesList.Count; j++) {
            if (cameraStatesList [j].Name.Equals (cameraStateName)) {
                return cameraStatesList [j];
            }
        }

        return null;
    }

    public void setAsCurrentPlayerActive (string playerName)
    {
        currentPlayeraInfoActive.playerCameraGameObject.transform.SetParent (currentPlayeraInfoActive.pauseManager.transform);
        currentPlayeraInfoActive.playerControllerGameObject.transform.SetParent (currentPlayeraInfoActive.pauseManager.transform);

        currentPlayeraInfoActive.playerParentGameObject.SetActive (false);

        Vector3 playerPosition = currentPlayeraInfoActive.playerControllerGameObject.transform.position;
        Quaternion playerRotation = currentPlayeraInfoActive.playerControllerGameObject.transform.rotation;

        Quaternion cameraRotation = currentPlayeraInfoActive.playerCameraGameObject.transform.rotation;

        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].Name.Equals (playerName)) {
                currentPlayeraInfoActive = playerList [i];
            }
        }

        currentPlayeraInfoActive.playerControllerGameObject.transform.position = playerPosition;
        currentPlayeraInfoActive.playerControllerGameObject.transform.rotation = playerRotation;

        currentPlayeraInfoActive.playerCameraGameObject.transform.rotation = cameraRotation;

        currentPlayeraInfoActive.playerParentGameObject.SetActive (true);
    }

    public void setAsCurrentCharacterToControlByName (string playerName)
    {
        int currentPlayerIndex = playerList.FindIndex (s => s.Name.Equals (playerName));

        if (currentPlayerIndex == -1) {
            return;
        }

        if (keepActiveOnlyCurrentPlayerSelected) {
            setCharactersStateValue (currentPlayerIndex, true);
        }

        previousCharacterInfoActive = null;

        if (currentPlayeraInfoActive != null) {
            previousCharacterInfoActive = currentPlayeraInfoActive;
        }

        int playerListCount = playerList.Count;

        for (int i = 0; i < playerListCount; i++) {
            if (changeCameraPositionAtOnceOnCharacterChange) {
                playerList [i].playerCameraManager.setUseSmoothCameraTransitionBetweenCharactersState (false);
            }

            if (currentPlayerIndex == i) {

                Vector3 cameraPosition = previousCharacterInfoActive.playerCameraManager.getMainCamera ().transform.position;
                Quaternion cameraRotation = previousCharacterInfoActive.playerCameraManager.getMainCamera ().transform.rotation;

                playerList [i].playerCameraManager.setPreviousCharacterControl (previousCharacterInfoActive.playerControllerGameObject, cameraPosition,
                    cameraRotation, previousCharacterInfoActive.playerCameraManager.isFirstPersonActive ());
            }
        }


        bool isTouchPlatformActive = false;

        for (int i = 0; i < playerListCount; i++) {
            playerList [i].mainFriendListManager.removeAllFriends ();

            playerList [i].playerInput.switchPlayerID (1);

            bool touchPlatform = playerList [i].playerInput.isUsingTouchControls ();

            if (currentPlayerIndex == i) {
                currentPlayeraInfoActive = playerList [i];

                if (currentPlayeraInfoActive.mainSwitchCompanionSystem != null) {
                    currentPlayeraInfoActive.mainSwitchCompanionSystem.activateEventToSetCharacterAsPlayer ();
                }

                playerList [i].eventToSetCharacterAsPlayer.Invoke ();

                currentPlayeraInfoActive.useByAI = false;

                currentCharacterToControlIndex = i;

                if (touchPlatform) {
                    currentPlayeraInfoActive.pauseManager.setUseTouchControlsState (true);

                    //					currentPlayeraInfoActive.playerInput.changeControlsType (true);

                    currentPlayeraInfoActive.playerInput.enableOrDisableTouchCameraControl (true);

                    currentPlayeraInfoActive.playerInput.enableOrDisableTouchMovementJoystickForButtons (true);

                    isTouchPlatformActive = true;
                }
            } else {
                if (playerList [i].mainSwitchCompanionSystem != null) {
                    playerList [i].mainSwitchCompanionSystem.activateEventToSetCharacterAsAI ();
                }

                playerList [i].eventToSetCharacterAsAI.Invoke ();

                playerList [i].useByAI = true;

                if (touchPlatform) {
                    playerList [i].pauseManager.setUseTouchControlsState (false);

                    playerList [i].playerInput.enableOrDisableTouchCameraControl (false);

                    playerList [i].playerInput.enableOrDisableTouchMovementJoystickForButtons (false);

                    //					playerList [i].playerInput.changeControlsType (false);
                }


            }
        }

        if (isTouchPlatformActive) {
            if (!menuPause.isCursorVisible ()) {
                menuPause.setCursorVisibleState (true);

                menuPause.setCursorLockState (false);
            }
        }

        if (keepActiveOnlyCurrentPlayerSelected) {
            setCharactersStateValue (currentPlayerIndex, false);

            if (setCurrentPlayerPositionToPreviousCharacter) {
                if (previousCharacterInfoActive != null) {
                    currentPlayeraInfoActive.playerControllerGameObject.transform.SetPositionAndRotation (
                        previousCharacterInfoActive.playerControllerGameObject.transform.position,
                        previousCharacterInfoActive.playerControllerGameObject.transform.rotation);

                    currentPlayeraInfoActive.playerCameraGameObject.transform.SetPositionAndRotation (
                        previousCharacterInfoActive.playerCameraGameObject.transform.position,
                        previousCharacterInfoActive.playerCameraGameObject.transform.rotation);
                }
            }

            if (previousCharacterInfoActive != null) {
                GKC_Utils.removeTargetFromAIEnemyList (previousCharacterInfoActive.playerControllerGameObject);
            }
        } else {
            for (int i = 0; i < playerListCount; i++) {
                if (playerList [i].playerControllerGameObject != currentPlayeraInfoActive.playerControllerGameObject) {
                    findObjectivesSystem currentfindObjectivesSystem = playerList [i].playerControllerGameObject.GetComponentInChildren<findObjectivesSystem> ();

                    if (currentfindObjectivesSystem != null) {

                        if (currentfindObjectivesSystem.isPartnerFound ()) {
                            currentfindObjectivesSystem.removePartner ();
                        }

                        if (playerList [i].followMainPlayerOnSwitchCompanion) {
                            currentfindObjectivesSystem.addPlayerAsPartner (currentPlayeraInfoActive.playerControllerGameObject);
                        } else {
                            currentfindObjectivesSystem.removeCurrentPartner ();
                        }
                    }
                }
            }
        }
    }

    void setCharactersStateValue (int currentPlayerIndex, bool setStateOnlyOnCurrentPlayer)
    {
        int playerListCount = playerList.Count;

        for (int i = 0; i < playerListCount; i++) {
            bool setActiveResult = true;

            if (currentPlayerIndex != i) {
                setActiveResult = false;
            }

            bool canSetCharacterState = false;

            if (setStateOnlyOnCurrentPlayer) {
                if (currentPlayerIndex == i) {
                    canSetCharacterState = true;
                }
            } else {
                if (currentPlayerIndex != i) {
                    canSetCharacterState = true;
                }
            }

            if (canSetCharacterState) {
                if (setActiveResult) {
                    if (playerList [i].playerControllerManager.isUsingDevice ()) {
                        GKC_Utils.checkIfStopUseDevice (playerList [i].playerControllerGameObject);
                    }
                }

                if (playerList [i].playerControllerGameObject.activeSelf != setActiveResult) {
                    playerList [i].playerControllerGameObject.SetActive (setActiveResult);
                }

                if (playerList [i].playerCameraGameObject.activeSelf != setActiveResult) {
                    playerList [i].playerCameraGameObject.SetActive (setActiveResult);
                }

                GameObject playerManagersParentGameObject = playerList [i].playerControllerManager.getPlayerManagersParentGameObject ();

                if (playerManagersParentGameObject != null) {
                    if (playerManagersParentGameObject.activeSelf != setActiveResult) {
                        playerManagersParentGameObject.SetActive (setActiveResult);
                    }
                }

                GameObject hudAndMenus = playerList [i].pauseManager.getHudAndMenus ();

                if (hudAndMenus != null) {
                    if (hudAndMenus.activeSelf != setActiveResult) {
                        hudAndMenus.SetActive (setActiveResult);
                    }
                }

                if (currentPlayerIndex == i) {
                    playerList [i].mainSwitchCompanionSystem.activateEventToSetCharacterAsOnlyPlayerActive ();
                } else {
                    playerList [i].mainSwitchCompanionSystem.activateEventToSetCharacterAsPlayerNotActive ();
                }
            }
        }
    }

    public void toogleUseOnlyKeyboardGamepad (string playerName)
    {
        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].Name.Equals (playerName)) {

                playerList [i].useOnlyKeyboard = !playerList [i].useOnlyKeyboard;

                playerList [i].playerInput.useOnlyKeyboard = playerList [i].useOnlyKeyboard;

                GKC_Utils.updateComponent (playerList [i].playerInput);
            }
        }

        setPlayerID (true);

        GKC_Utils.updateDirtyScene ("Toggle use of keyboard or gamepad on player " + playerName, gameObject);
    }

    public void setNextCharacterToControl ()
    {
        if (playerList.Count == 1) {
            return;
        }

        currentCharacterToControlIndex++;

        if (currentCharacterToControlIndex >= playerList.Count) {
            currentCharacterToControlIndex = 0;
        }

        setAsCurrentCharacterToControlByName (playerList [currentCharacterToControlIndex].Name);
    }

    public void setAsCurrentCharacterToControlByPlayerGameObject (GameObject playerGameObjectToSearch)
    {
        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].playerControllerGameObject == playerGameObjectToSearch) {
                setAsCurrentCharacterToControlByName (playerList [i].Name);

                return;
            }
        }
    }

    public void setAsCurrentCharacterToControlByPlayerTransform (Transform playerTransformToSearch)
    {
        if (playerTransformToSearch != null) {
            setAsCurrentCharacterToControlByPlayerGameObject (playerTransformToSearch.gameObject);
        }
    }

    public menuPause getPauseManagerFromPlayerByIndex (int index)
    {
        if (index < playerList.Count) {
            return playerList [index].pauseManager;
        }

        return null;
    }

    public void setCharactersManagerPauseState (bool state)
    {
        for (int i = 0; i < playerList.Count; i++) {

            if (!playerList [i].useByAI) {

                if (playerList [i].pauseManager != null && playerList [i].pauseManager.isMenuPaused () != state) {
                    playerList [i].pauseManager.setMenuPausedState (state);
                }
            }
        }
    }

    public bool isCharacterInList (GameObject characterToSearch)
    {
        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].playerControllerGameObject == characterToSearch) {
                return true;
            }
        }

        return false;
    }

    public bool anyCharacterDead ()
    {
        for (int i = 0; i < playerList.Count; i++) {
            if (playerList [i].playerControllerManager.isPlayerDead ()) {
                return true;
            }
        }

        return false;
    }



    public static void checkPanelsActiveOnGamepadOrKeyboard (bool checkKeyboard, int playerID)
    {
        for (int i = 0; i < playerListCount; i++) {
            if (playerListValue [i].playerControllerManager.getPlayerID () == playerID) {

                playerListValue [i].playerInput.checkPanelsActiveOnGamepadOrKeyboard (checkKeyboard);

                return;
            }
        }
    }

    public playerController getMainPlayerController ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerControllerManager;
        }

        return null;
    }

    public GameObject getMainPlayerGameObject ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerControllerGameObject;
        }

        return null;
    }

    public playerCamera getMainPlayerCamera ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerCameraManager;
        }

        return null;
    }

    public Camera getMainPlayerCameraComponent ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerCameraManager.getMainCamera ();
        }

        return null;
    }

    public Transform getMainPlayerCameraTransform ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerCameraManager.getMainCamera ().transform;
        }

        return null;
    }

    public Transform getMainPlayerTransform ()
    {
        if (playerList.Count >= 1) {
            return playerList [0].playerControllerGameObject.transform;
        }

        return null;
    }

    public void getCameraStateListString ()
    {
        cameraStatesListString = new string [cameraStatesList.Count];

        for (int i = 0; i < cameraStatesList.Count; i++) {
            string newName = cameraStatesList [i].Name;

            cameraStatesListString [i] = newName;
        }

        updateComponent ();
    }

    public void inputSetNextCharacterToControl ()
    {
        if (!setCurrentCharacterToControlAndAIAtStart) {
            return;
        }

        if (Time.time > lastTimeCharacterChange + delayTimeToChangeBetweenCharacters) {
            if (currentPlayeraInfoActive != null) {
                if (currentPlayeraInfoActive.playerControllerManager.playerIsBusy () ||
                    currentPlayeraInfoActive.playerControllerManager.isPlayerDriving ()) {
                    return;
                }
            }

            setNextCharacterToControl ();

            lastTimeCharacterChange = Time.time;
        }
    }

    public int getPlayerListCount ()
    {
        return playerList.Count;
    }

    public void checkGameInfoToLoad (int lastSaveNumber, string currentSaveDataPath, string extraFileString)
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.checkGameInfoToLoad (lastSaveNumber, currentSaveDataPath, extraFileString);
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.checkGameInfoToLoad (lastSaveNumber, currentSaveDataPath, extraFileString);
            }
        }
    }

    public void deleteGameInfo (int saveNumberToDelete, string currentSaveDataPath, string extraFileString)
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.deleteGameInfo (saveNumberToDelete, currentSaveDataPath, extraFileString);
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.deleteGameInfo (saveNumberToDelete, currentSaveDataPath, extraFileString);
            }
        }
    }

    public void showSaveInfoDebug (int saveNumberToShow, string currentSaveDataPath, string extraFileString)
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.showSaveInfoDebug (saveNumberToShow, currentSaveDataPath, extraFileString);
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.showSaveInfoDebug (saveNumberToShow, currentSaveDataPath, extraFileString);
            }
        }
    }

    public void checkComponentsToInitialize ()
    {
        checkPlayerElementsOnScene ();

        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.checkComponentsToInitialize ();
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.checkComponentsToInitialize ();
            }
        }
    }

    public void checkSaveInfoListToLoadOnStartOnAllScenes ()
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.checkSaveInfoListToLoadOnStartOnAllScenes ();
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.checkSaveInfoListToLoadOnStartOnAllScenes ();
            }
        }
    }

    public void saveGameInfoFromEditor (int lastSaveNumber, string infoTypeName, string extraFileString)
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.saveGameInfoFromEditor (lastSaveNumber, infoTypeName, extraFileString);
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.saveGameInfoFromEditor (lastSaveNumber, infoTypeName, extraFileString);
            }
        }
    }

    public void saveGameWhenReturningHomeMenu ()
    {
        if (getPlayerListCount () > 1) {
            for (int i = 0; i < playerList.Count; i++) {
                playerList [i].mainSaveGameSystem.saveGameWhenReturningHomeMenu ();
            }
        } else {
            if (getPlayerListCount () == 1) {
                playerList [0].mainSaveGameSystem.saveGameWhenReturningHomeMenu ();
            }
        }
    }

    public void checkIfPlayerIsLookingAtDeadTarget (Transform deadTarget, Transform placeToShoot)
    {
        int playerListCount = playerList.Count;

        for (int i = 0; i < playerListCount; i++) {

            playerCamera currentPlayerCamera = playerList [i].playerCameraManager;

            if (currentPlayerCamera != null && currentPlayerCamera.isPlayerLookingAtTarget ()) {
                Transform currentTargetToLook = currentPlayerCamera.getCurrentTargetToLook ();

                bool isSameObject = false;

                if (currentTargetToLook == deadTarget) {
                    isSameObject = true;
                }

                if (!isSameObject) {
                    if (currentTargetToLook == placeToShoot) {
                        isSameObject = true;
                    }
                }

                if (!isSameObject) {
                    if (applyDamage.checkIfWeakSpotListContainsTransform (deadTarget.gameObject, currentTargetToLook)) {
                        isSameObject = true;
                    }
                }

                if (isSameObject) {
                    //print ("target found, disabling look at target");

                    currentPlayerCamera.checkIfPlayerIsLookingAtDeadTarget (false);

                    currentPlayerCamera.checkEventOnLockOnEnd ();
                } else {
                    //print ("TARGET NOT FOUND");
                }
            }
        }
    }

    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);
    }

    [System.Serializable]
    public class playerInfo
    {
        public string Name;
        public GameObject playerParentGameObject;
        public GameObject playerControllerGameObject;
        public GameObject playerCameraGameObject;
        public playerController playerControllerManager;
        public playerCamera playerCameraManager;

        public playerInputManager playerInput;
        public menuPause pauseManager;

        public playerComponentsManager mainPlayerComponentsManager;
        public friendListManager mainFriendListManager;

        public switchCompanionSystem mainSwitchCompanionSystem;

        public saveGameSystem mainSaveGameSystem;

        public bool useByAI;

        public bool showEventSettings;
        public UnityEvent eventToSetCharacterAsAI;
        public UnityEvent eventToSetCharacterAsPlayer;

        public bool useOnlyKeyboard;

        public bool followMainPlayerOnSwitchCompanion = true;
    }

    [System.Serializable]
    public class cameraStates
    {
        public string Name;
        public int numberfOfPlayers;

        public List<cameraInfo> cameraInfoList = new List<cameraInfo> ();
    }

    [System.Serializable]
    public class cameraInfo
    {
        public float newX = 0;
        public float newY = 0;
        public float newW = 0;
        public float newH = 0;
    }
}
