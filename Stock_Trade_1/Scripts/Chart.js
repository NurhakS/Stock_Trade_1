

const data = {
    labels: [
      'Completed',
      'Progress'
    ],
    datasets: [{
      label: 'My First Dataset',
      data: [2180, 1586],
      backgroundColor: [
        'green',
        '#cecece'
      ],
      hoverOffset: 0
    }]
  };

  const config = {
    type: 'doughnut',
    data: data,
    options: {
      plugins: {
        legend: {
          display: false
        }
      },
      borderWidth: 0,
      cutout: '83%',
      rotation: 70
    }
  };

  const myChart = new Chart(
    document.getElementById('campaign_report'),
    config
  );