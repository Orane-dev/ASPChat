import { GetUserId } from './ChatFunc.js'

export let isConnected = false;

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/chat/signal')
    .build();

hubConnection.on('Receive', (message) => {
    try {
        const currentUserId = GetUserId();
        console.log(message);
        const chatContent = document.getElementById("chat-content");
        let messageElement = document.createElement('div');

        const messageSendSpan = document.createElement('span');
        messageSendSpan.classList.add('message-date');
        messageSendSpan.textContent = new Date(message.sendTime).toTimeString().split(' ')[0];

        messageElement.innerHTML = message.text.replace(/\n/g, '<br>');
        messageElement.classList.add("chat-message");
        messageElement.dataset.messageId = message.id;

        messageElement.appendChild(messageSendSpan);

        if (message.user_id == currentUserId) {
            messageElement.classList.add('message-sent')
        } else {
            messageElement.classList.add('message-received')
            MarkMessagesAsRead(message.id, chatContent.dataset.currentChatId);
        }

        chatContent.appendChild(messageElement);
        messageElement.scrollIntoView({ behavior: 'smooth' });
    } catch (error) {
        console.log(error.toString())
    }
    
});

hubConnection.on("ReadNotify", (chatId, countOfRead) => {
    const chatBadge = document.querySelector(`a[data-chatid="${chatId}"]`).querySelector('span.badge')

    if (parseInt(chatBadge.textContent) - countOfRead == 0) {
        chatBadge.remove();
    } else {
        chatBadge.textContent = pasrseInt(chatBadge.textContent) - countOfRead;
    }
});

hubConnection.start()
    .then(() => {
        console.log("Connection started");
        isConnected = true;
    })
    .catch(error => {
        console.log("Connection failed: ", error);
});

export function SendMessage(chatMessage) {
    hubConnection.invoke('SendToChat', chatMessage)
        .catch(error => {
            return console.log(error.toString());
        });
}

export function MarkMessagesAsRead(messageId, chatId) {
    hubConnection.invoke("MarkMessagesAsReaded", parseInt(messageId), parseInt(chatId))
}