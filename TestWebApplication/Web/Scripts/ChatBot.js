var ChatBot = (function () {
    function ChatBot() {
        $(document).ready(function () {
            chat.init();
        });
    }
    ChatBot.prototype.init = function () {
        chat.connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        chat.sendBtnSelector = "#sendButton";
        chat.messagesListSelector = "#messagesList";
        chat.userInputSelector = "#userInput";
        chat.messageInputSelector = "#messageInput";
        $(chat.sendBtnSelector).attr("disabled", true);
        chat.chatBotEvents();
        chat.sendButtonClick();
    };
    ChatBot.prototype.chatBotEvents = function () {
        chat.connection.on("ReceiveMessage", function (user, message) {
            var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var encodedMsg = user + " says " + msg;
            var li = document.createElement("li");
            li.textContent = encodedMsg;
            $(chat.messagesListSelector).appendChild(li);
        });
        chat.connection.start().then(function () {
            $(chat.sendBtnSelector).attr("disabled", false);
        }).catch(function (err) {
            return console.error(err.toString());
        });
    };
    ChatBot.prototype.sendButtonClick = function () {
        $(chat.sendBtnSelector).on("click", function (event) {
            var user = $(chat.userInputSelector).val();
            var message = $(chat.messageInputSelector).val();
            if (message !== "") {
                chat.connection.invoke("SendMessage", user, message).catch(function (err) {
                    return console.error(err.toString());
                });
                $(chat.messageInputSelector).val("");
            }
            event.preventDefault();
        });
    };
    return ChatBot;
}());
var chat = new ChatBot();
