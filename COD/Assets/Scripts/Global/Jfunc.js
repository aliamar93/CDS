//Notification Functions
var notificationType = {
    success: "success",
    error: "error",
    info : "info",
    warning : "warning"
}

function getColorByType(notifiy)
{
    var color = '';
    switch (notifiy)
    {
        case "success":
            color = '#62c83e';
            break;
        case "warning":
            color = '#dec580';
            break;
        case "error":
            color = '#fe6464';
            break;
        case "info":
            color = '#85abbd';
            break;
        default:
            color = '#85abbd';
    }
    return color;
}

function showNotification(type, heading, Msg)
{
    var bgLoader = getColorByType(type);
    $.toast({
        heading: heading,
        text: Msg,
        position: 'top-right',
        loaderBg: bgLoader,
        icon: type,
        hideAfter: 4000,
        stack: 6
    });
}


function showConfirmationModal(heading, message, _callback)
{
    swal({
        title: heading,
        text: message,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Confirm",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            _callback();
        } else {
            swal("Cancelled", "Your record is safe :)", "error");
        }
    });
}

function showAlertModal(heading, message, type)
{
    swal(heading, message, type);
}


function showLoader() {
    $(".preloader").fadeIn();
}

function hideLoader() {
    $(".preloader").fadeOut()
}

//Notification Functions End

 function tblToJson(rows) {
    var JsonArray = [];
    $(rows).each(function (i, e) {
        $(e).find("input[type=checkbox]").each(function (i, ee) {
            $(e).find("[name=" + ee.name + "][type=hidden]").val($(ee).prop("checked"));
        });
        $(e).find("input[type=radio]:checked").each(function (i, ee) {
            $(e).find("[name=" + ee.name + "][type=hidden]").val($(ee).val());
        });

        var JsonElem = {};
        $(e).find("input[type=hidden],input[type=text],input[type=Number],select,textarea").each(function (ii, ee) {
            JsonElem[ee.name] = $(ee).val();
        });
        JsonArray.push(JsonElem);
    });
    return JsonArray;
 }

 function disable_Controls(model) {
     $('#' + model + ' input,#' + model + ' select,#' + model + ' textarea').prop('disabled', true);
     $('#' + model + ' input[type=hidden]').prop('disabled', false);
     $('.bootstrap-select').attr('disabled', true);
     $('.bootstrap-select > button').css('cursor', 'not-allowed');
     $('.chosen-select').prop('disabled', true).trigger("chosen:updated");
     $('#' + model + ' .btnAction').addClass("hide");
 }

 function enable_Controls(model) {
     $('#' + model + ' input,#' + model + ' select,#' + model + ' textarea').prop('disabled', false);
     $('#' + model + ' input[type=hidden]').prop('disabled', false);
     $('.bootstrap-select').attr('disabled', false);
     $('.bootstrap-select > button').css('cursor', 'allowed');
     $('.chosen-select').prop('disabled', false).trigger("chosen:updated");
     $('#' + model + ' .btnAction').removeClass("hide");
 }

 function PadLeft(i, l, s) {
     var o = i.toString();
     if (!s) { s = '0'; }
     while (o.length < l) {
         o = s + o;
     }
     return o;
 };


 function onlyInteger(evet) {
     var charCode = (evet.which) ? evet.which : event.keyCode
     if (charCode != 9) {
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;
     }
     return true;
 }

 function onlyIntegerForMileage(evet, value) {
     if (value > 0) {
         var charCode = (evet.which) ? evet.which : event.keyCode
         if (charCode != 9) {
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;
         }
         return true;
     }
     else {
         return false;
     }
 }

 function checkIfEventExists(elemSelector, event) {
     var isEventAttach = false;

     $(elemSelector).each(function (i, elem) {
         var elem_events = $._data(elem, "events");

         if (elem_events != undefined && elem_events[event] != undefined) {
             isEventAttach = true;
         }
         else
             isEventAttach = false;
     });
     return isEventAttach;
 }


 function datetimepicker() {
     $('.datetimepicker').datetimepicker({ format: "DD-MMM-YYYY HH:mm" }); //, minDate: moment().toDate()
 }

 function datepicker() {
     $('.datepicker').datetimepicker({ format: "DD-MMM-YYYY" }); //, minDate: moment().toDate()
 }

 function datepickerDefault() {
     $('.datepickerDefault').datetimepicker({ format: "DD-MMM-YYYY" });
 }

 function dateMonth() {
     $('.dateMonth').datetimepicker({ format: "MMMM" });
 }
 function dateYear() {
     $('.dateYear').datetimepicker({ format: "YYYY" });
 }

 function tblToJson2(rows) {
     var JsonArray = [];
     $(rows).each(function (i, e) {
         $(e).find('.inputele').each(function (j, k)
         {
             $(k).find("input[type=checkbox]").each(function (j, ee) {
                 $(k).find("[name=" + ee.name + "][type=hidden]").val($(ee).prop("checked"));
             });
             $(k).find("input[type=radio]:checked").each(function (j, ee) {
                 $(k).find("[name=" + ee.name + "][type=hidden]").val($(ee).val());
             });

             var JsonElem = {};
             $(k).find("input[type=hidden],input[type=text],input[type=Number],select,textarea").each(function (ii, ee) {
                 JsonElem[ee.name] = $(ee).val();
             });
             JsonArray.push(JsonElem);
         });

        
     });
     return JsonArray;
 }

 function tblToJson3(rows) {
     var JsonArray = [];
     $(rows).each(function (i, e) {
         $(e).find('.permissionRow').each(function (j, k) {

             $(k).find("input[type=checkbox]").each(function (j, ee) {
                 $(k).find("[name=" + ee.name + "][type=hidden]").val($(ee).is(":checked"));
             });

             var JsonElem = {};
             //$(k).find("input[type=hidden],input[type=text],input[type=Number],input[type=checkbox],select,textarea").each(function (ii, ee) {
             //    JsonElem[ee.name] = $(ee).attr("type") == "checkbox" ? $(ee).is(":checked") : $(ee).val();
             //});
             $(k).find("input[type=hidden],input[type=text],input[type=Number],select,textarea").each(function (ii, ee) {
                 JsonElem[ee.name] = $(ee).val();
             });
             JsonArray.push(JsonElem);
         });


     });
     return JsonArray;
 }


