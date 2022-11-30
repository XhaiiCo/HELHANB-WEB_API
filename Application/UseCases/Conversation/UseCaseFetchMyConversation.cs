using Application.Services.User;
using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef.Repository.conversation;

namespace Application.UseCases.Conversation;

public class UseCaseFetchMyConversation : IUseCaseParameterizedQuery<IEnumerable<DtoOutputMyConversation>, int>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserService _userService;


    public UseCaseFetchMyConversation(IConversationRepository conversationRepository, IUserService userService)
    {
        _conversationRepository = conversationRepository;
        _userService = userService;
    }

    public IEnumerable<DtoOutputMyConversation> Execute(int id)
    {
        var dbConversations = _conversationRepository.FetchByUserId(id);
        var mapper = Mapper.GetInstance();

        return dbConversations.Select(conv => new DtoOutputMyConversation
        {
            Id = conv.Id,
            Recipient = conv.IdUser1 == id
                ? mapper.Map<DtoOutputMyConversation.DtoOutputUserInMyConversation>(
                    _userService.FetchById(conv.IdUser2))
                : mapper.Map<DtoOutputMyConversation.DtoOutputUserInMyConversation>(
                    _userService.FetchById(conv.IdUser1))
        });
    }
}