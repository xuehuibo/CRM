define(['jquery'], function() {
    var HttpStatusHandle = function (val,failedUrl) {
        switch (val) {
            case 200:
                return true;
            case 400:
                //BadRequest
                alert("系统异常");
                return false;
            case 203:
                //NonAuthoritativeInformation 
                alert("登陆失效");
                location.href = failedUrl;
                return false;
            case 204:
                //NoContent 
                alert("未找到数据");
                return false;
            case 406:
                //NotAcceptable
                alert("没有权限");
                return false;
            default:
                alert("未知错误");
                return false;
        }
    }
    return HttpStatusHandle;
});