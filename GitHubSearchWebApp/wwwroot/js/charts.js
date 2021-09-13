$(document).ready(function () {
    donutChart();
    $(window).resize(function () {
        window.donutChart.redraw();
    });
});



var statisticsLanguages = {
    "url": "https://localhost:5001/api/experiences/statistics/languages",
    "method": "GET",
    "timeout": 0,
};

function donutChart() {
    $.ajax(statisticsLanguages).done(function (languages) {
        var statisticsArray = [];
        for (language in languages) {
            console.log(language + " " + languages[language])
            let languageTranslated = language.replace("Plus", "+").replace("Plus", "+").replace("Sharp", "#");
            statisticsArray.push({ label: languageTranslated, value: languages[language] });

            // add technology to dropdown
            var technology = document.createElement("a");
            technology.setAttribute("class", "dropdown-item");
            technology.setAttribute("data-value", languageTranslated);
            technology.setAttribute("href", "#");
            technology.textContent = languageTranslated;
            technology.addEventListener("click", function () {
                console.log(this.getAttribute("data-value"));
                addNewChart(this.getAttribute("data-value"));
                $(this).parents(".dropdown").find('.btn').html($(this).text());
            });
            document.getElementById("technologies").appendChild(technology);
        }
        window.donutChart = Morris.Donut({
            element: "donut-chart",
            data: statisticsArray,
            resize: true,
            redraw: true,
        });
    });
}



function addNewChart(language) {
    
    var statistics = {
        "url": "https://localhost:5001/api/experiences/statistics/" + language.replace("+", "Plus").replace("+", "Plus").replace("#", "Sharp"),
        "method": "GET",
        "timeout": 0,
    };
    $.ajax(statistics).done(function (statistics) {
        $('#canvas').remove();
        $('#displayCharts').append('<canvas id="canvas"><canvas>');

        var ctx = document.getElementById("canvas").getContext('2d');

        var developers = [];
        var repositoriesData = [];
        var codeData = [];
        for (statistic in statistics) {
            developers.push(statistic);
            var data = statistics[statistic].split(" ");
            codeData.push(data[0]/(10 ** 3));
            repositoriesData.push(data[1]);
        }
        console.log(language + " " + developers + " " + codeData + " " + repositoriesData);
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: developers,
                datasets: [{
                    label: 'repositories (no)',
                    data: repositoriesData,
                    backgroundColor: "rgba(0, 204, 0)"
                }, {
                    label: 'code Size (MB)',
                    data: codeData,
                    backgroundColor: "rgba(0, 153, 255)"
                }]
            }

        });
    });
}