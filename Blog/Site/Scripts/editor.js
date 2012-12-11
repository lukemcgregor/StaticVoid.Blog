$(function () {
    $(window).resize(function (event) {
        ResizeEditor();
    });

    $('#editor-preview').resize(function () {
        ResizeEditor();
    });

    $("textarea.mdd_editor").MarkdownDeep({
        help_location: "/Content/mdd_help.htm",
        ExtraMode: true,
        resizebar: false
    });
    ResizeEditor();
});
function ResizeEditor() {
    var possibleHeight = (window.innerHeight - ($('body').height() - $('#editor-row').height())) -50;
    if (possibleHeight < 200) {
        possibleHeight = 200;
    }

    //if (possibleHeight < $('#editor-preview').height())
    //    possibleHeight = $('#editor-preview').height();

    $('#editor-row').height(possibleHeight);
    $('#editor-preview').height(possibleHeight);
}