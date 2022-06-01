toByte = function (value, unit) {
    var multiplier = 0;
    if (unit.startsWith("K") || unit.startsWith("k")) {
        multiplier = 1024;
    }
    else if (unit.startsWith("M") || unit.startsWith("m")) {
        multiplier = 1024 * 1024;
    }
    return value * multiplier;
};

function getChar(pKeyCode) {
    if (pKeyCode == 0) return '';
    else return String.fromCharCode(pKeyCode);
}

Character = {
    isKeyThaiCharacter: function (e) {
        var pKeyCode;
        if (window.event) {
            pKeyCode = e.keyCode;
        }
        else {
            pKeyCode = e.which;
        }
        if (pKeyCode == null) {
            return false;
        }
        var thai_char = /^[ก-๛. ]+$/;
        var eng_char = /^[A-Za-z0-9()]+$/;
        if (thai_char.test(getChar(pKeyCode))) {
            if (pKeyCode.toString() == "3647" || (parseInt(pKeyCode, 10) >= 3665 && parseInt(pKeyCode, 10) <= 3673)) {  // ฿,๑-๙
                return false;
            } else {
                return true;
            }
        }
        else if (pKeyCode.toString() == "0" || pKeyCode.toString() == "8") { //Other + BackSpace
            window.event.keyCode = 0;
        }
        return false;
    },
    isKeyNumberCharacter: function (e) {
        var pKeyCode;
        if (window.event) {
            pKeyCode = e.keyCode;
        }
        else {
            pKeyCode = e.which;
        }
        if (pKeyCode == null) {
            return false;
        }
        var num_char = /^[0-9]+$/;
        if (num_char.test(getChar(pKeyCode))) {
            return true;
        }
        else if (pKeyCode.toString() == "0" || pKeyCode.toString() == "8") { //Other + BackSpace
            window.event.keyCode = 0;
        }
        return false;
    },
    isKeyPointNumberCharacter: function (e) {
        var pKeyCode;
        if (window.event) {
            pKeyCode = e.keyCode;
        }
        else {
            pKeyCode = e.which;
        }
        if (pKeyCode == null) {
            return false;
        }
        var num_char = /^[0-9.]+$/;
        if (num_char.test(getChar(pKeyCode))) {
            return true;
        }
        else if (pKeyCode.toString() == "0" || pKeyCode.toString() == "8") { //Other + BackSpace
            window.event.keyCode = 0;
        }
        return false;
    },
    isKeyEngNumCharacter: function (e) {
        var pKeyCode;
        if (window.event) {
            pKeyCode = e.keyCode;
        }
        else {
            pKeyCode = e.which;
        }
        if (pKeyCode == null) {
            return false;
        }
        var eng_char = /^[A-Za-z0-9.-]+$/;
        if (eng_char.test(getChar(pKeyCode))) {
            return true;
        }
        else if (pKeyCode.toString() == "0" || pKeyCode.toString() == "8") { //Other + BackSpace
            window.event.keyCode = 0;
        }
        return false;
    },
    isKeyEngCharacter: function (e) {
        var pKeyCode;
        if (window.event) {
            pKeyCode = e.keyCode;
        }
        else {
            pKeyCode = e.which;
        }
        if (pKeyCode == null) {
            return false;
        }
        var eng_char = /^[A-Za-z]+$/;
        if (eng_char.test(getChar(pKeyCode))) {
            return true;
        }
        else if (pKeyCode.toString() == "0" || pKeyCode.toString() == "8") { //Other + BackSpace
            window.event.keyCode = 0;
        }
        return false;
    }
};