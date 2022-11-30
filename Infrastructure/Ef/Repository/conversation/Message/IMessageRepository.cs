using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.conversation.Message;

public interface IMessageRepository
{
    DbMessage Create(DbMessage dbMessage);
    IEnumerable<DbMessage> FetchByConversationid(int id);
}