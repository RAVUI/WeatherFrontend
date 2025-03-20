console.log("map.js loaded");

let mapInstance = null;

window.initMap = (elementId, latitude, longitude, zoom) => {
    console.log(`initMap called with elementId: ${elementId}, lat: ${latitude}, lon: ${longitude}, zoom: ${zoom}`);

    // Remove existing map instance if it exists
    if (mapInstance) {
        mapInstance.remove();
    }

    // Initialize the Leaflet map
    mapInstance = L.map(elementId).setView([latitude, longitude], zoom);

    // Add OpenStreetMap base layer (free)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(mapInstance);

    // Add OpenWeatherMap temperature layer (replace YOUR_API_KEY)
    const weatherLayer = L.tileLayer('https://tile.openweathermap.org/map/temp_new/{z}/{x}/{y}.png?appid=YOUR_API_KEY', {
        attribution: 'Weather data © <a href="https://openweathermap.org/">OpenWeatherMap</a>',
        opacity: 0.7
    }).addTo(mapInstance);
};

window.addMarker = (latitude, longitude, popupText) => {
    if (mapInstance) {
        L.marker([latitude, longitude])
            .addTo(mapInstance)
            .bindPopup(popupText)
            .openPopup();
    }
};

window.fitBounds = (locations) => {
    if (mapInstance && locations && locations.length > 0) {
        const bounds = L.latLngBounds(locations.map(loc => [loc.latitude, loc.longitude]));
        mapInstance.fitBounds(bounds, { padding: [50, 50] });
    }
};

window.checkElementExists = (elementId) => {
    return !!document.getElementById(elementId);
};

window.isFunctionDefined = (functionName) => {
    return typeof window[functionName] === 'function';
};