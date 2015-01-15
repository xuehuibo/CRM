define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'CustomerCode': null,
            'Name': null,
            'Phone1': null,
            'Phone2': null,
            'Email': null,
            'Remark': null,
            'BuildDate': null,
            'BuildUser': null,
            'EditDate': null,
            'EditUser': null,
            'SerialNo': null,
            'Enabled':false
        },
        idAttribute: 'Id',
        urlRoot:'/api/CustContactApi'
    });
});