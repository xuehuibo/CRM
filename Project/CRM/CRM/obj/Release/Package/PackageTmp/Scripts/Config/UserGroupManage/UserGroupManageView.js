define([
        'Config/UserGroupManage/UserGroupCollection',
        'Config/UserGroupManage/UserGroupView',
        'HttpStatusHandle'
    ],
    function (UserGroupCollection, UserGroupView, HttpStatusHandle) {
        return Backbone.View.extend({
            initialize: function() {
                this.userGroups = new UserGroupCollection();
                this.listenTo(this.userGroups, 'add', this.AddOne);
                this.userGroups.fetch({
                    error:function(model, rst) {
                        HttpStatusHandle(rst, '读取用户组');
                    }
                });
            },
            AddOne:function(userGroup) {
                this.$('#userGrpPanel').append(new UserGroupView(userGroup).render());
            }
        });
    });