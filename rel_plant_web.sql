declare num   number; 
begin 
      select count(1)  into num  from  user_tables where TABLE_NAME = 'REL_PlANT_WEB'; 
      if   num=1   then 
          execute immediate 'drop table REL_PlANT_WEB'; 
      end   if; 
end; 
 
declare num number;
begin
    select count(1) into num from user_sequences where sequence_name='rpw_sequence';
    if num =1 then
    execute immediate 'drop sequence rpw_sequence';
    end if;
end;
 

declare num number;
begin
    select count(1) into num from user_triggers where trigger_name='rpw_trig';
    if num=1 then
    execute immediate 'drop trigger rpw_trig';
    end if;
end;
 

create table REL_PlANT_WEB
(
  rel_id     number not null primary key,
  plant_code nvarchar2(20) not null,
  web        number not null
);

-- Add comments to the columns 
comment on column REL_PlANT_WEB.rel_id is '����������';
comment on column REL_PlANT_WEB.plant_code is '��������';
comment on column REL_PlANT_WEB.web is 'վ�����ͣ�1 Ԥ����  2 �ȶ� ';

CREATE SEQUENCE rpw_sequence
   minvalue 1
   nomaxvalue
   start with 1
   increment by 1
   nocache;

create trigger rpw_trig before
insert on REL_PlANT_WEB for each row when (new.rel_id is null)
begin
 select rpw_sequence.nextval into:new.rel_id from dual;
end;

   
