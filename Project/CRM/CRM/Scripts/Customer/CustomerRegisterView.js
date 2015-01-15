define([
        'Customer/CustomerModel',
        'Shared/CheckInputer/CheckInputerView',
        'Shared/LenovoInputer/LenovoInputerView',
        'HttpStatusHandle'
    ],
    function (CustomerModel,CheckInputerView, LenovoInputerView, HttpStatusHandle) {
        return Backbone.View.extend({
            initialize: function() {
                this.model = new CustomerModel();
                this.custCode = new CheckInputerView({
                    'dataSource': 'Customer.CustomerCode',
                    'placeHolder':'请输入供应商编码'
                });
                this.owner = new LenovoInputerView({
                    'placeholder': '请输入责任人，不填写则登记为公共客户',
                    'dataSource': 'User'
                });
            },
            render: function () {
                this.$('#customerCode').append(this.custCode.render());
                this.$('#owner').append(this.owner.render());
            },
            events: {
                'click #btnSave': 'Save',
                'click #btnCancel': 'Cancel',
                'click #assignToMe': 'AssignToMe'
            },
            Save: function() {
                if (this.custCode.Status() == 0) {
                    return;
                }
                if (this.$('#customerName').val() == '') {
                    alert('客户名称不能为空！');
                    return;
                }
                this.$('#btnSave').button('loading');
                this.$('#btnCancel').addClass('disabled');
                var me = this;
                this.model.save({
                    'CustomerCode': this.custCode.Value(),
                    'CustomerName': this.$('#customerName').val(),
                    'Remark': this.$('#remark').val(),
                    'Owner': this.owner.Value(),
                    'Enabled':true
                }, {
                    success: function() {
                        me.$('#btnSave').button('reset');
                        me.$('#btnCancel').removeClass('disabled');
                        if (confirm("用户创建成功，是否添加联系人？")) {
                            window.AppRouter.navigate('#goto/Customer/ContactRegister/customerId=' + me.model.get('Id'), true);
                        } else {
                            me.model.set({
                                'Id': null,
                                'CustomerCode': null,
                                'CustomerName':null,
                            });
                            me.Cancel();
                        }
                    },
                    error: function (model, rst) {
                        me.$('#btnSave').button('reset');
                        me.$('#btnCancel').removeClass('disabled');
                        HttpStatusHandle(rst, '保存客户信息');
                    }
                });
            },
            Cancel: function() {
                this.owner.Reset();
                this.custCode.Reset();
                this.model.set({
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
                    'Enabled': false
                });
                this.$('#customerName').val('');
                this.$('#remark').val('');

            },
            AssignToMe: function () {
                this.owner.Value(window.signUser.UserCode);
            }
        });
    });