$(".test").on("click", function () {
    alert("her");
    $(this).closest("div").append("<div>\
                                            <input type='textbox'><input type='button' value='Reply' id='btnReply'>\
                                        </div>");
});
$("#btnReply").on("click", function () {
    var postId = $(this).closest("div").attr("post-id");
    var parentCommentId = $(this).closest("div").attr("comment-id");
    var comment = $(this).prev("input[type=text]").val();
    alert("hy");    //Insert to Comments list using REST Api call
    var listItem = {
        Title: "your reply title",
        PostTitleId: postId,
        Body: comment,
        ParentCommentId: parentCommentId
    };
});