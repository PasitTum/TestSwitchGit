/* ถ้าใช้ angular ไม่ต้องเรียกไฟล์นี้ก็ได้ */

/* ---- Begin DateTimePicker ---- */
//$('.datepicker').datepicker({
//	format: 'dd/mm/yyyy',
//	todayBtn: true,
//	language: 'th',             //เปลี่ยน label ต่างของ ปฏิทิน ให้เป็น ภาษาไทย   (ต้องใช้ไฟล์ bootstrap-datepicker.th.min.js นี้ด้วย)
//	thaiyear: true,              //Set เป็นปี พ.ศ.
//	autoclose:true
//}).datepicker("setDate", "0");  //กำหนดเป็นวันปัจุบัน
$('.datepicker').datepicker({
    language: 'th-th',
    format: 'dd/mm/yyyy',
    autoclose: true
});

$("input[id$='_datetext']").datepicker().on('changeDate', function (ev) {
    // ev.date
    var objid = $(this).attr("id");
    var orgid = objid.replace("_datetext", "");
    var $dateObj = $("#" + orgid + '_datetext');
    var $timeObj = $("#" + orgid + '_timetext');
    var $hiddenObj = $("#" + orgid);
    ChangeHiddenDateTime($hiddenObj, $dateObj, $timeObj);
});

$("input[id$='_datetext']").change(function (e) {
    var objid = $(this).attr("id");
    var orgid = objid.replace("_datetext", "");
    var $dateObj = $("#" + orgid + '_datetext');
    var $timeObj = $("#" + orgid + '_timetext');
    var $hiddenObj = $("#" + orgid);
    ChangeHiddenDateTime($hiddenObj, $dateObj, $timeObj);
});

$("input[id$='_timetext']").change(function (e) {
    var objid = $(this).attr("id");
    var orgid = objid.replace("_timetext", "");
    var $dateObj = $("#" + orgid + '_datetext');
    var $timeObj = $("#" + orgid + '_timetext');
    var $hiddenObj = $("#" + orgid);
    ChangeHiddenDateTime($hiddenObj, $dateObj, $timeObj);
});

if ($('.timepicker').length) {
    $('.timepicker').datetimepicker({
        format: 'HH:mm:ss',
        keepOpen: false,
        debug: false
    });

    $("input[id$='_timetext']").datetimepicker().on('dp.change', function (ev) {
        // ev.date
        var objid = $(this).attr("id");
        var orgid = objid.replace("_timetext", "");
        var $dateObj = $("#" + orgid + '_datetext');
        var $timeObj = $("#" + orgid + '_timetext');
        var $hiddenObj = $("#" + orgid);
        ChangeHiddenDateTime($hiddenObj, $dateObj, $timeObj);
    });
}

ChangeHiddenDateTime = function ($hiddenObj, $dateObj, $timeObj) {
    var dateValue = "";
    var timeValue = "";
    if ($dateObj) {
        if ($dateObj.val()) {
            dateValue = $dateObj.data("datepicker").date.toISOString().split('T')[0];
        }
    }
    if ($timeObj) {
        timeValue = $timeObj.val();
    }
    var newValue = "";
    if (dateValue) {
        newValue = dateValue;
        if (timeValue) {
            newValue += " " + timeValue;
        }
    }
    if ($hiddenObj) {
        $hiddenObj.val(newValue);
    }
    if (angular) {
        angular.element($dateObj).triggerHandler('input');
        if ($timeObj) {
            angular.element($timeObj).triggerHandler('input');
        }
    }

}

$(".dp-icon").on("click",function (e) {
    inputObj = $(this).data("input");
    if (inputObj) {
        $("#" + inputObj).focus();
    }
});
/* ----End DateTimePicker ---- */