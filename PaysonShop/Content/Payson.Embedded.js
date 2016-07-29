window.checkout = (function ($) {
    var initiated = false;
    var checkoutContainer;
    var checkoutIframe;
    var embeddedUrl;

    function updateContainerHeight() {
        checkoutContainer.animate({ height: event.data }, 'fast');
    }

    var checkout = {
        initiate: function(checkoutContainerId) {
            this.checkoutContainer = $(checkoutContainerId);
            this.checkoutIframe = checkoutContainer.find("iframe");

            if (!checkoutIframe) {
                alert("Unable to find checkout iframe. Make sure the div referenced contains the checkout iframe.");

                initiated = false;

                return;
            }

            var iframeSrc = checkoutIframe.attr('src');
            var last = iframeSrc.lastIndexOf('/');
            this.embeddedUrl = iframeSrc.substr(0, last);
            
            window.addEventListener("message", updateContainerHeight, false);

            initiated = true;
        },
        updateContainerHeight: function () {
            if (initiated) {
                updateContainerHeight();
            } else {
                alert('Need to initiate the checkout before you can update its height.');
            }
        },
        updateAmount: function () {
            if (initiated) {
                checkoutIframe[0].contentWindow.postMessage('updatePage', embeddedUrl);
            } else {
                alert('Need to initiate the checkout before you can refresh it.');
            }
        }
    };
     
    return checkout;
}(jQuery));