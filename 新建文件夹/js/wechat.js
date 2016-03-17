// Initialize your app
var myApp = new Framework7({
    modalTitle: '卡得万利商业保理',
    ajaxLinks: 'a.ajax'
});

// Export selectors engine
var $$ = Dom7;

// Add view
var mainView = myApp.addView('.view-main', {
    // Because we use fixed-through navbar we can enable dynamic navbar
    dynamicNavbar: true
});

// Callbacks to run specific code for specific pages, for example for About page:
myApp.onPageInit('about', function (page) {
    // run createContentPage func after link was clicked
    $$('.create-page').on('click', function () {
        createContentPage();
    });
});

// Loading flag
var loading = false;

// Last loaded index
var lastIndex = $$('.list-block li').length;

// Max items to load
var maxItems = 60;

// Append items per load
var itemsPerLoad = 10;

// Attach 'infinite' event handler
$$('.infinite-scroll').on('infinite', function () {

    // Exit, if loading in progress
    if (loading) return;

    // Set loading flag
    loading = true;

    // Emulate 1s loading
    setTimeout(function () {
        // Reset loading flag
        loading = false;

        if (lastIndex >= maxItems) {
            // Nothing more to load, detach infinite scroll events to prevent unnecessary loadings
            myApp.detachInfiniteScroll($$('.infinite-scroll'));
            // Remove preloader
            $$('.infinite-scroll-preloader').remove();
            return;
        }

        // Generate new items HTML
        var html = '';
        for (var i = lastIndex + 1; i <= lastIndex + itemsPerLoad; i++) {
            html += '<div class="item-content">'
            html += '<div class="item-inner shop-inner">'
            html += '<div class="item-title-row">'
            html += '<div class="item-title refund-title">'
            html += '<span class="glyphicon glyphicon-calendar glyphicon-date" aria-hidden="true"></span>'
            html += '<span class="">清算日期：2015-05-22</span>'
            html += '</div>'
            html += '</div>'
            html += '<div class="item-text shop-subtitle">'
            html += '<div class="refund-subtext">'
            html += '<label class="refund-subtext-label">清算金额：</label>'
            html += '<span>6955.3</span>'
            html += '</div>'
            html += '<div class="refund-subtext">'
            html += '<label class="refund-subtext-label">账户管理费：</label>'
            html += '<span>6955.3</span>'
            html += '</div>'
            html += '<div class="refund-subtext">'
            html += '<label class="refund-subtext-label">实还日期：</label>'
            html += '<span>6955.3</span>'
            html += '</div>'
            html += '<div class="refund-subtext">'
            html += '<label class="refund-subtext-label">实还金额：</label>'
            html += '<span>6955.3</span>'
            html += '</div>'
            html += '<div class="refund-subtext">'
            html += '<label class="refund-subtext-label">卡得万利付款：</label>'
            html += '<span>6955.3</span>'
            html += '</div>'
            html += '</div>'
            html += '</div>'
            html += '</div>'
        }

        // Append new items
        $$('.list-block ul').append(html);

        // Update last loaded index
        lastIndex = $$('.list-block li').length;
    }, 1000);
});