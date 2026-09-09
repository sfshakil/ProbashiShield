var loadingPanel = '<div class="justify-content-center text-center common-loading-panel"><i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span></div>';
var loadingPanelLarge = `<div class="d-flex large-loading-panel">
	                        <div class="d-flex flex-grow-1 justify-content-center align-items-center">
		                        <i class="fa fa-spinner fa-spin fa-3x fa-fw"></i>
	                        </div>    
                        </div>`;

var noDataSuccessPanel = '<div class="mt-2 justify-content-center text-center"><span>No data found</span></div>';
var noDataErrorPanel = '<div class="justify-content-center text-center"><span>No data found.</span></div>';

function DisposeAllCustomTooltips() {
    let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"].custom-tooltip-link'));
    let tooltipInstances = tooltipTriggerList.map(el => bootstrap.Tooltip.getInstance(el));
    tooltipInstances.forEach(instance => {
        if (instance) {
            instance.dispose();
        }
    });

    $.each($('.custom-tooltip'), function (domIndex, domItem) {
        $(domItem).remove();
    });
}

function GetArrayBuffer(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
}

function AddCustomSearchOptionToDataTable(tableId, dtApi, funcName) {
    let input = $('div.dt-search input').unbind();
    $searchButton = $('<button class="btn btn-sm btn-success DtExtraBtn">')
        .text('Search')
        .click(function () {
            dtApi.search(input.val()).draw();
        });
    $reloadButton = $('<button class="btn btn-sm btn-primary DtExtraBtn">')
        .text('Reload')
        .click(function () {
            funcName();
        });
    $('div.dt-search').append($searchButton, $reloadButton);

    $("#" + tableId).wrap("<div class='dtCustomWrapper'></div>");
}

function AddCustomSearchOptionToDataTableForUploadFeedback(divId, tableId, dtApi, funcName, dropdownOptions) {
    let input = $(divId).find('div.dt-search input').unbind();
    input.attr('placeholder', 'CIF or Feedback Type');
    input.attr('id', 'uploadFeedbackSearchInput');

    let $container = $('<div id ="uploadFeedbackContainer" class="d-flex align-items-center gap-2 my-2"></div>');

    let $dropdown = $('<select id="customSearchFeedbackDropdown" class="form-select form-select-sm"></select>');

    dropdownOptions.forEach(function (option) {
        let $option = $('<option></option>')
            .attr('value', option.value)
            .text(option.text);

        if (option.selected) {
            $option.attr('selected', 'selected');
        }

        $dropdown.append($option);
    });

    let $searchButton = $('<button class="btn btn-sm btn-success DtExtraBtn">')
        .text('Search')
        .click(function () {
            let searchText = input.val();
            let selectedFilter = $('#customSearchFeedbackDropdown').val();
            dtApi.settings()[0].customSearchText = searchText;
            dtApi.settings()[0].customSearchFilter = selectedFilter;
            dtApi.draw();
        });
    let $reloadButton = $('<button class="btn btn-sm btn-primary DtExtraBtn">')
        .text('Reload')
        .click(function () {
            funcName();
        });
    $container.append(input, $dropdown, $searchButton, $reloadButton);
    $(divId).find('div.dt-search').html($container);
    $("#" + tableId).wrap("<div class='dtCustomWrapper'></div>");
}

function AddCustomSearchOptionToDataTableForUploadOfferLog(divId, tableId, dtApi, funcName, dropdownOptions) {
    let input = $(divId).find('div.dt-search input').unbind();
    input.attr('placeholder', 'CIF or Product Name');
    input.attr('id', 'uploadOfferLogSearchInput');

    let $container = $('<div id ="uploadOfferLogContainer" class="d-flex align-items-center gap-2 my-2"></div>');

    let $dropdown = $('<select id="customSearchOfferLogDropdown" class="form-select form-select-sm"></select>');

    dropdownOptions.forEach(function (option) {
        let $option = $('<option></option>')
            .attr('value', option.value)
            .text(option.text);

        if (option.selected) {
            $option.attr('selected', 'selected');
        }

        $dropdown.append($option);
    });

    let $searchButton = $('<button class="btn btn-sm btn-success DtExtraBtn">')
        .text('Search')
        .click(function () {
            let searchText = input.val();
            let selectedFilter = $('#customSearchOfferLogDropdown').val();
            dtApi.settings()[0].customSearchText = searchText;
            dtApi.settings()[0].customSearchFilter = selectedFilter;
            dtApi.draw();
        });
    let $reloadButton = $('<button class="btn btn-sm btn-primary DtExtraBtn">')
        .text('Reload')
        .click(function () {
            funcName();
        });
    $container.append(input, $dropdown, $searchButton, $reloadButton);
    $(divId).find('div.dt-search').html($container);
    $("#" + tableId).wrap("<div class='dtCustomWrapper'></div>");
}

function AddCustomSearchOptionToDataTableForRestriction(divId, tableId, dtApi, funcName, placeHolder) {
    let input = $(divId).find('div.dt-search input').unbind();
    input.attr('placeholder', placeHolder);
    input.attr('id', tableId+'SearchInput');

    let $container = $('<div id ="uploadFeedbackContainer" class="d-flex align-items-center gap-2 my-2"></div>');

    let $searchButton = $('<button class="btn btn-sm btn-success DtExtraBtn">')
        .text('Search')
        .click(function () {
            dtApi.search(input.val()).draw();
        });
    let $reloadButton = $('<button class="btn btn-sm btn-primary DtExtraBtn">')
        .text('Reload')
        .click(function () {
            funcName();
        });
    $container.append(input, $searchButton, $reloadButton);
    $(divId).find('div.dt-search').html($container);
    $("#" + tableId).wrap("<div class='dtCustomWrapper'></div>");
}

function DetroyAndClearDataTable(tableId) {
    let tableSelector = $('#' + tableId);
    let dtTable = tableSelector.DataTable();

    tableSelector.DataTable().destroy();
    tableSelector.DataTable().clear().draw();
    return dtTable;
}

function ShowLoadingPanelInsideDatatable(tableId) {
    $('#' + tableId).html(loadingPanel);
}

function HideLoadingPanelInsideDatatable(tableId) {
    $('#' + tableId).find("div.common-loading-panel").remove();
}

function BindEmptyDataTable(tableId) {
    $('#' + tableId).DataTable({
        "scrollX": true,
        "filter": false,
        "paging": false,
        "ordering": false
    });
}

function BindClickEventForDataTableDynamicElementByClass(elementClass, functionName, paramAttrs) {
    $.each($('.' + elementClass), function (domIndex, domItem) {
        if (!$(domItem).hasClass('click-bound')) {
            $(domItem).addClass('click-bound');
            $(domItem).on('click', function () {
                if (paramAttrs && paramAttrs.length > 0) {
                    let paramVals = [];
                    $.each(paramAttrs, function (index, item) {
                        paramVals.push($(domItem).attr(item));
                    });
                    functionName.apply(this, paramVals);
                }
                else {
                    functionName();
                }
            });
        }
    });
}

function BindChangeEventForDataTableDynamicElementByClass(elementClass, functionName, paramAttrs) {
    $.each($('.' + elementClass), function (domIndex, domItem) {
        $(domItem).on('change', function () {
            if (paramAttrs && paramAttrs.length > 0) {
                let paramVals = [];
                $.each(paramAttrs, function (index, item) {
                    paramVals.push($(domItem).attr(item));
                });
                functionName.apply(this, paramVals);
            }
            else {
                functionName();
            }
        });
    });
}

function GenerateEmptyDataTable(tableId, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "columns": colData
    });
}

function GenerateDataTableOnError(tableId, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found.'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "columns": colData
    });
}

function GenerateDataTableOnErrorWithoutHeader(tableId) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "dom": 't',
        "language": {
            emptyTable: 'No data found.'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function () {
            $(this.api().table().header()).hide();
        }
    });
}

