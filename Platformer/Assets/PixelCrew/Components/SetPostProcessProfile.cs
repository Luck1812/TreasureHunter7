using UnityEngine;
using UnityEngine.Rendering;

namespace PixelCrew.Components
{
    public class SetPostProcessProfile : MonoBehaviour
    {
        [SerializeField] private VolumeProfile _profile;

        public void SetProfile()
        {
            var volumes = FindObjectsOfType<Volume>();
            foreach (var volume in volumes)
            {
                if(!volume.isGlobal) continue;

                volume.profile = _profile;
                break;
            }
        }
    }
}