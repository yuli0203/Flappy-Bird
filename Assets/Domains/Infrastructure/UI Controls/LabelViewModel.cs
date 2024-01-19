
using TMPro;
using UnityEngine;

namespace UI
{
    public class LabelViewModel : MonoBehaviour
    {
        [SerializeField] TMP_Text label;

        public void Init(string labelDesciption)
        {
            if (this.label.text.Equals(labelDesciption))
                return;

            this.label.text = labelDesciption;
        }
    }
}
