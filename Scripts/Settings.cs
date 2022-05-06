//SETTINGS SCRIPT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum Difficulties
    {
        DEBUG,
        EASY,
        MEDIUM,
        HARD
    }

    public static Difficulties difficulty;
}
