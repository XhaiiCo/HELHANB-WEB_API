using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.conversation.Message;

public interface IMessageRepository
{
    DbMessage Create(DbMessage dbMessage);
    DbMessage FetchById(int id);
    IEnumerable<DbMessage> FetchByConversationid(int id);
    IEnumerable<DbMessage> FetchByConversationidNotView(int id);
    DbMessage UpdateMessageViewToTrue(int id);
}