define([
    'text!Shared/CheckInputer/Tpls/CheckInputerTpl.html',
    'Shared/CheckInputer/CheckResultModel',
    'HttpStatusHandle'
], function (tpl,CheckResultModel, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'form-group',
        template: _.template(tpl),
        //setting 包含 dataSource 、allowEmpty
        initialize: function (setting) {
            this.setting = setting;
            if (this.setting == undefined) {
                alert('配置错误,配置信息为undefined');
                return;
            }
            if (this.setting.dataSource == undefined || setting.dataSource == null || setting.dataSource == '') {
                alert('配置错误,dataSource必须配置');
                return;
            }
            if (this.setting.allowEmpty == undefined) {
                this.setting.allowEmpty = false;
            }
            this.model = new CheckResultModel();
        },
        render: function() {
            this.$el.html(this.template());
            return this.el;
        },
        Error: function () {
            this.$el.addClass('form-group has-error has-feedback');
            this.$('label').removeClass('sr-only');
            this.$('span').removeClass();
            this.$('span').addClass('glyphicon form-control-feedback glyphicon-remove');
            this.$('label').text(this.model.get('ErrorText'));
        },
        Success: function () {
            this.$el.removeClass();
            this.$el.addClass('form-group has-success has-feedback');
            this.$('label').addClass('sr-only');
            this.$('span').removeClass();
            this.$('span').addClass('glyphicon form-control-feedback glyphicon-ok');
            this.$('label').text('');
        },
        Warning: function () {
            this.$el.removeClass();
            this.$el.addClass('form-group has-warning has-feedback');
            this.$('label').removeClass('sr-only');
            this.$('span').removeClass();
            this.$('span').addClass('glyphicon form-control-feedback glyphicon-warning-sign');
            this.$('label').text(this.model.get('ErrorText'));
        },
        Reset: function () {
            this.model.set({
                'Status': null,
                'ErrorText':null
            });
            this.$el.removeClass();
            this.$el.addClass('form-group');
            this.$('label').addClass('sr-only');
            this.$('span').addClass('hide');
        },
        events: {
            'change input':'ValueChanged'
        },
        ValueChanged: function () {
            if (this.$('input').val() == '') {
                if (this.setting.allowEmpty) {
                    this.Reset();
                } else {
                    this.model.set({
                        'Status': 2,
                        'ErrorText': '该项不允许为空'
                    });
                    this.Warning();
                }
                return;
            }
            var me = this;
            this.model.fetch({
                data: {
                    dataSource:this.setting.dataSource,
                    value: this.$('input').val()
                },
                success: function (model, rst) {
                    switch (rst.Status) {
                        case 0:
                        me.Error();
                        break;
                        case 1:
                        me.Success();
                        break;
                        case 2:
                        me.Warning();
                        break;
                    }
                },
                error: function (model, rst) {
                    HttpStatusHandle(rst);
                }
            });
        },
        Status: function () {
            if (this.$('input').val() == '' && !this.setting.allowEmpty) {
                this.model.set({
                    'Status': 2,
                    'ErrorText': '该项不允许为空'
                });
                this.Warning();
            }
            return this.model.get('Status');
        },
        Value:function() {
            return this.$('input').val();
        },
        SetValue:function(value) {
            this.$('input').val(value);
        },
        SetReadonly: function (status) {
            if (status) {
                this.$('input').attr('readonly', "readonly");
            } else {
                this.$('input').removeAttr('readonly');
            }
        }
    });
});