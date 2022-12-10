using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ef.Repository.conversation.Message;

public class MessageRepository : IMessageRepository
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

    public DbMessage FetchById(int id)
    {
        using var context = _contextProvider.NewContext();

        var result = context.Messages.FirstOrDefault(mess => mess.Id == id);

        if (result == null)
            throw new Exception($"Message avec l'id {id} n'a pas été trouvé");

        return result;
    }

    public IEnumerable<DbMessage> FetchByConversationid(int id)
    {
        using var context = _contextProvider.NewContext();

        return context.Messages.Where(mess => mess.ConversationId == id).ToList();
    }

    public IEnumerable<DbMessage> FetchByConversationidNotView(int id)
    {
        using var context = _contextProvider.NewContext();

        return context.Messages.Where(mess => mess.ConversationId == id && mess.View == false).ToList();
    }

    public DbMessage UpdateMessageViewToTrue(int id)
    {
        using var context = _contextProvider.NewContext();

        var message = FetchById(id);

        message.View = true;
        context.Attach(message);
        context.Entry(message).State = EntityState.Modified;
        context.SaveChanges();

        return message;
    }
}