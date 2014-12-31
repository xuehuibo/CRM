define([], function() {
    return function (rst) {
        if (rst.status == 200) {
            return true;
        }
        console.info(rst.statusText);
        switch (rst.statusText) {
            case 'System Exception':
                //BadRequest
                alert("系统发生异常！请联系管理员！");
                return false;
            case 'Sigin Failure':
                //NonAuthoritativeInformation 
                alert("登陆失效，请重新登陆！");
                location.href = "/Home/NoAuth";
                return false;
            case 'Data Not Found':
                alert("未查询到相关数据！");
                return false;
            case 'No Authority':
                alert("您没有权限访问该数据！");
                return false;
            default:
                alert("系统发生未知错误，请联系管理员！");
                return false;
        }
    }
});