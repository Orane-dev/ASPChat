import { GetAllUserViewMessage, LoadChatMessages, CreateChatWithSearchResult } from './ChatFunc.js'
import { SendMessage, isConnected } from './ChatSignalR.js'

document.querySelectorAll('.list-group-item').forEach(item => {
    item.addEventListener('click', async event => {
        event.preventDefault();
        try {
            const chatTitle = document.getElementById('chat-title');
            chatTitle.textContent = 'Chat with:' + item.textContent;

            const chatContent = document.getElementById('chat-content');
            chatContent.dataset.currentChatId = item.dataset.chatid;

            await LoadChatMessages(item.dataset.chatid);

            if (item.querySelector('span.badge') != null) {
                GetAllUserViewMessage();
            }

            if (isConnected) {
                document.getElementById('send-button').disabled = false;
            }

        } catch (error) {
            console.error(error.toString());
        }
    });
});

document.getElementById("send-button").addEventListener('click', () => {
    let chatId = document.getElementById("chat-content").dataset.currentChatId;
    const messageBox = document.getElementById('message-box');
    let message = messageBox.value;
    messageBox.value = '';
    const chatMessage = {
        text: message,
        userId: null,
        chatId: parseInt(chatId),
        sendTime: new Date().toISOString()
    }
    SendMessage(chatMessage);
});

document.getElementById("chat-content").addEventListener("scroll", function () {
    const chatArea = document.getElementById("chat-content");
    const messages = document.getElementsByClassName("chat-message");
    const chatDate = document.getElementById("chat-date");

    let currentMessageDate = '';

    Array.from(messages).forEach(message => {
        const messageRect = message.getBoundingClientRect();
        const chatAreaRect = chatArea.getBoundingClientRect();

        if (messageRect.top >= chatAreaRect.top && messageRect.bottom <= chatAreaRect.bottom) {
            const messageDate = message.dataset.date;

            if (messageDate !== currentMessageDate) {
                currentMessageDate = messageDate;
                chatDate.textContent = currentMessageDate;
            }
        }
    });
});

document.getElementById('search-input').addEventListener('input', function () {
    const resultList = document.getElementById('search-list');
    const searchItem = this.value;

    if (searchItem.length == 0 && resultList.childElementCount > 0) {
        resultList.innerHTML = '';
        return;
    }

    fetch(`/Chat/SearchChatUser?searchItem=${encodeURIComponent(searchItem)}`)
        .then(response => {
            if (response.status == 204) {
                throw Error('Not Found');
            } else {
                return response.json()
            }
        })
        .then(data => {
            resultList.innerHTML = '';

            data.forEach(item => {
                const li = document.createElement('li');
                li.textContent = item.email;
                li.dataset.userSearchId = item.userId;
                li.dataset.userSearchEmail = item.email;
                li.onclick = function () {
                    CreateChatWithSearchResult(this);
                };
                resultList.appendChild(li);
            })
        })
        .catch(error => {
            resultList.innerHTML = `<li>${error}</li>`;
        });

});