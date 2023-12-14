
using UnityEngine;

namespace UI
{
    public class LabelButtonViewModel : AnimatedButtonViewModel
    {
        [SerializeField] private LabelViewModel label;

        public void Init(string text)
        {
            label.Init(text);
        }
    }
}