// Common/Messages/ResidenceChangedMessage.cs
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SyndicApp.Mobile.Common.Messages;
public sealed class ResidenceChangedMessage : ValueChangedMessage<bool>
{
    public ResidenceChangedMessage(bool _) : base(_) { }
}
