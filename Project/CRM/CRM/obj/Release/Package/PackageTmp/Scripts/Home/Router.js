define([], function() {
    return Backbone.Router.extend({
        initialize:function() {
            Backbone.history.start();
        },
        routes: {
            '': 'Index',
            'goto/:controller/:action': 'Goto',
            'goto/:controller/:action/:param': 'GotoWithParam',
            '*error': function(error) {
                alert('error hash: ' + error);
            }
        },
        Index: function() {
            $('#mainPanel').load('/Home/HomePage');
        },
        Goto: function (controller, action) {
            $('#mainPanel').load(controller + '/' + action);
        },
        GotoWithParam: function (controller, action, param) {
            var url = controller + '/' + action;
            if (param != undefined && param != '') {
                url = url + '?' + param;
            }
            $('#mainPanel').load(url);
        }
    });
});