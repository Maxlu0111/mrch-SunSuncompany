using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MRCH.Tools.Objects
{
    public class TransformStateMatchListener : MonoBehaviour
    {
        [SerializeField, Required] private TransformStateSwitcher switcher;

        [TitleGroup("Match Location")] [SerializeField]
        private bool matchLocation;

        [TitleGroup("Match Location"), SerializeField, ShowIf(nameof(matchLocation))]
        private int targetLocationIndex;
        
        [TitleGroup("Match Rotation")][SerializeField] 
        private bool matchRotation;

        [TitleGroup("Match Rotation")][SerializeField, ShowIf(nameof(matchRotation))]
        private int targetRotationIndex;

        [TitleGroup("Match Scale")][SerializeField] 
        private bool matchScale;

        [TitleGroup("Match Scale")][SerializeField, ShowIf(nameof(matchScale))]
        private int targetScaleIndex;

        [TitleGroup("Events")] [SerializeField]
        private UnityEvent onMatched;

        [TitleGroup("Events")] [SerializeField] 
        private UnityEvent onUnmatched;

        private bool _wasMatched = false;

        private void OnEnable()
        {
            if (!switcher) return;
            switcher.OnLocationSwitched += HandleSwitched;
            switcher.OnRotationSwitched += HandleSwitched;
            switcher.OnScaleSwitched += HandleSwitched;
        }

        private void OnDisable()
        {
            if (!switcher) return;
            switcher.OnLocationSwitched -= HandleSwitched;
            switcher.OnRotationSwitched -= HandleSwitched;
            switcher.OnScaleSwitched -= HandleSwitched;
        }

        private void HandleSwitched(int index, Vector3 value)
        {
            var isMatched = IsFullyMatched();

            switch (isMatched)
            {
                // Only fire events on state transitions, not every tween completion
                case true when !_wasMatched:
                    onMatched?.Invoke();
                    break;
                case false when _wasMatched:
                    onUnmatched?.Invoke();
                    break;
            }

            _wasMatched = isMatched;
        }

        private bool IsFullyMatched()
        {
            if (matchLocation && switcher.CurrentLocationIndex != targetLocationIndex) return false;
            if (matchRotation && switcher.CurrentRotationIndex != targetRotationIndex) return false;
            if (matchScale && switcher.CurrentScaleIndex != targetScaleIndex) return false;

            // If nothing is being matched, treat as "not matched" to avoid false positives
            return matchLocation || matchRotation || matchScale;
        }
    }
}