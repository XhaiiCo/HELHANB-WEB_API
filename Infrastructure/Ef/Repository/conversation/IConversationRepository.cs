using System.Data.Common;
using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.conversation;

public interface IConversationRepository
{
    DbConversation Create(DbConversation dbConversation);
    DbConversation FetchByUsersIds(int user1, int user2);
    DbConversation FetchById(int id);
}