if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tCustContacts') and o.name = 'FK_TCUSTCON_REFERENCE_TCUSTOME')
alter table tCustContacts
   drop constraint FK_TCUSTCON_REFERENCE_TCUSTOME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tProject') and o.name = 'FK_TPROJECT_REFERENCE_TCUSTOME')
alter table tProject
   drop constraint FK_TPROJECT_REFERENCE_TCUSTOME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tProjectEvent') and o.name = 'FK_TPROJECT_REFERENCE_TPROJECT')
alter table tProjectEvent
   drop constraint FK_TPROJECT_REFERENCE_TPROJECT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUser') and o.name = 'FK_TUSER_REFERENCE_TDEPT')
alter table tUser
   drop constraint FK_TUSER_REFERENCE_TDEPT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUser') and o.name = 'FK_TUSER_REFERENCE_TUSERGRO')
alter table tUser
   drop constraint FK_TUSER_REFERENCE_TUSERGRO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUserGroupFun') and o.name = 'FK_TUSERGRO_REFERENCE_TFUNCTIO')
alter table tUserGroupFun
   drop constraint FK_TUSERGRO_REFERENCE_TFUNCTIO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUserGroupFun') and o.name = 'FK_TUSERGRO_REFERENCE_TUSERGRO')
alter table tUserGroupFun
   drop constraint FK_TUSERGRO_REFERENCE_TUSERGRO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tCustContacts')
            and   type = 'U')
   drop table tCustContacts
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tCustomer')
            and   type = 'U')
   drop table tCustomer
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tDept')
            and   type = 'U')
   drop table tDept
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tDirection')
            and   type = 'U')
   drop table tDirection
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tFunction')
            and   type = 'U')
   drop table tFunction
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tProject')
            and   type = 'U')
   drop table tProject
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tProjectEvent')
            and   type = 'U')
   drop table tProjectEvent
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUser')
            and   type = 'U')
   drop table tUser
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUserGroup')
            and   type = 'U')
   drop table tUserGroup
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUserGroupFun')
            and   type = 'U')
   drop table tUserGroupFun
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tXtLog')
            and   type = 'U')
   drop table tXtLog
go

execute sp_revokedbaccess dbo
go

/*==============================================================*/
/* User: dbo                                                    */
/*==============================================================*/
execute sp_grantdbaccess dbo
go

/*==============================================================*/
/* Table: tCustContacts                                         */
/*==============================================================*/
create table tCustContacts (
   Id                   numeric              not null,
   CustomerCode         nvarchar(20)         not null,
   Name                 nvarchar(20)         not null,
   Phone1               varchar(20)          null,
   Phone2               varchar(20)          null,
   Email                varchar(50)          null,
   Remark               nvarchar(100)        null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TCUSTCONTACTS primary key (Id)
)
go

/*==============================================================*/
/* Table: tCustomer                                             */
/*==============================================================*/
create table tCustomer (
   Id                   numeric              identity,
   CustomerCode         nvarchar(20)         not null,
   CustomerName         nvarchar(100)        not null,
   Status               numeric(1)           not null,
   Remark               nvarchar(500)        null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TCUSTOMER primary key (Id),
   constraint AK_KEY_2_TCUSTOME unique (CustomerCode)
)
go

/*==============================================================*/
/* Table: tDept                                                 */
/*==============================================================*/
create table tDept (
   Id                   numeric              identity,
   DeptCode             varchar(20)          not null,
   DeptName             nvarchar(20)         not null,
   ParentCode           varchar(20)          null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TDEPT primary key (Id),
   constraint AK_KEY_TDEPT unique (DeptCode)
)
go

/*==============================================================*/
/* Table: tDirection                                            */
/*==============================================================*/
create table tDirection (
   Module               varchar(10)          null,
   Value                numeric(2)           null,
   Display              nvarchar(100)        null,
   constraint AK_KEY_1_TDIRECTI unique (Module, Value)
)
go

/*==============================================================*/
/* Table: tFunction                                             */
/*==============================================================*/
create table tFunction (
   Id                   numeric              identity,
   FunCode              varchar(20)          not null,
   FunName              nvarchar(50)         not null,
   Enabled              bit                  not null default 0,
   FunCmd               varchar(100)         null,
   SerialNo             numeric              not null,
   ParentCode           varchar(20)          null,
   FunType              numeric(1)           not null,
   constraint PK_TFUNCTION primary key (Id),
   constraint AK_KEY_3_TFUNCTIO unique (SerialNo, ParentCode),
   constraint AK_KEY_2_TFUNCTIO unique (FunCode)
)
go

