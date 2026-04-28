$('.day, .month, .year').on('paste', function (e) {
    var pastedData = "";
    if ((e.originalEvent || e).clipboardData) {
        pastedData = (e.originalEvent || e).clipboardData.getData('text/plain');
    } else if (window.clipboardData) {
        pastedData = window.clipboardData.getData('Text');
    }

    var splitData = pastedData.match(/(\d{1,2})[\.\-\/ ](\d{1,2})[\.\-\/ ](\d{2,4})/);
    if (splitData) {
        var dateContainer = $(this).parents('.date-container');
        if (dateContainer != null) {
            dateContainer.find('.day').val(splitData[1]);
            dateContainer.find('.month').val(splitData[2]);
            dateContainer.find('.year').val(splitData[3]);
            e.preventDefault();
        }
    }
});