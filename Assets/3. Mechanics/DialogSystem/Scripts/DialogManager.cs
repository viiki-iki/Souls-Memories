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
            //evento enviando o DialogTreeEnum.TreeOptions id
            //exemplo de uso: mask para desligar clicks no cenario
            GameObject dialogTree = Instantiate(dialogTreePrefab, prefabParent);
            DialogTreeTemplate script = dialogTree.GetComponent<DialogTreeTemplate>();
            script.DisplayQuestion(currentTree.questions[0]);
            //manda evento com a question? para afins como npc ou mudança de cena? ou animação
            var nn = currentTree.id;
            print(nn);
        }

        public void OnOptionSelected(int index)
        {
            //if (index == currentQuestion.GetCorrectOptionIndex()) { questionText.text = "hahahaha"; }

            //dentro do option colocar o resultado = ou fecha o dialogbox ou vai pra proxima question
        }

        void GetNextQuestion()
        {
            //DisplayQuestion();
            //futuro
        }
    }
}