using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount price)
        {
            var def = DefsFacade.Instance.Items.Get(price.Id);

            _icon.sprite = def.Icon;
            _value.text = price.Count.ToString();
        }
    }
}