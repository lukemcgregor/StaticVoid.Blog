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

    selectedCount = ko.computed(function () {
        var count = 0;
        $.each(self.posts(), function (index, post) {
            if (post.selected()) count++;
        });
        return count;
    });

    self.allSelected = ko.computed(function () {
        return self.posts().length === selectedCount();
    });

    self.restoreButtonText = ko.computed(function () {
        if(selectedCount()===0)
        {
            return 'Restore no posts';
        }
        else if (selectedCount() === 1) {
            return 'Restore 1 post';
        }
        else {
            return 'Restore ' + selectedCount() + ' posts';
        }
    });

    self.clickSelectAll = function () {
        if (self.posts().length === selectedCount()) {
            $.each(self.posts(), function (index, post) {
                post.selected(false);
            });
        }
        else {
            $.each(self.posts(), function (index, post) {
                post.selected(true);
            });
        }
        return false;
    };

    self.restoreSelected = function () {
        var selectedGuids=  new Array();

        $.each(self.posts(), function (index, post) {
            if (post.selected()) selectedGuids.push(post.PostGuid());
        });

        $.ajax({
            url: 'recovery/restore-posts/' + self.correlationToken(),
            type: 'POST',
            data: JSON.stringify(selectedGuids),
            contentType: 'application/json'
        }).done(function (data) {
            self.cancel();
        });
        return false;
    };

    var initialise = function () {
        $('.drop-upload').fileupload({
            dropZone: $('.drop-zone'),
            add: function (e, data) {
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

    self.selected = ko.observable(false);
    self.toggleSelected = function () {
        self.selected(!self.selected());
        return false;
    };

    return self;
};

$(function () {
    ko.applyBindings(new restoreViewModel(), $('#restore-section')[0]);
});