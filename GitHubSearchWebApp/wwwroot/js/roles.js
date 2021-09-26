var usersRoles = [];
var usersRolesUpdated = [];

function getUsersRoles(usersRolesParam) {
    var roleInputs = this.document.getElementsByClassName("role");
    for (var i = 0; i < roleInputs.length; i++) {
        var user = {
            userId: roleInputs[i].id,
            isUser: 0,
            isAdministrator: 0,
            isTeamLead: 0,
        }
        usersRolesParam[user.userId] = user;
    }
    for (var i = 0; i < roleInputs.length; i++) {
        let user = usersRolesParam[roleInputs[i].id];
        if (roleInputs[i].checked) {
            console.log(roleInputs[i].value);
            if (roleInputs[i].value == "User") {
                user.isUser = 1;
            }
            if (roleInputs[i].value == "Administrator") {
                user.isAdministrator = 1;
            }
            if (roleInputs[i].value == "TeamLead") {
                user.isTeamLead = 1;
            }
        }
        usersRolesParam[user.userId] = user;
    }
}

//document.getElementsByClassName("role").addEventListener("change", function (event) {
//    console.log("checked" + " " + event.target.id)
//    event.target.checked = true;
//});

//document.getElementsByClassName("role").addEventListener("input", function (event) {
//    console.log("checked" + " " + event.target.id)
//    event.target.checked = true;
//});

window.addEventListener("load", function () {
    getUsersRoles(usersRoles);
});


document.getElementById("rolesForm").addEventListener("submit", function () {
    event.preventDefault();
    console.log("Roles updated");
    getUsersRoles(usersRolesUpdated);
    console.log(`Roles before update ${usersRoles}`);
    console.log(`Roles after update ${usersRolesUpdated}`);
    updateUserRoles();

});

function updateUserRoles() {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Cookie", ".AspNetCore.Antiforgery.87tdcVOTNOU=CfDJ8FLYrQrO7-hGqFCarwSqqPzsUO-doVUOX-B1HmoO1f0Ezq0GeEJcRLipZKT2_UO2k2R6Q7vjjbJQMYlMz82jh88C-K_kbufuNVk_foNzsiZp8sN6U2ZVYdsvFzvkdk1Qc1KcvDOqW9bwo-WiV6qcCOQ");

    var raw = JSON.stringify({
        "id": "cae6246c-a34b-493e-83e6-42aad5ba830c",
        "isUser": 1,
        "isTeamLead": 1,
        "isAdministrator": 0
    });

    var requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://localhost:5001/updateRoles", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}