console.log("popups works");

var connection = new signalR.HubConnectionBuilder().withUrl("/updates").build();

connection.start().then(function () {
    console.log("connection established");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("RepositoryUpdate", function (data) {
    $("#alerts").prepend(
        `<div class="text-center alert alert-primary alert-dismissible fade show" role="alert">
            User ${data.user} made a push in the repository ${data.repository}.
            <i>Total change:${data.size}Kb</i> <br>
            <strong>Please refresh :) </strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`);
});