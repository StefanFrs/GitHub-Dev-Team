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

window.addEventListener("load", function () {
    getUsersRoles(usersRoles);
});

document.getElementById("updateRoles").addEventListener("click", function () {
    console.log("Roles updated");
    getUsersRoles(usersRolesUpdated);
    console.log(`Roles before update ${usersRoles}`);
    console.log(`Roles after update ${usersRolesUpdated}`);
})