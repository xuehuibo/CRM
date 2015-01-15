define([
    'Shared/LenovoInputer/OptionModel',
    'Shared/LenovoInputer/OptionCollection', 
    'Shared/LenovoInputer/OptionView',
    'HttpStatusHandle',
    'text!Shared/LenovoInputer/Tpls/LenovoInputerTpl.html'],
    function (OptionModel,OptionCollection, OptionView, HttpStatusHandle,tpl) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'dropdown',
        events: {
            'change input': 'ConditionChanged',
            'click li': 'ClickOption'
        },
        //初始化函数，输入OptionModel,此model会作为最终结果输出，输入远程url
        //setting包含default, showValue, placeHolder, dataSource
        initialize: function(setting) {
            var me = this;
            this.setting = setting;
            if (this.setting == undefined) {
                alert('配置错误,配置信息为undefined');
                return;
            }
            if (this.setting.dataSource == undefined || setting.dataSource == null || setting.dataSource == '') {
                alert('配置错误,dataSource必须配置');
                return;
            }
            if (this.setting.showValue == undefined) {
                this.setting.showValue = false;
            }
            if (this.setting.readOnly == undefined) {
                this.Readonly(false);
            }
            this.model = new OptionModel();
            this.collection = new OptionCollection();
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.collection, 'add', this.AddOne);
            this.listenTo(this.collection, 'reset', this.Reset);
        },
        Reset:function() {
            this.model.set({
                'Value': null,
                'Display': null,
                'Stamp': null
            });
            this.$('ul').empty();
        },
        template: _.template(tpl),
        render: function () {
            this.$el.html(this.template({
                    'Option': this.model.toJSON(),
                    'PlaceHolder': this.setting.placeHolder,
                    'ShowValue': this.setting.showValue,
                    'ReadOnly':this.setting.readOnly
                })
            );
            return this.el;
        },
        AddOne:function(option) {
            this.$('ul').append(new OptionView(option).render());
        },
        Clear: function () {
            this.collection.reset();
        },
        ShowOptions:function() {
            this.$el.addClass('open');
        },
        HideOptions:function() {
            this.$el.removeClass('open');
        },
        ConditionChanged: function () {
            if (this.$('input').val() == '') {
                //条件为空，清空
                this.collection.reset();
                return;
            }
            if (this.$('input').val().length < 2) {
                alert('至少输入两个字符才能查询信息！');
                return;
            }
            var self = this;
            this.collection.fetch({
                data: {
                    'dataSource': this.setting.dataSource,
                    'condition': this.$('input').val()
                },
                success: function (model, rst) {
                    self.ShowOptions();
                },
                error:function(model,rst) {
                    HttpStatusHandle(rst,'');
                },
                //reset:true,
                wait:true,
                add: true,
                remove: true,
                merge:true
            });
        },
        ClickOption:function(e) {
            this.model.set({
                'Display': $(e.currentTarget).attr('Display'),
                'Value': $(e.currentTarget).attr('Value'),
                'Stamp': new Date().getTime()
            });
            this.HideOptions();
        },

        //以下为属性读写器
        Display:function() {
            return this.model.get('Display');
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

        Value: function (value) {
            if (value != undefined) {
                if (value == '') {
                    this.Reset();
                    return this.model.get('Value');
                }
                var me = this;
                this.collection.fetch({
                    data: {
                        'dataSource': this.setting.dataSource,
                        'condition': value
                    },
                    success: function(model, rst) {
                        var m = model.findWhere({ 'Value': value });
                        if (m == undefined) {
                            alert('未找到默认值');
                            return;
                        }
                        me.model.set(m.toJSON());
                    },
                    error: function(model, rst) {
                        HttpStatusHandle(rst, '');
                    },
                    wait: true,
                    add: true,
                    remove: true,
                    merge: true
                });
            }
            return this.model.get('Value');
        },
    });
})