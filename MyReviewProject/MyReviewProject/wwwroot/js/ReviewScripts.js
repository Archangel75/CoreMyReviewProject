$(document).ready(function () {
});

window.onload = function () {
    $(".comment-content").each(function () {
        if (this.id != "undefined" && this.id != "") {
            var id = this.id;
            var text = $("#" + id).text();
            var parsedtext = $.parseHTML(text);
            $("#" + id).html(parsedtext);
        }
    });
}


$(".submitcomment").click(function () {
    var comment = $(".leavecomment").html();
    if (comment.length > 0) {
        if ($("#wrongcomment").css("display") == "block")
            $("#wrongcomment").css("display", "none");
        var id = $("#answerto").text();
        if (typeof id == 'undefined' || id == "")
            id = -1;
        var ReviewId = $("#hidId").val();
        $.post("/Review/PostComment", { comment: comment, id: id, ReviewId: ReviewId }, function (success) {
            if (success > 0) {
                $(".leavecomment").empty();
            }            
        });
    }
    else {
        $("#wrongcomment").css("display","block");
    }    
});

$("i").click(function () {
    var ElemId = $(this).attr('id');
    if ($(this).attr("class") == "fa fa-reply") {
        if ($(".leavecomment").html().indexOf("<blockquote>") < 0) {
            $(this).css("color", "var(--errorpagelink)");
            var quote = $("#mydiv" + ElemId).text();
            var user = $("#box" + ElemId).find("a").text();
            quote = $.parseHTML("<div class='blockquote' id='quote" + ElemId + "'><a>" + user + "</a>" + quote + "</div> <br>");
            $("#answerto").append(ElemId);
            $(".leavecomment").append(quote);
            $(".leavecomment").append("<br>");
        }
    }
    else {
        var id = "isLiked" + ElemId.toString()
        var isLiked = $("#" + id).val()
        if (!isLiked) {
            $(this).css("color", "var(--errorpagelink)");
            $.post("/Review/PostLike", { id: ElemId }, function (success) {
            });
        }
        else {
            $(this).css("color", "var(--darkazure)");
            $.post("/Review/RemoveLike", { id: ElemId }, function (success) {
            });
        }
    }
});