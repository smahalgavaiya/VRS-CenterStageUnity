using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GrabberMP : MonoBehaviour, IOnEventCallback
{
    ObjectGrabber grabber;
    PhotonView parentView;
    // Start is called before the first frame update
    void Start()
    {
        grabber = GetComponent<ObjectGrabber>();
        parentView = transform.root.GetComponent<PhotonView>();
        if (PhotonNetwork.IsConnected)
        {
            //req'd for scripts that dont have a photonview on the same gameobject. otherwise onevent doesnt fire.
            PhotonNetwork.AddCallbackTarget(this);
        }
        Invoke("CheckMP", 0.5f);
    }

    void CheckMP()
    {
        if(PhotonNetwork.IsConnected)
        {
            grabber.mpOverride = true;
            CancelInvoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(parentView.IsMine && grabber)
        {
            if (grabber.checkDrive() != GrabberAction.NoAction)
            {
                //grabber.PickUpOrPutDownObject();
                //view.RPC("runGrab", RpcTarget.AllBuffered);
                int actor = parentView.OwnerActorNr;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All,
                    CachingOption = EventCaching.AddToRoomCache
                };
                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };
                PhotonNetwork.RaiseEvent((byte)FTC_EventCode.grabber,actor,raiseEventOptions,sendOptions);
            }
            //runGrab();

        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)FTC_EventCode.grabber)
        {
            int actor = (int)photonEvent.CustomData;
            if(parentView.OwnerActorNr == actor)
            {
                grabber.pickupCheck();
            }
        }
    }
}
