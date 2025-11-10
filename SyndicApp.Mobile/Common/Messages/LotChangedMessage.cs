using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SyndicApp.Mobile.Common.Messages
{
    public class LotChangedMessage : ValueChangedMessage<bool>
    {
        public LotChangedMessage(bool value) : base(value) { }
    }
}
