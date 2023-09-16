using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamepadUI
{
    public static string GetGamepadIcon(string button)
    {
#if UNITY_WEBGL
        
#endif

        //string[] keys = button.Split('/');
        //foreach(string key in keys)
        //{
            switch (button)
            {
                case "LS":
                    return "8";
                case "RS X":
                    return "]";
                case "D-PAD Y":
                    return "A";
                case "D-PAD X":
                    return "S";
                case "A":
                    return ",";
                case "LT":
                    return "k";
                case "RT":
                    return "b";
                case "B":
                    return ".";
                case "Y":
                    return "/";
                case "X":
                    return "Q";
                case "LT/X":
                    return "kQ";
                case "Press RT":
                    return "b";
                case "PRESS RIGHT STICK PRESS":
                    return "\\";
                case "Select":
                    return "B";
                case "LB | RB":
                case "XINPUTCONTROLLERWINDOWS|LEFTSHOULDER|XINPUTCONTROLLERWINDOWS|RIGHTSHOULDER":
                case "LB/RB":
                    return "fz";
                case "XINPUTCONTROLLERWINDOWS|LEFTTRIGGER|XINPUTCONTROLLERWINDOWS|BUTTONWEST":
                    return "kQ";
            }
        //}
        
        return " ";
    }

}
