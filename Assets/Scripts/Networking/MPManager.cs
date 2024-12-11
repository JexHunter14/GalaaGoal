// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using System.Collections;
public class MPManager : Photon.Pun.MonoBehaviourPunCallbacks{
    public string GameVersion;
    private string roomName = "MyRoom";
    private bool isSpawnerInstantiated = false;

    private static Vector3[] playerPositions = {
      new Vector3(-4, 7, 0),
      new Vector3(3, 6, 0),
      new Vector3(-4,-7,0),
      new Vector3(3,-6,0)
    };

    private static Vector3[] healthPositions = {
      new Vector3(-300, 153, 0),
      new Vector3(172, 153, 0),
      new Vector3(-300,-183,0),
      new Vector3(172,-183,0)
    };

    void Start()
    {
        ConnectToMaster();
    }

    // Connect to the Photon Master server
    public void ConnectToMaster()
    {
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        //Debug.Log("Attempting to connect to the Photon Master server...");
    }

    // Called when connected to the Photon Master server
    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master server. Now joining the lobby...");
        PhotonNetwork.JoinLobby();
    }

    // Called when the client joins the lobby
    public override void OnJoinedLobby()
    {
        //Debug.Log("Joined the lobby. Now joining or creating a room...");
        JoinRoom();
    }

    // Join a specific room
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // Create a room if joining fails
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Debug.LogWarning($"JoinRoom failed: {message}. Creating a new room...");
        CreateRoom();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        //Debug.Log("Created a new room: " + roomName);
    }

    public override void OnJoinedRoom()
    {
      //Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom.Name);



      int numPlayers = PhotonNetwork.CurrentRoom.PlayerCount;


    // Instantiate Player and Score objects

    GameObject playerObject = PhotonNetwork.Instantiate("Player", playerPositions[numPlayers-1], Quaternion.identity, 0);
    GameObject scoreObject = PhotonNetwork.Instantiate("Score", Vector3.zero, Quaternion.identity, 0);
    GameObject Health = PhotonNetwork.Instantiate("Health", healthPositions[numPlayers - 1], Quaternion.identity, 0);

    playerObject.tag = "Player";
    // Get the required components
    DiamondCollectorOnline diamondCollector = playerObject.GetComponent<DiamondCollectorOnline>();
    scorerules scoreRules = scoreObject.GetComponent<scorerules>();
    updateHealth upHealth = Health.GetComponent<updateHealth>();


    SpriteRenderer playerRender = playerObject.GetComponent<SpriteRenderer>();
    Text ScoreText = scoreObject.GetComponent<Text>();

    string newname;


    GameObject canvasObject = GameObject.Find("Canvas");
      if (canvasObject == null)
          {
            //Debug.LogError("Canvas not found in the scene. Ensure a Canvas exists and is named 'Canvas'.");
            return;
      } else {
               //Debug.LogError("Canvas found");
             }
    GameObject endGamePanel = PhotonNetwork.Instantiate("endGamePanel", Vector3.zero, Quaternion.identity);
    endGamePanel.transform.SetParent(canvasObject.transform, false);
    upHealth.transform.SetParent(canvasObject.transform, false);
    PhotonView PanelPV = endGamePanel.GetComponent<PhotonView>();
    PanelPV.RPC("setFalse", RpcTarget.AllBuffered);


    Color playerCountColor;

    if(numPlayers == 1)
    {
      playerCountColor = Color.red;
      if(PhotonNetwork.IsMasterClient){
      GameObject timer = PhotonNetwork.Instantiate("Timer", new Vector3(77,190,0), Quaternion.identity);
      }
      newname = "Player1";

    }
    else if (numPlayers == 2)
    {
      playerCountColor = Color.blue;
      newname = "Player2";
    }
    else if (numPlayers == 3)
    {
      playerCountColor = Color.magenta;
      newname = "Player3";
    }
    else
    {
      playerCountColor = Color.green;
      newname = "Player4";
    }

    if (diamondCollector == null || scoreRules == null)
    {
        //Debug.LogError("Required components missing on instantiated objects!");
        return;
    }
    // Set local references
    diamondCollector.myScoreObject = scoreObject;
    diamondCollector.name = newname;
    scoreRules.diamondcollectoronline = diamondCollector;
    upHealth.player = playerObject;


    int Pv =  playerObject.GetPhotonView().ViewID;
    int Sv = scoreObject.GetPhotonView().ViewID;
    int Hv = Health.GetPhotonView().ViewID;

    playerRender.color = playerCountColor;
    ScoreText.color = playerCountColor;
    // Synchronize references across all clients
   photonView.RPC("SyncReferences", RpcTarget.AllBuffered, Pv, Sv, numPlayers, Hv);

    // Transfer ownership of the score object to the local player
    PhotonView scorePV = scoreObject.GetComponent<PhotonView>();
    scorePV.TransferOwnership(PhotonNetwork.LocalPlayer);

        // Only the master client will check for the spawner creation
        if (PhotonNetwork.IsMasterClient)
        {
          //Debug.Log("on the master client");
          //Debug.LogError("on the master client");
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SpawnerInstantiated")
                && (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnerInstantiated"])
            {
                //Debug.Log("Spawner already instantiated, skipping spawner creation.");
            }
            else
            {
                // Instantiate the spawner and update room properties
                //Debug.LogError("Instantitating diamond should only work on master client though");
                PhotonNetwork.Instantiate("DiamondSpawner", Vector3.zero, Quaternion.identity);
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                props.Add("SpawnerInstantiated", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }

  [PunRPC]
  public void SyncReferences(int playerViewID, int scoreViewID, int numPlayers, int HealthViewID)
  {
    string newname;
      Color playerCountColor;

      if(numPlayers == 1)
      {
        playerCountColor = Color.red;
        newname = "Player1";
      }
      else if (numPlayers == 2)
      {
        playerCountColor = Color.blue;
        newname = "Player2";
      }
      else if (numPlayers == 3)
      {
        playerCountColor = Color.magenta;
        newname = "Player3";
      }
      else
      {
        playerCountColor = Color.green;
        newname = "Player4";
      }

      PhotonView playerPV = PhotonView.Find(playerViewID);
      PhotonView scorePV = PhotonView.Find(scoreViewID);
      PhotonView healthPV = PhotonView.Find(HealthViewID);

      if (playerPV != null && scorePV != null && healthPV != null)
      {
          DiamondCollectorOnline diamondCollector = playerPV.GetComponent<DiamondCollectorOnline>();
          scorerules scoreRules = scorePV.GetComponent<scorerules>();
          SpriteRenderer playerRender = playerPV.GetComponent<SpriteRenderer>();
          updateHealth upHealth = healthPV.GetComponent<updateHealth>();
          Text ScoreText = scorePV.GetComponent<Text>();
          playerPV.gameObject.tag = "Player";
          GameObject canvasObject = GameObject.Find("Canvas");
          healthPV.gameObject.transform.SetParent(canvasObject.transform, false);

          if (diamondCollector != null && scoreRules != null)
          {
              // Synchronize references
              diamondCollector.myScoreObject = scorePV.gameObject;
              diamondCollector.Health = healthPV.gameObject;

              upHealth.player = playerPV.gameObject;

              scoreRules.diamondcollectoronline = diamondCollector;
              diamondCollector.name = newname;

              playerRender.color = playerCountColor;
              ScoreText.color = playerCountColor;
              //Debug.Log($"References synchronized: Player {playerViewID}, Score {scoreViewID}");
          }
      }
  }

    private IEnumerator AssignScoreObjectDelayed(DiamondCollectorOnline collector, GameObject scoreObject)
    {
        yield return new WaitForEndOfFrame(); // Wait until the end of the current frame

        // Assign the score object
        collector.myScoreObject = scoreObject;

        // Log success for debugging
        //Debug.Log($"Score object successfully assigned to player {PhotonNetwork.LocalPlayer.ActorNumber}");
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // Check if the "SpawnerInstantiated" property was updated
        if (propertiesThatChanged.ContainsKey("SpawnerInstantiated"))
        {
            if ((bool)propertiesThatChanged["SpawnerInstantiated"])
            {
                //Debug.Log("Spawner has been instantiated, received via Room Properties.");
                isSpawnerInstantiated = true;
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene loaded: " + scene.name);
    }

    public void ReturnToMainMenu(){
      StartCoroutine(LeaveRoomAndDisconnect());
    }

    private IEnumerator LeaveRoomAndDisconnect()
    {
      if(PhotonNetwork.InRoom){
        //Debug.LogError("Leaving Room started");
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom){
          //Debug.LogError("Leaving Room");
          yield return null;
        }
      }
      if(PhotonNetwork.InLobby){
        //Debug.LogError("Leaving Lobby started");
        PhotonNetwork.LeaveLobby();
        while(PhotonNetwork.InLobby){
          //Debug.LogError("Leaving Lobby");
          yield return null;
        }
      }

      {
        //Debug.LogError("Disconnecting from Photon...");
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            //Debug.LogError("Disconnecting...");
            yield return null;
        }
    }
      //Debug.LogError("Returing to main menu");
      SceneManager.LoadScene(1);
    }
    public override void OnLeftRoom(){
      //Debug.Log("Left Room Successfully");
    }
    public override void OnDisconnected(DisconnectCause cause){
      //Debug.Log($"Disconnected from Photon: {cause}");
    }
}
