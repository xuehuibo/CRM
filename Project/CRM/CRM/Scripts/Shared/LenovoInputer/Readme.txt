说明：	联想输入控件，输入两位以上字符，敲击回车或者离开焦点 触发change事件时，会从后台检索相关信息，并在下拉列表中显示备选信息，供用户点取。
引入：	'Shared/LenovoInputer/LenovoInputerView',
初始化：var inputer=new LenovoInputerView({
                    'placeHolder': '请输入责任人，不填写则登记为公共客户',
                    'dataSource': 'User',
					'showValue':true,
					'readOnly':true
                });
		最后调用inputer.render()，将返回值插入dom。
参数：	placeHolder-值为空时，文本框显示信息内容
		dataSource-【必填值】 数据源，后台根据此参数判断该取什么数据，如'User'代表从用户表取数据，具体值需跟后台协商 
		showValue:最终显示结果是否显示【值】，如showValue为true  显示内容为 0001-user1   ，为false 显示内容为user1
		readOnly:该控件是否为只读
方法：
		向控件传送一个值，并从后台检索出信息显示
	Value()、Value（string）
		读取或设置控件当前Value值，设置值时，触发校验
	Display()
		读取控件当前Display值
	Reset()
		清空控件值
	Readonly(bool)
		读取或设置当前控件只读属性
对应后端：'LenovoInputerApi'