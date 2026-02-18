using JM.Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace JM.Notifications
{
    public class GameMessagePresenter : MonoBehaviour, IMessagePresenter
    {
        [SerializeField] private TMP_Text _text;

        private ILocalizationService _localizationService;

        [Inject]
        public void Construct(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Show(string messageID)
        {
            _text.text =  _localizationService.Get(messageID);
        }
    }
}
