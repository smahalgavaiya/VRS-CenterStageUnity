using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationController : MonoBehaviour, IOnEventCallback
{
    public List<string> randomLocs = new List<string>();
    private int chosenLoc;

    private void Awake()
    {
        FieldManager.OnFieldSetup += SetRandomization;
        PhotonNetwork.AddCallbackTarget(this);
        //SetRandomization();
    }

    private void OnDestroy()
    {
        FieldManager.OnFieldSetup -= SetRandomization;
    }


    public void SetRandomization()
    {
        
        if(PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int chosenLoc = GetRandomization();
                System.Object[] data = { chosenLoc };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All,
                    CachingOption = EventCaching.AddToRoomCache
                };
                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };
                PhotonNetwork.RaiseEvent((byte)FTC_EventCode.gameSetup, data, raiseEventOptions, sendOptions);
            }
        }
        else
        {
            SetRandomizationValue();
        }
        
    }

    public void SetRandomizationValue(int chosenLoc = -1)
    {
        if(chosenLoc < 0)
        {
            chosenLoc = Random.Range(0, randomLocs.Count);
        }
        
        PlaceRandomizationObject[] objs = FindObjectsByType<PlaceRandomizationObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (PlaceRandomizationObject obj in objs)
        {
            obj.OnRandomization.AddListener((obj) => FieldManager.fm.quickAttachPhotonView(obj));
            obj.Place(randomLocs[chosenLoc]);
        }
    }

    public int GetRandomization()
    {
        chosenLoc = Random.Range(0, randomLocs.Count);
        return chosenLoc;
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)FTC_EventCode.gameSetup)
        {
            System.Object[] data = (System.Object[])photonEvent.CustomData;
            int loc = (int)data[0];
            SetRandomizationValue(loc);
        }
    }
}
