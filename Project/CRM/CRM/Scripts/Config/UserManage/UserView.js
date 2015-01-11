define(['text!tpl/Config/UserManage/UserTpl.html'], function(tpl) {
    return Backbone.View.extend({
        tagName:'tr',
        initialize:function(model) {
            this.model = model;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
        },
        template:_.template(tpl),
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        }
    });
});