function GenerateDataTableOnSuccess(tableId, dataSource, colData, drawCallbackData, colName1, colName2) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (row, data, dataIndex) {
            if (colName2 && data[colName2] && colName1 == 'custAcceptStatus' && data[colName1] == 'Follow-Up') {
                $('td', row).addClass('bg-danger-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Interested') {
                $('td', row).addClass('bg-success-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Follow-Up') {
                $('td', row).addClass('bg-warning-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Not Interested') {
                $('td', row).addClass('bg-danger-subtle');
            }
        },
        "drawCallback": function (settings) {
            $('body').tooltip({
                selector: '.ppc-tooltip-item',
                trigger: 'hover manual'
            });

            if (drawCallbackData && drawCallbackData.length > 0) {
                $.each(drawCallbackData, (index, item) => {
                    BindClickEventForDataTableDynamicElementByClass(item.className, item.applyingMethod, item.bindingValues);
                });
            }
        },
        "initComplete": function () {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessOfBanksOffers(tableId, dataSource, colData, drawCallbackData, colName1, colName2, colName3, colName4) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": true,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (row, data, dataIndex) {
            var isPreApproved = colName3 && data[colName3] && data[colName3].toLowerCase().startsWith("pre-");
            if (isPreApproved) {
                $('td', row).addClass('text-success fw-bold');
            }

            if (colName2 && data[colName2] && colName1 == 'custAcceptStatus' && data[colName1] == 'Follow-Up') {
                $('td', row).addClass('bg-danger-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Interested') {
                $('td', row).addClass('bg-success-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Follow-Up') {
                $('td', row).addClass('bg-warning-subtle');
            }

            if (colName1 == 'custAcceptStatus' && data[colName1] == 'Not Interested') {
                $('td', row).addClass('bg-danger-subtle');
            }

            if (colName4 == 'isLeadInitiated' && data[colName4] == true) {
                var currentValue = $("td:eq(1)", row).text();
                $("td:eq(1)", row).html('<a href="javascript:void(0);" class="subidha-lead-link fw-semibold' + (isPreApproved ? ' text-primary fw-bold' : '') + '" value1="' + data.lmsOfferLogId + '"data-bs-custom-class="custom-tooltip-item-cls" data-bs-title="Check lead status" data-bs-toggle="tooltip" data-bs-placement="top">' + currentValue + '</a>');
            }
        },
        "drawCallback": function (settings) {
            $('body').tooltip({
                selector: '.ppc-tooltip-item',
                trigger: 'hover manual'
            });

            if (drawCallbackData && drawCallbackData.length > 0) {
                $.each(drawCallbackData, (index, item) => {
                    BindClickEventForDataTableDynamicElementByClass(item.className, item.applyingMethod, item.bindingValues);
                });
            }

            $(document)
            .off('click.subidha', '.subidha-lead-link')
            .on('click.subidha', '.subidha-lead-link', function (e) {
                e.preventDefault();

                const dt = $('#' + tableId).DataTable();
                const rowData = dt.row($(this).closest('tr')).data();

                GetSubidhaLeadStatus(rowData);
            });

        },
        "initComplete": function () {
            AddCustomRadioSearchOptionToDataTable(tableId, this.api());
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessWithMaxheightAndActions(tableId, dataSource, colData, drawCallbackData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "scrollY": '175px',
        scrollCollapse: true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "drawCallback": function (settings) {
            $('body').tooltip({
                selector: '.ppc-tooltip-item',
                trigger: 'hover manual'
            });

            if (drawCallbackData && drawCallbackData.length > 0) {
                $.each(drawCallbackData, (index, item) => {
                    BindClickEventForDataTableDynamicElementByClass(item.className, item.applyingMethod, item.bindingValues);
                });
            }
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessWithPaging(tableId, dataSource, colData, colName1) {
    var dtTable = DetroyAndClearDataTable(tableId);
    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": true,
        "paging": true,
        "filter": false,
        "bInfo": true,
        "scrollX": true,
        "scrollY": '200px',
        "scrollCollapse": true,
        "dom": "t<'row col-12 text-xs pt-1'<'col-7 my-auto'<'float-start'l>><'col-5 p-0 my-1'<'float-end'p>>>",
        "lengthMenu": [[5, 10, 20], [5, 10, 20]],
        "pageLength": 5,
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "drawCallback": function (settings) {
            $('[data-bs-toggle="tooltip"]').tooltip();
            BindClickEventForDataTableDynamicElementByClass('lead-link', GetLeadStatus, ["value1"]);
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (colName1 == 'isLeadCreated' && aData[colName1] == '2') {
                $('td', nRow).addClass('bg-danger-subtle');
            }

            var info = $('#' + tableId).DataTable().page.info();
            var rowNumber = iDisplayIndex + 1 + info.page * info.length;

            $("td:eq(0)", nRow).html(rowNumber);

            if (colName1 == 'isLeadCreated' && aData[colName1] == '1') {
                var currentValue = $("td:eq(1)", nRow).text();
                $("td:eq(1)", nRow).html('<a href="javascript:void(0);" class="lead-link fw-semibold" value1="' + aData.id + '"data-bs-custom-class="custom-tooltip-item-cls" data-bs-title="Check lead status" data-bs-toggle="tooltip" data-bs-placement="top">' + currentValue + '</a>');
            }

            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData,
        "aaSorting": []
    });
}

function GenerateDataTableOnSuccessE2E(tableId, dataSource, colData, drawCallbackData, colName1, colName2) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "fnRowCallback": function (row, data, dataIndex) {
            $('td', row).removeClass('bg-warning-subtle');
            $('td', row).removeClass('bg-danger-subtle');

            if (colName1 && data[colName1]) {
                $('td', row).addClass('bg-warning-subtle');
            }
            else if (colName2 && data[colName2] == 'D') {
                $('td', row).addClass('bg-danger-subtle');
            }
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "drawCallback": function (settings) {
            $('body').tooltip({
                selector: '.ppc-tooltip-item',
                trigger: 'hover manual'
            });

            if (drawCallbackData && drawCallbackData.length > 0) {
                $.each(drawCallbackData, (index, item) => {
                    BindClickEventForDataTableDynamicElementByClass(item.className, item.applyingMethod, item.bindingValues);
                });
            }
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessSpecialWithSerialAndOrderAndPaging(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": true,
        "paging": true,
        "filter": false,
        "bInfo": true,
        "scrollX": true,
        "scrollY": '200px',
        "scrollCollapse": true,
        "dom": "t<'row col-lg-12 text-xs'<'col-lg-3 pe-0'l><'col-lg-5 my-auto text-center pe-0'i><'col-lg-4 pe-0'<'float-lg-end float-sm-Center'p>>>",
        "pageLength": 10,
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var info = $('#' + tableId).DataTable().page.info();
            $("td:eq(0)", nRow).html(iDisplayIndex + 1 + info.page * info.length);
            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData,
        "aaSorting": []
    });
}

function GenerateDataTableOnSuccessForBufferTool(tableId, dataSource, colData, additionalData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": true,
        "paging": true,
        "filter": false,
        "bInfo": true,
        "scrollX": true,
        "scrollY": '200px',
        "scrollCollapse": true,
        "dom": "t<'row col-lg-12 text-xs'<'col-lg-3 pe-0'l><'col-lg-5 my-auto text-center pe-0'i><'col-lg-4 pe-0'<'float-lg-end float-sm-Center'p>>>",
        "pageLength": 10,
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "drawCallback": function () {
            if (dataSource.length > 0) {
                var formattedAmt = additionalData.toLocaleString('en-US', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });

                $("#total-charge-amt").text(formattedAmt);
                $("#total-coll-amt").text(formattedAmt);
            } else {
                $("#total-charge-amt").text("");
                $("#total-coll-amt").text("");
            }
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var info = $('#' + tableId).DataTable().page.info();
            $("td:eq(0)", nRow).html(iDisplayIndex + 1 + info.page * info.length);
            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData,
        "aaSorting": []
    });
}

function GenerateDataTableOnSuccessSpecialWithSerialAndOrder(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": true,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "scrollY": '200px',
        "scrollCollapse": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var info = $('#' + tableId).DataTable().page.info();
            $("td:eq(0)", nRow).html(iDisplayIndex + 1 + info.page * info.length);
            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData,
        "aaSorting": []
    });
}

function GenerateDataTableOnSuccessSpecialWithoutSerialAndOrder(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": true,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "scrollY": '400px',
        "scrollCollapse": true,
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData,
        "aaSorting": []
    });
}

function GenerateDataTableOnSuccessSpecialWithRowBG(tableId, dataSource, colData, colName) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData[colName] == 'Pending') {
                $('td', nRow).addClass('bg-danger-subtle');
            }
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}


function GenerateDataTableOnSuccessSpecial(tableId, dataSource, colData, colName, colIndex) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData[colName] == 0) {
                $("td:eq(" + colIndex + ")", nRow).addClass('text-danger fw-bold');
            }
            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessSpecialWithActions(tableId, dataSource, colData, colName, colIndex) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "drawCallback": function (settings) {
            $('body').tooltip({
                selector: '.ppc-tooltip-item',
                trigger: 'hover manual'
            });
            BindClickEventForDataTableDynamicElementByClass('show-details-btn', ShowProductDetails, ["value1"]);
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData[colName] == 0) {
                $("td:eq(" + colIndex + ")", nRow).addClass('text-danger fw-bold');
            }
            else {
                var cellText = $('td', nRow).eq(0).text();
                let linkHtml = "<button data-bs-toggle='tooltip' data-bs-placement='top' data-bs-custom-class='custom-tooltip-item-cls' data-bs-title='Click to View Details' class='p-0 m-0 fw-semibold btn btn-link text-small ppc-tooltip-item show-details-btn' value1='" + cellText + "'>" + cellText + "</button>"
                $('td', nRow).eq(0).html(linkHtml);
            }
            return nRow;
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessSpecialWithoutHeader(tableId, dataSource, colData, colName, colIndex) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function () {
            $(this.api().table().header()).hide();
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData[colName] == 0) {
                $("td:eq(" + colIndex + ")", nRow).addClass('text-danger fw-bold');
            }
            return nRow;
        },
        "data": dataSource,
        "columns": colData
    });
}


