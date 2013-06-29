function AppViewModel(options) {
    var self;

    var mappingOptions = {
        'Posts': {
            create: function (options) {
                return (new (function () {
                    this.cssClass = ko.computed(function () {
                        if (this.Item2() == self.SelectedPost.Id()) {
                            return 'active';
                        }
                        return '';
                    }, this);

                    ko.mapping.fromJS(options.data, {}, this);
                })());
            }
        }
    };

    self = ko.mapping.fromJS(options.data, mappingOptions);

    self.selectPost = function () {
        $('.post-detail-contents').hide();
        $('.post-detail-loader').show();

        $.getJSON(options.postDetailUrl + '/' + this.Item2(), function (data) {
            //TODO: Must be a better way of remapping new data
            //Have tried ko.mapping.fromJS(data, self.SelectedPost);
            self.SelectedPost.Id(data.Id);
            self.SelectedPost.Title(data.Title);
            self.SelectedPost.Url(data.Url);
            self.SelectedPost.Description(data.Description);
            self.SelectedPost.HasDraftContent(data.HasDraftContent);
            self.SelectedPost.Status(data.Status);
            self.SelectedPost.PublishedDate(data.PublishedDate);
            self.SelectedPost.LastModified(data.LastModified);

            BindClicks();
            $('.post-detail-loader').hide();
            $('.post-detail-contents').show();
        });
    };
    return self;
}

$(function () {
    globalNav.select('/manage');
    globalNav.menuItems()[0].selected(true);
});