var restoreViewModel = function () {
    var self = {};

    self.upload = function () {
        $('.drop-zone').find('input').click();
    };
    self.correlationToken = ko.observable('');

    self.dataLoaded = ko.observable(0);
    self.dataTotal = ko.observable(0);
    self.percentageLoaded = ko.computed(function () {
        return parseInt(self.dataLoaded() / self.dataTotal() * 100);
    });

    self.loading = ko.computed(function () {
        return self.dataLoaded() != self.dataTotal();
    });

    self.loadComplete = ko.observable(false);

    self.posts = ko.observableArray();

    self.cancel = function () {
        self.loadComplete(false);
    };

    var initialise = function () {
        $('.drop-upload').fileupload({
            dropZone: $('.drop-zone'),
            add: function (e, data) {
                debugger;
                self.correlationToken(newGuid());
                self.dataLoaded(data.loaded);
                self.dataTotal(data.total);
                var jqXHR = data.submit();
            },
            progress: function (e, data) {
                self.dataLoaded(data.loaded);
                self.dataTotal(data.total);
            },
            fail: function (e, data) {
                // Something has gone wrong!
                data.context.addClass('error');
            },
            dataType: 'json',
            done: function (e, data) {
                self.posts([]);
                $.each(data.result.posts, function (index, post) {
                    self.posts.push(new postViewModel({ data: post }));
                });
                self.loadComplete(true);
            }
        });

        // Prevent the default action when a file is dropped on the window
        $(document).on('drop dragover', function (e) {
            e.preventDefault();
        });
    };

    initialise();//kickoff

    return self;
};

var postViewModel = function (options) {
    var self = ko.mapping.fromJS(options.data, {}, this);

    return self;
};

$(function () {
    ko.applyBindings(new restoreViewModel(), $('#restore-section')[0]);
});