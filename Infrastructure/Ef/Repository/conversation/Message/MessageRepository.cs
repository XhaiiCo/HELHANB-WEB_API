using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.conversation.Message;

public class MessageRepository: IMessageRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public MessageRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbMessage Create(DbMessage dbMessage)
    {
        using var context = _contextProvider.NewContext();

        context.Messages.Add(dbMessage);
        context.SaveChanges();

        return dbMessage;
    }
}