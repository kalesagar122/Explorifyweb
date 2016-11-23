$(document).ready(function () {
    function refreshTable(tableId, urlData) {
        $.getJSON(urlData, null, function (json) {
            var table = $(tableId).dataTable();
            var oSettings = table.fnSettings();

            table.fnClearTable(this);

            for (var i = 0; i < json.aaData.length; i++) {
                table.oApi._fnAddData(oSettings, json.aaData[i]);
            }

            oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
            table.fnDraw();
        });
    }

    $("#JobData").dataTable({
        "bServerSide": true,
        "sAjaxSource": "/PostJob/GetAjaxJobswData",
        "bProcessing": true,
        "aoColumns": [
                        {
                            "sName": "Posted Date"
                        },
                        {
                            "sName": "Expire Date"
                        },
                        {
                            "sName": "Job Title"
                        },
                        {
                            "sName": "Company Name"
                        },
						{ "sName": "Company Id" },
                        {    
                            "sName": "Id",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (oObj) {
                                return "<a class='btn btn-default btn-sm btn-icon icon-left ' href='/PostJob/ViewJobDetail?id=" + oObj.aData[5] + "' title='View'><i class='icon-eye-open'></i></a> <a class='delete btn btn-danger btn-sm btn-icon icon-left' href='/PostJob/DeleteJob?id=" + oObj.aData[5] + "' title='Delete'><i class='icon-remove'></i></a>";
                                //return "<div class='btn-group'><button class='btn btn-default btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='false'>" + $("#Actions").attr("value") + "<span class='caret'></span></button><ul class='dropdown-menu' role='menu'><li><a  href='/Offer/Create?ID=" + oObj.aData[5] + "' title='View'>" + $("#View").attr("value") + "</a></li><li><a href='/Offer/Create?ID=" + oObj.aData[5] + "' title='Edit'>" + $("#Edit").attr("value") + "</a></li><li><a class='deletecompany' href='/Offer/Delete?id=" + oObj.aData[5] + "' title='Delete'>" + $("#Delete").attr("value") + "</a></li></ul></div>";
                            }
                        }
        ],
        "fnDrawCallback": function () {
           

                $(".delete").unbind('click').bind('click', function () {
                    var turl = this.href;
                    var anchor = this;
                $("#deleteModal").modal("show");
                $("button#confirm").unbind('click').bind('click', function (e) {
                    $("#deleteModal").modal("hide");
                    myApp.showPleaseWait();
                    $.ajax({
                        type: "POST",
                        url: turl,
                        cache: false,
                        success: function (result) {
                            myApp.hidePleaseWait();
                            var opts;
                            if (result.success) {
                                opts = {
                                    "closeButton": true,
                                    "debug": false,
                                    "positionClass": "toast-bottom-left",
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "1000",
                                    "timeOut": "5000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };
                                toastr.success(result.message, "Successfully deleted!", opts);
                                refreshTable("#JobData", "/PostJob/GetAjaxJobswData");

                            } else {
                                opts = {
                                    "closeButton": true,
                                    "debug": false,
                                    "positionClass": "toast-top-full-width",
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "1000",
                                    "timeOut": "5000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };
                                toastr.error(result.message, "Error", opts);
                            }
                        },
                        error: function (jqXhr, exception) {
                            if (jqXhr.status === 0) {
                                alert("Not connect.\n Verify Network.");
                            } else if (jqXhr.status === 404) {
                                alert("Requested page not found. [404]");
                            } else if (jqXhr.status === 500) {
                                alert("Internal Server Error [500].");
                            } else if (exception === "parsererror") {
                                alert('Requested JSON parse failed.');
                            } else if (exception === "timeout") {
                                alert("Time out error.");
                            } else if (exception === 'abort') {
                                alert('Ajax request aborted.');
                            } else {
                                alert('Uncaught Error.\n' + jqXhr.responseText);
                            }
                            myApp.hidePleaseWait();
                        }
                    });
                    //refreshTableTypeOfGrow("#TypeOfGrowDataTable", "/TypeOfGrow/GetAjaxTypeOfGrowData");
                });
                return false;
            });
        }
    });
});