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
    bool canScore = false;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponentInChildren<Rigidbody>();
        parentView = GetComponentInParent<PhotonView>();
        if (PhotonNetwork.IsConnected)
        {
            //req'd for scripts that dont have a photonview on the same gameobject. otherwise onevent doesnt fire.
            PhotonNetwork.AddCallbackTarget(this);
        }
        CollisionNotifier.onCollideGlobal += SetInCorrectZone;
    }

    private void OnDestroy()
    {
        CollisionNotifier.onCollideGlobal -= SetInCorrectZone;
    }

    public void SetInCorrectZone(GameObject collidingObj, bool isEntering)
    {
        //in this case the colliding object should be the root robot.
        ScoreObjectTypeLink link = collidingObj.transform.root.gameObject.GetComponent<ScoreObjectTypeLink>();
        if (link && link.ScoreObjectType_.name == "Robot")
        {
            canScore = isEntering;
        }

    }

    public void Release(float force)
    {
        if (!released)
        {
            if (PhotonNetwork.IsConnected && parentView.IsMine)
            {
                ScoreObjectTypeLink link = GetComponent<ScoreObjectTypeLink>();
                link.LastTouchedTeamColor = transform.root.gameObject.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor;

                FieldManager.fm.quickAttachPhotonView(gameObject);
                int actor = parentView.OwnerActorNr;
                System.Object[] data = { actor, force, link.LastTouchedTeamColor };
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
            else
            {
                DoRelease(force);
            }
        }

    }

    private void DoRelease(float force, TeamColor color = TeamColor.Either)
    {
        ScoreObjectTypeLink link = GetComponent<ScoreObjectTypeLink>();
        if (color == TeamColor.Either)
        {
            link.LastTouchedTeamColor = transform.root.gameObject.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor;
        }
        else
        {
            link.LastTouchedTeamColor = color;
        }
        
        if(!canScore)
        {
            link.LastTouchedTeamColor = TeamColor.Either;
        }
        
        transform.parent = null;
        rig.isKinematic = false;
        rig.velocity = -transform.up * (force * power);
        //rig.AddRelativeForce(-transform.forward* (force * 100));
        released = true;
        if(PhotonNetwork.IsConnected)
        {
            //FieldManager.fm.quickAttachPhotonView(gameObject);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("plane event");
        if (photonEvent.Code == (byte)FTC_EventCode.plane)
        {
            System.Object[] data = (System.Object[])photonEvent.CustomData;
            int actor = (int)data[0];
            float force = (float)data[1];
            TeamColor color = (TeamColor)data[2];
            if (parentView.OwnerActorNr == actor)
            {
                DoRelease(force,color);
            }
        }
    }
}
