define([
        'text!tpl/Config/DeptManage/DeptTpl.html'
    ],
    function (tpl) {
        return Backbone.View.extend({
            tagName: "li",
            initialize: function(model) {
                this.model = model;
                this.listenTo(this.model, 'destroy', this.remove);
                this.listenTo(this.model, 'change', this.render);
            },
            template: _.template(tpl),
            render: function() {
                this.$el.html(this.template(this.model.toJSON()));
                return this.el;
            },
            AddChild: function(childView) {
                this.$('ul:first').append(childView.render());
            },
            events: {
                'click span:first': 'ShowBtn',
                'click .deptAdd:first': 'ClickAdd',
                'click .deptDel:first': 'ClickDel',
                'click .deptEdit:first': 'ClickEdit',
                'click .btn-group:first':'ClickBtn'
            },
            ShowBtn: function() {
                $('.btn-group').addClass('hide');
                this.$('.btn-group:first').toggleClass('hide');
            },
            ClickAdd:function() {
                console.info(this.model.toJSON());
            },
            ClickDel: function () {
                console.info(this.model.toJSON());
                this.model.destroy();
            },
            ClickEdit:function() {
                console.info(this.model.toJSON());
            },
            ClickBtn: function () {
                this.$('.btn-group:first').addClass('hide');
            }
        });
    });