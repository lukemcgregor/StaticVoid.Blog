define(
    [
        'jquery',
        'knockout',
        'admin/global-nav'
    ], function ($, ko, globalNav)
    {
        console.debug('dashboard');

        var mappingOptions = {
            create: function (options) {
                var vm = new postViewModel();

                ko.mapping.fromJS(options.data, { 'copy': ["parent"] }, vm);
                return vm;
            }
        };

        var postViewModel = function () {
            var self = this;

            self.PostId = ko.observable();
            self.Title = ko.observable();
            self.selected = ko.observable(false);
            self.selectPost = function () {
                $.each(self.parent.Posts(), function (index, item) {
                    item.selected(false);
                });
                self.selected(true);

                self.parent.isLoadingPost(true);
                $.getJSON(self.parent.postDetailUrl() + '/' + self.PostId(), function (data) {
                    ko.mapping.fromJS(data, {}, self.parent.SelectedPost);

                    self.parent.isLoadingPost(false);
                });
            };
        };

        globalNav.select('/manage');
        return function () {
            var self = this;

            self.isLoadingPost = ko.observable(false);
            self.Posts = ko.observableArray();
            self.postDetailUrl = ko.observable();

            self.updatePosts = function (data) {
                $.each(data.Posts, function (index, post) {
                    post.parent = self;
                });

                ko.mapping.fromJS(data.Posts, mappingOptions, self.Posts);

                if (self.Posts().length > 0) {
                    self.Posts()[0].selectPost();
                }
            };

            self.SelectedPost = {
                Id: ko.observable(),
                Title: ko.observable(),
                Url: ko.observable(),
                Description: ko.observable(),
                HasDraftContent: ko.observable(),
                Status: ko.observable(),
                PublishedDate: ko.observable(),
                LastModified: ko.observable(),
                friendlyPublishedDate: ko.computed(function () {
                    if (!self.SelectedPost || !self.SelectedPost.PublishedDate()) {
                        return '';
                    }
                    return new Date(parseInt(self.SelectedPost.PublishedDate().substr(6))).toDateString();
                }),
                friendlyLastModified: ko.computed(function () {
                    if (!self.SelectedPost || !self.SelectedPost.LastModified()) {
                        return '';
                    }
                    return new Date(parseInt(self.SelectedPost.LastModified().substr(6))).toDateString();
                })
            };
        }
    }
);