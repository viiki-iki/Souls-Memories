using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "SO/QuestionData", fileName = "QuestionData", order = 1)]
    public class Question : ScriptableObject
    {
        [TextArea(2, 5)]
        [SerializeField] string dialogLine = "oi bla bla bla";
        [SerializeField] string[] options = new string[4];
        // [SerializeField] int correctOption;

        public string GetQuestion() { return dialogLine; }
        //  public int GetCorrectOptionIndex() {return correctOption;}
        public string GetOption(int index) { return options[index]; }
    }
}