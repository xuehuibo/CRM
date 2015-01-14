define(['Shared/LenovoInputer/OptionCollection', 'Shared/LenovoInputer/OptionView', 'HttpStatusHandle', 'text!tpl/Shared/LenovoInputer/LenovoInputerTpl.html'], function (OptionCollection, OptionView, HttpStatusHandle,tpl) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'dropdown',
        //初始化函数，输入OptionModel,此model会作为最终结果输出，输入远程url
        initialize: function (model,placeHolder,dataSource) {
            this.model = model;
            this.dataSource = dataSource;
            this.placeHolder = placeHolder;
            this.collection = new OptionCollection();
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.collection, 'add', this.AddOne);
            this.listenTo(this.collection, 'reset', this.Clear);
        },
        template: _.template(tpl),
        render: function () {
            this.$el.html(this.template({
                    'Option': this.model.toJSON(),
                    'PlaceHolder': this.placeHolder
                })
            );
            return this.el;
        },
        AddOne:function(option) {
            this.$('ul').append(new OptionView(option).render());
        },
        Clear: function () {
            this.$('ul').empty();
            //this.collection.each(this.AddOne, this);
        },
        events: {
            'change input': 'ConditionChanged',
            'click li':'ClickOption'
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
                this.model.set({
                    'Display':null,
                    'Value': null,
                    'Stamp': new Date().getTime()
                });
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
                    'dataSource':this.dataSource,
                    'condition': this.$('input').val()
                },
                success: function (model, rst) {
                    self.ShowOptions();
                },
                error:function(model,rst) {
                    HttpStatusHandle(rst,'');
                },
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
            this.collection.reset();
            this.HideOptions();
        }
    });
})