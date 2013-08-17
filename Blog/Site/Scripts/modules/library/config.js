var require = {
    baseUrl: "/scripts/modules",
    paths: {
        "library": "library",
        "admin": "admin",
        "manage": "manage",
        "knockout": "//cdnjs.cloudflare.com/ajax/libs/knockout/2.2.1/knockout-min",
        "knockout-mapping": "//cdnjs.cloudflare.com/ajax/libs/knockout.mapping/2.3.5/knockout.mapping",
        "jquery": "//cdnjs.cloudflare.com/ajax/libs/jquery/1.10.1/jquery.min",
        "jquery-ui": "http://code.jquery.com/ui/1.10.3/jquery-ui",
        "bootstrap": "//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min",
        "prettify": "//cdnjs.cloudflare.com/ajax/libs/prettify/r298/prettify",
        "modernizr": "//cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min",
        "knockout-validation": "//cdnjs.cloudflare.com/ajax/libs/knockout-validation/1.0.2/knockout.validation.min",
        "ace": "http://d1n0x3qji82z53.cloudfront.net/src-min-noconflict/ace",
        "ba-resize": "//cdnjs.cloudflare.com/ajax/libs/jquery-resize/1.1/jquery.ba-resize.min",
        "markdown-deep": "/Scripts/MarkdownDeep",
        "markdown-deep-editor": "/Scripts/MarkdownDeepEditor",
        "markdown-deep-editor-ui": "/Scripts/MarkdownDeepEditorUI",
    },
    shim: {
        "bootstrap": ['jquery'],
        "ba-resize": ['jquery'],
        "markdown-deep": ['jquery'],
        "markdown-deep-editor": ['markdown-deep'],
        "markdown-deep-editor-ui": ['markdown-deep-editor']
    },
    deps: ['knockout', 'knockout-mapping'],
    callback: function (ko, mapping) {
        console.debug('KO Mapping loaded');
        ko.mapping = mapping;
    }
};