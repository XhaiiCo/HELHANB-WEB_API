using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.conversation;

namespace Application.UseCases.Conversation;

public class UseCaseCreateConversation : IUseCaseWriter<DtoOutputCreatedConversation, DtoInputCreateConversation>
{
    private readonly IConversationRepository _conversationRepository;

    public UseCaseCreateConversation(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// If a conversation between two users already exists, return it, else create it and return it
    /// </summary>
    /// <returns>
    /// A DtoOutputCreatedConversation object
    /// </returns>
    public DtoOutputCreatedConversation Execute(DtoInputCreateConversation input)
    {
        var dbConservation = Mapper.GetInstance().Map<DbConversation>(input);

        DbConversation result;
        
        try
        {
            //If already exists
            result = _conversationRepository.FetchByUsersIds(input.IdUser1, input.IdUser2);
        }
        catch (KeyNotFoundException e)
        {
            //Else create it
            result = _conversationRepository.Create(dbConservation);
        }

        return Mapper.GetInstance().Map<DtoOutputCreatedConversation>(result);
    }
}