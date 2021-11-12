
$(document).ready(function () {

    var connection = new WebSocketManager.Connection(
        "ws://" + window.location.host + "/chat");

    connection.enableLogging = true;

    connection.clientMethods["SendMessage"] = (sender, message) => {
        var mesStr = sender + " said: " + message;
        console.log(mesStr);

        $('#messages').append("<li>" + mesStr + "</li>");
    }

    connection.start();

    var $messageContent = $('#message-content');

    $messageContent.keyup(function (e) {
        if (e.keyCode == 13) {

            var message = $messageContent.val().trim();
            if (message.length == 0)
                return false;

            connection.invoke("SendMessage", connection.connectionId, message);
            $messageContent.val('');
        }
    });
    $('#messages').scrollTop($('#messages').prop('scrollHeight'));
});