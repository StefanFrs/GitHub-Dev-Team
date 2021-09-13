var settings = {
    "url": "https://localhost:5001/allDevelopers",
    "method": "GET",
    "timeout": 0,
};

$.ajax(settings).done(function (responses) {
    var users = document.getElementsByClassName("users")[0];
    
    responses.forEach((response) => {
        //const headers = {
        //        "Authorization":`Token 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`
        //}
        //var userLink = "https://api.github.com/users/" + response.gitLogin;
        //var userResponse = await fetch(userLink, {
        //    "method": "GET",
        //    "headers": headers
        //});
        //console.log(userResponse)
        //var userIcon = userResponse["avatar_url"];

        var newCard = document.createElement("DIV");
        newCard.classList.add("user-card");
        newCard.classList.add("mt-5");
        newCard.innerHTML = `<div class="row justify-content-between">
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

                                            <div class="skill-item d-flex align-items-center">
                                                <div class="dot mr-2">
                                                    </div>
                                                    <div class="skill-name ">
                                                        <p>C#</p>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="col-lg-4 mt-3">
                                            <button class="btn btn-dark btn-lg button-view"><i class="fas fa-expand-arrows-alt"></i>
                                                View</button>
                                        </div>
                                    </div>
                                    <hr class="card-hr">
                                </div>
                            </div>`;
        users.appendChild(newCard);
        //async function getRepos(user) {
            
        //    var repositoriesJson = await repositoriesResponse.json();
        //    console.log(repositoriesJson);
            //repositoriesJson.forEach(repo => {
            //   //var languagesResponse = await fetch(repo["languages_url"]);
            //   // var languagesJson = await languagesResponse.json();
            //   // console.log(languagesJson);
            //})
        //}

        //getRepos(response.gitLogin);
    })
})