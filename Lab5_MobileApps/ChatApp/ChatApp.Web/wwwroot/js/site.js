
(function () {
    var txtMessage = $('#txtMessage');
    var btnSend = $('#btnSend');
    var listMessages = $('#listMessages');

    var connection = new signalR.HubConnectionBuilder().withUrl('/chathub').build();

    var userName = $('#userName').val();
    

    $(btnSend).click(function () {
        var userMessage = $(txtMessage).val();
        connection.invoke('SendMessage', {
            userName: userName,
            message: userMessage
        }).catch(function (error) {
            alert('Cant send the message, check console log');
            console.log(error);
        })
        $('#txtMessage').val('');
    })

    connection.start().then(function () {
        console.log('Connected to the SignalR Hub.');
        $(btnSend).removeAttr('disabled');

        connection.invoke("RegisterUser", userName)
            .catch(function (error)
            {
                console.error(error);
                alert("Can't join to the conversation")
            })
    })

    connection.on('RecieveMessage', function (obj) {
        console.log(obj)
        $(listMessages).prepend('<li>['
            + obj.timeStampString + ']'
            + '<span class="font-weight-bold">user: ' + obj.username
            + '</span> | message: ' + obj.message
            + '</li>'
        )
    })

    connection.on('UserJoined', function (userName) {
        console.log(userName)
        $(listMessages).prepend('<li class="font-weight-bold text-success">user ' + userName
            + ' joined the conversation.</li>'
        )
    })
})();