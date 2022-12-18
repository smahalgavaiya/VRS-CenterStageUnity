using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ReleaseObjectInField : MonoBehaviourPunCallbacks
{
    public List<GameObject> Objects;
    public List<GameObject> Location;
    public bool cloneObject = false;
    TeamColor team;
    PhotonView view;

    private void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
        if(view == null) { view = PowerPlayFieldManager.attachView(gameObject); }
    }

    private void Reset()
    {
        Objects.Clear();
        for(int i = 0; i < 2; i++)//for team colors.
        {
            Objects.Add(null);
            Location.Add(null);
        }  
    }

    public void Release(int team)
    {
        if (PhotonNetwork.IsConnected)
        {
            view.RPC("ReleaseObj", RpcTarget.All, team);
        }
        else
        {
            ReleaseObj(team);
        }
    }

    [PunRPC]
    public void ReleaseObj(int team)
    {
        GameObject obj = Objects[(int)team];
        if (cloneObject)
        {
            obj = Instantiate(Objects[team]);
            obj.GetComponent<ColorSwitcher>().TeamColor_ = (TeamColor)team;
            obj.GetComponent<ColorSwitcher>().SetColor();
        }
        obj.transform.position = Location[(int)team].transform.position;
        obj.GetComponent<Rigidbody>().isKinematic = false;

        if (PhotonNetwork.IsConnected)
        {
            PowerPlayFieldManager.attachView(obj,true);
        }

    }
}
