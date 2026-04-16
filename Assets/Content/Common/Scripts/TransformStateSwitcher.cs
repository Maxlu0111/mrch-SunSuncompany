using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MRCH.Tools.Objects
{
    public class TransformStateSwitcher : MonoBehaviour
    {
        [SerializeField, InfoBox("Keep it empty will set the target to itself.")]
        private GameObject target;

        [TitleGroup("Location Switcher"), SerializeField]
        private bool useLocSwitcher;

        [TitleGroup("Location Switcher"), SerializeField, ShowIf(nameof(useLocSwitcher)), ListDrawerSettings(ShowIndexLabels = true)]
        private List<Vector3> locations;

        [TitleGroup("Location Switcher"), SerializeField, ShowIf(nameof(useLocSwitcher)), Unit(Units.Second)]
        private float defaultMoveDuration = 1f;

        [TitleGroup("Location Switcher"), SerializeField, ShowIf(nameof(useLocSwitcher))]
        private bool snapping;

        [TitleGroup("Location Switcher"), ShowInInspector, ShowIf(nameof(useLocSwitcher)), ReadOnly]
        private int _locationIndex;

        private Tween _locationTween;

        [TitleGroup("Rotation Switcher"), SerializeField]
        private bool useRotSwitcher;

        [TitleGroup("Rotation Switcher"), SerializeField, ShowIf(nameof(useRotSwitcher)),ListDrawerSettings(ShowIndexLabels = true)]
        private List<Vector3> rotations;

        [TitleGroup("Rotation Switcher"), SerializeField, ShowIf(nameof(useRotSwitcher)), Unit(Units.Second)]
        private float defaultRotationDuration = 1f;

        [TitleGroup("Rotation Switcher"), SerializeField, ShowIf(nameof(useRotSwitcher))]
        private RotateMode rotateMode = RotateMode.Fast;

        [TitleGroup("Rotation Switcher"), ShowInInspector, ShowIf(nameof(useRotSwitcher)), ReadOnly]
        private int _rotationIndex;

        private Tween _rotationTween;

        [TitleGroup("Scale Switcher"), SerializeField]
        private bool useScaleSwitcher;

        [TitleGroup("Scale Switcher"), SerializeField, ShowIf(nameof(useScaleSwitcher)), ListDrawerSettings(ShowIndexLabels = true)]
        private List<Vector3> scales;

        [TitleGroup("Scale Switcher"), SerializeField, ShowIf(nameof(useScaleSwitcher)), Unit(Units.Second)]
        private float defaultScaleDuration = 1f;

        [TitleGroup("Scale Switcher"), ShowInInspector, ShowIf(nameof(useScaleSwitcher)), ReadOnly]
        private int _scaleIndex;

        private Tween _scaleTween;

        // C# events — code subscribers. Args: (finalIndex, finalValue)
        public event Action<int, Vector3> OnLocationSwitched;
        public event Action<int, Vector3> OnRotationSwitched;
        public event Action<int, Vector3> OnScaleSwitched;

        // Public getters so subscribers can query current state
        public int CurrentLocationIndex => _locationIndex;
        public int CurrentRotationIndex => _rotationIndex;
        public int CurrentScaleIndex => _scaleIndex;

        public Vector3 CurrentLocation => locations is { Count: > 0 } ? locations[_locationIndex] : Vector3.zero;
        public Vector3 CurrentRotation => rotations is { Count: > 0 } ? rotations[_rotationIndex] : Vector3.zero;
        public Vector3 CurrentScale => scales is { Count: > 0 } ? scales[_scaleIndex] : Vector3.one;

        private void Awake()
        {
            if (!target)
                target = gameObject;
        }

        [Button, DisableInEditorMode]
        public void SwitchToNextLocation()
        {
            if (!useLocSwitcher || locations == null || locations.Count == 0) return;

            _locationIndex = (_locationIndex + 1) % locations.Count;
            var targetValue = locations[_locationIndex];
            var idx = _locationIndex;

            _locationTween?.Kill();
            _locationTween = target.transform
                .DOLocalMove(targetValue, defaultMoveDuration, snapping)
                .OnComplete(() => OnLocationSwitched?.Invoke(idx, targetValue));
        }

        [Button, DisableInEditorMode]
        public void SwitchToNextRotation()
        {
            if (!useRotSwitcher || rotations == null || rotations.Count == 0) return;

            _rotationIndex = (_rotationIndex + 1) % rotations.Count;
            var targetValue = rotations[_rotationIndex];
            var idx = _rotationIndex;

            _rotationTween?.Kill();
            _rotationTween = target.transform
                .DOLocalRotate(targetValue, defaultRotationDuration, rotateMode)
                .OnComplete(() => OnRotationSwitched?.Invoke(idx, targetValue));
        }

        [Button, DisableInEditorMode]
        public void SwitchToNextScale()
        {
            if (!useScaleSwitcher || scales == null || scales.Count == 0) return;

            _scaleIndex = (_scaleIndex + 1) % scales.Count;
            var targetValue = scales[_scaleIndex];
            var idx = _scaleIndex;

            _scaleTween?.Kill();
            _scaleTween = target.transform
                .DOScale(targetValue, defaultScaleDuration)
                .OnComplete(() => OnScaleSwitched?.Invoke(idx, targetValue));
        }

        public void ChangeTarget(GameObject pTarget)
        {
            target = pTarget ? pTarget : gameObject;
        }

        private void OnDestroy()
        {
            _locationTween?.Kill();
            _rotationTween?.Kill();
            _scaleTween?.Kill();
        }
    }
}