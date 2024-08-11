import { MarkMessagesAsRead } from './ChatSignalR.js';

export function GetAllUserViewMessage() {
    const chatContent = document.getElementById('chat-content');
    let messages = chatContent.querySelectorAll('.message-received');
    let lastReaded = null;
    messages.forEach(message => {
        lastReaded = message.dataset.messageId;
    });

    MarkMessagesAsRead(lastReaded, chatContent.dataset.currentChatId)
}
export function LoadChatMessages(chatId) {
    const userId = GetUserId();
    return fetch(`/Chat/GetMessages?chatId=${chatId}`)
        .then(response => response.json())
        .then(messages => {
            const chatContent = document.getElementById('chat-content');
            chatContent.innerHTML = '';

            messages.forEach(message => {
                const messageSendSpan = document.createElement("span")
                messageSendSpan.classList.add('message-date');
                messageSendSpan.textContent = new Date(message.sendTime).toTimeString().split(' ')[0];

                const messageElement = document.createElement('div');
                messageElement.classList.add('chat-message');
                messageElement.innerHTML = message.text.replace(/\n/g, '<br>');
                messageElement.dataset.messageId = message.id;
                messageElement.dataset.date = new Date(message.sendTime).toDateString()
                if (message.user_id == userId) {
                    messageElement.classList.add('message-sent');
                } else {
                    messageElement.classList.add('message-received');
                }

                messageElement.appendChild(messageSendSpan)
                chatContent.appendChild(messageElement);
            });
            chatContent.scrollTop = chatContent.scrollHeight;

            return Promise.resolve();
        })
        .catch(error => {
            console.log('Error', error);
            return Promise.reject(error);
        });
}
export function CreateChatWithSearchResult(userElement) {
    fetch('/Chat/CreateChat', {
        method: 'POST',
        body: JSON.stringify({
            companionId: userElement.dataset.userSearchId,
        }),
        headers: {
            "Content-type": 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {

                return response.text().then(result => {
                    const resultList = document.getElementById('search-list')
                    resultList.innerHTML = `<li>${result}</li>`;
                    throw new Error(result);
                });
            }
        })
        .then(data => {
            const chatId = data
            hubConnection.invoke('AddToChatGroup', chatId.toString());
            window.location.reload();
        })
        .catch(error => console.log(error.toString()))
}

export function GetUserId() {
    const userId = document.cookie.split('; ')
        .find(c => c.startsWith('user_id'))
        .split('=')[1];

    return parseInt(userId);
}