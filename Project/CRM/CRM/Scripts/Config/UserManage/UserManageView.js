define([
    'Config/UserManage/UserView',
    'Config/UserManage/UserModel',
    'Config/UserManage/UserCollection',
    'HttpStatusHandle'
], function (UserView, UserModel, UserCollection, HttpStatusHandle) {
    return Backbone.View.extend({
        initialize:function() {
            this.userCollection = new UserCollection();
            this.listenTo(this.userCollection, 'add', this.AddOne);
            this.listenTo(this.userCollection, 'destroy', this.AddAll);
            this.userCollection.fetch({
                success:function() {
                    
                },
                error:function(model,rst) {
                    HttpStatusHandle(rst, '读取用户信息');
                }
            });
        },
        AddOne: function (user) {
            this.$('tbody').append(new UserView(user).render());
        },
        AddAll: function () {
            this.$('tbody').empty();
            this.userCollection.each(this.AddOne, this);
        }
    });
});