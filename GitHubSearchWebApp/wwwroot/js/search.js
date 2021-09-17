var results = [];

class UI {
    static async displayResults(data) {
        await Store.getResults(data);
        if (results.length === 0) {
            console.log("none")
            UI.addNoResults();
        }
        results.forEach((result) => {
            console.log(result);
            UI.addSearchResultToList(result)
        });
    }

    static addSearchResultToList(result) {
        const list = document.querySelector('#search-results-list');
        const button = document.createElement('button');
        button.setAttribute("onclick", `window.open("${ result.htmlUrl }", "_blank")`);
        button.classList.add('list-group-item');
        button.classList.add('list-group-action');
        button.innerHTML = `
            <ul class="list-group list-group-horizontal align-items-center justify-content-center">
                <li class="list-group-item border-0 m-auto">
                    <img src="${result.avatarUrl}" width="50" height="50" class="rounded-circle" alt="Cinque Terre">
                </li>
                <li class="list-group-item border-0 m-auto">
                    ${result.name}
                </li>
                <li class="list-group-item border-0 m-auto">
                    ${result.username}
                </li>
            </ul>`
        list.appendChild(button);
    }

    static addNoResults() {
        document.querySelector('#search-results-list').innerHTML = "No results found."
    }

    static resetResults() {
        results.length = 0;
        document.querySelector('#search-results-list').innerHTML = '';
    }
}

class Store {
    static getResults(data) {
        if (data === []) {
            return [];
        }
        data.forEach((result) => results.push(result));
        return results;
    }

}

document.querySelector('#submitBtn').addEventListener('click', (e) => {
    e.preventDefault;
    var search = document.getElementById("search_bar").value;
    console.log(search)
    if (search) {
        UI.resetResults();
        document.getElementById("spinner").setAttribute("style", "");
        fetch("/api/gitrepository/" + search)
            .then((result) => result.json())
            .then((data) => {
                console.log(data);
                UI.displayResults(data);
                document.getElementById("spinner").setAttribute("style", "display:none");
            })
    }
    
})