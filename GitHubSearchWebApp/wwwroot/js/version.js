var settings = {
    "url": "/api/version",
    "method": "GET",
    "timeout": 0,
};

$.ajax(settings).done(function (response) {
    $("#version").text(response);
});

