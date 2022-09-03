using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DisplayKeyBinds : MonoBehaviour
{
    [SerializeField] private InputActionAsset controls;
    public PlayerInput input;
    public GameObject bindPrefab;
    private InputActionMap _inputActionMap;
    private UIDataFill datafill;
    // Start is called before the first frame update
    void Start()
    {
        datafill = GetComponent<UIDataFill>();
        _inputActionMap = controls.FindActionMap("Gameplay");
        foreach(InputAction action in _inputActionMap.actions)
        {
            Bind b = new Bind { bind = action.GetBindingDisplayString(), name = action.name };
            GameObject kb = Instantiate(bindPrefab, transform);
            datafill.Fill(b, kb);
        }
    }
}
