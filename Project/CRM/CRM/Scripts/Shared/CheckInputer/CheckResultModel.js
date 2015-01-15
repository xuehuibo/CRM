define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Status': 2,
            'ErrorText': '当前项不允许为空'
        },
        urlRoot:'/api/CheckInputerApi'
    });
});