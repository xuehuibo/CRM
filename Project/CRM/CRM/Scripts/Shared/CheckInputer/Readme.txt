说明：	校验输入控件，输入字符，敲击回车或者离开焦点 触发change事件时，会从后台校验数据有效性。
引入：	'Shared/CheckInputer/CheckInputerView',
初始化：var inputer=new InputerView({
                    'placeHolder': '请输入责任人，不填写则登记为公共客户',
                    'dataSource': 'User',
					'allowEmpty':true,
					'readOnly':true
                });
		最后调用inputer.render()，将返回值插入dom。
参数：	placeHolder-值为空时，文本框显示信息内容
		dataSource-【必填值】 数据源，后台根据此参数判断该取什么数据，如'User'代表从用户表取数据，具体值需跟后台协商 
		allowEmpty-指定该输入控件是否允许为空。
		readOnly -指定控件是否为只读
方法：
	Status()
		读取控件的当前校验状态
	Reset()
		清空控件值
	Value()、Value（string） 
		读取或设置控件的值，并从后台检索出信息显示
	Readonly()、Readonly(bool)
		读取或设置当前控件只读属性
	AllowEmpty()、AllowEmpty(bool)
		读取或设置当前控件是否允许为空
对应后端：'CheckInputerApi'