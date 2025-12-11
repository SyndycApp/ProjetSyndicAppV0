using CommunityToolkit.Mvvm.ComponentModel;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Communication
{
    public partial class ConversationItemViewModel : ObservableObject
    {
        public ConversationDto Conversation { get; }

        [ObservableProperty]
        private string displayName;

        public string DisplayInitial
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DisplayName))
                    return "?";

                // retire espaces, tabulations, caractères invisibles
                var clean = DisplayName.Trim();

                if (clean.Length == 0)
                    return "?";

                return clean[0].ToString().ToUpper();
            }
        }

        public ConversationItemViewModel(ConversationDto dto, string name)
        {
            Conversation = dto;
            DisplayName = name;
        }

        public MessageDto DernierMessage => Conversation.DernierMessage;
        public Guid Id => Conversation.Id;
    }
}
