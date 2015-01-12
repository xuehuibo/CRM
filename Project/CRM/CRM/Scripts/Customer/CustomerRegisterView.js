define(['Customer/CustomerModel'], function(CustomerModel) {
    return Backbone.View.extend({
        initialize: function() {
            this.model = new CustomerModel();
            this.listenTo(this.model, 'change', this.render);
        },
        render: function() {

        }
    });
});