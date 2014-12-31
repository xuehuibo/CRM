﻿define(['Home/AuthModel'], function (AuthModel) {
    return Backbone.View.extend({
        initialize: function () {
            this.model = new AuthModel();
        },
        events: {
            'click #btnSignin': 'Signin',
            'change #remain':'RemainCheck'
        },
        Signin: function () {
            console.info('click');
            if ($('#userCode').val() == '' || $('#uPwd').val() == '') {
                alert("用户名和密码不能为空！");
                return;
            }
            $('#btnSignin').button('loading');
            //登录
            this.model.save({
                'UserCode': $('#userCode').val(),
                'UPwd': $('#uPwd').val(),
                'Remain': document.getElementById("remain").checked
            }, {
                success: function (model) {
                    location.href = "/Home";
                },
                error: function (model, rst) {
                    $('#btnSignin').button('reset');
                    if (rst.statusText == "Data Not Found") {
                        alert("用户名或密码错误！");
                    } else {
                        alert(rst.statusText);
                    }
                },
                wait:true
            });
        },
        RemainCheck:function() {
            if (!document.getElementById("remain").checked) {
                //取消保持登录，清除Token
                console.info(document.cookie);
                document.cookie = document.cookie + ';path=/;expire=' + new Date(0).toGMTString();
            }
        }
    });
});