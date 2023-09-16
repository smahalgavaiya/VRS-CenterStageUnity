using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class KBindOverride
{
    public string officialName;
    public string overrideName;
}

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
        input = FindFirstObjectByType<PlayerInput>();
        CourseData course = SimManager.CurrentCourse;
        foreach (InputAction action in _inputActionMap.actions)
        {
            //Bind b = new Bind { bind = action.GetBindingDisplayString(), name = action.name };
            string bindName = action.name;
            string origName = action.name;
            string newName = course.getBindName(bindName);
            if (newName!="") { bindName = newName; }
            string binding = action.GetBindingDisplayString().ToUpper();
            if(binding == "")//This was added because unity decided to break getbindingdisplaystring on axis controls
            {
                string b = action.ToString();
                string[] barr = b.Split('/');
                foreach (string snip in barr)
                {
                    if(snip.Contains("Gameplay") || snip.Contains(origName) || snip.Contains("Keyboard"))
                    {
                        continue;
                    }
                    string outp = snip.TrimEnd(',');
                    outp = outp.TrimEnd(']');
                    binding += outp.ToUpper() + "|";
                }
                binding = binding.TrimEnd('|');
            }

            if(bindName == "Unused" || bindName == "ShowControls")
            {
                continue;
            }

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "bind", binding },
                { "name", bindName},
                { "controlScheme", input.currentControlScheme }
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            if(input.currentControlScheme.ToLower() == "gamepad")
            {
                data["bind"] = binding.Split('|')[1];
                data["bind"] = data["bind"].TrimStart(' ');
            }
            else
            {
                data["bind"] = binding.Split('|')[0];
            }
#endif
            GameObject kb = Instantiate(bindPrefab, transform);
            datafill.Fill(data, kb);
        }
    }
}
