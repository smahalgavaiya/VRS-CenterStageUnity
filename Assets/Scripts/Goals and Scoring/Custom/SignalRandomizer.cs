using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SignalRandomizer : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] List<GameObject> locations;

    private void Start()
    {
        SetRandomLocation();
    }

    public override void OnConnected()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            SetRandomLocation();
        }
    }

    void SetRandomLocation()
    {
        foreach (GameObject location in locations)
        {
            location.SetActive(false);
        }

        int randomLocation = Random.Range(0, 3);

        if (!PhotonNetwork.IsConnected)
        {
            SetLocation(randomLocation);
            return;
        }

        if (!PhotonNetwork.IsMasterClient) { return; }
        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All

        };


        PhotonNetwork.RaiseEvent(0, (int)randomLocation, options, ExitGames.Client.Photon.SendOptions.SendReliable);

    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == (byte)0)
        {
            SetLocation((int)photonEvent.CustomData);
        }
    }

    [PunRPC]
    void SetLocation(int idx)
    {
        locations[idx].SetActive(true);
    }

}
