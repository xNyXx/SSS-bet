$(document).ready(function () {
    $('.select').select2({
        placeholder: "asds",
    });

    var url_string = window.location.href; //window.location.href
    var url = new URL(url_string);
    var c = url.searchParams.get("vk");
    if(c=="1") {
        $.notify({
            // options
            message: 'Success!'
        }, {
            // settings
            type: 'success'
        });
    }
    if(c=="2") {
        $.notify({
            // options
            message: 'Account doesn\'t exist!'
        }, {
            // settings
            type: 'danger'
        });
    }
    if(c=="3") {
        $.notify({
            // options
            message: 'Error occured, try againg later!'
        }, {
            // settings
            type: 'danger'
        });
    }


    $('.sport-table-wager-button').on('click', function () {
        var teamName = $(this).data('team-name'),
            confrontation = $(this).data('confrontation'),
            vager = $(this).data('wager-count'),
            score = $(this).data('score');
        matchId = $(this).data('match');

        $('.modal-sport-error').hide()
        $('.modal-sport-wager').html(teamName);
        $('.modal-sport-wager-count').html(vager);
        $('#bet-match').val(matchId);
        $('.modal-sport-confrontation').html(confrontation);
        $('.modal-sport-live-count').html('[' + score + ']');
        $('#bet-sum').val(10);
        $('.modal-sport-win #possible-win').html("$" + parseFloat($('#bet-sum').val()) * parseFloat($('.modal-sport-wager-count').text()));

    })


    $('#bet-sum').on("keyup change", function (e) {
        var sum = $(this).val();
        if (!sum) $('.modal-sport-win').hide();
        else {
            $('.modal-sport-win').show();
            $('.modal-sport-win #possible-win').html("$" + parseFloat(sum) * parseFloat($('.modal-sport-wager-count').text()));
        }
    });

    $('.modal-sport-place').click(function (e) {
        e.preventDefault();

        var sum = $('#bet-sum').val();
        if (!sum) {
            $('#bet-sum').addClass('is-invalid');
            return;
        }
        $('#bet-sum').removeClass('is-invalid');
        $('.modal-sport-error').hide()
        $.ajax({
            method: "post",
            url: "/Bet",
            data: {
                id: $('#bet-match').val(),
                sum: sum,
                bet: $('.modal-sport-wager').text()
            },
            dataType: 'json',
            beforeSend: function () {
                $('.modal-sport-place').prop('disabled', true).text('Loading...');

            },
            complete: function () {
                $('.modal-sport-place').prop('disabled', false).text('Place bet');
            },
            success: function (data) {
                if (data.status == 'success') {
                    $('.modal').modal('hide')
                    $.notify({
                        // options
                        message: 'Your bet applied!'
                    }, {
                        // settings
                        type: 'success'
                    });
                } else {
                    $('.modal-sport-error').show().text(data.message)
                }
            }
        });
    });

});

function coloriseSignInIcon(iconClass, textClass) {
    $('#sign-in-icon').removeClass(textClass);
    $('#sign-in-text').removeClass(iconClass);
    $('#sign-in-icon').addClass(iconClass);
    $('#sign-in-text').addClass(textClass);
}

window.addEventListener('scroll', function () {
    elem = this.document.getElementById('fixibleHeader');
    y = elem.getBoundingClientRect().top;
    if (y == 0 && pageYOffset < 130) {
        $('#fixibleHeader').removeClass('header-fixed')
    }
    if (y <= 0 && this.pageYOffset > 130) {
        $('#fixibleHeader').addClass('header-fixed')
    }
});
document.getElementById('navbarToggler').addEventListener('click', function () {
    if ($('#navbarToggler').hasClass('active')) {
        $('#navbarToggler').removeClass('active');
        $('#headerMain').removeClass('active');
        $('#navbar-main-container').removeClass('active');
        $('#navbar-main-top').removeClass('active');
        $('#fixibleHeader').removeClass('active');
    } else {
        $('#navbarCollapseToggler').removeClass('active');
        $('#headerRight').removeClass('active');
        $('#navbarToggler').addClass('active');
        $('#headerMain').addClass('active');
        $('#navbar-main-container').addClass('active');
        $('#navbar-main-top').addClass('active');
        $('#fixibleHeader').addClass('active');
    }
});
document.getElementById('navbarCollapseToggler').addEventListener('click', function () {
    if ($('#navbarCollapseToggler').hasClass('active')) {
        $('#navbarCollapseToggler').removeClass('active');
        $('#headerRight').removeClass('active');

    } else {
        $('#navbarToggler').removeClass('active');
        $('#headerMain').removeClass('active');
        $('#navbar-main-container').removeClass('active');
        $('#navbar-main-top').removeClass('active');
        $('#fixibleHeader').removeClass('active');
        $('#navbarCollapseToggler').addClass('active');
        $('#headerRight').addClass('active');

    }
});

function switchLogReg() {
    let hostUrl = window.location.protocol + '//' + window.location.host + '/';
    if ($('#logRegSwitch').data("currentpage") == "reg") {
        $.ajax({
            url: hostUrl + "RegLog/GetLogPartial",
            method: "GET",
            success: function (data) {
                console.log(hostUrl + "RegLog/GetLogPartial")
                $('#regLogWraper').empty().append(data);
            }
        })
        return

    }
    if ($('#logRegSwitch').data("currentpage") == "log") {
        $.ajax({
            url: hostUrl + "RegLog/GetRegPartial",
            method: "GET",
            success: function (data) {
                console.log(hostUrl + "RegLog/GetRegPartial")
                $('#regLogWraper').empty().append(data);
            }
        })
        return
    }
}

function checkCustomCheckBox() {
    if ($('#rememberMeCheckBox').hasClass('checked')) {
        $('#hiddenCheckbox').prop('checked', true);
        $('#rememberMeCheckBox').removeClass('checked')
        console.log($('#hiddenCheckbox').is(':checked'))
    } else {
        $('#hiddenCheckbox').prop('checked', false);
        $('#rememberMeCheckBox').addClass('checked');
        console.log($('#hiddenCheckbox').is(':checked'))
    }
}

/*document.getElementById('rememberMeCheckBox').addEventListener('click', function () { checkCustomCheckBox() });
document.getElementById('rememberMeText').addEventListener('click', function () { checkCustomCheckBox() });
*/
