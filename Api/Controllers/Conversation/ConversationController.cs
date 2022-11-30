using Application.UseCases.Conversation;
using Application.UseCases.Conversation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Conversation;

[ApiController]
[Route("api/v1/conversation")]
public class ConversationController : ControllerBase
{
    private readonly UseCaseCreateConversation _useCaseCreateConversation;
    private readonly UseCaseCreateMessage _useCaseCreateMessage;
    private readonly UseCaseFetchMyConversation _useCaseFetchMyConversation ;

    public ConversationController(UseCaseCreateConversation useCaseCreateConversation,
        UseCaseCreateMessage useCaseCreateMessage, UseCaseFetchMyConversation useCaseFetchMyConversation)
    {
        _useCaseCreateConversation = useCaseCreateConversation;
        _useCaseCreateMessage = useCaseCreateMessage;
        _useCaseFetchMyConversation = useCaseFetchMyConversation;
    }

    private bool IsTheIdOfConnectedUser(int id)
    {
        return "" + id == User.Identity?.Name;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputCreatedConversation> Create(DtoInputCreateConversation dto)
    {
        if (!IsTheIdOfConnectedUser(dto.IdUser1) && !IsTheIdOfConnectedUser(dto.IdUser2)) return Unauthorized();

        return Ok(_useCaseCreateConversation.Execute(dto));
    }

    [HttpPost("messages")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputCreatedMessage> CreateMessage(DtoInputCreateMessage dto)
    {
        if (!IsTheIdOfConnectedUser(dto.SenderId)) return Unauthorized();
        
        try
        {
            return Ok(_useCaseCreateMessage.Execute(dto));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet("{id:int}/myConversations")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputMyConversation>> FetchMyConversations(int id)
    {
        if (!IsTheIdOfConnectedUser(id)) return Unauthorized() ;
        
        return Ok(_useCaseFetchMyConversation.Execute(id));
    }
}