//Re-Initiate Validation
 function validateModal(formID)
 {
     $.validator.unobtrusive.parse($("#" + formID));
     $("#" + formID).validate();
 }

 function onlyAlphabets(e, t) {
     try {
         if (window.event) {
             var charCode = window.event.keyCode;
         }
         else if (e) {
             var charCode = e.which;
         }
         else { return true; }
         if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
             return true;
         else
             return false;
     }
     catch (err) {
         alert(err.Description);
     }
 }
 function isNumberKey(evt) {
     var charCode = (evt.which) ? evt.which : event.keyCode
     if (charCode > 31 && (charCode < 48 || charCode > 57))
         return false;
     return true;
 }

 function AlphaNumCheck(e) {
     var charCode = (e.which) ? e.which : e.keyCode;
     if (charCode == 8) return true;

     var keynum;
     var keychar;
     var charcheck = /^[a-zA-Z0-9\s]+$/
     if (window.event) // IE
     {
         keynum = e.keyCode;
     }
     else {
         if (e.which) // Netscape/Firefox/Opera
         {
             keynum = e.which;
         }
         else return true;
     }

     keychar = String.fromCharCode(keynum);
     return charcheck.test(keychar);
 }

 function separatorWithoutDecimal(Str) {
     var num = Math.ceil(parseInt(Str)).toString();
     if (Number(num) > 999) {
         while (/(\d+)(\d{3})/.test(num)) {
             num = num.replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
         }
     }
     return num;

 }

 function separatorWithDecimal(Str) {
     var num = (parseFloat(Str)).toFixed(2).toString();
     if (Number(num) > 999) {
         while (/(\d+)(\d{3})/.test(num)) {
             num = num.replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
         }
     }
     return num;
 }

 function isdecimalKey(evt, element) {

     var charCode = (evt.which) ? evt.which : event.keyCode
     if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
         return false;
     else {
         var len = $(element).val().length;
         var index = $(element).val().indexOf('.');
         if (index > 0 && charCode == 46) {
             return false;
         }
         if (index > 0) {
             var CharAfterdot = (len + 1) - index;
             if (CharAfterdot > 3) {
                 return false;
             }
         }

     }
     return true;
 }


 function formatDate(date) {
     var monthNames = [
       "Jan", "Feb", "Mar",
       "Apr", "May", "Jun", "Jul",
       "Aug", "Sep", "Oct",
       "Nov", "Dec"
     ];

     var day = date.getDate();
     var monthIndex = date.getMonth();
     var year = date.getFullYear();

     return day + '-' + monthNames[monthIndex] + '-' + year;
 }
 function formatDatetoYYYYMMDD(date) {
     var d = new Date(date),
         month = '' + (d.getMonth() + 1),
         day = '' + d.getDate(),
         year = d.getFullYear();

     if (month.length < 2) month = '0' + month;
     if (day.length < 2) day = '0' + day;

     return [year, month, day].join('-');
 }

 var add_minutes = function (dt, minutes) {
     return new Date(dt.getTime() + minutes * 60000);
 }

