"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("TakeMessage", function (viewmodel) {
    var mess = JSON.parse(viewmodel);

    var container = document.createElement("div");
    container.classList.add("container");
    container.classList.add("alert");
    container.classList.add("alert-secondary");

    var h1 = document.createElement("h1");
    var p1 = document.createElement('p');
    var p2 =  document.createElement('p');
    h1.textContent = mess.UserName;
    p1.textContent = mess.Message;
    p2.textContent = mess.Time;
    container.appendChild(h1);
    container.appendChild(p1);
    container.appendChild(p2);

    document.getElementById("messages").appendChild(container);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName").value;
    var message = document.getElementById("message").value;
    var time = new Date();
    connection.invoke("SendMessage", user, message, time).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});