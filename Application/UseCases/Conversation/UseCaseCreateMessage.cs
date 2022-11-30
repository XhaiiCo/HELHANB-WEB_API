using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.conversation;
using Infrastructure.Ef.Repository.conversation.Message;

namespace Application.UseCases.Conversation;

public class UseCaseCreateMessage : IUseCaseWriter<DtoOutputCreatedMessage, DtoInputCreateMessage>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;


    public UseCaseCreateMessage(IMessageRepository messageRepository, IConversationRepository conversationRepository)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
    }

    public DtoOutputCreatedMessage Execute(DtoInputCreateMessage input)
    {
        var dbConversation = _conversationRepository.FetchById(input.conversationId);

        //Check if the user belong to the conversation
        if (input.SenderId != dbConversation.IdUser1 && input.SenderId != dbConversation.IdUser2)
            throw new Exception("Vous ne faites pas partie de la conversation");


        var dbMessage = Mapper.GetInstance().Map<DbMessage>(input);
        dbMessage.View = false;
        dbMessage.SendTime = DateTime.Now;

        var result = Mapper.GetInstance().Map<DtoOutputCreatedMessage>(_messageRepository.Create(dbMessage));

        return result;
    }
}