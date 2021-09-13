$(document).ready(function () {
    donutChart();

    $(window).resize(function () {
        window.donutChart.redraw();
    });
});

function donutChart() {
    window.donutChart = Morris.Donut({
        element: "donut-chart",
        data: [{
                label: "HTML",
                value: 30,
            },
            {
                label: "CSS",
                value: 35,
            },
            {
                label: "C#",
                value: 5,
            },
            {
                label: "JS",
                value: 20,
            },
        ],
        resize: true,
        redraw: true,
    });
}

var ctx = document.getElementById("myChart").getContext('2d');
var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ["Mark", "Tom", "Wade", "Brad", "Jack"],
        datasets: [{
            label: 'repositories',
            data: [12, 19, 3, 17, 28],
            backgroundColor: "rgba(0, 204, 0)"
        }, {
            label: 'commits',
            data: [30, 13, 8, 5, 16],
            backgroundColor: "rgba(0, 153, 255)"
        }]
    }
   
});