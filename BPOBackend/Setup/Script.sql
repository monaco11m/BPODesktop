create or replace function get_LabelStorageUrls(id integer)
returns table(url text)
as
$$
begin
return query
select l."Url"
from "LabelStorageUrls" l 
--where l."Id"=id;
limit 5000; 
end
$$
language plpgsql

select l."Url",* 
from "LabelStorageUrls" l 
where l."Id"='403479'
limit 1 

select get_LabelStorageUrls(403479)

DROP FUNCTION get_labelstorageurls(integer)

describe table "LabelStorageUrls"
