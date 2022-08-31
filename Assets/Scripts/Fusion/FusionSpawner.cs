using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class FusionSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    public Transform[] spawnPositions;

    public InputActionManager IAM;

    [SerializeField] private NetworkPrefabRef _sliderPrefab;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    private void OnGUI()
    {
        if(_runner == null)
        {
            if (GUI.Button(new Rect(250,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(250,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }            
        }
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined: " + player.PlayerId);
        if(runner.IsServer)
        {
            Debug.Log("Spawning Player");
            Vector3 spawnPos = spawnPositions[player.RawEncoded%runner.Config.Simulation.DefaultPlayers].TransformPoint(spawnPositions[player.RawEncoded%runner.Config.Simulation.DefaultPlayers].position);
            Debug.Log("Spawn Positon Set: x" + spawnPos.x + " y" + spawnPos.y + " z" + spawnPos.z);
            NetworkObject networkPlayerObject = _runner.Spawn(_sliderPrefab,spawnPos,Quaternion.identity,player);
            DriveReceiverSpinningWheels dr = networkPlayerObject.GetComponent<DriveReceiverSpinningWheels>();
            Drive frontRight = Instantiate(dr.frontRight);
            Drive frontLeft = Instantiate(dr.frontLeft);
            Drive backRight = Instantiate(dr.backRight);
            Drive backLeft = Instantiate(dr.backLeft);
            dr.frontRight = frontRight;
            dr.frontLeft = frontLeft;
            dr.backRight = backRight;
            dr.backLeft = backLeft;
            dr.UpdateDrivers();
            IAM.frontRightWheel = frontRight;
            IAM.frontLeftWheel = frontLeft;
            IAM.backRightWheel = backRight;
            IAM.backLeftWheel = backLeft;
            IAM.robot = networkPlayerObject.gameObject;

            Debug.Log("Player Object: " + networkPlayerObject.Name);
            _spawnedCharacters.Add(player,networkPlayerObject);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if(_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
