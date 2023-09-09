using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamepadUI
{
    public static string GetGamepadIcon(string button)
    {
        switch(button)
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
            case "B":
                return ".";
            case "Y":
                return "/";
            case "Press RT":
                return "b";
            case "PRESS RIGHT STICK PRESS":
                return "\\";
            case "Select":
                return "B";
            case "LB | RB":
                return "fz";
        }
        return " ";
    }

}
