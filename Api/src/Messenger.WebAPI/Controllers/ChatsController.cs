using MediatR;
using Messenger.Application.Features.Chats.Queries.GetById;
using Messenger.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ISender _sender;

        public ChatsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetChatById(
            [FromRoute] Guid chatId)
        {
            var queryResult = await _sender.Send(new GetChatByIdQuery(chatId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }
    }
}
