using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TeamInventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<string,List<GamepieceStack>> inventory = new Dictionary<string,List<GamepieceStack>>();
    private List<string> stackTypes = new List<string>();
    private int activePiece = 0;

    public Team assignedTeam;

    public UnityEvent<GameObject> onRelease;

    public string CurrentPiece { get { return $"{stackTypes[activePiece]}";  } } // this needs to be optimized or displayobjval shouldnt call per frame
    public string PieceCount { get { return $"x{CurrentCount()}";  } } // this needs to be optimized or displayobjval shouldnt call per frame
    public Color Color { get { return CurrentColor();  } } 
    void Start()
    {
        List<GamepieceStack> stacks = GetComponentsInChildren<GamepieceStack>().ToList();
        foreach(GamepieceStack stack in stacks)
        {
            if(!inventory.ContainsKey(stack.piecePrefab.name))
            {
                List<GamepieceStack> st = new List<GamepieceStack>();
                st.Add(stack);
                inventory.Add(stack.piecePrefab.name, st);
                stackTypes.Add(stack.piecePrefab.name);
            }
            else
            {
                inventory[stack.piecePrefab.name].Add(stack);
            }
        }
    }
    //change prefab to scoring type and get prefab from that?

    public int CurrentCount()
    {
        string active = stackTypes[activePiece];

        List<GamepieceStack> stacks = inventory[active];
        int cnt = 0;
        foreach(GamepieceStack stack in stacks)
        {
            cnt += stack.Count;
        }
        return cnt;
    }

    public Color CurrentColor()
    {
        string active = stackTypes[activePiece];
        Color purple = new Color32(191, 64, 191,255);
        string color = active.Split("Pixel")[0];
        switch(color)
        {
            case "White":
                return Color.white;
            case "Purple":
                return purple;
            case "Green":
                return Color.green;
            case "Yellow":
                return Color.yellow;
        }
        return Color.red;
    }

    public void Release()
    {
        List<GamepieceStack> curStacks = inventory[stackTypes[activePiece]];
        foreach(GamepieceStack stack in curStacks)
        {
            if(stack.Count > 0) 
            { 
                GameObject piece = stack.Release();
                onRelease.Invoke(piece);
                break; 
            }
        }
        if (CurrentCount() == 0) { NextType(); }//should be next type WITH non zero count. and disable button if none left
    }

    public void NextType()
    {
        activePiece++;
        if(activePiece >= stackTypes.Count ) { activePiece = 0; }
    }

}
