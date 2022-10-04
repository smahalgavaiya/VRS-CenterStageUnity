using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class RobotNetControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonView v = GetComponent<PhotonView>();
            //if (GetComponent<PhotonView>().IsMine)
            {
                ColorSwitcher c = GetComponent<ColorSwitcher>();
                c.TeamColor_ = MultiplayerSetting.multiplayerSetting.getPlayerTeam(v.ControllerActorNr);
                c.SetColor();
                //ideally should probably only do this if .isMine and set up RPCs to other players to update players of their own color.
            }
            if(!GetComponent<PhotonView>().IsMine)
            {//have to disable Control + Rigidbodies on non-player robot so that movement is properly tracked
                GetComponent<DriveReceiverSpinningWheels>().enabled = false;
                ArticulationBody[] bodies = GetComponentsInChildren<ArticulationBody>();
                foreach(ArticulationBody b in bodies)
                {
                    b.enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
