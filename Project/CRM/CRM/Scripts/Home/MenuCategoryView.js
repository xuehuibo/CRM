define(['text!Home/Tpls/MenuCategoryTpl.html'], function (tpl) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'panel panel-success',
        initialize:function(menus) {
            this.model = menus;
        },
        template: _.template(tpl),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        }
    });
});