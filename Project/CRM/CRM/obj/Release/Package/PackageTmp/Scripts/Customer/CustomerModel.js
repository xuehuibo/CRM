define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'CustomerCode': null,
            'CustomerName': null,
            'Status': null,
            'Remark': null,
            'BuildDate': null,
            'BuildUser': null,
            'EditDate': null,
            'EditUser': null,
            'Owner': null,
            'Enabled':false
        },
        idAttribute: 'Id',
        urlRoot:'/api/CustomerApi'
    });
})