define(['text!tpl/Config/UserGroupManage/UserGroupFunTpl.html'], function(tpl) {
    return Backbone.View.extend({
        tagName: 'li',
        className: 'list-group-item',
        template:_.template(tpl),
        initialize: function (userGroupFunModel) {
            this.model = userGroupFunModel;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.move);
        },
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            this.$('label').tooltip();
            return this.el;
        }
    });
});