using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "SO/DialogTree", fileName = "DialogTree", order = 1)]
    public class DialogTree : ScriptableObject
    {
        public DialogTreeEnum.TreeOptions id;
        public Question[] questions;
    }
}

