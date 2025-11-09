using CommunityToolkit.Mvvm.Messaging.Messages;
namespace SyndicApp.Mobile.Common.Messages;
public sealed class BatimentChangedMessage : ValueChangedMessage<bool>
{
    public BatimentChangedMessage(bool v) : base(v) { }
}
