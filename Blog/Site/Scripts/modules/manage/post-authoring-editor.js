define(
    [
        'jquery',
        'ba-resize',
        'markdown-deep-editor-ui'
    ], function ($) {
        var resizeEditor = function() {
            var possibleHeight = (window.innerHeight - ($('body').height() - $('#editor-row').height())) - 50;
            if (possibleHeight < 200) {
                possibleHeight = 200;
            }

            $('#editor-row').height(possibleHeight);
            $('#editor-preview').height(possibleHeight);
        }

        return {
            onStart: function() {
                $(window).resize(function (event) {
                    resizeEditor();
                });

                $('#editor-preview').resize(function () {
                    resizeEditor();
                });

                $("textarea.mdd_editor").MarkdownDeep({
                    help_location: "/Content/mdd_help.htm",
                    ExtraMode: true,
                    resizebar: false
                });
                resizeEditor();
            }
        };
    }
);