function GenerateDataTableOnSuccessWithoutHeader(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "initComplete": function () {
            $(this.api().table().header()).hide();
        },
        "data": dataSource,
        "columns": colData
    });
}

function GenerateDataTableOnSuccessRCSpan(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);
    let analysisCount = 0;
    $.fn.dataTable.ext.errMode = 'none';

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "scrollY": '150px',
        "scrollCollapse": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "createdRow": function (row, data, dataIndex) {
            if (data.rowcount == 1) {
                analysisCount++;

                $("td:eq(3)", row).attr('rowspan', data.subcount);
                $("td:eq(0)", row).attr('rowspan', data.subcount);
            }
            else {
                $("td:eq(3)", row).remove();
                $("td:eq(0)", row).remove();
            }
            if (data.colspan > 1) {
                $("td:eq(1)", row).attr('colspan', data.colspan);
                $("td:eq(1)", row).addClass('text-center');

                $("td:eq(3)", row).remove();
                $("td:eq(2)", row).remove();
            }

            if (analysisCount == 1) {
                $('td', row).addClass('bg-success-subtle');
            }
            else if (analysisCount == 2) {
                $('td', row).addClass('bg-primary-subtle');
            }
            else {
                $('td', row).addClass('bg-warning-subtle');
            }

        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}


function GenerateDataTableOnSuccessWithRowSpan(tableId, dataSource, colData) {
    var dtTable = DetroyAndClearDataTable(tableId);
    $.fn.dataTable.ext.errMode = 'none';
    let bgClass = '';

    $('#' + tableId).DataTable({
        "processing": false,
        "destroy": true,
        "ordering": false,
        "paging": false,
        "filter": false,
        "bInfo": false,
        "scrollX": true,
        "scrollY": '200px',
        "scrollCollapse": true,
        "dom": 't',
        "language": {
            emptyTable: 'No data found'
        },
        "preDrawCallback": function (settings) {
            dtTable.clear().draw();
        },
        "createdRow": function (row, data, dataIndex) {
            if (data.itemRowNum == 1) {
                if (!bgClass || bgClass == 'bg-primary-subtle') {
                    bgClass = 'bg-success-subtle';
                }
                else {
                    bgClass ='bg-primary-subtle';
                }

                $("td:eq(0)", row).attr('rowspan', data.groupRowCount);
            }
            else {
                $("td:eq(0)", row).remove();
            }

            $('td', row).addClass(bgClass);
        },
        "initComplete": function (settings, json) {
            this.api().columns.adjust().draw();
        },
        "data": dataSource,
        "columns": colData
    });
}


function HandleAjaxErrorEvent(jqXHR, exception) {
    if (jqXHR.status == 401) {
        bootbox.alert({
            message: "You have been logged out. Please login again.",
            callback: function () {
                window.location = '/Account/Login';
            }
        });
    }
    else if (jqXHR.status === 0) {
        ShowPushMessage('Network Problem.', 'danger');
    }
    else if (jqXHR.status == 400) {
        ShowPushMessage(JSON.parse(jqXHR.responseText), 'danger');
    }
    else if (jqXHR.status == 403) {
        ShowPushMessage("You don't have access of the following action.", 'danger');
    }
    else if (jqXHR.status == 404) {
        ShowPushMessage("Content not found.", 'danger');
    }
    else if (jqXHR.status == 500) {
        ShowPushMessage("Problem in processing the request.", 'danger');
    }
    else if (exception === 'timeout') {
        ShowPushMessage("Request timed out.", 'danger');
    }
    else if (exception === 'abort') {
        ShowPushMessage("Request aborted.", 'danger');
    }
    else if (jqXHR.responseText) {
        ShowPushMessage(jqXHR.responseText, 'danger');
    }
    else {
        ShowPushMessage("Error in completing the action.", 'danger');
    }
}


function BindClickEvent(elementId, functionName, paramVals) {
    $('#' + elementId).on('click', function () {
        if (paramVals && paramVals.length > 0) {
            functionName.apply(this, paramVals);
        }
        else {
            functionName();
        }
    });
}

function BindClickEventByClass(elementClass, functionName, paramAttrs) {
    $.each($('.' + elementClass), function (domIndex, domItem) {
        if (!$(domItem).hasClass('click-bound')) {
            $(domItem).addClass('click-bound');
            $(domItem).on('click', function () {
                if (paramAttrs && paramAttrs.length > 0) {
                    let paramVals = [];
                    $.each(paramAttrs, function (index, item) {
                        paramVals.push($(domItem).attr(item));
                    });
                    functionName.apply(this, paramVals);
                }
                else {
                    functionName();
                }
            });
        }
    });
}


function BindClickEventByAttr(elementId, functionName, paramAttrs) {
    $('#' + elementId).on('click', function () {
        if (paramAttrs && paramAttrs.length > 0) {
            let paramVals = [];
            $.each(paramAttrs, function (index, item) {
                paramVals.push($('#' + elementId).attr(item));
            });
            functionName.apply(this, paramVals);
        }
        else {
            functionName();
        }
    });
}

function BindChangeEvent(elementId, functionName, paramVals) {
    $('#' + elementId).on('change', function () {
        if (paramVals && paramVals.length > 0) {
            functionName.apply(this, paramVals);
        }
        else {
            functionName();
        }
    });
}

function BindClickEventForDynamicElementWithVals(elementId, functionName, paramVals) {
    $(document).on('click', ('#' + elementId), function () {
        if (paramVals && paramVals.length > 0) {
            functionName.apply(this, paramVals);
        }
        else {
            functionName();
        }
    });
}


// notificationType = 'info', 'warning', 'success', 'danger'
function ShowPushMessage(message, notificationType, delayMiliSeconds) {
    if (notificationType == 'success') {
        notificationType = 'bg-success'
    } else if (notificationType == 'warning') {
        notificationType = 'bg-warning'
    } else if (notificationType == 'danger') {
        notificationType = 'bg-danger'
    } else {
        notificationType = 'bg-info'
    }

    if (!delayMiliSeconds) {
        delayMiliSeconds = 5000;
    }

    let randNum = moment().valueOf() + '' + Math.floor(Math.random() * 1122);
    let toastId = 'toaster-' + randNum;

    if (!$("#MasterToaster").html()) {
        let toastContainer = '<div id="MasterToaster" class="toast-container position-absolute position-fixed top-0 start-50 translate-middle-x"></div >';
        $(toastContainer).appendTo('body');
    }

    let toasterBody = `<div id="` + toastId + `" class="toast-custom mb-2 bg-opacity-100 toast align-items-center text-white ` + notificationType + ` border-0" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="` + delayMiliSeconds + `">
                          <div class="d-flex justify-content-between">
                            <div></div>
                            <div class="toast-body fw-bold text-center">` + message + `</div>
                            <button type="button" class="btn-close btn-close-white mt-auto mb-auto me-2" data-bs-dismiss="toast" aria-label="Close"></button>
                          </div>
                        </div>`;

    $("#MasterToaster").append(toasterBody);
    new bootstrap.Toast($('#' + toastId)).show();
}


function ShowLoadingPanelTranparent() {
    let dialog = bootbox.dialog({
        message: '<p id="TransparentModal" class="text-center mb-0 transparent-modal-style"><i class="fas fa-spinner fa-pulse"></i> Processing...</p>',
        closeButton: false,
        centerVertical: true
    });

    dialog.init(function () {
        dialog.attr("id", "TransparentModalRoot");
        $('.modal-backdrop').attr('id', 'TransparentModalBackDrop');
        $("#TransparentModal").parent().parent().parent().css({ "background-color": "transparent", "border": "0" });
    });
}

function ShowLoadingPanelTranparentAfterDelay(DelayInMilis) {
    let dialog = bootbox.dialog({
        message: '<p id="TransparentModal" class="text-center mb-0 transparent-modal-style"><i class="fas fa-spinner fa-pulse"></i> Processing...</p>',
        closeButton: false,
        centerVertical: true
    });

    if (!DelayInMilis) {
        DelayInMilis = 500;
    }
    setTimeout(
        function () {
            dialog.init(function () {
                dialog.attr("id", "TransparentModalRoot");
                $('.modal-backdrop').attr('id', 'TransparentModalBackDrop');
                $("#TransparentModal").parent().parent().parent().css({ "background-color": "transparent", "border": "0" });
            });
        }, DelayInMilis);

}

