$(document).ready(function () {

    /***************** Waypoints ******************/

    $('.wp1').waypoint(function () {
        $('.wp1').addClass('animated fadeInLeft');
    }, {
        offset: '75%'
    });
    $('.wp2').waypoint(function () {
        $('.wp2').addClass('animated fadeInRight');
    }, {
        offset: '75%'
    });
    $('.wp3').waypoint(function () {
        $('.wp3').addClass('animated fadeInLeft');
    }, {
        offset: '75%'
    });
    $('.wp4').waypoint(function () {
        $('.wp4').addClass('animated fadeInRight');
    }, {
        offset: '75%'
    });
    $('.wp5').waypoint(function () {
        $('.wp5').addClass('animated fadeInLeft');
    }, {
        offset: '75%'
    });
    $('.wp6').waypoint(function () {
        $('.wp6').addClass('animated fadeInRight');
    }, {
        offset: '75%'
    });
    $('.wp7').waypoint(function () {
        $('.wp7').addClass('animated fadeInUp');
    }, {
        offset: '75%'
    });
    $('.wp8').waypoint(function () {
        $('.wp8').addClass('animated fadeInLeft');
    }, {
        offset: '75%'
    });
    $('.wp9').waypoint(function () {
        $('.wp9').addClass('animated fadeInRight');
    }, {
        offset: '75%'
    });

    /***************** Initiate Flexslider ******************/
    $('.flexslider').flexslider({
        animation: "slide"
    });

    /***************** Initiate Fancybox ******************/

    $('.single_image').fancybox({
        padding: 4
    });

    $('.fancybox').fancybox({
        padding: 4,
        width: 1000,
        height: 800
    });

    /***************** Tooltips ******************/
    $('[data-toggle="tooltip"]').tooltip();

    /***************** Nav Transformicon ******************/

    /* When user clicks the Icon */
    $('.nav-toggle').click(function () {
        $(this).toggleClass('active');
        $('.header-nav').toggleClass('open');
        event.preventDefault();
    });
    /* When user clicks a link */
    $('.header-nav li a').click(function () {
        $('.nav-toggle').toggleClass('active');
        $('.header-nav').toggleClass('open');

    });

    /***************** Header BG Scroll ******************/

    $(function () {
        $(window).scroll(function () {
            var scroll = $(window).scrollTop();

            if (scroll >= 20) {
                $('section.navigation').addClass('fixed');
                $('header').css({
                    "border-bottom": "none",
                    "padding": "35px 0"
                });
                $('header .member-actions').css({
                    "top": "26px",
                });
                $('header .navicon').css({
                    "top": "34px",
                });
            } else {
                $('section.navigation').removeClass('fixed');
                $('header').css({
                    "border-bottom": "solid 1px rgba(255, 255, 255, 0.2)",
                    "padding": "50px 0"
                });
                $('header .member-actions').css({
                    "top": "41px",
                });
                $('header .navicon').css({
                    "top": "48px",
                });
            }
        });
    });
    /***************** Smooth Scrolling ******************/

    $(function () {

        $('a[href*=#]:not([href=#])').click(function () {
            if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {

                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top - 90
                    }, 2000);
                    return false;
                }
            }
        });

    });

    /********************** Social Share buttons ***********************/
    var share_bar = document.getElementsByClassName('share-bar');
    var po = document.createElement('script');
    po.type = 'text/javascript';
    po.async = true;
    po.src = 'https://apis.google.com/js/platform.js';
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(po, s);

    for (var i = 0; i < share_bar.length; i++) {
        var html = '<iframe allowtransparency="true" frameborder="0" scrolling="no"' +
            'src="https://platform.twitter.com/widgets/tweet_button.html?url=' + encodeURIComponent(window.location) + '&amp;text=' + encodeURIComponent(document.title) + '&amp;via=ramswarooppatra&amp;hashtags=ramandantara&amp;count=horizontal"' +
            'style="width:105px; height:21px;">' +
            '</iframe>' +

            '<iframe src="//www.facebook.com/plugins/like.php?href=' + encodeURIComponent(window.location) + '&amp;width&amp;layout=button_count&amp;action=like&amp;show_faces=false&amp;share=true&amp;height=21&amp;appId=101094500229731&amp;width=150" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:150px; height:21px;" allowTransparency="true"></iframe>' +

            '<div class="g-plusone" data-size="medium"></div>';

        // '<iframe src="https://plusone.google.com/_/+1/fastbutton?bsv&amp;size=medium&amp;url=' + encodeURIComponent(window.location) + '" allowtransparency="true" frameborder="0" scrolling="no" title="+1" style="width:105px; height:21px;"></iframe>';

        //share_bar[i].innerHTML = html;
        //share_bar[i].style.display = 'inline-block';
    }

    /********************** Embed youtube video *********************/
    $('.player').YTPlayer();


    /********************** Toggle Map Content **********************/
    $('#btn-show-map').click(function () {
        $('#map-content').toggleClass('toggle-map-content');
        $('#btn-show-content').toggleClass('toggle-map-content');
    });
    $('#btn-show-content').click(function () {
        $('#map-content').toggleClass('toggle-map-content');
        $('#btn-show-content').toggleClass('toggle-map-content');
    });

    /********************** Add to Calendar **********************/
    var myCalendar = createCalendar({
        options: {
            class: '',
            // You can pass an ID. If you don't, one will be generated for you
            id: ''
        },
        data: {
            // Event title
            title: "Lauras & Marvins Hochzeit",

            // Event start date
            start: new Date('Oct 25, 2025 13:00'),

            // Event duration (IN MINUTES)
            //duration: 120,

            // You can also choose to set an end time
            // If an end time is set, this will take precedence over duration
            end: new Date('Oct 26, 2025 05:00'),

            // Event Address
            address: 'Beans Restaurant, Kaffeedamm 2, 30900 Wedemark',

            // Event Description
            description: "Wir freuen uns auf euch! Bei Fragen meldet euch gerne bei unseren Trauzeugen Jonas (Tel +49171 6940327) oder Vivien (Tel +49176 20627648)"
        }
    });

    $('#add-to-cal').html(myCalendar);


    /********************** RSVP **********************/

    $('#rsvp-form').on('submit', function (e) {
        e.preventDefault();
        var data = $(this).serialize();

        $('#alert-wrapper').html(alert_markup('info', '<strong>Eine Sekunde!</strong>Wir speichern die Anmeldung.'));

        $.post('__FUNCTION_API_URL__/Rsvp', data)
            .done(function (data) {
                console.log(data);
                if (data.result === "error") {
                    $('#alert-wrapper').html(alert_markup('danger', data.message));
                } else {
                    $('#alert-wrapper').html('');
                    $('#rsvp-modal').modal('show');
                }
            })
            .fail(function (data) {
                console.log(data);

                if (data.status === 409) {
                    $('#alert-wrapper').html(alert_markup('danger', '<strong>Sorry!</strong> Du hast dich bereits angemeldet. Melde dich bei uns, wenn du Daten ändern möchtest.'));
                }
                else {
                    $('#alert-wrapper').html(alert_markup('danger', '<strong>Sorry!</strong> Es gibt Probleme mit der Anmeldung. Melde dich am besten bei uns.'));
                }

            });
    });

});

