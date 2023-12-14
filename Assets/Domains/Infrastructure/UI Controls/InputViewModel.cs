
using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InputViewModel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;


        public event Action<InputViewModel> OnInputFieldEdited;

        public string Text => inputField.text;

        private void Awake()
        {
            inputField.onEndEdit.AddListener(NotifyInputFieldEdited);
        }
        private void Destroy()
        {
            inputField.onEndEdit.RemoveAllListeners();
        }

        private void NotifyInputFieldEdited(string text)
        {
            OnInputFieldEdited?.Invoke(this);
        }
    }
}
