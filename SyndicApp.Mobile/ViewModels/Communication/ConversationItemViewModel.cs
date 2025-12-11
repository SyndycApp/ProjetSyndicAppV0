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

                // Nettoyage
                var parts = DisplayName
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    return "?";

                // 1 mot → 1 lettre
                if (parts.Length == 1)
                    return parts[0][0].ToString().ToUpper();

                // Plusieurs mots → initiales du premier + dernier
                string first = parts[0][0].ToString().ToUpper();
                string last = parts[^1][0].ToString().ToUpper();

                return first + last;
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