function ShowLoadingPanel() {
    bootbox.dialog({
        message: '<p id="TransparentModal" class="text-center mb-0 transparent-modal-text-style"><i class="fas fa-spinner fa-pulse"></i> Processing...</p>',
        closeButton: false,
        centerVertical: true
    });
}

function ShowLoadingPanelWithTimeout(timeoutMiliSeconds) {
    bootbox.dialog({
        message: '<p id="TransparentModal" class="text-center mb-0 transparent-modal-text-style"><i class="fas fa-spinner fa-pulse"></i> Processing...</p>',
        closeButton: false,
        centerVertical: true
    }).init(function () {
        setTimeout(function () {
            $("#TransparentModal").parent().parent().parent().parent().parent().modal('hide');
        }, timeoutMiliSeconds);
    });
}

function HideLoadingPanel() {
    if ($('#TransparentModal').length > 0) {
        $("#TransparentModal").parent().parent().parent().parent().parent().modal('hide');
    }
}

function HideLoadingPanelAfterDelay(DelayInMilis) {
    if ($('#TransparentModal').length > 0) {
        if (!DelayInMilis) {
            DelayInMilis = 500;
        }
        setTimeout(
            function () {
                $("#TransparentModal").parent().parent().parent().parent().parent().modal('hide');
            }, DelayInMilis);
    }
}

function RemoveLoadingPanelAfterDelay(DelayInMilis) {
    if (!DelayInMilis) {
        DelayInMilis = 500;
    }
    setTimeout(
        function () {
            $('#TransparentModalRoot').remove();
            $('#TransparentModalBackDrop').remove();
        }, DelayInMilis);
}

function ShowCustomModalBase(ContentBodyToLoad) {
    var html = '<div class="modal fade" id="DynamicModalBody" tabindex="-1">'
    html += '<div class="modal-dialog modal-xl">';
    html += '<div class="position-relative text-semi-md modal-content">';

    html += '<div class="modal-body pt-2 pb-2" id="CustomModalBody">';
    html += '</div>';

    html += '<div class="position-absolute end-0 modal-header CustomModalHeaderSmall p-0">';
    html += '<button type="button" class="p-2 pt-1 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';

    html += '<div class="p-2 position-absolute end-0 bottom-0 modal-footer CustomModalFooterSmall">';

    html += '<button type="button" class="m-0 btn btn-danger btn-xs" data-bs-dismiss="modal" id="DynamicModalCloseBtn">Close</button>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    $("#MasterModal").html(html);
    $("#CustomModalBody").load(ContentBodyToLoad,
        function (responseText, textStatus, XMLHttpRequest) {
            if (textStatus == "success") {
                $("#DynamicModalBody").modal('show');
            }
    });
}

function ShowCustomModalBaseFromHtml(ContentBodyHtml, isFooterdNeeded) {
    var html =
        `<div class="modal fade" id="DynamicModalBody" tabindex="-1">
            <div class="modal-dialog modal-xl">
                <div class="position-relative text-semi-md modal-content` + (isFooterdNeeded ? '' : ' pb-1') + `">
                    <div class="modal-body p-2" id="CustomModalBody">
                    </div>

                    <div class="position-absolute end-0 modal-header CustomModalHeaderSmall p-0">
                        <button type="button" class="p-2 pt-1 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="modal-footer p-0 CustomModalFooterSmall">`;

    if (isFooterdNeeded) {
        html += `<button type="button" class="btn btn-danger btn-xs" data-bs-dismiss="modal" id="DynamicModalCloseBtn">Close</button>`;
    }
    html += `</div>
            </div>
        </div>
    </div>`;

    $("#MasterModal").html(html);
    $("#CustomModalBody").html(ContentBodyHtml);
    $("#DynamicModalBody").modal('show');
}

function ShowCustomSmallModalBaseFromHtml(ContentBodyHtml, isFooterdNeeded) {
    var html =
        `<div class="modal fade" id="DynamicModalBody" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="position-relative text-semi-md modal-content` + (isFooterdNeeded ? '' : ' pb-1') + `">
                    <div class="modal-body p-2" id="CustomModalBody">
                    </div>

                    <div class="position-absolute end-0 modal-header CustomModalHeaderSmall p-0">
                        <button type="button" class="p-2 pt-1 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>`;

    if (isFooterdNeeded) {
        html += `<div class="p-2 pb-1 position-absolute end-0 bottom-0 modal-footer CustomModalFooterSmall">
                    <button type="button" class="btn btn-danger btn-xs" data-bs-dismiss="modal" id="DynamicModalCloseBtn">Close</button>
                </div>`;
    }
    html += `</div>
        </div>
    </div>`;

    $("#MasterModal").html(html);
    $("#CustomModalBody").html(ContentBodyHtml);
    $("#DynamicModalBody").modal('show');
}

function HideCustomModalBaseFromHtml() {
    $("#DynamicModalBody").removeClass("in");
    $(".modal-backdrop").remove();
    $('body').removeClass('modal-open');
    $('body').addClass('pe-0');
    $("#DynamicModalBody").modal('hide');
}

function UpdateCustomModalBase(ContentBodyToLoad) {
    $("#CustomModalBody").load(ContentBodyToLoad);
}

function ShowCustomSmallModalBase(ContentBodyToLoad) {
    var html = '<div class="modal fade" id="DynamicModalBody" tabindex="-1">'
    html += '<div class="modal-dialog modal-lg">';
    html += '<div class="position-relative text-semi-md modal-content">';

    html += '<div class="modal-body p-2" id="CustomModalBody">';
    html += '</div>';

    html += '<div class="position-absolute end-0 modal-header CustomModalHeaderSmall p-0">';
    html += '<button type="button" class="p-2 pt-1 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';

    html += '<div class="p-2 position-absolute end-0 bottom-0 modal-footer CustomModalFooterSmall">';

    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    $("#MasterModal").html(html);
    $("#CustomModalBody").load(ContentBodyToLoad,
        function (responseText, textStatus, XMLHttpRequest) {
            if (textStatus == "success") {
                $("#DynamicModalBody").modal('show');
            }
        });
}

function ShowCustomSmallModalBaseHtml(htmlContent) {
    var html = '<div class="modal fade" id="DynamicModalBody" tabindex="-1">';
    html += '<div class="modal-dialog modal-lg">';
    html += '<div class="position-relative text-semi-md modal-content">';

    html += '<div class="modal-body pt-2 pb-2" id="CustomModalBody">';
    html += htmlContent;
    html += '</div>';

    html += '<div class="position-absolute end-0 modal-header CustomModalHeaderSmall p-0">';
    html += '<button type="button" class="p-2 pt-1 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';

    html += '<div class="p-2 position-absolute end-0 bottom-0 modal-footer CustomModalFooterSmall">';
    html += '</div>';

    html += '</div>';
    html += '</div>';
    html += '</div>';

    $("#MasterModal").html(html);
    $("#DynamicModalBody").modal('show');
}

function ShowCustomSmallModalBaseFromHtmlBody(htmlBody, showFooter = false) {
    var html = '<div class="modal fade" id="DynamicModalBody" tabindex="-1">'
    html += '<div class="modal-dialog modal-lg">';
    html += '<div class="modal-content ps-3 pe-3">';

    html += '<div class="modal-header CustomModalHeaderSmall p-0">';
    html += '<button type="button" class="p-0 close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';

    html += '<div class="modal-body p-0 overflow-x-auto" id="CustomModalBody">';
    html += '</div>';

    if (showFooter) {
        html += '<div class="modal-footer p-0 CustomModalFooterSmall">';
        html += '<button type="button" class="btn btn-danger btn-xs me-0" data-bs-dismiss="modal" id="DynamicModalCloseBtn">Close</button>';
        html += '</div>';
    }

    html += '</div>';
    html += '</div>';
    html += '</div>';

    $("#MasterModal").html(html);
    $("#CustomModalBody").html(htmlBody);
    $("#DynamicModalBody").modal('show');
}

function ShowCustomModalBaseWithHideLoadingPanel(ContentBodyToLoad) {
    var html = '<div class="modal fade" id="DynamicModalBody" tabindex="-1">'
    html += '<div class="modal-dialog modal-xl">';
    html += '<div class="modal-content">';

    html += '<div class="modal-header CustomModalHeaderSmall p-0">';
    html += '<button type="button" class="close custom-modal-close-icon ms-auto" data-bs-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';

    html += '<div class="modal-body pt-0 pb-0" id="CustomModalBody">';
    html += '</div>';
    html += '<div class="modal-footer p-0 CustomModalFooterSmall">';

    html += '<button type="button" class="btn btn-danger btn-xs" data-bs-dismiss="modal" id="DynamicModalCloseBtn">Close</button>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    $("#MasterModal").html(html);
    $("#CustomModalBody").load(ContentBodyToLoad,
        function (responseText, textStatus, XMLHttpRequest) {
            if (textStatus == "success") {
                $("#DynamicModalBody").modal('show');
                HideLoadingPanelAfterDelay();
            }
        });
}