/********************** Extras **********************/

let map;
let storedMarkers = [];
let infoWindow;

// Google map
const markers = [
    {
        position: { lat: 52.574871898933154, lng: 9.719410040320284 },
        title: 'Beans Restaurant',
        category: 'Restaurant',
        address: 'Kaffeedamm 2, 30900 Wedemark',
        icon: 'fa-utensils',
        background: '#F44336',
        borderColor: '#D32F2F',
    },
    {
        position: { lat: 52.60793216818634, lng: 9.689291614949479 },
        title: 'Hotel Ballands',
        category: 'Hotel',
        address: 'Ahornallee 4, 29690 Lindwedel',
        icon: 'fa-hotel',
        background: '#2196F3',
        borderColor: '#1976D2',
    },
    {
        position: { lat: 52.549745830694036, lng: 9.746664811751025 },
        title: 'Pension Genat',
        category: 'Hotel',
        address: 'Industrieweg 4, 30900 Wedemark',
        icon: 'fa-hotel',
        background: '#2196F3',
        borderColor: '#1976D2',
    },
    {
        position: { lat: 52.544730590968705, lng: 9.725521454206698 },
        title: 'Pension Beermann',
        category: 'Hotel',
        address: 'Hermann-Löns-Straße 7, 30900 Wedemark',
        icon: 'fa-hotel',
        background: '#2196F3',
        borderColor: '#1976D2',
    },
    {
        position: { lat: 52.37127997346491, lng: 9.736146320769006 },
        title: 'Standesamt Hannover (Altes Rathaus)',
        category: 'Standesamt',
        address: 'Karmarschstraße 42, 30159 Hannover',
        icon: 'fa-ring',
        background: '#4CAF50',
        borderColor: '#388E3C',
    },
    {
        position: { lat: 52.37008670938518, lng: 9.737431924595946 },
        title: 'Parkplatz Marktstraße',
        category: 'Parken',
        address: 'Markstraße 47, 30159 Hannover',
        icon: 'fa-Parken',
        background: '#8BC34A',
        borderColor: '#689F38',
    },
    {
        position: { lat: 52.37090027499516, lng: 9.737617816992994 },
        title: 'CONTIPARK Parkhaus Markthalle',
        category: 'Parken',
        address: 'Röselerstraße 7, 30159 Hannover',
        icon: 'fa-Parken',
        background: '#8BC34A',
        borderColor: '#689F38',
    },
    {
        position: { lat: 52.58066254815064, lng: 9.72739739740679 },
        title: 'S-Bahn Station Bennemühlen',
        category: 'Anreise',
        address: 'Am Bahnhof 1, 30900 Wedemark',
        icon: 'fa-train-subway',
        background: '#f3ad21',
        borderColor: '#9e7c37',
    }
];

