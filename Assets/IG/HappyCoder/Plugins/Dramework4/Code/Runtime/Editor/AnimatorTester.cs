#if UNITY_EDITOR

using System;
using System.Collections.Generic;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Getting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Editor.Tools.Animations
{
    public class AnimatorTester : DBehaviour
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 100;

        [FoldoutGroup("Components")] [BoxGroup("Components/Animator", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Animator:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private Animator _animator;

        [SerializeField] [PropertyOrder(3)]
        private List<ClipItem> _clips;

        #endregion

        #region ================================ METHODS

        [Button(ButtonSizes.Medium)] [PropertyOrder(1)]
        private void Initialize()
        {
            _clips.Clear();

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
                _clips.Add(new ClipItem(clip, _animator));
        }

        [Button(ButtonSizes.Medium)] [PropertyOrder(2)]
        private void Stop()
        {
            _animator.enabled = false;
        }

        #endregion
    }

    [Serializable] [HideLabel]
    internal class ClipItem
    {
        #region ================================ FIELDS

        private static readonly int IsLooping = Animator.StringToHash("isLooping");

        [HorizontalGroup(marginRight: 4)]
        [LabelWidth(30)] [LabelText("Clip:")]
        [SerializeField] [ReadOnly]
        private AnimationClip Clip;

        [HorizontalGroup(marginRight: 4, width: 50)]
        [LabelWidth(36)] [LabelText("Loop:")]
        [SerializeField]
        private bool Loop;

        [SerializeField] [HideInInspector]
        private Animator _animator;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal ClipItem(AnimationClip clip, Animator animator)
        {
            Clip = clip;
            _animator = animator;
            SetLoop();
        }

        #endregion

        #region ================================ METHODS

        [HorizontalGroup(40)]
        [Button(ButtonSizes.Medium)]
        private void Play()
        {
            _animator.enabled = true;
            SetLoop();
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play(Clip.name);
        }

        private void SetLoop()
        {
            var serializedClip = new SerializedObject(Clip);
            var loopTime = serializedClip.FindProperty("m_AnimationClipSettings.m_LoopTime");
            loopTime.boolValue = Loop;
            serializedClip.ApplyModifiedProperties();
        }

        #endregion
    }
}

#endif