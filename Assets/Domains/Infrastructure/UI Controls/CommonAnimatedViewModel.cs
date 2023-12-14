
using Animations;
using Cysharp.Threading.Tasks;
using Logging;
using UnityEngine;
using VContainer;

namespace UI
{
    public class CommonAnimatedViewModel : MonoBehaviour, IAnimated
    {
        public AnimationClip entry;
        public AnimationClip exit;
        public CanvasGroup opacityCanvas;

        public Animation Animation { get; set; }
        public GameObject GameObject => animatedTransform?.gameObject;
        protected IAnimationService animationService;
        protected EventSystemViewModel eventSystem;

        [SerializeField] protected RectTransform animatedTransform;

        [Inject]
        public void Constrcut(IAnimationService animationService, EventSystemViewModel eventSystem)
        {
            this.animationService = animationService;
            this.eventSystem = eventSystem;
        }

        public async UniTask AnimatedEntryOrExit(bool isEntry = true, bool immediate = false)
        {
            if (immediate)
            {
                opacityCanvas.alpha = isEntry ? 1 : 0;
                return;
            }

            var clip = isEntry ? entry : exit;
            await animationService.PlayClip(this, clip);
        }

        private void OnValidate()
        {
            if (animatedTransform == null)
            {
                CuteLogger.LogError($"Unassigned animatedTransform for {gameObject.name}");
            }
        }
    }
}