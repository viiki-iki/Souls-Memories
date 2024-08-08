using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogSystem;

[CreateAssetMenu(menuName = "SO/DialogTree", fileName = "DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    public DialogTreeEnum.TreeOptions id;
    public Question[] questions;
}

