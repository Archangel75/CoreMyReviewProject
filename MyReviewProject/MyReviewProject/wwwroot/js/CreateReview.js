
//for autocomplete and for checking the proper input in text input id=Objectname.
//(function () {
//    $("[data-autocomplete-source]").each(function () {
//        var target = $(this);
//        target.autocomplete({ source: target.attr("data-autocomplete-source") });
//    });
//});

function checkExistSubject(obj) {
    $.get("/Review/CheckExistSubject", { term: obj.value }, function (data) {
        if (!data.correct) {
            $('#buttoncreate').show();
            if ($('#Objectname').hasClass("valid")) {
                $('#Objectname').removeClass("valid");
            $('#Objectname').addClass("invalid");
            $('#Objectname').attr("aria-label", "Похоже, что никто до вас не создавал опрос на это. Пожалуйста, создайте страницу продукта.");

        }
        else {
            if ($('#Objectname').hasClass("invalid")) {
                $('#Objectname').removeClass("invalid");
            }
            $('#Objectname').addClass("valid");
        }
    });
}                   		

//For Opening and closing ModalReview window.
var modal = document.getElementById('SubjectModal');
            var btn = document.getElementById("CreateSubject");
            var span = document.getElementsByClassName("close")[0];
            var txtbx = document.getElementById("subjName");
            btn.onclick = function () {
                modal.style.display = "block";
                txtbx.value = document.getElementById("Objectname").value;
            }
            span.onclick = function () {
                modal.style.display = "none";
            }
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }

//For stars rating.			
var logID = 'log',
                        log = $('<div id="' + logID + '"></div>');
                    $('body').append(log);
                    $('[type*="radio"]').change(function () {
                        var me = $(this);
                        log.html(me.attr('value'));
                    });			

					
			
	
					
					