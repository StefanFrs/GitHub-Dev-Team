console.log("popups works");

var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();

connection.start().then(function () {
    console.log("connection established");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("RepositoryUpdate", function (data) {
    $("#alerts").append(
        `<div class="alert alert-primary alert-dismissible fade show" role="alert">
            User ${data.user} made a push in the repository ${data.repo} at ${data.data}.
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`);
});