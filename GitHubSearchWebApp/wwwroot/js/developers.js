var settingsUsers = {
    "url": "https://localhost:5001/allDevelopers",
    "method": "GET",
    "timeout": 0,
};


$.ajax(settingsUsers).done(function (responses) {
    var users = document.getElementsByClassName("users")[0];

    responses.forEach((response, index) => {
        var newCard = document.createElement("DIV");
        newCard.classList.add("user-card");
        newCard.classList.add("mt-5");
        newCard.innerHTML = `
                                <div class="row justify-content-between" id="row${index}">
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
                                            <ul class= "technoList" id="ulTechList${index}">

                                            </ul>
                                    </div>
                                <div class="col-lg-4 mt-3">
                                    <button type="button" class="btn btn-dark btn-lg button-view" data-toggle="modal" data-target="#exampleModalLong"><i class="fas fa-expand-arrows-alt"></i>
                                    View</button>
                                </div>
                            </div>
                            <hr class="card-hr">
                             `;

        users.appendChild(newCard);

        var settingsLanguages = {
            "url": "https://localhost:5001/api/experiences/programmingLanguages/" + response.gitLogin,
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
            var skillsContainer = document.getElementsByClassName("technoList")[index];

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
                    var newLanguage = document.createElement("LI");
                    newLanguage.classList.add("skill-item");
                    newLanguage.classList.add("d-flex");
                    newLanguage.classList.add("align-items-center");
                    newLanguage.innerHTML = `<div class="skill-item d-flex align-items-center">
                                                <div class="dot mr-2" style="background:${newLanguageColour}">
                                                </div>
                                                <div class="skill-name ">
                                                    <h5 class="language">${element}</h5>
                                                </div>
                                            </div>`;
                    skillsContainer.appendChild(newLanguage);
                });
            })
        });
    })
})

function myFunction() {
    var input, filter, ul, li, a, i, txtValue, liSkills, j, technoLi, ul1;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    ul = document.getElementById("myUL");
    li = ul.childNodes;

    var user = [];
    for (i = 0; i < li.length; i++) {
        ul1 = document.getElementById("ulTechList" + i);
        technoLi = ul1.childNodes;
        var contains = false;
        for (j = 1; j < technoLi.length; j++) {
            a = technoLi[j].childNodes[0];
            txtValue = a.textContent || a.innerText;
            if (txtValue.toUpperCase().includes(filter)) {
                contains = true;
            }
        }
        if (!contains) {
            document.getElementById("row" + i).setAttribute("style", "display: none;");
        } else {
            document.getElementById("row" + i).setAttribute("style", "");
        }
    }
    console.log(user);
}