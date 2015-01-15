define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Status': 1,
            'ErrorText': '当前项不允许为空'
        },
        urlRoot:'/api/CheckInputerApi'
    });
});