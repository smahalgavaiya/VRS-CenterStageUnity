using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkControls : MonoBehaviour
{
    public void ExitGame()
    {
        SettingsControl.RemoveMPComponents();
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public void Reconnect()
    {
        PhotonNetwork.ConnectToBestCloudServer();
    }
}