function ShowSuccessPopupMessage(Message) {
    var html = '<div id="SuccessPopupModal" class="modal fade" tabindex="-1">'
    html += '      <div class="modal-dialog modal-confirm">'
    html += '	             <div class="modal-content">'
    html += '		               <div class="modal-header">'
    html += '                           <div class="icon-box">'
    html += '                               <i class="fas fa-check-circle"></i>'
    html += '                           </div>'
    html += '	                       <h4 class="modal-title w-100">Successful!</h4>'
    html += '                       </div>'
    html += '                      <div class="modal-body">'
    html += '                         <p class="text-center">' + Message + '</p>'
    html += '                      </div>'
    html += '                     <div class="modal-footer p-0">'
    html += '	                    <button class="btn btn-success btn-block w-100" data-bs-dismiss="modal">OK</button>'
    html += '	                 </div>'
    html += '	             </div>'
    html += '             </div>'
    html += '       </div>';
    $("#MasterModal").html(html);

    setTimeout(function () {
        $("#SuccessPopupModal").modal('show');
    }, 500);
}

function ShowFailurePopupMessage(Message) {
    var html = '<div id="FailurePopupModal" class="modal fade" tabindex="-1">'
    html += '      <div class="modal-dialog modal-confirm">'
    html += '	             <div class="modal-content">'
    html += '		               <div class="modal-header">'
    html += '                           <div class="icon-box-failure">'
    html += '                               <i class="fas fa-exclamation-circle"></i>'
    html += '                           </div>'
    html += '	                       <h4 class="modal-title w-100">Failed!</h4>'
    html += '                       </div>'
    html += '                      <div class="modal-body">'
    html += '                         <p class="text-center">' + Message + '</p>'
    html += '                      </div>'
    html += '                     <div class="modal-footer p-0">'
    html += '	                    <button class="btn btn-danger btn-block w-100" data-bs-dismiss="modal">OK</button>'
    html += '	                 </div>'
    html += '	             </div>'
    html += '             </div>'
    html += '       </div>';
    $("#MasterModal").html(html);

    setTimeout(function () {
        $("#FailurePopupModal").modal('show');
    }, 500);
}

function SetCustomModalEventListenerForCalender(calenderId, modalId) {
    document.getElementById(modalId).addEventListener('hidden.bs.modal', () => {
        const widget = document.querySelector('.tempus-dominus-widget');
        if (widget) {
            widget.remove();
        }
    });

    document.getElementById(calenderId).addEventListener('hide.td', function () {
        setTimeout(() => {
            let modal = document.getElementById(calenderId).closest('.modal-dialog');
            let modalContent = document.getElementById(calenderId).closest('.modal-content');
            const modalContentRect = modalContent.getBoundingClientRect();
            modal.style.height = modalContentRect.height + 'px';
            modal.classList.remove('overflow-y-auto');
        }, 100);
    });

    document.getElementById(calenderId).addEventListener('show.td', function () {
        setTimeout(() => {
            const modal = document.getElementById(calenderId).closest('.modal-dialog');
            const modalRect = modal.getBoundingClientRect();
            modal.style.height = modalRect.height + 1 + 'px';
            const widget = document.querySelector('.tempus-dominus-widget');
            if (widget) {
                const widgetRect = widget.getBoundingClientRect();
                if (widgetRect.bottom > modalRect.bottom) {
                    modal.classList.add('verflow-y-auto');
                    modal.style.height = modalRect.height + (widgetRect.bottom - modalRect.bottom) + 'px';
                }
            }
        }, 100);
    });
}


// format docs: https://getdatepicker.com/6/options/display.html
//let localeNameBang = "bn-IN";
let localeName = "en-US";
let dateFomat = "dd-MMM-yyyy";
let dateFomatMoment = "DD-MMM-yyyy";
let dateTimeFomat = "dd-MMM-yyyy hh:mm:ss T";
let dateTimeFomatMoment = "DD-MMM-yyyy hh:mm:ss A";
let makeDateTimeFieldReadOnly = true;
let calanderMinDate = moment(new Date("01-Jan-1900")).format(dateFomatMoment);
let calanderMaxDate = moment(new Date("31-Dec-10000")).format(dateFomatMoment);
let calanderMinDateTime = moment(new Date("01-Jan-1900")).format(dateTimeFomatMoment);
let calanderMaxDateTime = moment(new Date("31-Dec-10000")).format(dateTimeFomatMoment);


function GetFormattedDate(date) {
    var formattedDate = moment(date).format(dateFomatMoment);
    return formattedDate;
}

function GetFormattedDateTime(date) {
    var formattedDate = moment(date).format(dateTimeFomatMoment);
    return formattedDate;
}

function ClearTempusDominusDateField(fieldId) {
    $('#' + fieldId).val('');
}

function GetCurrentDate() {
    return moment(new Date()).format(dateFomatMoment);
}

function GetCurrentDateTime() {
    return moment(new Date()).format(dateTimeFomatMoment);
}
function GetCurrentDateMaxTime() {
    return moment(new Date()).add(1, 'days').startOf('day').format(dateTimeFomatMoment);
}

function CurrentDate(fromDate, toDate) {
    var currentDate = moment(new Date()).format(dateFomatMoment);;
    $("#" + fromDate).val(currentDate);

    var currentDate = moment(new Date()).format(dateFomatMoment);;
    $("#" + toDate).val(currentDate);
}

function DateField(fieldId) {
    new tempusDominus.TempusDominus(document.getElementById(fieldId), {
        localization: {
            locale: localeName,
            format: dateFomat
        },
        display: {
            buttons: {
                today: true,
                clear: true,
                close: true
            },
            components: {
                calendar: true,
                clock: false,
                hours: false,
                minutes: false,
                seconds: false
            }
        },
        useCurrent: false
    });

    if (makeDateTimeFieldReadOnly) {
        var selector = "#" + fieldId;
        $(selector).attr("readonly", "readonly");

        var childFields = selector + " :input";
        $(childFields).attr("readonly", "readonly");
    }
}
function SetAndGetDateField(fieldId) {
    let picker = new tempusDominus.TempusDominus(document.getElementById(fieldId), {
        localization: {
            locale: localeName,
            format: dateFomat
        },
        display: {
            buttons: {
                today: true,
                clear: true,
                close: true
            },
            components: {
                calendar: true,
                clock: false,
                hours: false,
                minutes: false,
                seconds: false
            }
        },
        useCurrent: false
    });

    if (makeDateTimeFieldReadOnly) {
        var selector = "#" + fieldId;
        $(selector).attr("readonly", "readonly");

        var childFields = selector + " :input";
        $(childFields).attr("readonly", "readonly");
    }

    return picker;
}

function DateFieldWithCurrentDate(fieldId) {
    new tempusDominus.TempusDominus(document.getElementById(fieldId), {
        localization: {
            locale: localeName,
            format: dateFomat
        },
        display: {
            buttons: {
                today: true,
                clear: true,
                close: true
            },
            components: {
                calendar: true,
                clock: false
            }
        },
        useCurrent: false,
        defaultDate: new Date()
    });

    if (makeDateTimeFieldReadOnly) {
        var selector = "#" + fieldId;
        $(selector).attr("readonly", "readonly");
    }
}

function SetCurrentDate(fieldId) {
    let currDate = moment(new Date()).locale(localeName);
    var selector = "#" + fieldId;
    $(selector).val(currDate);
}

function SetCurrentDateTime(fieldId) {
    let currDateTime = moment(new Date()).format(dateTimeFomatMoment);
    var selector = "#" + fieldId;
    $(selector).val(currDateTime);
}

function DateFieldWithMaxCurrentDate(fieldId) {
    let currDate = moment(new Date()).format(dateFomatMoment);
    const picker = SetAndGetDateField(fieldId);

    picker.updateOptions({
        restrictions: {
            maxDate: currDate
        }
    });
}

function DateFieldWithMaxDate(fieldId, maxDate) {
    const picker = SetAndGetDateField(fieldId);

    picker.updateOptions({
        restrictions: {
            maxDate: new Date(maxDate)
        }
    });
}

function DateFieldWithMinCurrentDate(fieldId) {
    let currDate = moment(new Date()).format(dateFomatMoment);
    const picker = SetAndGetDateField(fieldId);

    picker.updateOptions({
        restrictions: {
            minDate: currDate
        }
    });
}

function DateFieldWithMinDate(fieldId, maxDate) {
    const picker = SetAndGetDateField(fieldId);

    picker.updateOptions({
        restrictions: {
            minDate: new Date(maxDate)
        }
    });
}

