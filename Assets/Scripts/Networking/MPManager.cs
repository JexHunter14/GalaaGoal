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
public class MPManager : Photon.Pun.MonoBehaviourPunCallbacks{
// {
//   public string GameVersion;
//   //public TextMeshProUGUI connectState;
//   private string roomName = "MyRoom";
//   private bool isSpawnerInstantiated = false;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//       ConnectToMaster();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//
//     }
//
//     private void FixedUpdate(){
//         //connectState.text = PhotonNetwork.NetworkClientState.ToString();
//     }
//
// //     public void ConnectToMaster(){
// //       PhotonNetwork.GameVersion = GameVersion;
// //       PhotonNetwork.ConnectUsingSettings();
// //     }
// //     public void CreateorJoin(){
// //       PhotonNetwork.JoinRandomRoom();
// //     }
// //     public virtual void OnPhotonRandomJoinFailed(){
// //       RoomOptions rm = new RoomOptions {
// //         MaxPlayers = 2,
// //         IsVisible = true
// //       };
// //       int rndID = Random.Range(0, 100);
// //       PhotonNetwork.CreateRoom("Default: " + rndID, rm, TypedLobby.Default);
// //     }
// //     public virtual void OnJoinedRoom(){
// //       GameObject Player = PhotonNetwork.Instantiate("Capsule", Vector2.zero, Quaternion.identity, 0);
// //     }
// public void ConnectToMaster()
//     {
//         PhotonNetwork.GameVersion = GameVersion;
//         PhotonNetwork.AutomaticallySyncScene = true;
//         PhotonNetwork.ConnectUsingSettings();
//         Debug.Log("Attempting to connect to the Photon Master server...");
//     }
//
//     // Called when the client successfully connects to the Photon Master server
//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to Master server. Now joining the lobby...");
//         PhotonNetwork.JoinLobby();  // Join the default lobby
//     }
//
//     // Called when the client joins the lobby
//     public override void OnJoinedLobby()
//     {
//         Debug.Log("Joined the lobby. Now ready for matchmaking.");
//         JoinRoom();  // Now we can join a room safely
//     }
//
//     // Called when the client fails to join a random room
//     public override void OnJoinRoomFailed(short returnCode, string message)
//     {
//         Debug.LogWarning($"JoinRoom failed: {message}. Creating a new room...");
//         CreateRoom();  // If no room is found, create one
//     }
//
//     // Attempt to join a random room
//     public void JoinRoom()
//     {
//         PhotonNetwork.JoinRoom(roomName);
//
//     }
//
//     // Create a new room if joining fails
//     public void CreateRoom()
//     {
//         //string roomName = "Room" + Random.Range(0, 10000);
//         RoomOptions roomOptions = new RoomOptions();
//         roomOptions.MaxPlayers = 4;
//         PhotonNetwork.CreateRoom(roomName, roomOptions);
//         Debug.Log("Created a new room: " + roomName);
//     }
//   public override void OnJoinedRoom(){
//     Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom.Name);
//     GameObject Player = PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity, 0);
//     if (PhotonNetwork.IsMasterClient){
//       if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SpawnerInstantiated")
//       && (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnerInstantiated"]){
//         Debug.Log("Spawner already instantiated, skipping spawner creation");
//       } else {
//         PhotonNetwork.Instantiate("DiamondSpawner", Vector3.zero, Quaternion.identity);
//         ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
//         props.Add("SpawnerInstantiated", true);
//         PhotonNetwork.CurrentRoom.SetCustomProperties(props);
//
//         photonView.RPC("NotifySpawnerInstatiated", RpcTarget.Others);
//       }
//     }
//     //PhotonNetwork.LoadLevel(2);
//     // foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
//     // {
//     //     Debug.Log($"Player in Room: {player.NickName} | ID: {player.ActorNumber}");
//     // }
//   }
//   [PunRPC]
//   public void NotifySpawnerInstatiated(){
//     Debug.Log("Spawner has been instantiated by the master client");
//     isSpawnerInstantiated = true;
//
//   }
//   public override void OnMasterClientSwitched(Player newMasterClient){
//     Debug.Log("New Master Client: " + newMasterClient.NickName);
//   }
//   public override void OnEnable(){
//     base.OnEnable();
//     Debug.Log("OnEnable called: Subscribing to Photon and sceneLoaded event");
//     PhotonNetwork.AddCallbackTarget(this);
//     SceneManager.sceneLoaded += OnSceneLoaded;
//   }
//   // public override void OnDisable(){
//   //   base.OnDisable();
//   //    Debug.Log("OnDisable called: Unsubscribing from Photon and sceneLoaded event");
//   //   PhotonNetwork.RemoveCallbackTarget(this);
//   //   SceneManager.sceneLoaded -= OnSceneLoaded;
//   // }
//   public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
//     Debug.Log("Scene loaded: " + scene.name);
//     if(scene.buildIndex == 2){
//       Debug.Log("Scene 2 loaded. Instantiating player...");
//
//        //GameObject Player = PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity, 0);
//     }
//   }
// }



    public string GameVersion;
    private string roomName = "MyRoom";
    private bool isSpawnerInstantiated = false;

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
        Debug.Log("Attempting to connect to the Photon Master server...");
    }

    // Called when connected to the Photon Master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master server. Now joining the lobby...");
        PhotonNetwork.JoinLobby();
    }

    // Called when the client joins the lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the lobby. Now joining or creating a room...");
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
        Debug.LogWarning($"JoinRoom failed: {message}. Creating a new room...");
        CreateRoom();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Created a new room: " + roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogError("successfully joined room");
        // Instantiate the player object
        PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity, 0);

        // Only the master client will check for the spawner creation
        if (PhotonNetwork.IsMasterClient)
        {
          Debug.Log("on the master client");
          Debug.LogError("on the master client");
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SpawnerInstantiated")
                && (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnerInstantiated"])
            {
                Debug.Log("Spawner already instantiated, skipping spawner creation.");
            }
            else
            {
                // Instantiate the spawner and update room properties
                Debug.LogError("Instantitating diamond should only work on master client though");
                PhotonNetwork.Instantiate("DiamondSpawner", Vector3.zero, Quaternion.identity);
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                props.Add("SpawnerInstantiated", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // Check if the "SpawnerInstantiated" property was updated
        if (propertiesThatChanged.ContainsKey("SpawnerInstantiated"))
        {
            if ((bool)propertiesThatChanged["SpawnerInstantiated"])
            {
                Debug.Log("Spawner has been instantiated, received via Room Properties.");
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
        Debug.Log("Scene loaded: " + scene.name);
    }
}
