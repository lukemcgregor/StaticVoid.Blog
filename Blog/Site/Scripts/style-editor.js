function StyleEditorViewModel(options) {
    var self = {};

    self.css = ko.observable(options.data.css);
    self.html = ko.observable(options.data.html);

    self.templateMode = ko.observable(templateModes.noDomCustomisation);

    self.noscriptMetaRefreshSelected = ko.computed(function () {
        return self.templateMode() === templateModes.noscriptMetaRefresh;
    });
    self.selectNoscriptMetaRefresh = function () {
        //TODO: Not Implemented yet
        //self.templateMode(templateModes.noscriptMetaRefresh);
        return false;
    };

    self.domRipSelected = ko.computed(function () {
        return self.templateMode() === templateModes.domRip;
    });
    self.selectDomRip = function () {
        //TODO: Not Implemented yet
        //self.templateMode(templateModes.domRip);
        return false;
    };

    self.bodyOnlySelected = ko.computed(function () {
        return self.templateMode() === templateModes.bodyOnly;
    });
    self.selectBodyOnly = function () {
        //TODO: Not Implemented yet
        //self.templateMode(templateModes.bodyOnly);
        return false;
    };

    self.noNojsFallbackSelected = ko.computed(function () {
        return self.templateMode() === templateModes.noNojsFallback;
    });
    self.selectNoNojsFallback = function () {
        //TODO: Not Implemented yet
        //self.templateMode(templateModes.noNojsFallback);
        return false;
    };

    self.noDomCustomisationSelected = ko.computed(function () {
        return self.templateMode() === templateModes.noDomCustomisation;
    });
    self.selectNoDomCustomisation = function () {
        self.templateMode(templateModes.noDomCustomisation);
        return false;
    };

    self.htmlEditable = ko.computed(function () {
        return self.templateMode() !== templateModes.noDomCustomisation;
    });

    self.containerWidth = ko.observable(0);
    self.containerHeight = ko.observable(0);

    self.editorWidth = ko.computed(function () {
        return self.containerWidth() + 'px';
    });

    self.editorHeight = ko.computed(function () {
        return self.containerHeight() + 'px';
    });

    self.container = $('#style-editor');

    var lastAutosave = {
        time: Date.now(),
        css: self.css(),
        html: self.html(),
        templateMode: self.templateMode()
    };

    var lastInteraction = Date.now();

    var now = ko.observable(Date.now());

    var tick = function () {
        now(Date.now());
    };

    var isTypingTimeout = 5000;
    self.isTyping = ko.computed(function (){
        return now() - self.lastInteraction < isTypingTimeout;
    });

    self.autosave = ko.observable(true);

    var autosaveTimeout = 5000;
    ko.computed(function () {
        if (self.autosave() && !self.isTyping() && now() - lastAutosave.time > autosaveTimeout)
        {
            if (lastAutosave.css != self.css() ||
                lastAutosave.html != self.html() ||
                lastAutosave.templateMode != self.templateMode()) {
                //autosave
                $.ajax({
                    url: '../save-blog-template',
                    type: 'POST',
                    data: blogTemplateModel(),
                    contentType: 'application/json'
                }).done(function (data) {
                    if (data.success) {
                        lastAutosave.css = self.css();
                        lastAutosave.html = self.html();
                        lastAutosave.templateMode = self.templateMode();
                    }
                });

            }
            lastAutosave.time = Date.now();
        }
    });

    self.update = function () {
        self.containerWidth(self.container.width());
        self.containerHeight(Math.max(window.innerHeight - 160, 600));
    };

    var blogTemplateModel = function () {
        return JSON.stringify({
            Css: self.css(),
            HtmlTemplate: self.html(),
            TemplateMode: self.templateMode()
        });
    };

    self.applyTemplate = function () {
        $.ajax({
            url: '../apply-blog-template',
            type: 'POST',
            data: blogTemplateModel(),
            contentType: 'application/json'
        });
    };

    self.load = function () {
        var cssEditor = ace.edit("css-editor");
        cssEditor.setTheme("ace/theme/twilight");
        cssEditor.getSession().setMode("ace/mode/less");
        cssEditor.setValue(self.css());
        cssEditor.getSession().on('change', function (e) {
            self.css(cssEditor.getValue());
        });
        //var htmlEditor = ace.edit("html-template-editor");
        //htmlEditor.setTheme("ace/theme/twilight");
        //htmlEditor.getSession().setMode("ace/mode/html");

        $(window).resize(function () {
            self.update();
        });
        self.update();

    };

    //kick off the autosave
    setInterval(tick, 1000);
    return self;
}

