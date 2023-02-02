using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class DisplayViewInfo : MonoBehaviour
{
    public GameObject objectToTrack;
    public Component selComp;
    public string selVar;
    public TextMeshProUGUI text;

    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetNewConeView(GameObject obj)
    {
        objectToTrack = obj;
        view = objectToTrack.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view != null)
        {
            text.text = $"VID:{view.ViewID}\nOwner:{view.Owner},{view.OwnerActorNr}\nIsOwner:{view.IsMine}\n";
        }

    }
}
