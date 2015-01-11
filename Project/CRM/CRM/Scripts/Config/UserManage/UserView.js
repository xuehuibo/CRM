define([
    'Shared/DeptSelecter/DeptSelecterView',
    'Config/DeptManage/DeptModel',
    'Config/UserGroupManage/UserGroupModel',
    'Shared/UserGroupSelecter/UserGroupSelecterView',
    'text!tpl/Config/UserManage/UserTpl.html',
    'md5',
    'HttpStatusHandle'
], function (DeptSelecterView, DeptModel, UserGroupModel, UserGroupSelecterView, tpl,md5, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'tr',
        initialize: function(model) {
            this.model = model;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
        },
        template: _.template(tpl),
        render: function() {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        BeginEdit: function () {
            this.$('[data-toggleedit=true]').toggleClass('hide');
            this.dept = new DeptModel();
            this.userGroup = new UserGroupModel();
            this.$('.deptSelecter').html(new DeptSelecterView(this.dept).render());
            this.$('.userGroupSelecter').html(new UserGroupSelecterView(this.userGroup).render());
        },
        EndEdit: function() {
            this.$('[data-toggleedit=true]').toggleClass('hide');
        },
        events: {
            'click .edit': 'BeginEdit',
            'click .del': 'Del',
            'click .status': 'SetStatus',
            'click .cancel': 'EditCancel',
            'click .save':'EditSave'
        },
        EditCancel: function() {
            this.dept.destroy();
            this.userGroup.destroy();
            this.model.destroy();
        },
        EditSave: function () {
            var me = this;
            this.$('.save').button('loading');
            this.$('.cancel').button('loading');
            this.model.save({
                    'UserCode': this.$('.userCode').val(),
                    'UserName': this.$('.userName').val(),
                    'DeptCode': this.dept.get('DeptCode'),
                    'GroupCode': this.userGroup.get('GroupCode'),
                    'Md5': md5.hexMd5(this.$('.userCode').val())
                },
                {
                    success: function() {
                        me.$('.save').button('reset');
                        me.$('.cancel').button('reset');
                        me.dept.destroy();
                        me.userGroup.destroy();
                        me.EndEdit();
                    },
                    error: function(model, rst) {
                        me.$('.save').button('reset');
                        me.$('.cancel').button('reset');
                        HttpStatusHandle(rst, '保存用户数据');
                    },
                    wait:true
                }
            );
        }

    });
});