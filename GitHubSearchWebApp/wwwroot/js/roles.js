var usersRoles = [];

window.addEventListener("load", function () {
    var roleInputs = this.document.getElementsByClassName("role");
    for (var i = 0; i < roleInputs.length; i++) {
        var user = {
            userId: roleInputs[i].id,
            isUser: 0,
            isAdministrator: 0,
            isTeamLead: 0,
        }
        usersRoles[user.userId] = user;
    }
    for (var i = 0; i < roleInputs.length; i++) {
        let user = usersRoles[roleInputs[i].id];
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
        usersRoles[user.userId] = user;
    }
    console.log(usersRoles);
});