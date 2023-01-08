using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonDebug : MonoBehaviour
{
    public enum PhotonType { Connected,ConnectedReady,MasterClient };
    public PhotonType msg;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(msg)
        {
            case PhotonType.Connected:
                text.text = $"Connected : {PhotonNetwork.IsConnected}";
                break;
            case PhotonType.ConnectedReady:
                text.text = $"ConnectedReady : {PhotonNetwork.IsConnectedAndReady}";
                break;
            case PhotonType.MasterClient:
                text.text = $"isMasterClient : {PhotonNetwork.IsMasterClient}";
                break;
            default: break;
        }
    }
}
