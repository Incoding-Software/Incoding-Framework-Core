namespace Incoding.Core.CQRS.Core
{
    public interface ISettingCommandComposite
    {        
        ISettingCommandComposite Quote(IMessage message, MessageExecuteSetting executeSetting = null);
    }
}