说明：	联想输入控件，输入两位以上字符，敲击回车或者离开焦点 触发change事件时，会从后台检索相关信息，并在下拉列表中显示备选信息，供用户点取。
引入：	'Shared/LenovoInputer/LenovoInputerView',
初始化：var inputer=new LenovoInputerView({
                    'default':'admin',
                    'placeholder': '请输入责任人，不填写则登记为公共客户',
                    'datasource': 'User'，
					'showValue':true
                });
		最后调用inputer.render()，将返回值插入dom。
参数：	default-初始值，会根据这个初始值从后台调取信息，显示
		placeholder-值为空时，文本框显示信息内容
		datasource-【必填值】 数据源，后台根据此参数判断该取什么数据，如'User'代表从用户表取数据，具体值需跟后台协商 
		showValue:最终显示结果是否显示【值】，如showValue为true  显示内容为 0001-user1   ，为false 显示内容为user1
方法：
	SetValue（string） 
		向控件传送一个值，并从后台检索出信息显示
	Value()
		获取控件当前Value值
	Display()
		获取控件当前Display值
	Reset()
		清空控件值