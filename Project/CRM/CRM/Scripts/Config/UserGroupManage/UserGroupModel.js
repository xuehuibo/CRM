define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'GroupCode': null,
            'GroupName': null,
            'UserGroupFun': [],
            'People':0
        },
        idAttribute: 'Id',
        urlRoot:'/api/UserGroupApi'
    });
});