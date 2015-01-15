define([
    'text!Shared/CheckInputer/Tpls/CheckInputerTpl.html',
    'Shared/CheckInputer/CheckResultModel',
    'HttpStatusHandle'
], function (tpl,CheckResultModel, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'form-group',
        template: _.template(tpl),
        events: {
            'change input': 'ValueChanged'
        },
        //setting 包含 dataSource 、allowEmpty,readOnly
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
            if (this.setting.readOnly == undefined) {
                this.Readonly(false);
            }
            this.model = new CheckResultModel();
        },
        render: function() {
            this.$el.html(this.template({
                'ReadyOnly': this.setting.readOnly,
                'Value':this.value
            }));
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
        ValueChanged: function () {
            this.value = this.$('input').val();
            if (this.value == '') {
                if (this.setting.allowEmpty) {
                    this.Reset();
                } else {
                    this.model.set({
                        'Status': 0,
                        'ErrorText': '该项不允许为空'
                    });
                    this.Error();
                }
                return;
            }
            var me = this;
            this.model.fetch({
                data: {
                    dataSource:this.setting.dataSource,
                    value: this.value
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
                    'Status': 0,
                    'ErrorText': '该项不允许为空'
                });
                this.Error();
            }
            return this.model.get('Status');
        },
        //以下为属性设置读取器
        Value: function (value) {
            if (value != undefined) {
                this.value = value;
                this.$('input').val(value);
            }
            return this.value;
        },
        Readonly: function (value) {
            if (value != undefined) {
                this.setting.readOnly = value;
                if (value) {
                    this.$('input').attr('readonly', "readonly");
                } else {
                    this.$('input').removeAttr('readonly');
                }
            }
            return this.setting.readOnly;
        },
        AllowEmpty: function (value) {
            if (value != undefined) {
                this.setting.allowEmpty = value;
            } 
            return this.setting.allowEmpty;
        }
    });
});