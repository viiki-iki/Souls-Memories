using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogSystem
{
    public class DialogTreePrefab : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] GameObject[] optionButton;

        public void DisplayQuestion(Question currentQuestion)
        {
            questionText.text = currentQuestion.GetQuestion();

            for (int i = 0; i < optionButton.Length; i++)
            {
                TextMeshProUGUI buttonText = optionButton[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = currentQuestion.GetOption(i);
            }
        }
    }
}