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

                var parts = DisplayName
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    return "?";

                if (parts.Length == 1)
                    return parts[0][0].ToString().ToUpper();

                return parts[0][0].ToString().ToUpper() +
                       parts[^1][0].ToString().ToUpper();
            }
        }

        public ConversationItemViewModel(ConversationDto dto, string name)
        {
            Conversation = dto;
            DisplayName = name;
        }

        public MessageDto DernierMessage => Conversation.DernierMessage;

        public bool EstVu => Conversation.DernierMessage?.IsRead ?? false;

        public Guid Id => Conversation.Id;
    }

}
