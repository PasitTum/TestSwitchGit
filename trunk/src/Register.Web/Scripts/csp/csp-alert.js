cspAlert = function (message, title, icon, confirmButtonText) {
    if (!(confirmButtonText)) confirmButtonText = "ปิด";
    if (typeof Swal !== 'undefined') {
        return Swal.fire({
            title: title,
            // text: message,
            html : message,
            icon: icon,
            confirmButtonText: confirmButtonText
        });
    } else {
        return alert(message);
        //var promise = new Promise(function (resolve, reject) {
        //    // resolve({ value: true });
        //});
        //return promise;
    }
};

alert = function (message) {
    return cspAlert(message, undefined, "info", undefined);
};

// title จะส่งมาหรือไม่ส่งก็ได้
alertSuccess = function (message, title) {
    return cspAlert(message, title, "success", undefined);
}

// title จะส่งมาหรือไม่ส่งก็ได้
alertError = function (message, title) {
    if (!(title)) title = "เกิดข้อผิดพลาด";
    return cspAlert(message, title, "error", undefined);
}

// title จะส่งมาหรือไม่ส่งก็ได้
alertWarning = function (message, title) {
    return cspAlert(message, title, "warning", undefined);
}

cspConfirm = function (message, title, okText, cancelText) {
    if (!(okText)) okText = "ตกลง";
    if (!(cancelText)) cancelText = "ยกเลิก";
    if (typeof Swal !== 'undefined') {
        return Swal.fire({
            title: title,
            //text: message,
            html : message,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: okText,
            cancelButtonText: cancelText,
            customClass: {
                confirmButton: 'btn btn-success',
                cancelButton: 'ml-3 btn btn-danger'
            },
            buttonsStyling: false,
            allowOutsideClick: false,
            allowEscapeKey: false
        });
        //    .then((result) => {
        //    if (result.value) {
        //        Swal.fire(
        //          'Deleted!',
        //          'Your file has been deleted.',
        //          'success'
        //        )
        //    }
        //});
    } else {
        var promise = new Promise(function (resolve, reject) {
            // do a thing, possibly async, then…
            if (confirm(message)) {
                resolve({ value: true });
            }
            else {
                resolve({ value: false });
                //reject(Error("It broke"));
                //reject({ value: false });
            }
        });
        return promise;
    }
};

/*
วิธีใช้ cspConfirm
cspConfirm('ยืนยัน ?')
            .then((result) => {
            if (result.value) {
               // ok di something
            }
        });
*/