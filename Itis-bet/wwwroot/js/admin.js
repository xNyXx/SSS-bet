function toggleMenu() {
    let logo = $('#logo');
    let menu = $('#menu');
    let content = $('#content');
    let className = 'toggled'
    console.log(logo)
    console.log(menu)
    console.log(className)
    if (logo.hasClass(className) && menu.hasClass(className) && content.hasClass(className)) {
        logo.removeClass(className);
        menu.removeClass(className);
        content.removeClass(className);
    }
    else {
        logo.addClass(className);
        menu.addClass(className);
        content.addClass(className);
    }
} 
function getTableItems() {
    let sport = $('#sport-selector').val()
    let hostUrl = window.location.protocol + '//' + window.location.host + '/';
    $.ajax({
        url: hostUrl + "Admin/GetBlogTableItems?sport=" + sport,
        method: "GET",
        success: function (data) {
            $("#table").empty().append(data);
        }
    })
}