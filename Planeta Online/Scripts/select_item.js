$(document).ready(function () {
    var currentPageAdress = window.location.href;
    var requestPath = currentPageAdress.substr(currentPageAdress.indexOf('/', 9));

    if (requestPath.includes('Administrator')) {
        $('#admin').addClass('active')
    }
    else if (requestPath == '/' || requestPath.includes('Events')) {
        $('#events').addClass('active');
    }
    else if (requestPath.includes('Books')) {
        $('#books').addClass('active')
    }
    else if (requestPath.includes('Home/About')) {
        $('#about').addClass('active')
    }
    
    else if (requestPath.includes('Blog')) {
        $('#blog').addClass('active')
    }
});