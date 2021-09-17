﻿var settingsUsers = {
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
                                    <div class="skills-list d-flex justify-content-start align-items-center flex-wrap">
                                            <ul class= "technoList" id="ulTechList${index}">

                                            </ul>
                                    </div>
                                </div>
                                <div class="col-lg-4 mt-3">
                                    <button type="button" onClick="FetchDataIntoModal('${response.gitLogin}', '${response.id}')" class="btn btn-dark btn-lg button-view" data-toggle="modal" data-target="#exampleModalLong"><i class="fas fa-expand-arrows-alt"></i>
                                    View</button>
                                </div>
                            
                        </div>
                            <hr class="card-hr">`;

        users.appendChild(newCard);

        var settingsLanguages = {
            "url": "/api/experiences/programmingLanguages/" + response.gitLogin,
            "method": "GET",
            "timeout": 0,

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

function FetchDataIntoModal(gitLogin, userId) {
    document.getElementsByClassName("dropdown-menu")[0].setAttribute("currentUser", gitLogin);


    // Header
    var settings = {
        "url": "/developer/" + gitLogin,
        "method": "GET",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        document.getElementById("devName").innerHTML = response.fullName;
        document.getElementById("devGitName").innerHTML = response.gitLogin;
        document.getElementById("devEmail").innerHTML = response.email;
        document.getElementById("devPicture").setAttribute("src", response.avatarURL);
    });

    var settings = {
        "url": "/developer/repoCount/" + userId,
        "method": "GET",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        document.getElementById("repoCount").innerHTML = response;
    });

    //Skill list
    var settings = {
        "url": "/api/experiences/programmingLanguages/" + gitLogin,
        "method": "GET",
        "timeout": 0,
        "data": {
            "gitLogin": gitLogin
        }
    };

    $.ajax(settings).done(function (response) {
        document.getElementById("languageList").innerHTML = "";
        response.forEach(language => {

            var modalListItem = document.createElement("LI");
            modalListItem.setAttribute("id", "id-" + language);
            modalListItem.classList.add("list-item");
            modalListItem.classList.add("col-6");
            modalListItem.classList.add("py-2");
            modalListItem.innerHTML = `<div class="box" >
                                            <div class="skill-name" id="devSkill">
                                                <p>${language}</p>
                                            </div>
                                            <div class="col">
                                                <div class="progress" id="devProgress" style="border-radius: 10px;">
                                                    <div id="progress-${language}" class="progress-bar rounded-pill bg-warning" role="progressbar" style="width: 30%" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                            <div class="codeSize" id="codeSize">
                                                <p id="${language}"></p>
                                            </div>
                                            <p class="skill-name ml-2">code(KB)</p>
                                        </div>`;
            document.getElementById("languageList").appendChild(modalListItem)

            var settings = {
                "url": "/developer/codeSize/" + userId + "/" + language,
                "method": "GET",
                "timeout": 0,
                "data": {
                    language: language
                }
            };

            $.ajax(settings).done(function (response) {
                document.getElementById(language).innerHTML = response;
                document.getElementById("id-" + language).setAttribute("id", response);
            });
        })

        // Show projects
        var settings = {
            "url": "/api/experiences/" + gitLogin + "/" + response[0],
            "method": "GET",
            "timeout": 0,
        };

        $.ajax(settings).done(function (response) {
            document.getElementById("projectsList").innerHTML = "";
            response.forEach(gitProject => {
                var newGitProject = document.createElement("LI")
                newGitProject.classList.add("list-item")
                newGitProject.innerHTML =
                    `<div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${gitProject.name}</h5>
                            <a href="${gitProject.url}">Check it out <b>nigga</b>.</a>
                        </div>
                    </div>`;
                document.getElementById("projectsList").appendChild(newGitProject);
            })
        });
    }).then(function () {
        var totalCode = 0;
        var listItems = document.getElementById("languageList").childNodes;
        for (var i = 0; i < listItems.length; i++) {
            console.log(listItems[i].id)
            console.log(listItems[i])
            totalCode += listItems[i].value;

        }
        console.log(totalCode)
    });

    //Skill dropdown
    var settings = {
        "url": "/api/experiences/programmingLanguages/" + gitLogin,
        "method": "GET",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        document.getElementsByClassName("dropdown-menu")[0].innerHTML = "";

        response.forEach(language => {
            var option = document.createElement("A");
            option.classList.add("dropdown-item");
            option.addEventListener("click", function () {
                UpdateModal(this.getAttribute("data-value"));
                $(this).parents(".dropdown").find('.btn').html($(this).text());
            });
            option.setAttribute("data-value", language)
            option.innerHTML = language;
            document.getElementsByClassName("dropdown-menu")[0].appendChild(option);
        })
    });
}

function UpdateModal(value) {

    var AllLanguagesArray = [{ "language": "Java", "number": 1 },
    { "language": "CPlusPlus", "number": 2 },
    { "language": "C", "number": 3 },
    { "language": "CSharp", "number": 4 },
    { "language": "JavaScript", "number": 5 },
    { "language": "CSS", "number": 6 },
    { "language": "TypeScript", "number": 7 },
    { "language": "Vue", "number": 8 },
    { "language": "JupyterNotebook", "number": 9 },
    { "language": "Python", "number": 10 },
    { "language": "Go", "number": 11 },
    { "language": "Ruby", "number": 12 },
    { "language": "PHP", "number": 13 },
    { "language": "Scala", "number": 14 },
    { "language": "Shell", "number": 15 },
    { "language": "Kotlin", "number": 16 },
    { "language": "Swift", "number": 17 },
    { "language": "Perl", "number": 18 },
    { "language": "ObjectiveC", "number": 19 },
    { "language": "Webassembly", "number": 20 },
    { "language": "HTML", "number": 21 },
    { "language": "Dart", "number": 22 },
    { "language": "Dockerfile", "number": 23 },
    { "language": "Haskell", "number": 24 },
    { "language": "Starlark", "number": 24 },
    { "language": "SystemVerilog", "number": 24 }]
    var gitLogin = document.getElementsByClassName("dropdown-menu")[0].getAttribute("currentUser");
    document.getElementsByClassName("dropdown-menu")[0].setAttribute("currentLanguage", value)
    var settings = {
        "url": "/developer/" + gitLogin,
        "method": "GET",
        "timeout": 0,
        "data": {
            "value": value
        }
    };

    $.ajax(settings).done(function (response) {
        var programmingLanguageNumber = AllLanguagesArray.filter(x => x.language === value)[0].number;
        var description = response.experiences.filter(x => x.programmingLanguage === programmingLanguageNumber)[0].description;
        if (description == "") {
            description = "Be the first to review!";
        }
        document.getElementById("review").innerHTML = "";
        document.getElementById("review").innerHTML = description;
    });


    var settings = {
        "url": "/api/experiences/" + gitLogin + "/" + value,
        "method": "GET",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        document.getElementById("projectsList").innerHTML = "";
        response.forEach(gitProject => {
            var newGitProject = document.createElement("LI")
            newGitProject.classList.add("list-item")
            newGitProject.setAttribute("target", "_blank")
            newGitProject.innerHTML =
                `<div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${gitProject.name}</h5>
                            <a href="${gitProject.url}">Check it out <b>nigga</b>.</a>
                        </div>
                    </div>`;
            document.getElementById("projectsList").appendChild(newGitProject);
        })
    });
}

function UpdateReview() {
    var AllLanguagesArray = [{ "language": "Java", "number": 1 },
    { "language": "CPlusPlus", "number": 2 },
    { "language": "C", "number": 3 },
    { "language": "CSharp", "number": 4 },
    { "language": "JavaScript", "number": 5 },
    { "language": "CSS", "number": 6 },
    { "language": "TypeScript", "number": 7 },
    { "language": "Vue", "number": 8 },
    { "language": "JupyterNotebook", "number": 9 },
    { "language": "Python", "number": 10 },
    { "language": "Go", "number": 11 },
    { "language": "Ruby", "number": 12 },
    { "language": "PHP", "number": 13 },
    { "language": "Scala", "number": 14 },
    { "language": "Shell", "number": 15 },
    { "language": "Kotlin", "number": 16 },
    { "language": "Swift", "number": 17 },
    { "language": "Perl", "number": 18 },
    { "language": "ObjectiveC", "number": 19 },
    { "language": "Webassembly", "number": 20 },
    { "language": "HTML", "number": 21 },
    { "language": "Dart", "number": 22 },
    { "language": "Dockerfile", "number": 23 },
    { "language": "Haskell", "number": 24 },
    { "language": "Starlark", "number": 24 },
    { "language": "SystemVerilog", "number": 24 }]
    var gitLogin = document.getElementsByClassName("dropdown-menu")[0].getAttribute("currentUser");
    var language = document.getElementsByClassName("dropdown-menu")[0].getAttribute("currentLanguage");
    var programmingLanguageNumber = AllLanguagesArray.filter(x => x.language === language)[0].number;

    var newReview = document.getElementById("newReview").value;
    console.log(gitLogin, language, programmingLanguageNumber, newReview);
    var settings = {
        "url": "/api/experiences/" + gitLogin + "/" + language + "/" + newReview,
        "method": "PUT",
        "timeout": 0,
    };
    $.ajax(settings).done(function () {
        document.getElementById("closeModal").click();
    });


}

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