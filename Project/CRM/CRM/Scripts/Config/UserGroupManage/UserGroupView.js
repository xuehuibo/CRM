define([
    'Config/UserGroupManage/UserGroupFunView',
    'Config/UserGroupManage/UserGroupFunModel',
    'text!tpl/Config/UserGroupManage/UserGroupTpl.html',
    'HttpStatusHandle'
],
    function (UserGroupFunView, UserGroupFunModel, tpl, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'panel panel-warning',
        initialize:function(userGroup) {
            this.model = userGroup;
            this.funs = [];
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.move);
        },
        template:_.template(tpl),
        render: function () {
            var fun = 0;
            for (var j = 0; j < this.model.get('UserGroupFun').length; j++) {
                if (this.model.get('UserGroupFun').Queriable) {
                    fun++;
                }
            }
            this.$el.html(this.template({
                'UserGroup': this.model.toJSON(),
                'Fun':fun
            }));
            var userGroupFun = this.model.get('UserGroupFun');
            for (var i = 0; i < userGroupFun.length; i++) {
                var userGroupFunModel = new UserGroupFunModel(userGroupFun[i]);
                this.$('ul').append(new UserGroupFunView(userGroupFunModel).render());
            }
            return this.el;
        },
        events: {
            'click [data-action="edit"]': 'Edit',
            'click [data-action="cancel"]': 'Cencel',
            'click [data-action="addFun"]': 'AddFun',
            'click [data-action="remove"]':'Remove'
        },
        Edit: function () {
            //编辑用户组
            this.$('[data-role="btnGrp"]').toggleClass('hide');
            this.$('ul').toggleClass('hide');
            this.$('.form-group:first').toggleClass('hide');
            this.$('h4:first').toggleClass('hide');
            this.$('.panel-footer').toggleClass('hide');
        },
        Cencel: function () {
            console.info(this.$('[name="groupCode"]'));
            //取消编辑
            this.$('[data-role="btnGrp"]').toggleClass('hide');
            this.$('ul').toggleClass('hide');
            this.$('.panel-footer').toggleClass('hide');
            this.$('.form-group').toggleClass('hide');
            this.$('h4').toggleClass('hide');
        },
        AddFun:function() {
            //增加功能
        },
        Remove: function () {
            if (this.model.get('People') > 0) {
                if (!confirm("该用户组下还有员工，删除该组将使这些员工失去归属组。确定删除？")) {
                    return;
                }
            }
            //删除
            this.model.destroy(
            {
                error: function(model, rst) {
                    HttpStatusHandle(rst, '删除用户组');
                },
                wait:true
            });
        }
    });
});