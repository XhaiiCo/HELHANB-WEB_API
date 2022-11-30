using Application.UseCases.Conversation;
using Application.UseCases.Conversation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Conversation;

[ApiController]
[Route("api/v1/conversation")]
public class ConversationController: ControllerBase
{

    private readonly UseCaseCreateConversation _useCaseCreateConversation;

    public ConversationController(UseCaseCreateConversation useCaseCreateConversation)
    {
        _useCaseCreateConversation = useCaseCreateConversation;
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
}