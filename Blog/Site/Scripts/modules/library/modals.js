define(
    [
        'jquery',
        'bootstrap'
    ],
    function ($) {
        console.debug('modals setup');

        var bindForm = function (dialog, modalId, refresh) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#' + modalId + '-modal').modal('hide');
                            // Refresh:
                            if (refresh) {
                                location.reload();
                            }
                        } else {
                            $('#' + modalId + '-container').html(result);
                            bindForm();
                        }
                    }
                });
                return false;
            });
        };

        var setupModal = function (modalId, refresh, loadFunctionName) {
            $('.' + modalId).click(function () {
                $('#' + modalId + '-container').load(this.href, function () {
                    $('#' + modalId + '-modal').modal({
                        backdrop: 'static',
                        keyboard: true
                    }, 'show');
                    bindForm(this, modalId, refresh);
                    if (typeof (window[loadFunctionName]) === 'function') {
                        window[loadFunctionName]();
                    }
                });
                return false;
            });

            currentModals.push({ modalId: modalId, refresh: refresh, loadFunctionName: loadFunctionName });
        };

        var currentModals = [];

        var res = {
            setupModal: setupModal,
            reBindClicks: function(){
                $.each(currentModals, function(index, modal){
                    setupModal(modal.modalId, modal.refresh, modal.loadFunctionName);
                });
            }
        };

        return res;
    }
);