function DateTimeField(fieldId) {
    new tempusDominus.TempusDominus(document.getElementById(fieldId), {
        localization: {
            locale: localeName,
            format: dateTimeFomat
        },
        display: {
            buttons: {
                today: true,
                clear: true,
                close: true
            },
            components: {
                calendar: true,
                clock: true,
                hours: true,
                minutes: true,
                seconds: true
            }
        }
    });

    if (makeDateTimeFieldReadOnly) {
        var selector = "#" + fieldId;
        $(selector).attr("readonly", "readonly");

        var childFields = selector + " :input";
        $(childFields).attr("readonly", "readonly");
    }
}

function SetAndGetDateTimeField(fieldId) {
    let picker = new tempusDominus.TempusDominus(document.getElementById(fieldId), {
        localization: {
            locale: localeName,
            format: dateTimeFomat
        },
        display: {
            buttons: {
                today: true,
                clear: true,
                close: true
            },
            components: {
                calendar: true,
                clock: true,
                hours: true,
                minutes: true,
                seconds: true
            }
        }
    });

    if (makeDateTimeFieldReadOnly) {
        var selector = "#" + fieldId;
        $(selector).attr("readonly", "readonly");

        var childFields = selector + " :input";
        $(childFields).attr("readonly", "readonly");
    }

    return picker;
}

function SetSpecificDateTime(fieldId, dateTime) {
    $("#" + fieldId).val(dateTime);
}

function DateTimeFieldWithMaxCurrentDate(fieldId) {
    let currDateTime = moment(new Date()).format(dateTimeFomatMoment);
    const picker = SetAndGetDateTimeField(fieldId);

    picker.updateOptions({
        restrictions: {
            maxDate: currDateTime
        }
    });
}

function DateTimeFieldWithMaxDate(fieldId, maxDate) {
    const picker = SetAndGetDateTimeField(fieldId);

    picker.updateOptions({
        restrictions: {
            maxDate: new Date(maxDate)
        }
    });
}

function DateTimeFieldWithMinCurrentDate(fieldId) {
    let currDateTime = moment(new Date()).format(dateTimeFomatMoment);
    const picker = SetAndGetDateTimeField(fieldId);

    picker.updateOptions({
        restrictions: {
            minDate: currDateTime
        }
    });
}

function DateTimeFieldWithMinDate(fieldId, maxDate) {
    const picker = SetAndGetDateTimeField(fieldId);

    picker.updateOptions({
        restrictions: {
            minDate: new Date(maxDate)
        }
    });
}

function SetDateRange(fromDateId, toDateId) {
    const fromPicker = SetAndGetDateField(fromDateId);
    const toPicker = SetAndGetDateField(toDateId);

    document.getElementById(fromDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            toPicker.updateOptions({
                restrictions: {
                    minDate: selectedDate
                }
            });
        }
        else {
            toPicker.updateOptions({
                restrictions: {
                    minDate: calanderMinDate
                }
            });
        }
    });

    document.getElementById(toDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: selectedDate
                }
            });
        }
        else {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: calanderMaxDate
                }
            });
        }
    });

    document.getElementById(fromDateId).addEventListener("error.td", (e) => {
        if (e.detail.date) {
            var DateTimeVal = moment(e.detail.date).startOf('day').toDate();
            fromPicker.dates.setValue(tempusDominus.DateTime.convert(DateTimeVal));
        }
    });

    document.getElementById(toDateId).addEventListener("error.td", (e) => {
        if (e.detail.date) {
            var DateTimeVal = moment(e.detail.date).startOf('day').toDate();
            toPicker.dates.setValue(tempusDominus.DateTime.convert(DateTimeVal));
        }
    });
}

function SetDateRangeWithMaxCurrentDate(fromDateId, toDateId) {
    const fromPicker = SetAndGetDateField(fromDateId);
    const toPicker = SetAndGetDateField(toDateId);
    let currDate = moment(new Date()).endOf('day').format(dateFomatMoment);
    fromPicker.updateOptions({
        restrictions: {
            maxDate: currDate
        }
    });

    toPicker.updateOptions({
        restrictions: {
            maxDate: currDate
        }
    });

    document.getElementById(fromDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            toPicker.updateOptions({
                restrictions: {
                    minDate: selectedDate
                }
            });
        }
        else {
            toPicker.updateOptions({
                restrictions: {
                    minDate: calanderMinDate
                }
            });
        }
    });

    document.getElementById(toDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: selectedDate
                }
            });
        }
        else {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: currDate
                }
            });
        }
    });

    document.getElementById(fromDateId).addEventListener("error.td", (e) => {
        if (e.detail.date) {
            var DateTimeVal = moment(e.detail.date).startOf('day').toDate();
            fromPicker.dates.setValue(tempusDominus.DateTime.convert(DateTimeVal));
        }
    });

    document.getElementById(toDateId).addEventListener("error.td", (e) => {
        if (e.detail.date) {
            var DateTimeVal = moment(e.detail.date).startOf('day').toDate();
            toPicker.dates.setValue(tempusDominus.DateTime.convert(DateTimeVal));
        }
    });
}

function SetDateTimeRange(fromDateId, toDateId) {
    const fromPicker = SetAndGetDateTimeField(fromDateId);
    const toPicker = SetAndGetDateTimeField(toDateId);

    document.getElementById(fromDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            toPicker.updateOptions({
                restrictions: {
                    minDate: selectedDate
                }
            });
        }
        else {
            toPicker.updateOptions({
                restrictions: {
                    minDate: calanderMinDateTime
                }
            });
        }
    });

    document.getElementById(toDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: selectedDate
                }
            });
        }
        else {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: calanderMaxDateTime
                }
            });
        }
    });
}

function SetDateTimeRangeWithMaxCurrentDate(fromDateId, toDateId) {
    const fromPicker = SetAndGetDateTimeField(fromDateId);
    const toPicker = SetAndGetDateTimeField(toDateId);
    let currDateMaxTime = GetCurrentDateMaxTime();

    fromPicker.updateOptions({
        restrictions: {
            maxDate: currDateMaxTime
        }
    });

    toPicker.updateOptions({
        restrictions: {
            maxDate: currDateMaxTime
        }
    });

    document.getElementById(fromDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            toPicker.updateOptions({
                restrictions: {
                    minDate: selectedDate
                }
            });
        }
        else {
            toPicker.updateOptions({
                restrictions: {
                    minDate: calanderMinDateTime
                }
            });
        }
    });

    document.getElementById(toDateId).addEventListener("change.td", (e) => {
        let selectedDate = e.detail.date;
        if (selectedDate) {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: selectedDate
                }
            });
        }
        else {
            fromPicker.updateOptions({
                restrictions: {
                    maxDate: currDateMaxTime
                }
            });
        }
    });
}

function ClearCreateForm(formId) {
    $('#' + formId)[0].reset();
}

function ClearEditForm(formId) {
    $(':input', '#' + formId)
        .removeAttr('selected')
        .removeAttr('checked')
        .not(':button, :submit, :reset, :hidden, :checkbox')
        .val('');
}

function ClearModelstateErrors() {
    $(".validation-summary-errors").remove();
    $(".field-validation-error").remove();
}

function ClearModelstateErrorsByFormId(formId) {
    $("#" + formId + " .validation-summary-errors").empty();
    $("#" + formId + " .field-validation-error").empty();
}

function RemoveModelstateErrorsByFormId(formId) {
    $("#" + formId + " .validation-summary-errors").remove();
    $("#" + formId + " .field-validation-error").remove();
}

function ClearFormModelstateErrors(formId) {
    $('#' + formId).validate().resetForm();
}

function SetVaidationForForm(formId) {
    $("#" + formId).each(function () {
        $(this).validate({
            errorElement: "span",
            errorPlacement: function (error, element) {
                var containingDiv = element.parent('div');
                if (containingDiv.hasClass('input-group')) {
                    error.insertAfter(containingDiv);
                }
                else {
                    let lastInp = containingDiv.find(':input').last();
                    error.insertAfter(lastInp);
                }
                error.addClass('message m-0 p-0');
                error.css('color', 'red');
                error.css('display', 'block');
            }
        });
    });
}

function SetValidationToForm(formId) {
    $('#' + formId).validate({
        errorPlacement: function (error, element) {
            if (element.hasClass('select2-hidden-accessible')) {
                error.insertAfter(element.next('.select2'));
            } else {
                error.insertAfter(element);
            }
        }
    });
}

function ValidateForm(formId)     // function to manually call validator
{
    $("#" + formId).each(function () {
        $(this).validate({
            errorElement: "div",
            wrapper: "div", // a wrapper around the error message
            errorPlacement: function (error, element) {
                offset = element.offset();
                error.insertAfter(element);
                error.addClass('message');
                error.css('color', 'red');
                error.css('display', 'block');
            },

            onfocusout: false,
            invalidHandler: function (form, validator) {
                var errors = validator.numberOfInvalids();
                if (errors) {
                    validator.errorList[0].element.focus();
                }
            }
        });
    });
}


