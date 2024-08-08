using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTreeEnum
{
    public enum TreeOptions
    {
        Intro = 1 << 0,
        Mission1 = 1 << 1,
        Mission2 = 1 << 2,
    }
}