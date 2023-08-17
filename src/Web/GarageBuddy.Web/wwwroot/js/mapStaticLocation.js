function initMapStaticLocation(mapContainer, inputCoordinates) {
    let initialCoordinates = [42.7339, 25.4858];

    if (inputCoordinates && validateCoordinates(inputCoordinates)) {
        const coords = inputCoordinates.split(',').map(coord => parseFloat(coord.trim()));
        if (coords.length === 2) {
            initialCoordinates = coords;
        }
    }

    const map = L.map(mapContainer).setView(initialCoordinates, 12);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: ''
    }).addTo(map);

    const marker = L.marker(initialCoordinates).addTo(map)
        .bindPopup('Garage location');
}