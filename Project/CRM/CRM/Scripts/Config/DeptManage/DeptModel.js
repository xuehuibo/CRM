define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'DeptCode': null,
            'DeptName': null,
            'ParentCode': null,
            'Childs': []
        },
        urlRoot: '/api/DeptApi',
        idAttribute: 'Id'
    });
});