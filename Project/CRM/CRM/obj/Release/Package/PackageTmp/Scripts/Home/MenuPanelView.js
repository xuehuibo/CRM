define(['Home/MenuCategoryView', 'Home/MenuCategoryCollection'], function (MenuCategoryView, MenuCategoryCollection) {
    return Backbone.View.extend({
        initialize: function () {
            var me = this;
            menuCategorys = new MenuCategoryCollection();
            menuCategorys.fetch({
                data: {
                    'allMenu': true
                },
                success: function (model, rst) {
                    for (var i = 0; i < model.length; i++) {
                        me.AddMenuCategory(model.at(i));
                    }
                }
            });
        },
        AddMenuCategory: function (menuCategory) {
            
            this.$el.append(new MenuCategoryView(menuCategory).render());
        }
    });
})