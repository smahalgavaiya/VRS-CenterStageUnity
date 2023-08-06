using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrabberMP : MonoBehaviourPunCallbacks
{
    ObjectGrabber grabber;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        grabber = GetComponent<ObjectGrabber>();
        view = GetComponent<PhotonView>();
        Invoke("CheckMP", 0.5f);
    }

    void CheckMP()
    {
        if(PhotonNetwork.IsConnected)
        {
            grabber.mpOverride = true;
        }
    }

    [PunRPC]
    void runGrab()
    {
        grabber.PickUpOrPutDownObject();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(view.IsMine && grabber)
        {
            if (grabber.checkDrive() != GrabberAction.NoAction)
            {
                //grabber.PickUpOrPutDownObject();
                view.RPC("runGrab", RpcTarget.AllBuffered);
            }
            //runGrab();

        }
    }
}