async function initMap() {
    const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary("marker");

    var baseLocation = { lat: 52.574871898933154, lng: 9.719410040320284 };
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        zoom: 15,
        center: baseLocation,
        scrollwheel: false,
        mapId: "a7db4426599686f6",
    });

    infoWindow = new google.maps.InfoWindow({
        disableAutoPan: false,
        headerDisabled: true
    });

    markers.forEach((markerConfig, index) => {
        const iconElement = document.createElement("div");
        iconElement.innerHTML = `<i class="fa ${markerConfig.icon}"></i>`;

        const pin = new PinElement({
            scale: 1,
            glyph: iconElement,
            glyphColor: "white",
            background: markerConfig.background,
            borderColor: markerConfig.borderColor,
        });

        const marker = new AdvancedMarkerElement({
            position: markerConfig.position,
            map: map,
            title: markerConfig.title,
            content: pin.element,
        });

        const content = getInfoWindowContent(markerConfig);

        marker.addListener('click', () => {
            infoWindow.setContent(content);
            infoWindow.open({
                anchor: marker,
                map
            });
        });

        storedMarkers.push(marker);

        // Show info window for Beans Restaurant (first marker) on load
        if (index === 0) {
            // Use a slight delay to ensure the map is fully loaded
            setTimeout(() => {
                infoWindow.setContent(content);
                infoWindow.open({
                    anchor: marker,
                    map
                });
            }, 200);
        }
    });

    // After all markers are added
    addEdgeIndicatorListeners();

    // Initial update for edge indicators
    setTimeout(updateEdgeIndicators, 200);
}

function mapGoToMarker(index) {
    const marker = storedMarkers[index];
    const markerInfo = markers[index];

    // Close any open info window
    infoWindow.close();

    // Get current and target positions
    const currentLat = map.getCenter().lat();
    const currentLng = map.getCenter().lng();
    const targetLat = marker.position.lat;
    const targetLng = marker.position.lng;

    // Calculate distance to move
    const latDiff = targetLat - currentLat;
    const lngDiff = targetLng - currentLng;

    // Animate the pan
    const animationDuration = 750; // milliseconds
    const frames = 60;
    const frameTime = animationDuration / frames;
    let frameCount = 0;

    function animateFrame() {
        if (frameCount < frames) {
            // Calculate easing progress (ease-out function)
            const progress = 1 - Math.pow(1 - frameCount / frames, 3);

            // Calculate new center position
            const newLat = currentLat + latDiff * progress;
            const newLng = currentLng + lngDiff * progress;

            // Set the new center
            map.setCenter({ lat: newLat, lng: newLng });

            // Increment frame count
            frameCount++;

            // Request next frame
            setTimeout(animateFrame, frameTime);
        } else {
            // Animation complete, now set zoom and open info window
            map.setZoom(15);

            setTimeout(() => {
                infoWindow.setContent(getInfoWindowContent(markerInfo));
                infoWindow.open({
                    anchor: marker,
                    map
                });
            }, 200);
        }
    }

    // Start animation
    animateFrame();
}

