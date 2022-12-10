using Application.Services.User;
using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef.Repository.conversation;
using Infrastructure.Ef.Repository.conversation.Message;

namespace Application.UseCases.Conversation;

public class UseCaseFetchMyConversation : IUseCaseParameterizedQuery<IEnumerable<DtoOutputMyConversation>, int>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserService _userService;


    public UseCaseFetchMyConversation(IConversationRepository conversationRepository, IUserService userService,
        IMessageRepository messageRepository)
    {
        _conversationRepository = conversationRepository;
        _userService = userService;
        _messageRepository = messageRepository;
    }

    public IEnumerable<DtoOutputMyConversation> Execute(int id)
    {
        var dbConversations = _conversationRepository.FetchByUserId(id);
        var mapper = Mapper.GetInstance();

        var result = dbConversations.Select(conv => new DtoOutputMyConversation
        {
            Id = conv.Id,
            MessageNotView = _messageRepository.FetchByConversationidNotView(conv.Id)
                ?.FirstOrDefault(item => item.SenderId != id) != null,
            Recipient = conv.IdUser1 == id
                ? mapper.Map<DtoOutputMyConversation.DtoOutputUserInMyConversation>(
                    _userService.FetchById(conv.IdUser2))
                : mapper.Map<DtoOutputMyConversation.DtoOutputUserInMyConversation>(
                    _userService.FetchById(conv.IdUser1))
        });

        return result;
    }
}