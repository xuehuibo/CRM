define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'UserCode': null,
            'UserName': null,
            'DeptCode': null,
            'DeptName': null,
            'GroupCode': null,
            'GroupName': null,
            'Enabled': null,
            'BuildDate': null,
            'BuildUser': null,
            'EditDate': null,
            'EditUser': null
        },
        idAttribute: 'Id',
        urlRoot: '/api/UserApi'
    });
});