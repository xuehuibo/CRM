require.config({
    baseUrl:'/Scripts/',
    paths: {
/*        'jquery': ['jquery'],
        'backbone': ['backbone'],
        'underscore': ['underscore'],
        'bootstrap': ['bootstrap']*/
    },
    shim: {
        'jquery': {
          exports:'$'  
        },
        'underscore': {
            exports:'_'
        },
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports:'Backbone'
        },
        'bootstrap': {
            deps: ['jquery', 'css!/Content/css/bootstrap.min.css', 'css!../Content/css/dashboard.css']
        }
    }
});
require(['jquery', 'underscore', 'bootstrap', 'backbone']);
