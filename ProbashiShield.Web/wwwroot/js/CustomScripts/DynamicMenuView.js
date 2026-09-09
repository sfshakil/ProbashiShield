jQuery(function ($) {

    var parent = $('li.sidebar-parent > ul > li > a.active').closest('li.sidebar-parent');
    if (parent) {
        parent.addClass('active');
        parent.children("ul:first").slideToggle();
    }

    $(".sidebar-parent > a").click(function () {
        $(".sidebar-child").slideUp(200);
        if ($(this).parent().hasClass("active")) {
            $(".sidebar-parent").removeClass("active");
            $(this).parent().removeClass("active");
        }
        else {
            $(".sidebar-parent").removeClass("active");
            $(this).next(".sidebar-child").slideDown(200);
            $(this).parent().addClass("active");
        }
    });

    $(document).ready(function () {
        $('#sidebarCollapse').on('click', function () {
            $('#sidebar').toggleClass('active collapsed-sidebar');
            AdjustDatatable();
            RefreshAllCustomTooltips();
        });
    });
});

function RefreshAllCustomTooltips() {
    DisposeAllCustomTooltips();

    setTimeout(function () {
        $.each($('.custom-tooltip-link'), function (domIndex, domItem) {
            let toolTipItem = new bootstrap.Tooltip($(domItem), {
                trigger: 'manual',
                html: true
            });
            toolTipItem.show();
        });
    }, 1000);
}

function AdjustDatatable() {
    setTimeout(function () {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    }, 750);
}



