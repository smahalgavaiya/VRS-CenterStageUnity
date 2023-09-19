using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour, IOnEventCallback
{
    private Rigidbody rig;
    bool released = false;
    public float power = 50;
    private PhotonView parentView;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponentInChildren<Rigidbody>();
        parentView = GetComponentInParent<PhotonView>();
    }

    public void Release(float force)
    {
        if(!released)
        {
            
            if (PhotonNetwork.IsConnected && parentView.IsMine)
            {

                FieldManager.fm.quickAttachPhotonView(gameObject);
                int actor = parentView.OwnerActorNr;
                System.Object[] data = { actor, force };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All,
                    CachingOption = EventCaching.AddToRoomCache
                };
                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };
                PhotonNetwork.RaiseEvent((byte)FTC_EventCode.plane, data, raiseEventOptions, sendOptions);
            }
            
            
        }


    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)FTC_EventCode.plane)
        {
            System.Object[] data = (System.Object[])photonEvent.CustomData;
            int actor = (int)data[0];
            float force = (float)data[1];
            if (parentView.OwnerActorNr == actor)
            {
                ScoreObjectTypeLink link = GetComponent<ScoreObjectTypeLink>();
                link.LastTouchedTeamColor = transform.root.gameObject.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor;
                transform.parent = null;
                rig.isKinematic = false;
                rig.velocity = -transform.up * (force * power);
                //rig.AddRelativeForce(-transform.forward* (force * 100));
                released = true;

            }
        }
    }
}
