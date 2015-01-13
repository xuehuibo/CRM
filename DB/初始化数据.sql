USE [CRMDB]
GO
/****建立菜单*****/
INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '00','系统设置',1 ,'',0,null,0,0
union all 
select '01','客户管理',1 ,'',1,null,0,1
union all 
select '02','项目管理',1 ,'',2,null,0,1
union all 
select '03','运维管理',1 ,'',3,null,0,1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '0001','部门设置',1 ,'/Config/DeptManage',0,'00',1,0
union all 
select '0002','用户组',1 ,'/Config/UserGroupManage',1,'00',1,0
union all 
select '0003','用户管理',1 ,'/Config/UserManage',2,'00',1,0
union all 
select '0004','系统日志',0 ,'',3,'00',1,0
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0101','客户登记',1 ,'/Customer/CustomerRegister',0,'01',1
union all 
select '0102','公共客户',1 ,'/Customer/PublicCustomer',1,'01',1
union all 
select '0103','私有客户',1 ,'/Customer/PrivateCustomer',2,'01',1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0201','建立项目',1 ,'',0,'02',1
union all 
select '0202','项目跟踪',1 ,'',1,'02',1
union all 
select '0203','项目跟踪',1 ,'',2,'02',1
union all 
select '0204','客户拜访',1 ,'',3,'02',1
union all 
select '0205','项目签约',1 ,'',4,'02',1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0301','客户回访',1 ,'',0,'03',1
union all 
select '0302','客户投诉',1 ,'',1,'03',1
union all 
select '0303','维护登记',1 ,'',2,'03',1
union all 
select '0304','项目续约',1 ,'',3,'03',1
union all 
select '0305','结束撤机',1 ,'',4,'03',1
go

/****建立部门*****/

INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select 'root' ,'公司',null,'',''
 GO    

 INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select '01' ,'测试部门','root','',''
 GO

 /****建立用户组*****/
INSERT INTO [dbo].[tUserGroup]([GroupCode],[GroupName],[GroupType],[BuildUser],[EditUser])
select 'admin','管理员组',0,'脚本建立',''
GO

 /****建立用户*****/
INSERT INTO [dbo].[tUser]([UserCode],[UserName],[UPassword],[Token],[DeptCode],[GroupCode],[Enabled],[BuildUser],[EditUser])
select 'admin','管理员',0xC0E024D9200B5705BC4804722636378A,null,'root','admin',1,'脚本建立',''
GO

 /****建立用户菜单*****/
INSERT INTO [dbo].[tUserGroupFun]([GroupCode],[FunCode],[BuildUser],[EditUser],[Queriable])
select a.GroupCode,b.FunCode,'','',1 from tUserGroup a,tFunction b 

