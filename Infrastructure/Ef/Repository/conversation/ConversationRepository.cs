using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.conversation;

public class ConversationRepository : IConversationRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public ConversationRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbConversation Create(DbConversation dbConversation)
    {
        using var context = _contextProvider.NewContext();

        context.Conversations.Add(dbConversation);
        context.SaveChanges();

        return dbConversation;
    }

    public DbConversation FetchByUsersIds(int user1, int user2)
    {
        using var context = _contextProvider.NewContext();

        var dbConversation = context.Conversations.FirstOrDefault(conv =>
            (conv.IdUser1 == user1 && conv.IdUser2 == user2) ||
            (conv.IdUser1 == user2 && conv.IdUser2 == user1));
        
        if (dbConversation == null) throw new KeyNotFoundException($"La conversation n'a pas été trouvée");

        return dbConversation;
    }

    public DbConversation FetchById(int id)
    {
        using var context = _contextProvider.NewContext();

        var dbConversation = context.Conversations.FirstOrDefault(conv => conv.Id == id);
        
        if (dbConversation == null) throw new KeyNotFoundException($"La conversation n'a pas été trouvée");

        return dbConversation;
    }
}