function ApplyAllSelect2Item() {
    $('.select2-item-custom1-sm').select2({
        width: '100%',
        containerCssClass: "form-select form-select-sm full-radius pe-3 pb-0 h-100 custom-container-select2-sm",
        minimumResultsForSearch: -1
    });
}

function DestroyAndApplySelect2ItemInModal(itemId, parentId, selectData, formatterFunc, selectorFormatterFunc) {
    let $select = $('#' + itemId);
    $select.empty();
    $select.select2('destroy');

    $select.select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select full-radius p-0 h-100 custom-container-select2",
        minimumResultsForSearch: -1,
        data: selectData,
        templateResult: formatterFunc,
        templateSelection: selectorFormatterFunc
    });

    SetRepositionForSearchlessSelect2ItemInModal();
}

function DestroyAndApplySelect2SmItemInModal(itemId, parentId, selectData, formatterFunc, selectorFormatterFunc) {
    let $select = $('#' + itemId);
    $select.empty();
    $select.select2('destroy');

    $select.select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select form-select-sm full-radius p-0 h-100 custom-container-select2-sm",
        minimumResultsForSearch: -1,
        data: selectData,
        templateResult: formatterFunc,
        templateSelection: selectorFormatterFunc
    });

    SetRepositionForSearchlessSelect2ItemInModal();
}


function ApplyAllSelect2ItemInModal(parentId) {
    $('.select2-item-custom2').select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select full-radius p-0 h-100 custom-container-select2",
        minimumResultsForSearch: -1
    });

    $('.select2-item-custom2-sm').select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select form-select-sm full-radius p-0 h-100 custom-container-select2-sm",
        minimumResultsForSearch: -1
    });

    SetRepositionForSearchlessSelect2ItemInModal();
}

function SetRepositionForSearchlessSelect2ItemInModal() {
    $('.select2-item-custom2, .select2-item-custom2-sm').on('select2:open', function () {
        const select = $(this);
        requestAnimationFrame(() => {
            const container = $('.select2-container--open .select2-dropdown--above');
            if (container.length) {
                const instance = select.data('select2');
                if (instance?.dropdown?._positionDropdown) {
                    instance.dropdown._positionDropdown();
                }
            }
        });
    });
}

function ApplyAllSelect2ItemInModalWithSearch(parentId) {
    $('.select2-item-custom3').select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select full-radius p-0 h-100 custom-container-select2"
    });

    $('.select2-item-custom3-sm').select2({
        width: '100%',
        dropdownParent: $('#' + parentId),
        containerCssClass: "form-select form-select-sm full-radius p-0 h-100 custom-container-select2-sm"
    });
}


const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function validateEmail(email) {
    return emailRegex.test(email);
}

function StopEnterRedirect(inputId) {
    $('#' + inputId).on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
        }
    });
}

