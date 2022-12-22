using Application.UseCases.Conversation;
using Application.UseCases.Conversation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Int32;

namespace API.Controllers.Conversation;

[ApiController]
[Route("api/v1/conversation")]
public class ConversationController : ControllerBase
{
    private readonly UseCaseCreateConversation _useCaseCreateConversation;
    private readonly UseCaseCreateMessage _useCaseCreateMessage;
    private readonly UseCaseFetchMyConversation _useCaseFetchMyConversation;
    private readonly UseCaseFetchMessageForAConversation _useCaseFetchMessageForAConversation;
    private readonly UseCasePutMessageViewToTrue _useCasePutMessageViewToTrue;

    public ConversationController(
        UseCaseCreateConversation useCaseCreateConversation,
        UseCaseCreateMessage useCaseCreateMessage,
        UseCaseFetchMyConversation useCaseFetchMyConversation,
        UseCaseFetchMessageForAConversation useCaseFetchMessageForAConversation,
        UseCasePutMessageViewToTrue useCasePutMessageViewToTrue
    )
    {
        _useCaseCreateConversation = useCaseCreateConversation;
        _useCaseCreateMessage = useCaseCreateMessage;
        _useCaseFetchMyConversation = useCaseFetchMyConversation;
        _useCaseFetchMessageForAConversation = useCaseFetchMessageForAConversation;
        _useCasePutMessageViewToTrue = useCasePutMessageViewToTrue;
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
        if (!IsTheIdOfConnectedUser(dto.IdUser1) && !IsTheIdOfConnectedUser(dto.IdUser2))
            return Unauthorized();

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

    [HttpGet("myConversations")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputMyConversation>> FetchMyConversations()
    {
        if (User.Identity?.Name == null) return Unauthorized();
        var userId = Parse(User.Identity?.Name);

        return Ok(_useCaseFetchMyConversation.Execute(userId));
    }

    [HttpGet("{convId:int}/messages")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<DtoOutputMessage>> FetchMessagesForAConversation(int convId)
    {
        try
        {
            var dto = new DtoInputFetchMessagesForAConversation
            {
                conversationId = convId,
                UserId = Parse(User.Identity?.Name)
            };

            var result = _useCaseFetchMessageForAConversation.Execute(dto);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPut("{convId:int}/view")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult PutMessageViewToTrue(int convId)
    {
        try
        {
            var dto = new DtoInputPutMessageViewToTrue
            {
                conversationId = convId,
                UserId = Parse(User.Identity?.Name)
            };
            _useCasePutMessageViewToTrue.Execute(dto);

            return Ok();
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
}