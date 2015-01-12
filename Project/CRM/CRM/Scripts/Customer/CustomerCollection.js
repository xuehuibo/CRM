define(['Customer/CustomerModel'], function(CustomerModel) {
    return Backbone.Collection.extend({
        model: CustomerModel,
        url: '/api/CustomerApi'
    });
});