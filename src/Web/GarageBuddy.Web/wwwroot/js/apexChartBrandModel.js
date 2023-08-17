function renderApexChart(modelCounts, brandNames, containerSelector) {
    const optionsProfileVisit = {
        annotations: {
            position: "back",
        },
        dataLabels: {
            enabled: false,
        },
        chart: {
            type: "bar",
            height: 300,
            toolbar: {
                show: false
            },
        },
        fill: {
            opacity: 1,
        },
        plotOptions: {},
        series: [
            {
                name: "Model Count",
                data: modelCounts
            },
        ],
        colors: "#435ebe",
        xaxis: {
            categories: brandNames
        },
    };

    const chartProfileVisit = new ApexCharts(
        document.querySelector(containerSelector),
        optionsProfileVisit
    );

    chartProfileVisit.render();
}