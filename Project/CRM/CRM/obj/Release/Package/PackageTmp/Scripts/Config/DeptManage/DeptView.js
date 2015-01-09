define([
        'text!tpl/Config/DeptManage/DeptTpl.html',
        'HttpStatusHandle',
        'Config/DeptManage/DeptModel'
    ],
    function (tpl, HttpStatusHandle, DeptModel) {
        var DeptView= Backbone.View.extend({
            tagName: "li",
            initialize: function(model) {
                this.model = model;
                this.listenTo(this.model, 'destroy', this.remove);
                this.listenTo(this.model, 'change', this.render);
            },
            template: _.template(tpl),
            render: function () {
                this.$el.html(this.template(this.model.toJSON()));
                return this.el;
            },
            AddChild: function (childView) {
                this.$('ul:first').append(childView.render());
            },
            events: {
                'click h4:first': 'ShowBtn',
                'click [data-action="add"]:first': 'ClickAdd',
                'click [data-action="del"]:first': 'ClickDel',
                'click [data-action="edit"]:first': 'ClickEdit',
                'click .btn-group:first': 'ClickBtn',
                'click [data-action="editOk"]:first': 'ClickEditOk',
                'click [data-action="editCancel"]:first':'ClickEditCancel'
            },
            ClickEditOk: function () {
                if (this.$('[name="deptCode"]:first').val() == '') {
                    alert('部门编码不允许为空！');
                    return;
                }
                if (this.$('[name="deptName"]:first').val() == '') {
                    alert('部门名称不允许为空！');
                    return;
                }
                this.model.save({
                    'DeptCode': this.$('[name="deptCode"]:first').val(),
                    'DeptName': this.$('[name="deptName"]:first').val()
                }, {
                    success: function() {
                        this.$('form:first').addClass('hide');
                    },
                    error: function(model,rst) {
                        HttpStatusHandle(rst, '编辑部门');
                    },
                    wait: true
            });
            },
            ClickEditCancel: function () {
                if (this.model.get('Id') == null) {
                    this.model.destroy();
                    return;
                }
                this.$('[name="deptCode"]:first').val(this.model.get('DeptCode'));
                this.$('[name="deptName"]:first').val(this.model.get('DeptName'));
                this.$('form:first').addClass('hide');
            },
            ShowBtn: function() {
                $('.btn-group').addClass('hide');
                this.$('.btn-group:first').toggleClass('hide');
            },
            ClickAdd: function () {
                //点击增加
                var deptView =new DeptView(new DeptModel({'ParentCode':this.model.get('DeptCode')}));
                this.AddChild(deptView);
                deptView.ClickEdit();
            },
            ClickDel: function () {
                if (this.model.get('DeptCode') == 'root') {
                    alert('根部门不允许删除！');
                    return;
                }
                if (this.model.get('People') > 0) {
                    if (!confirm("该部门下还有员工，删除该部门将使这些员工失去归属部门。确定删除？")) {
                        return;
                    }
                }
                //点击删除
                if (!this.$('ul:first').html()) {
                    this.model.destroy({
                        success:function(model, rst) {
                            
                        },
                        errot:function(model, rst) {
                            HttpStatusHandle(rst, '删除部门');
                        },
                        wait: true
                    });
                } else {
                    alert('该部门存在子部门，不允许删除！');
                }
            },
            ClickEdit: function () {
                //点击编辑
                this.$('form:first').removeClass('hide');
            },
            ClickBtn: function () {
                this.$('.btn-group:first').addClass('hide');
            }
        });
        return DeptView;
    });