using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class GoContainerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _gos;
        [SerializeField] private DropEvent _onDrop;

        [ContextMenu("DropStart")]
        public void Drop()
        {
            _onDrop?.Invoke(_gos);
        }
    }
}


