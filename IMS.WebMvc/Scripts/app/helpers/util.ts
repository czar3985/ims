var util = (function () {
    function getNumber(source: any): number {
        if (source === undefined)
            return -1;

        var num: number = parseInt(source);
        if (isNaN(num) == true)
            return -1;

        return num;
    }

    function getPrePath(): string {
        //return "/IMS";
        return "";
    }

    return {
        getNumber: getNumber,
        getPrePath: getPrePath
    }
})();