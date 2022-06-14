$(document).ready(connectToSignalR);

function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("MagicMethodNameNumberTwo",
        function (user, message) {
            console.log(user);
            console.log(message);
        });

    conn.start().then(function () {
        console.log("SignalR started!");
    }).catch(function (error) {
        console.log("Error starting SignalR: ");
        console.log(error);
    });
}