$(function () {
    $(document).on('click', 'li.nav-item.ppc-nav-item', function () {
        let parentItem = $(this).closest('ul.nav.nav-tabs');
        parentItem.find('span.btn-inside-nav-header').addClass('disabled');
        $(this).find('span.btn-inside-nav-header').removeClass('disabled');
    });

    $(document).on('click', 'li.nav-item.ppc-nav-item-offer', function () {
        let parentItem = $(this).closest('ul.nav.nav-tabs');
        parentItem.find('span.btn-inside-nav-header').addClass('disabled');
        $(this).find('span.btn-inside-nav-header').removeClass('disabled');
    });

    $(document).on('click', '.nav-item.ppc-nav-item', function () {
        AdjustDatatable();
    });

    $(document).on('click', '.nav-item.ppc-nav-item-offer', function () {
        AdjustDatatable();
    });

    $(document).on('click', '.custom-collapse-header-btn', function () {
        let parentDiv = $(this).closest('div.ppl-card');

        if (parentDiv.find('button.custom-arrow-btn').find('i').hasClass('fa-angle-down')) {
            parentDiv.find('button.custom-arrow-btn').find('i').removeClass('fa-angle-down').addClass('fa-angle-up');
        }
        else if (parentDiv.find('button.custom-arrow-btn').find('i').hasClass('fa-angle-up')) {
            parentDiv.find('button.custom-arrow-btn').find('i').removeClass('fa-angle-up').addClass('fa-angle-down');
        }
    });

    $(document).on('click', '.custom-arrow-btn', function () {
        if ($('i', this).hasClass('fa-angle-down')) {
            $('i', this).removeClass('fa-angle-down').addClass('fa-angle-up');
        }
        else if ($('i', this).hasClass('fa-angle-up')) {
            $('i', this).removeClass('fa-angle-up').addClass('fa-angle-down');
        }
    });


    $(document).on('click', '.custom-collapse-btn-link', function () {
        if ($(this).text() == 'see more') {
            $(this).text('see less');
        }
        else if ($(this).text() == 'see less') {
            $(this).text('see more');
        }
    });

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    $.ajaxSetup({
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                bootbox.alert({
                    message: "You have been logged out. Please login again.",
                    callback: function () {
                        window.location = '/Account/Login';
                    }
                });
            } else if (jqXHR.status === 0) {
                ShowPushMessage('Network Problem.', 'danger');
            } else if (jqXHR.status == 403) {
                ShowPushMessage("You don't have access of the following action.", 'danger');
            } else if (jqXHR.status == 404) {
                ShowPushMessage("Content not found.", 'danger');
            } else if (jqXHR.status == 500) {
                ShowPushMessage("Problem in processing the request.", 'danger');
            } else if (exception === 'timeout') {
                ShowPushMessage("Request timed out.", 'danger');
            } else if (exception === 'abort') {
                ShowPushMessage("Request aborted.", 'danger');
            } else if (jqXHR.responseText) {
                ShowPushMessage(jqXHR.responseText, 'danger');
            } else {
                ShowPushMessage("Error in completing the action.", 'danger');
            }
        }
    });

    /*************************** Jquery Validate ******************************/
    /**************************************************************************/

    $.validator.addMethod(  // validation for required (custom rule for class 'requiredField')
        "cRequired",
        $.validator.methods.required,
        "* required"
    );
    jQuery.validator.addClassRules('requiredField', {
        cRequired: true
    });

    $.validator.addMethod( // validation for min value of a number field
        "cMin",
        $.validator.methods.min,
        "minimum value is {0}"
    );
    $('input').on('click', function () {
        var fieldClass = $(this).attr("class");

        if (fieldClass && fieldClass.indexOf('min[') >= 0) { // class should be like min[4], min[5.07] etc
            var checkValue = parseFloat(fieldClass.trim().split('min[')[1].split(']')[0]);

            fieldClass = 'min[' + checkValue + ']';
            jQuery.validator.addClassRules(fieldClass, {
                cMin: checkValue
            });
        }
    });

    $.validator.addMethod( // validation for max value of a number field
        "cMax",
        $.validator.methods.max,
        "maximum value is {0}"
    );
    $('input').on('click', function () {
        var fieldClass = $(this).attr("class");

        if (fieldClass && fieldClass.indexOf('max[') >= 0) { // class should be like max[400], max[700.0207] etc
            var checkValue = parseFloat(fieldClass.trim().split('max[')[1].split(']')[0]);

            fieldClass = 'max[' + checkValue + ']';
            jQuery.validator.addClassRules(fieldClass, {
                cMax: checkValue
            });
        }
    });

    $.validator.addMethod( // validation for min length of a text field
        "cMinLength",
        $.validator.methods.minlength,
        "minimum length is {0}"
    );
    $('input').on('click', function () {
        var fieldClass = $(this).attr("class");

        if (fieldClass && fieldClass.indexOf('minlength[') >= 0) { // class should be like minlength[4], minlength[12] etc
            var checkValue = parseFloat(fieldClass.trim().split('minlength[')[1].split(']')[0]);

            fieldClass = 'minlength[' + checkValue + ']';
            jQuery.validator.addClassRules(fieldClass, {
                cMinLength: checkValue
            });
        }
    });

    $.validator.addMethod( // validation for max length of a text field
        "cMaxLength",
        $.validator.methods.maxlength,
        "maximum length is {0}"
    );
    $('input').on('click', function () {
        var fieldClass = $(this).attr("class");

        if (fieldClass && fieldClass.indexOf('maxlength[') >= 0) { // class should be like maxlength[4], maxlength[12] etc
            var checkValue = parseFloat(fieldClass.trim().split('maxlength[')[1].split(']')[0]);

            fieldClass = 'maxlength[' + checkValue + ']';
            jQuery.validator.addClassRules(fieldClass, {
                cMaxLength: checkValue
            });
        }
    });

    $.validator.addMethod( // validation for max length of a text field
        "cExactLength",
        function (value, element, param) {
            return this.optional(element) || value.length == param;
        },
        $.validator.format("accepted length is {0}")
    );
    $('input').on('click', function () {
        var fieldClass = $(this).attr("class");

        if (fieldClass && fieldClass.indexOf('exactlength[') >= 0) { // class should be like exactlength[4], exactlength[12] etc
            var checkValue = parseFloat(fieldClass.trim().split('exactlength[')[1].split(']')[0]);

            fieldClass = 'exactlength[' + checkValue + ']';
            jQuery.validator.addClassRules(fieldClass, {
                cExactLength: checkValue
            });
        }
    });


    $.validator.addMethod(      // validation for letter-space (custom rule for class 'letterField')
        'letterSpaceFieldRegEx',
        function (value) {
            return /^([a-zA-Z\ ])*$/.test(value); // only letters and spaces
        },
        'letters only'
    );
    jQuery.validator.addClassRules('letterField', {
        letterSpaceFieldRegEx: true
    });

    $.validator.addMethod(      // validation for letter-dot (custom rule for class 'letterDotField')
        'letterDotRegEx',
        function (value) {
            return /^([a-zA-Z\.\ ])*$/.test(value); // only letters, dots and spaces
        },
        'letters and dots only'
    );
    jQuery.validator.addClassRules('letterDotField', {
        letterDotRegEx: true
    });

    $.validator.addMethod(      // validation for letter-hyphen-dot (custom rule for class 'letterHyphenDotField')
        'letterHyphenDotRegEx',
        function (value) {
            return /^([a-zA-Z\-\.\ ])*$/.test(value); // only letters, hyphens, dots and spaces
        },
        'letters, hyphens and dots only'
    );
    jQuery.validator.addClassRules('letterHyphenDotField', {
        letterHyphenDotRegEx: true
    });


    $.validator.addMethod(      // validation for letter-digit-hyphen-dot (custom rule for class 'letterDigitHyphenDotField')
        'letterDigitHyphenDotRegEx',
        function (value) {
            return /^([0-9a-zA-Z\-\.\ ])*$/.test(value); // only letters, digits, hyphens, dots and spaces
        },
        'letters, digits, hyphens and dots only'
    );
    jQuery.validator.addClassRules('letterDigitHyphenDotField', {
        letterDigitHyphenDotRegEx: true
    });

    $.validator.addMethod(      // validation for letter-digit-hyphen-dot-underscore (custom rule for class 'letterDigitHyphenDotUnderscoreWithoutSpace')
        'letterDigitHyphenDotUnderscoreWithoutSpaceRegEx',
        function (value) {
            return /^([0-9a-zA-Z\-\.\_])*$/.test(value); // only letters, digits, hyphens, dots and underscores
        },
        'letters, digits, hyphens, dots and underscores only'
    );
    jQuery.validator.addClassRules('letterDigitHyphenDotUnderscoreWithoutSpace', {
        letterDigitHyphenDotUnderscoreWithoutSpaceRegEx: true
    });

    $.validator.addMethod(      // validation for letter-digit-hyphen-dot-underscore (custom rule for class 'letterDigitHyphenDotUnderscore')
        'letterDigitHyphenDotUnderscoreRegEx',
        function (value) {
            return /^([0-9a-zA-Z\-\.\_\ ])*$/.test(value); // only letters, digits, hyphens, dots, underscores and spaces
        },
        'letters, digits, hyphens, dots and underscores only'
    );
    jQuery.validator.addClassRules('letterDigitHyphenDotUnderscore', {
        letterDigitHyphenDotUnderscoreRegEx: true
    });

    $.validator.addMethod(      // validation for letter-digit (custom rule for class 'letterDigitField')
        'letterDigitFieldRegEx',
        function (value) {
            return /^([0-9a-zA-Z\ ])*$/.test(value); // letters, digits and spaces
        },
        'letters and digits only'
    );
    jQuery.validator.addClassRules('letterDigitField', {
        letterDigitFieldRegEx: true
    });

    $.validator.addMethod(  // validation for integer (custom rule for class 'integerField')
        "cDigits",
        $.validator.methods.digits,
        "digits only"
    );
    jQuery.validator.addClassRules('integerField', {
        cDigits: true
    });

    $.validator.addMethod(  // validation for decimal number (custom rule for class 'numberField')
        "cNumber",
        $.validator.methods.number,
        "invalid number"
    );
    jQuery.validator.addClassRules('numberField', {
        cNumber: true
    });

    $.validator.addMethod(      // validation for digit-hyphen-dot (custom rule for class 'digitHyphenDotField')
        'digitHyphenDotRegEx',
        function (value) {
            return /^([\d\-\.\ ])*$/.test(value); // only digits, hyphens, dots and spaces
        },
        'digits, hyphens and dots only'
    );
    jQuery.validator.addClassRules('digitHyphenDotField', {
        digitHyphenDotRegEx: true
    });

    $.validator.addMethod(      // validation for digit-hyphen-dot-character (custom rule for class 'digitHyphenDotLetterRegEx')
        'digitHyphenDotLetterRegEx',
        function (value) {
            return /^[a-zA-Z0-9\s\.\-]+$/.test(value); // letter, digits, hyphens and dots only
        },
        'letter, digits, hyphens and dots only'
    );
    jQuery.validator.addClassRules('digitHyphenDotLetterRegEx', {
        digitHyphenDotCharacterRegEx: true
    });

    $.validator.addMethod(      // validation for email (custom rule for class 'emailField')
        'emailFieldRegEx',
        function (value) {
            return /^((([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,})))?$/.test(value);
        },
        'invalid email'
    );
    jQuery.validator.addClassRules('emailField', {
        emailFieldRegEx: true
    });

    $.validator.addMethod(
        "dateFieldRegEx",
        function (value, element) {
            return this.optional(element) || /^(0?[1-9]|[12]\d|3[01])[\.\/\-](0?[1-9]|1[012])[\.\/\-]([12]\d)?(\d\d)$/.test(value);
        },
        "invalid date"
    );
    jQuery.validator.addClassRules('dateField', {
        dateFieldRegEx: true
    });

    $.validator.addMethod(
        "dateTimeFieldRegEx",
        function (value, element) {
            return this.optional(element) || /^(0?[1-9]|[12]\d|3[01])[\.\/\-](0?[1-9]|1[012])[\.\/\-]([12]\d)?(\d\d) ([01]\d|2[0-3]|[0-9])(:[0-5]\d){1,2}$/.test(value);
        },
        "invalid date and time"
    );
    jQuery.validator.addClassRules('dateTimeField', {
        dateTimeFieldRegEx: true
    });


    $.validator.addMethod(      // validation for password (custom rule for class 'passwordField')
        'passwordFieldRegEx',   // Should contain capital letter, small letter, number and special charecter.
        function (value) {
            return /^((?=[\x21-\x7E]*[0-9])(?=[\x21-\x7E]*[A-Z])(?=[\x21-\x7E]*[a-z])(?=[\x21-\x7E]*[\x21-\x2F|\x3A-\x40|\x5B-\x60|\x7B-\x7E])[\x21-\x7E]*)?$/.test(value);
        },
        'should contain capital letter, small letter, number and special charecter'
    );
    jQuery.validator.addClassRules('passwordField', {
        passwordFieldRegEx: true
    });

    $.validator.addMethod(      // validation for phone number (custom rule for class 'phoneField')
        'phoneFieldRegEx',
        function (value) {
            return /^((?:([\+]880[\ \-]?)|0)\d{10})?$/.test(value);
        },
        'invalid phone number'
    );
    jQuery.validator.addClassRules('phoneField', {
        phoneFieldRegEx: true
    });

    $.validator.addMethod(      // validation for web url (custom rule for class 'webUrlField')
        'webUrlFieldRegEx',
        function (value) {
            return /^((https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?)?$/i.test(value);
        },
        'invalid web url'
    );
    jQuery.validator.addClassRules('webUrlField', {
        webUrlFieldRegEx: true
    });

});

function HandleDatatableError(jqXHR, exception) {
    if (jqXHR.status === 0) {
        ShowPushMessage('Network Problem.', 'danger');
    }
    else if (jqXHR.status == 400) {
        ShowPushMessage(JSON.parse(jqXHR.responseText), 'danger');
    }
    else if (jqXHR.status == 401) {
        bootbox.alert({
            message: "You have been logged out. Please login again.",
            callback: function () {
                window.location = '/Account/Login';
            }
        });
    }
    else if (jqXHR.status == 403) {
        ShowPushMessage("You don't have access of the following action.", 'danger');
    }
    else if (jqXHR.status == 404) {
        ShowPushMessage("Content not found.", 'danger');
    }
    else if (jqXHR.status == 500) {
        ShowPushMessage("Problem in processing the request.", 'danger');
    }
    else if (exception === 'timeout') {
        ShowPushMessage("Request timed out.", 'danger');
    }
    else if (exception === 'abort') {
        ShowPushMessage("Request aborted.", 'danger');
    }
    else {
        ShowPushMessage("Error in completing the action.", 'danger');
    }
}