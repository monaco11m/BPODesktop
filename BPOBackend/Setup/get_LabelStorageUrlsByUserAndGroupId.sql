create or replace function get_LabelStorageUrlsByUserAndGroupId(userId text,groupId integer)
returns table(url text)
as
$$
begin
return query
select l."Url"
from "LabelStorageUrls" l 
where l."UserId"=userId;
end
$$
language plpgsql

--select get_LabelStorageUrlsByUserAndGroupId('fe69add2-2149-44dc-9415-cdd640b36925',0)