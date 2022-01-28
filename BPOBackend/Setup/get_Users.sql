create or replace function get_Users()
returns table(Id text,UserName character varying(256))
as
$$
begin
return query
select u."Id",u."UserName"
from "AspNetUsers" u 
where u.is_active=true;
end
$$
language plpgsql

--select get_Users()