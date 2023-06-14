class Messages {
    constructor(username, text) {
        this.userName = username;
        this.text = text;
    }
}

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chathub')
    .build();

connection.on('receiveMessage', addMessageToChat);

connection.start().then(
    function () { document.getElementById('submitButton').disabled = false; }
    ).catch(error => {
        console.error(error.message);
    });

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);


}

// userName is declared in razor page.
const username = userName;
const textInput = document.getElementById('messageText');
const nowInput = document.getElementById('now');
const chat = document.getElementById('chat');
const messagesQueue = [];

document.getElementById('submitButton').addEventListener('click', function (event) {
    var username = userName;
    var text = document.getElementById('messageText').value;

    let message = new Messages(username, text);

    sendMessageToHub(message);

});

function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    if (text.trim() === "") return;
    
    let message = new Message(username, text);
    console.log(message);
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container darker" : "container";

    let sender = document.createElement('p');
    sender.className = "sender";
    sender.innerHTML = message.userName;
    let text = document.createElement('p');
    text.innerHTML = message.text;



    container.appendChild(sender);
    container.appendChild(text);
    chat.appendChild(container);
}
