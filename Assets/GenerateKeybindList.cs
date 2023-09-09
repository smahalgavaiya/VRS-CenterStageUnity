using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.UI;

public class GenerateKeybindList : MonoBehaviour
{
    public InputActionAsset actions;
    public GameObject actionPrefab;
    public string actionMapName;
    public int bindingsOffset = 0;
    public GameObject bindOverlay;

    // Start is called before the first frame update
    void Start()
    {
        InputActionMap _inputActionMap = actions.FindActionMap(actionMapName);
        //InputDevice[] devices = actions.devices.Value.ToArray();
        CourseData course = SimManager.CurrentCourse;


        foreach (InputAction action in _inputActionMap.actions)
        {

            GameObject obj = GameObject.Instantiate(actionPrefab,transform);
            RebindActionUI bind = obj.GetComponent<RebindActionUI>();
            bind.actionReference = InputActionReference.Create(action);
            if(bindingsOffset != 0) { bindingsOffset = action.bindings.Count - 1; }//need to be able to get control schemes and the binds for them
            bind.bindingId = action.bindings[bindingsOffset].id.ToString();
            bind.rebindOverlay = bindOverlay;
            bind.rebindPrompt = bindOverlay.GetComponentInChildren<Text>();
            string rename = course.getBindName(bind.actionLabel.text);
            if (rename!="")
            {
                bind.actionLabel.text = rename;
            }
            
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        PlayerInput input = FindFirstObjectByType<PlayerInput>();
        input.DeactivateInput();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        PlayerInput input = FindFirstObjectByType<PlayerInput>();
        input.ActivateInput();
    }

}
