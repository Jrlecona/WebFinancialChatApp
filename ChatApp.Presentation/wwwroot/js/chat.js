"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


document.getElementById("createRoom").addEventListener("click", async () => {
    const roomName = document.getElementById("roomName").value;
    await connection.invoke("JoinRoom", roomName);
});

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;

    const room = document.getElementById("roomList").querySelector(".selected").innerText;

    if (room === "default" || document.querySelector(`.message[data-room="${room}"]`)) {
        const messageList = document.getElementById("messageList");
        const newMessage = document.createElement("div");
        newMessage.className = "message";
        newMessage.dataset.room = room;
        newMessage.innerHTML = `<span class="username">${user}:</span> <span class="text">${message}</span>`;
        messageList.appendChild(newMessage);
    }
});

connection.on("GetRooms", rooms => {
    const roomList = document.getElementById("roomList");
    roomList.innerHTML = "";

    for (let room of rooms) {
        const listItem = document.createElement("li");
        listItem.innerText = room;
        listItem.addEventListener("click", async () => {
            await connection.invoke("JoinRoom", room);
        });
        roomList.appendChild(listItem);
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("GetRooms")
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});