var settingsUsers = {
    "url": "/allDevelopers",
    "method": "GET",
    "timeout": 0,
};


$.ajax(settingsUsers).done(function (responses) {
    var users = document.getElementsByClassName("users")[0];

    responses.forEach((response, index) => {

        var newCard = document.createElement("DIV");
        newCard.classList.add("user-card");
        newCard.classList.add("mt-5");
        newCard.innerHTML = `<li>
                                <div class="row justify-content-between">
                                <div class="col-lg-8 d-flex justify-content-between">
                                    <div class="user-name">
                                        <h3>${response.fullName}</h3>
                                    </div>
                                    <div class="git-account-image d-flex">
                                        <p class="pr-2">${response.gitLogin}</p>
                                        <img src="images/670020.png">
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="card-email">
                                        <p>${response.email}</p>
                                    </div>
                                </div>
                                <div class="col-lg-8 mt-4">
                                    <div class="skills-list d-flex justify-content-around align-items-center flex-wrap">
                                   
                                    </div>
                                <div class="col-lg-4 mt-3">
                                    <button type="button" class="btn btn-dark btn-lg button-view" data-toggle="modal" data-target="#exampleModalLong"><i class="fas fa-expand-arrows-alt"></i>
                                    View</button>
                                </div>
                            </div>
                            <hr class="card-hr">
                             </li>`;

        users.appendChild(newCard);

        var settingsLanguages = {
            "url": "/api/experiences/programmingLanguages/" + response.gitLogin,
            "method": "GET",
            "timeout": 0,
            "headers": {
                "Authorization": "Bearer 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"
            },
            data: {
                index: index
            }
        };

        $.ajax(settingsLanguages).done(function (response) {
            var skillsContainer = document.getElementsByClassName("skills-list")[index];

            response.forEach(element => {
                $.getJSON('js/colours.json', { element: element }).done(function (json) {
                    var coloursDictionary = [];
                    for (var language in json) {
                        if (json.hasOwnProperty(language)) {
                            var item = json[language];
                            coloursDictionary.push({
                                name: language,
                                colour: item.color
                            });
                        }
                    }

                    var newLanguageColour = "";
                    var newLanguageObject = [];
                    if (coloursDictionary.some(e => e.name === element)) {
                        newLanguageObject = coloursDictionary.filter(obj => {
                            return obj.name === element;
                        })
                        newLanguageColour = newLanguageObject[0].colour;
                    }
                    else {
                        newLanguageColour = "gray";
                    }
                    var newLanguage = document.createElement("DIV");
                    newLanguage.classList.add("skill-item");
                    newLanguage.classList.add("d-flex");
                    newLanguage.classList.add("align-items-center");
                    newLanguage.innerHTML = `<div class="skill-item d-flex align-items-center">
                                                <div class="dot mr-2" style="background:${newLanguageColour}">
                                                </div>
                                                <div class="skill-name ">
                                                    <p>${element}</p>
                                                </div>
                                            </div>`;
                    skillsContainer.appendChild(newLanguage);
                });
            })
        });
    })
})
