define(['Customer/CustContactModel'], function(CustContactModel) {
    return Backbone.Collection.extend({
        model: CustContactModel,
        url: '/api/CustContactApi'
    });
});