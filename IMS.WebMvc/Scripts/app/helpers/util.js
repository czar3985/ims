var util = (function () {
    function getNumber(source) {
        if (source === undefined)
            return -1;
        var num = parseInt(source);
        if (isNaN(num) == true)
            return -1;
        return num;
    }
    function getPrePath() {
        //return "/IMS";
        return "";
    }
    return {
        getNumber: getNumber,
        getPrePath: getPrePath
    };
})();
//# sourceMappingURL=util.js.map