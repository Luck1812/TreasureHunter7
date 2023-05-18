using System;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories.Appearance
{
    [CreateAssetMenu(fileName = "Appearance", menuName = "Defs/Appearance")]
    public class AppearanceRepository : DefRepository<AppearanceDef>
    {
        
    }

    [Serializable]
    public struct AppearanceDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Creature _creature;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemWithCount _price;
        [SerializeField] private AppointmentOfAnimator[] _animatorAssignments;
        public string Id => _id;

        public Creature Creature => _creature;

        public AppointmentOfAnimator[] AnimatorAssignments => _animatorAssignments;

        public Sprite Icon => _icon;

        public ItemWithCount Price => _price;

    }

    [Serializable]
    public struct AppointmentOfAnimator
    {
        [SerializeField] private string _id;
        [SerializeField] private RuntimeAnimatorController _animator;
        [SerializeField] private AnimatorOverrideController _overrideAnimator;

        public string Id => _id;

        public RuntimeAnimatorController Animator => _animator;

        public AnimatorOverrideController OverrideAnimator => _overrideAnimator;
    }

    public enum Creature
    {
        Hero
    }
    
}