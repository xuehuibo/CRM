USE [CRMDB]
GO
/****�����˵�*****/
INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '00','ϵͳ����',1 ,'',0,null,0
union all 
select '01','�ͻ�����',1 ,'',1,null,0
union all 
select '02','��Ŀ����',1 ,'',2,null,0
union all 
select '03','��ά����',1 ,'',3,null,0
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0001','��������',1 ,'',0,'00',1
union all 
select '0002','���¹���',1 ,'',1,'00',1
union all 
select '0003','Ȩ�޹���',1 ,'',2,'00',1
union all 
select '0004','��������',1 ,'',3,'00',1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0101','�ͻ��Ǽ�',1 ,'',0,'01',1
union all 
select '0102','���¹���',1 ,'',1,'01',1
union all 
select '0103','Ȩ�޹���',1 ,'',2,'01',1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0201','������Ŀ',1 ,'',0,'02',1
union all 
select '0202','��Ŀ����',1 ,'',1,'02',1
union all 
select '0203','�ͻ��ݷ�',1 ,'',2,'02',1
union all 
select '0204','��ĿǩԼ',1 ,'',3,'02',1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType])
select '0301','�ͻ��ط�',1 ,'',0,'03',1
union all 
select '0302','�ͻ�Ͷ��',1 ,'',1,'03',1
union all 
select '0303','ά���Ǽ�',1 ,'',2,'03',1
union all 
select '0304','��Ŀ��Լ',1 ,'',3,'03',1
union all 
select '0305','��������',1 ,'',4,'03',1
go

/****��������*****/

INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select '' ,'��˾',null,'',''
 GO

 INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select '01' ,'���Բ���','','�ű�����',''
 GO

 /****�����û���*****/
INSERT INTO [dbo].[tUserGroup]([GroupCode],[GroupName],[GroupType],[BuildUser],[EditUser])
select 'admin','����Ա��',0,'�ű�����',''
GO

 /****�����û�*****/
INSERT INTO [dbo].[tUser]([UserCode],[UserName],[UPassword],[DeptCode],[GroupCode],[Enabled],[BuildUser],[EditUser])
select 'admin','����Ա','','','admin',1,'�ű�����',''
GO

 /****�����û��˵�*****/
INSERT INTO [dbo].[tUserGroupFun]([GroupCode],[FunCode])
select 'admin','00'
union all
select 'admin','0001'
union all
select 'admin','0002'
union all
select 'admin','0003'
union all
select 'admin','0004'
GO