/*==============================================================*/
/* Table: tProject                                              */
/*==============================================================*/
create table tProject (
   Id                   numeric              not null,
   ProjectCode          varchar(20)          not null,
   CustomerCode         nvarchar(20)         not null,
   ProjectName          nvarchar(100)        not null,
   Step                 numeric(2)           not null default 0,
   Completion           numeric(3)           not null default 0,
   BuildDate            datetime             not null default getdate(),
   BuildUserCode        nvarchar(50)         not null,
   EditDate             datetime             not null,
   EditUser             nvarchar(50)         not null,
   constraint PK_TPROJECT primary key (Id),
   constraint AK_KEY_TPROJECT unique (ProjectCode)
)
go

/*==============================================================*/
/* Table: tProjectEvent                                         */
/*==============================================================*/
create table tProjectEvent (
   Id                   numeric              not null,
   ProjectCode          varchar(20)          not null,
   SerialNo             numeric              not null,
   HappenTime           datetime             not null,
   AboutUser            nvarchar(50)         not null,
   AboutCustPeople      nvarchar(50)         not null,
   Remark               nvarchar(1000)       not null,
   Step                 numeric(2)           not null,
   Completion           numeric(3)           not null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TPROJECTEVENT primary key (Id),
   constraint AK_KEY_TPROJECTEVENT unique (ProjectCode, SerialNo)
)
go

/*==============================================================*/
/* Table: tUser                                                 */
/*==============================================================*/
create table tUser (
   Id                   numeric              identity,
   UserCode             varchar(20)          not null,
   UserName             nvarchar(20)         not null,
   UPassword            binary(16)           not null,
   DeptCode             varchar(20)          not null,
   GroupCode            varchar(20)          not null,
   Enabled              bit                  not null default 1,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   Token                binary(16)           null,
   constraint PK_TUSER primary key (Id),
   constraint AK_KEY_2_TUSER unique (UserCode)
)
go

/*==============================================================*/
/* Table: tUserGroup                                            */
/*==============================================================*/
create table tUserGroup (
   Id                   numeric              identity,
   GroupCode            varchar(20)          not null,
   GroupName            nvarchar(20)         not null,
   GroupType            numeric(2)           not null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TUSERGROUP primary key (Id),
   constraint AK_KEY_TUSERGROUP unique (GroupCode)
)
go

/*==============================================================*/
/* Table: tUserGroupFun                                         */
/*==============================================================*/
create table tUserGroupFun (
   Id                   numeric              identity,
   GroupCode            varchar(20)          not null,
   FunCode              varchar(20)          not null,
   constraint PK_TUSERGROUPFUN primary key (Id),
   constraint AK_KEY_TUSERGROUPFUN unique (GroupCode, FunCode)
)
go

/*==============================================================*/
/* Table: tXtLog                                                */
/*==============================================================*/
create table tXtLog (
   SerialNo             numeric              identity,
   LogDate              datetime             not null default getdate(),
   LogContent           nvarchar(1000)       not null,
   LogType              numeric(2)           not null,
   constraint PK_TXTLOG primary key (SerialNo)
)
go

alter table tCustContacts
   add constraint FK_TCUSTCON_REFERENCE_TCUSTOME foreign key (CustomerCode)
      references tCustomer (CustomerCode)
go

alter table tProject
   add constraint FK_TPROJECT_REFERENCE_TCUSTOME foreign key (CustomerCode)
      references tCustomer (CustomerCode)
go

alter table tProjectEvent
   add constraint FK_TPROJECT_REFERENCE_TPROJECT foreign key (ProjectCode)
      references tProject (ProjectCode)
go

alter table tUser
   add constraint FK_TUSER_REFERENCE_TDEPT foreign key (DeptCode)
      references tDept (DeptCode)
go

alter table tUser
   add constraint FK_TUSER_REFERENCE_TUSERGRO foreign key (GroupCode)
      references tUserGroup (GroupCode)
go

alter table tUserGroupFun
   add constraint FK_TUSERGRO_REFERENCE_TFUNCTIO foreign key (FunCode)
      references tFunction (FunCode)
go

alter table tUserGroupFun
   add constraint FK_TUSERGRO_REFERENCE_TUSERGRO foreign key (GroupCode)
      references tUserGroup (GroupCode)
go
