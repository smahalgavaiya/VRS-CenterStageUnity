using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamInventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<string,List<GamepieceStack>> inventory = new Dictionary<string,List<GamepieceStack>>();
    private List<string> stackTypes = new List<string>();
    private int activePiece = 0;

    public Team assignedTeam;

    public string CurrentPiece { get { return $"{stackTypes[activePiece]} : {CurrentCount()}";  } } // this needs to be optimized or displayobjval shouldnt call per frame
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

    public void Release()
    {
        List<GamepieceStack> curStacks = inventory[stackTypes[activePiece]];
        foreach(GamepieceStack stack in curStacks)
        {
            if(stack.Count > 0) { stack.Release(); break; }
        }
        if (CurrentCount() == 0) { NextType(); }//should be next type WITH non zero count. and disable button if none left
    }

    public void NextType()
    {
        activePiece++;
        if(activePiece >= stackTypes.Count ) { activePiece = 0; }
    }

}
