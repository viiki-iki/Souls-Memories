using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] DialogTree currentTree;
        [SerializeField] GameObject dialogTreePrefab;
        [SerializeField] Transform prefabParent;

        private void Start()
        {
            StartDialogTree();
        }

        public void StartDialogTree()
        {
            GameObject dialogTree = Instantiate(dialogTreePrefab, prefabParent);
            DialogTreePrefab script = dialogTree.GetComponent<DialogTreePrefab>();
            script.DisplayQuestion(currentTree.questions[0]);
        }

        public void OnOptionSelected(int index)
        {
            //if (index == currentQuestion.GetCorrectOptionIndex()) { questionText.text = "hahahaha"; }
        }

        void GetNextQuestion()
        {
            //DisplayQuestion();
            //futuro
        }
    }
}