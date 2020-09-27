declare var $: any;
declare var signalR: any;

class ChatBot {
    connection: any;
    sendBtnSelector: string;
    messagesListSelector: string;
    userInputSelector: string;
    messageInputSelector: string;

    constructor() {
        $(document).ready(() => {
            chat.init();
        });
    }
    init() {
        chat.connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        chat.sendBtnSelector = "#sendButton";
        chat.messagesListSelector = "#messagesList";
        chat.userInputSelector = "#userInput";
        chat.messageInputSelector = "#messageInput";
        $(chat.sendBtnSelector).attr("disabled", true);

        chat.chatBotEvents();
        chat.sendButtonClick();
    }
    chatBotEvents() {
        chat.connection.on("ReceiveMessage", function (user, message) {
            var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var encodedMsg = user + " says " + msg;
            var li = document.createElement("li");
            li.textContent = encodedMsg;
            $(chat.messagesListSelector).append(li);
        });
        chat.connection.start().then(function () {
            $(chat.sendBtnSelector).attr("disabled", false);
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }
    sendButtonClick() {
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
    }
}
let chat = new ChatBot();