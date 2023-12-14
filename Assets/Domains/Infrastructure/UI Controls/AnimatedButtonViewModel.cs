using Logging;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class AnimatedButtonViewModel : CommonAnimatedViewModel, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private bool isEnabled = true;

        [SerializeField] private float squishWidthRatio = 1.1f;
        [SerializeField] private float squishheightRatio = 0.9f;

        public event Action<AnimatedButtonViewModel> OnClick;

        private AnimationClip squish;
        private AnimationClip unsquish;

        private void Awake()
        {
            button.onClick.AddListener(NotifyButtonClick);


            var x1 = animatedTransform.sizeDelta.x;
            var x2 = animatedTransform.sizeDelta.x * squishWidthRatio;
            var y1 = animatedTransform.sizeDelta.y;
            var y2 = animatedTransform.sizeDelta.y * squishheightRatio;

            squish = CreateSquishAnimationByComponentSize(x1, x2, y1, y2);
            unsquish = CreateSquishAnimationByComponentSize(x2, x1, y2, y1);
        }

        private void OnDestroy()
        {
            OnClick = null;
            button.onClick.RemoveAllListeners();
        }

        private void NotifyButtonClick()
        {
            OnClick?.Invoke(this);
        }

        public void SetButtonInteractable(bool isEnabled)
        {
            button.interactable = isEnabled;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventSystem == null || animationService == null)
            {
                CuteLogger.LogError($"Inject correct bindings for {nameof(AnimatedButtonViewModel)} on {gameObject.name} gameobject");
                return;
            }

            if (!isEnabled)
                return;

            eventSystem.RequestSwipeLock();
            animationService.PlayClip(this, squish);
        }

        public async void OnPointerUp(PointerEventData eventData)
        {
            if (!isEnabled)
                return;
         
            await ResetButton();
            OnClick?.Invoke(this);
        }

        private async UniTask ResetButton()
        {
            await animationService.PlayClip(this, unsquish);
            eventSystem.RequestSwipeRelease();
        }

        private AnimationClip CreateSquishAnimationByComponentSize(float x1, float x2, float y1, float y2)
        {
            AnimationClip clip = new AnimationClip();
            clip.legacy = true;

            var keys = new Keyframe[2];
            keys[0] = new Keyframe(0.0f, x1);
            keys[1] = new Keyframe(0.2f, x2);
            var curve = new AnimationCurve(keys);
            clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.x", curve);

            var keys2 = new Keyframe[2];
            keys2[0] = new Keyframe(0.0f, y1);
            keys2[1] = new Keyframe(0.2f, y2);
            var curve2 = new AnimationCurve(keys2);
            clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve2);
            
            return clip;
        }
    }
}