function getInfoWindowContent(markerInfo) {
    return `
        <div class="info-window">
            <small>${markerInfo.category}</small>
            <h4>${markerInfo.title}</h4>
            ${markerInfo.address ? `<p><small>${markerInfo.address}</small></p>` : ''}
            ${markerInfo.address ? `<a href="https://www.google.com/maps/dir/?api=1&destination=${encodeURIComponent(markerInfo.address)}" target="_blank">Navigation</a>` : ''}
        </div>
    `;
}

// Track visible markers and update edge indicators
function updateEdgeIndicators() {
    const bounds = map.getBounds();
    if (!bounds) return;

    // Remove any existing edge indicators
    document.querySelectorAll('.map-edge-indicator').forEach(el => el.remove());

    // Group markers by edge
    const edgeMarkers = {
        top: [],
        right: [],
        bottom: [],
        left: []
    };

    // First pass: determine which edge each off-screen marker belongs to
    markers.forEach((markerConfig, index) => {
        const position = markerConfig.position;

        // Skip visible markers
        if (bounds.contains(position)) return;

        const mapDiv = map.getDiv();
        const mapWidth = mapDiv.offsetWidth;
        const mapHeight = mapDiv.offsetHeight;

        // Convert geo coordinates to pixel coordinates
        const projection = map.getProjection();
        const point = projection.fromLatLngToPoint(position);
        const center = projection.fromLatLngToPoint(map.getCenter());
        const zoom = map.getZoom();
        const scale = Math.pow(2, zoom);

        // Calculate pixel position relative to map center
        const pixelX = (point.x - center.x) * scale + mapWidth / 2;
        const pixelY = (point.y - center.y) * scale + mapHeight / 2;

        // Determine edge and add to appropriate group
        let edge;
        if (pixelX < 0) { // Left edge
            edge = 'left';
            edgeMarkers.left.push({ index, y: pixelY, config: markerConfig });
        } else if (pixelX > mapWidth) { // Right edge
            edge = 'right';
            edgeMarkers.right.push({ index, y: pixelY, config: markerConfig });
        } else if (pixelY < 0) { // Top edge
            edge = 'top';
            edgeMarkers.top.push({ index, x: pixelX, config: markerConfig });
        } else { // Bottom edge
            edge = 'bottom';
            edgeMarkers.bottom.push({ index, x: pixelX, config: markerConfig });
        }
    });

    // Second pass: position markers on each edge to avoid overlaps
    const edgePadding = 30;
    const indicatorSize = 40; // Width/height of indicator
    const minSpacing = 70; // Minimum pixels between indicators

    // Helper function to create an edge indicator
    function createEdgeIndicator(x, y, markerConfig, index, rotation) {
        const indicator = document.createElement('div');
        indicator.className = 'map-edge-indicator';
        indicator.style.cssText = `
            position: absolute;
            left: ${x}px;
            top: ${y}px;
            width: ${indicatorSize}px;
            height: ${indicatorSize}px;
            margin-left: -${indicatorSize / 2}px;
            margin-top: -${indicatorSize / 2}px;
            background-color: ${markerConfig.background};
            border: 3px solid white;
            box-shadow: 0 2px 6px rgba(0,0,0,0.3);
            border-radius: 50%;
            color: white;
            display: flex;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            z-index: 1; /* Lower than nav bar and map controls */
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        `;

        indicator.innerHTML = `
            <div style="display: flex; flex-direction: column; align-items: center;">
                <i class="fa ${markerConfig.icon}" style="font-size: 16px;"></i>
                <i class="fa fa-chevron-up" style="font-size: 10px; margin-top: 2px; transform: rotate(${rotation}deg);"></i>
            </div>
        `;

        indicator.title = markerConfig.title;

        indicator.addEventListener('mouseenter', () => {
            indicator.style.transform = 'scale(1.2)';
            indicator.style.boxShadow = '0 3px 8px rgba(0,0,0,0.5)';
        });

        indicator.addEventListener('mouseleave', () => {
            indicator.style.transform = 'scale(1)';
            indicator.style.boxShadow = '0 2px 6px rgba(0,0,0,0.3)';
        });

        indicator.addEventListener('click', () => {
            mapGoToMarker(index);
        });

        return indicator;
    }

    const mapDiv = map.getDiv();
    const mapWidth = mapDiv.offsetWidth;
    const mapHeight = mapDiv.offsetHeight;

    // Position top edge indicators
    if (edgeMarkers.top.length > 0) {
        edgeMarkers.top.sort((a, b) => a.x - b.x);

        // Distribute evenly if they would overlap
        if (edgeMarkers.top.length > 1) {
            const availableWidth = mapWidth - 80; // 40px padding on each side
            const idealSpacing = Math.max(minSpacing, availableWidth / edgeMarkers.top.length);

            edgeMarkers.top.forEach((marker, i) => {
                const x = 40 + i * idealSpacing;
                const indicator = createEdgeIndicator(x, edgePadding, marker.config, marker.index, 0);
                mapDiv.appendChild(indicator);
            });
        } else {
            // Just one marker, place at original x position
            const marker = edgeMarkers.top[0];
            const x = Math.min(Math.max(marker.x, 40), mapWidth - 40);
            const indicator = createEdgeIndicator(x, edgePadding, marker.config, marker.index, 0);
            mapDiv.appendChild(indicator);
        }
    }

    // Position bottom edge indicators
    if (edgeMarkers.bottom.length > 0) {
        edgeMarkers.bottom.sort((a, b) => a.x - b.x);

        if (edgeMarkers.bottom.length > 1) {
            const availableWidth = mapWidth - 80;
            const idealSpacing = Math.max(minSpacing, availableWidth / edgeMarkers.bottom.length);

            edgeMarkers.bottom.forEach((marker, i) => {
                const x = 40 + i * idealSpacing;
                const indicator = createEdgeIndicator(x, mapHeight - edgePadding, marker.config, marker.index, 180);
                mapDiv.appendChild(indicator);
            });
        } else {
            const marker = edgeMarkers.bottom[0];
            const x = Math.min(Math.max(marker.x, 40), mapWidth - 40);
            const indicator = createEdgeIndicator(x, mapHeight - edgePadding, marker.config, marker.index, 180);
            mapDiv.appendChild(indicator);
        }
    }

    // Position left edge indicators
    if (edgeMarkers.left.length > 0) {
        edgeMarkers.left.sort((a, b) => a.y - b.y);

        if (edgeMarkers.left.length > 1) {
            const availableHeight = mapHeight - 80;
            const idealSpacing = Math.max(minSpacing, availableHeight / edgeMarkers.left.length);

            edgeMarkers.left.forEach((marker, i) => {
                const y = 40 + i * idealSpacing;
                const indicator = createEdgeIndicator(edgePadding, y, marker.config, marker.index, 270);
                mapDiv.appendChild(indicator);
            });
        } else {
            const marker = edgeMarkers.left[0];
            const y = Math.min(Math.max(marker.y, 40), mapHeight - 40);
            const indicator = createEdgeIndicator(edgePadding, y, marker.config, marker.index, 270);
            mapDiv.appendChild(indicator);
        }
    }

    // Position right edge indicators
    if (edgeMarkers.right.length > 0) {
        edgeMarkers.right.sort((a, b) => a.y - b.y);

        if (edgeMarkers.right.length > 1) {
            const availableHeight = mapHeight - 80;
            const idealSpacing = Math.max(minSpacing, availableHeight / edgeMarkers.right.length);

            edgeMarkers.right.forEach((marker, i) => {
                const y = 40 + i * idealSpacing;
                const indicator = createEdgeIndicator(mapWidth - edgePadding, y, marker.config, marker.index, 90);
                mapDiv.appendChild(indicator);
            });
        } else {
            const marker = edgeMarkers.right[0];
            const y = Math.min(Math.max(marker.y, 40), mapHeight - 40);
            const indicator = createEdgeIndicator(mapWidth - edgePadding, y, marker.config, marker.index, 90);
            mapDiv.appendChild(indicator);
        }
    }
}

// Attach edge indicator update to map events
function addEdgeIndicatorListeners() {
    // Update indicators on map movement
    map.addListener('idle', updateEdgeIndicators);
    map.addListener('zoom_changed', updateEdgeIndicators);
}

// alert_markup
function alert_markup(alert_type, msg) {
    return '<div class="alert alert-' + alert_type + '" role="alert">' + msg + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span>&times;</span></button></div>';
}