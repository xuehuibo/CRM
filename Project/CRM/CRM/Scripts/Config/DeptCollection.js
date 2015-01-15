define(['Config/DeptModel'], function (DeptModel) {
    return Backbone.Collection.extend({
        model: DeptModel,
        url:'/api/DeptApi'
    });
});