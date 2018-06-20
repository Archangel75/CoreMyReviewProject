
//дропает
$(".dropdownCat dt a").on("click", function() {
    $(".dropdownCat dd ul").slideToggle("fast");
});
//прячет
$(".dropdownCat dd ul li a").on("click", function() {
    $(".dropdownCat dd ul").hide();
});
//получает значение
function getSelectedCatValue(id) {
  return $("#" + id)
    .find(".dropdownCat dt a span.value")
    .html();
}

$(document).bind("click", function(e) {
  var $clicked = $(e.target);
  if (!$clicked.parents().hasClass("dropdownCat")) $(".dropdownCat dd div ul").hide();  
});

//Categories
$('.mutliSelectCat input').on('click', function () {
        var title = $(this).closest('.mutliSelectCat').find('input').val(),
        title = $(this).val();
        $.get("/Review/GetSubCategories", { catname: title }, function (data) {
            $("#subCat").html(data.result);
            $(".mutliSelectSub").delegate('input', 'click', function () {
                var title5 = $(this).closest('.mutliSelectSub').find('input').val(),
                    title5 = $(this).val();
                $('.multiSelSub span').remove();
                var html5 = '<span title="' + title5 + '">' + title5 + '</span>';
                $('.multiSelSub').append(html5);
                $(".hidaSub").hide();
            });
        });
        $('.multiSelCat span').remove();
        var html = '<span title="' + title + '">' + title + '</span>';
        $('.multiSelCat').append(html);
        $(".hidaCat").hide();
        
});

//SubCategories
//дропает
$(".dropdownSub dt a").on("click", function () {
    $(".dropdownSub dd ul").slideToggle("fast");
});
//прячет
$(".dropdownSub dd ul li a").on("click", function () {
    $(".dropdownSub dd ul").hide();
});
//получает значение
function getSelectedSubValue(id) {
    return $("#" + id)
        .find(".dropdownSub dt a span.value")
        .html();
}
//прячет когда кликаем в сторону
$(document).bind("click", function (e) {
    var $clicked = $(e.target);
    if (!$clicked.parents().hasClass("dropdownSub") ) $(".dropdownSub dd div ul").hide();
});

//SubCategories
$('.mutliSelectSub input').on('click', function () {
    var title2 = $(this).closest('.mutliSelectSub').find('input').val(),
        title2 = $(this).val();
    $('.multiSelSub span').remove();
    var html2 = '<span title="' + title2 + '">' + title2 + '</span>';
    $('.multiSelSub').append(html2);
    $(".hidaSub").hide();
});

$("#createNewSubject").click(function () {
    var subCatName = document.getElementsByClassName('multiSelSub')[0];
    var subjName = document.getElementById('subjName');
    $.post("/Review/CreateSubject", { subcatname: subCatName.textContent, subjname: subjName.value }, function () {
        var modal = document.getElementById('SubjectModal');
        modal.style.display = "none";
        $('#Objectname').value = subjName.value;
        $('#Objectname').removeClass('invalid');
        $('#Objectname').addClass('valid');

    });
});