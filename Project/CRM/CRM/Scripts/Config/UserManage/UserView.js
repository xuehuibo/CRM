define([
    'Shared/LenovoInputer/LenovoInputerView',
    'Shared/CheckInputer/CheckInputerView',
    'text!Config/UserManage/Tpls/UserTpl.html',
    'md5',
    'HttpStatusHandle'
], function (LenovoInputerView, CheckInputerView,tpl, md5, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'tr',
        initialize: function(model) {
            this.model = model;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
        },
        template: _.template(tpl),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        BeginEdit: function () {
            this.$('[data-toggleedit=true]').toggleClass('hide');
            this.userCode = new CheckInputerView({
                'dataSource': 'User.UserCode',
                'readOnly': this.model.get('Id') != null
            }),
            this.userCode.Value(this.model.get('UserCode'));
            this.dept = new LenovoInputerView({
                'dataSource': 'Dept',
                'showValue': true,
                'placeHolder':'请输入部门编码或名称'
            });
            this.dept.Value(this.model.get('DeptCode'));
            this.userGroup = new LenovoInputerView({
                'dataSource': 'UserGroup',
                'showValue': true,
                'placeHolder': '请输入用户组编码或名称'
            });
            this.userGroup.Value(this.model.get('GroupCode'));
            this.$('.userCode').html(this.userCode.render());
            this.$('.deptSelecter').html(this.dept.render());
            this.$('.userGroupSelecter').html(this.userGroup.render());
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
        EditCancel: function () {
            this.userCode.remove();
            this.dept.remove();
            this.userGroup.remove();
            if (this.model.get('Id') == null) {
                this.model.destroy();
            } else {
                this.EndEdit();
            }
        },
        EditSave: function () {
            if (this.userCode.Status() == 0) {
                return;
            }
            if (this.$('.userName').val() == '') {
                alert('用户名称必须输入！');
                return;
            }
            var me = this;
            this.$('.save').button('loading');
            this.$('.cancel').addClass('disabled');
            this.model.save({
                    'UserCode': this.userCode.Value(),
                    'UserName': this.$('.userName').val(),
                    'DeptCode': this.dept.Value(),
                    'DeptName': this.dept.Display(),
                    'GroupCode': this.userGroup.Value(),
                    'GroupName': this.userGroup.Display(),
                    'Md5': md5.hexMd5(this.$('.userCode').val())
                },
                {
                    success: function() {
                        me.dept.remove();
                        me.userGroup.remove();
                    },
                    error: function(model, rst) {
                        me.$('.save').button('reset');
                        this.$('.cancel').removeClass('disabled');
                        HttpStatusHandle(rst, '保存用户数据');
                    },
                    wait: true,
                    change:true
                }
            );
        },
        Del:function() {
            if (!confirm('确定删除该用户？')) {
                return;
            }
            this.$('.edit').addClass('disabled');
            this.$('.status').addClass('disabled');
            this.$('.del').button('loading');
            this.model.destroy({
                error: function(model, rst) {
                    this.$('.edit').removeClass('disabled');
                    this.$('.status').removeClass('disabled');
                    this.$('.del').button('reset');
                    HttpStatusHandle(rst, '删除用户');
                },
                wait:true
            });
        },
        SetStatus:function() {
            var enabled = this.model.get('Enabled');
            this.$('.edit').addClass('disabled');
            this.$('.del').addClass('disabled');
            this.model.save({ 'Enabled': !enabled },
            {
                success:function() {
                    this.$('.edit').removeClass('disabled');
                    this.$('.del').removeClass('disabled');
                },
                error: function(model, rst) {
                    this.$('.edit').removeClass('disabled');
                    this.$('.del').removeClass('disabled');
                    HttpStatusHandle(rst, '修改用户状态');
                },
                wait: true
            });
        }
    });
});