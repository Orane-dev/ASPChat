using ASPChat.BuisnessLayer.Chat.Implementation;
using ASPChat.BuisnessLayer.Chat.Interfaces;
using ASPChat.DAL.Interfaces;
using ASPChat.Middleware;
using ASPChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPChat.Controllers
{
    [ChatAuthorize]
    public class ChatController : Controller
    {
        private IHttpContextAccessor _contextAccessor;
        private IAuthenticationDAL _authentication;
        private IChatManager _chatManager { get; set; }
        public ChatController(IAuthenticationDAL authenticationDAL, IHttpContextAccessor httpContextAccessor, IChatManager chatManager)
        {
            _contextAccessor = httpContextAccessor;
            _authentication = authenticationDAL;
            _chatManager = chatManager;
        }

        [HttpGet]
        [Route("/Chat")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                var chatList = await _chatManager.GetChatList(userId.Value);
                ViewBag.CurrentUserId = userId;
                return View(chatList);
            }
            return View();
        }

        [HttpGet]
        [Route("/Chat/GetMessages")]
        public async Task<ActionResult> ChatMessages(int chatId)
        {
            var messageList = await _chatManager.GetChatMessages(chatId);

            return Json(messageList);
        }

        [HttpPost]
        [Route("/Chat/CreateChat")]
        public async Task<ActionResult<int>> CreateChat([FromBody] CreateChatRequest chatRequest)
        {
            if (chatRequest == null || chatRequest.ChatCompanionId < 0)
            {
                return BadRequest("Invalid Data");
            }
            try
            {
                var userId = _contextAccessor?.HttpContext?.Session.GetInt32("userId");
                if (userId != null)
                {
                    var chatId = await _chatManager.CreateChat(
                        userId.Value,
                        chatRequest.ChatCompanionId);

                    if (chatId != -1)
                        return Ok(chatId);
                    throw new Exception("Error while create chat");
                }              
                throw new Exception("Internal server error");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }   
        }

        [HttpGet]
        public async Task<IActionResult> SearchChatUser(string searchItem)
        {
            var searchResult = await _chatManager.SearchForChatUser(searchItem);
            if (searchResult != null && searchResult.Any())
            {
                return Json(searchResult);
            }
            return NoContent();
        }
    }
}
