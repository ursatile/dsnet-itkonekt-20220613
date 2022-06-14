$(document).ready(connectToSignalR);

function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DisplayNewVehicleWithPrice", displayNewVehicleNotification);

    conn.start().then(function () {
        console.log("SignalR started!");
    }).catch(function (error) {
        console.log("Error starting SignalR: ");
        console.log(error);
    });
}

function displayNewVehicleNotification(user, message) {
    let $target = $("#signalr-notifications");
    var data = JSON.parse(message);
    var $html = $(`<div>
New vehicle alert: ${data.manufacturerName} ${data.modelName} (${data.year}, ${data.color})<br />
Cost: ${data.price} ${data.currencyCode}<br />
<hr />
<a href="/vehicles/details/${data.registration}">click here for details!</a>
</div>`);
    $html.css("background-color", data.color);
    $target.prepend($html);
    window.setTimeout(function () {
        $html.fadeOut(2000,
            function () {
                $html.remove();
            });
    }, 5000);
}